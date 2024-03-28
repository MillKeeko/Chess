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
