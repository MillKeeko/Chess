using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    public Vector2 Position;
    public List<General.PossibleMove> PossibleMovesList = new List<General.PossibleMove>();

    protected bool FirstMove = true;

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

    //  Evaluates all 64 tiles and determines which are possible moves for this piece
    //  Adds each move to the possibleMovesList
    public virtual void GeneratePossibleMoves()
    {
        
    }

    //  
    public virtual void MoveAttempt(Vector2 targetPosition)
    {
        foreach (General.PossibleMove move in PossibleMovesList)
        {
            
        }
    }

    protected void EmptyMovesList()
    {
        PossibleMovesList.Clear();
    }

    // Destroy target piece and move selected piece
    protected void MoveExecutor(Vector2 targetPosition)
    {
        //  Set first move to false if true
        //  Destroy target piece (nooooooo)
        //  Move piece (noooooooo)
    }

    protected bool IsRangeMoveBlocked(Vector2 targetPosition)
    {
        bool blockingBool = false;

        /*Vector2 moveUnitVector = CalculateMoveUnitVector(targetPosition);
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
        }*/

        return blockingBool;
    }

    //  
    protected Vector2 CalculateMoveUnitVector(Vector2 targetPosition)
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
        return FirstMove;
    }

    //
    //  Mutators
    //

    protected void SetFirstMoveFalse()
    {
        //Debug.Log("Setting first move false.");
        FirstMove = false;
    }
}
