using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  I don't like how I need a separate OnMoveExecuted event right now

//  Virtual MoveValidator for child classes to override
//  MoveExecutor moves the piece and can't be overridden
//  DestroyInstance which destroys the piece and can't be overridden
public class Piece : MonoBehaviour
{
    protected bool firstMove = true; // For some reason when this was in Awake() it was false in Pawn Start() call
    protected List<General.PossibleMove> possiblePieceMovesList = new List<General.PossibleMove>(); // Also broke when initialized in Awake()

    //  Delegates
    public delegate void OnPieceMoved (Piece piece, int oldX, int oldY);
    public delegate void OnPieceDestroyed (int x, int y);
    public delegate void OnMoveExecuted ();

    //  Events
    public static event OnPieceMoved onPieceMoved;
    public static event OnPieceDestroyed onPieceDestroyed;
    public static event OnMoveExecuted onMoveExecuted; // Wish I didn't have to use this

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual List<General.PossibleMove> GeneratePossibleMoves()
    {
        return possiblePieceMovesList;
    }

    public virtual void MoveAttempt(Vector3 targetPosition)
    {
        
    }

    protected void DestroyInstance()
    {
        //Debug.Log("Destroy " + this);
        int x = (int)this.transform.position.x;
        int y = (int)this.transform.position.y;

        onPieceDestroyed?.Invoke(x, y);
        Destroy(gameObject);
    }

    // Destroy target piece and move selected piece
    protected void MoveExecutor(Vector3 targetPosition)
    {
        //Debug.Log("MoveExecutor Start.");
        int oldX = (int)this.transform.position.x;
        int oldY = (int)this.transform.position.y;
        Piece targetPiece = TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y];

        if (GetFirstMove() == true) SetFirstMoveFalse();

        if (targetPiece != null)
        {
            targetPiece.DestroyInstance();
        }

        this.transform.position = targetPosition;
        onPieceMoved?.Invoke(this, oldX, oldY);
        onMoveExecuted?.Invoke();
    }

    protected bool IsRangeMoveBlocked(Vector3 targetPosition)
    {
        bool blockingBool = false;
        Vector2 moveUnitVector = CalculateMoveUnitVector(targetPosition);
        int distance = CalculateMoveDistance(targetPosition);

        for (int i = 1; i < Mathf.Abs(distance); i++)
        {
            Vector2 checkPosition = new Vector2((this.transform.position.x + (moveUnitVector.x * i)), 
                                                (this.transform.position.y + (moveUnitVector.y * i)));
            //Debug.Log("x * i " + (x * i));
            //Debug.Log("y * i " + (y * i));
            //Debug.Log("IsPieceBlocking() checking " + this + " at x " + (int)checkPosition.x + " y " + (int)checkPosition.y);
            if (TrackingHandler.pieceTracker[(int)checkPosition.x,(int)checkPosition.y] != null) 
            {
                blockingBool = true;
            }
            //Debug.Log("No piece in the way at x " + (int)checkPosition.x + " and y " + (int)checkPosition.y);
        }

        return blockingBool;
    }

    protected Vector2 CalculateMoveUnitVector(Vector3 targetPosition)
    {
        // Create unitary vector of the target relative to the piece
        Vector2 moveVector = new Vector2((int)targetPosition.x - (int)this.transform.position.x,
                                         (int)targetPosition.y - (int)this.transform.position.y);
        Vector2 unitMoveVector = moveVector.normalized;

        // Simplify unit vector
        int x, y = 0;
        if (unitMoveVector.x > 0) x = 1;
        else if (unitMoveVector.x < 0) x = -1;
        else x = 0;
        if (unitMoveVector.y > 0) y = 1;
        else if (unitMoveVector.y < 0) y = -1;
        else y = 0;

        Vector2 simplifiedUnitVector = new Vector2 (x, y);
        return simplifiedUnitVector;
    }

    protected int CalculateMoveDistance(Vector3 targetPosition)
    {
        // Find the distance the piece is moving
        int distanceX = (int)targetPosition.x - (int)this.transform.position.x;
        int distanceY = (int)targetPosition.y - (int)this.transform.position.y;
        int distance = 0;

        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY)) distance = Mathf.Abs(distanceX);
        else distance = Mathf.Abs(distanceY);

        return distance;
    }

    //
    //  Accessors
    //

    protected bool GetFirstMove()
    {
        return firstMove;
    }

    //
    //  Mutators
    //

    protected void SetFirstMoveFalse()
    {
        //Debug.Log("Setting first move false.");
        firstMove = false;
    }
}
