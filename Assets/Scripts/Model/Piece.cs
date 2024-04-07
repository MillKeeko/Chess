using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    public Vector2 Position;
    public List<General.PossibleMove> PossibleMovesList = new List<General.PossibleMove>();
    public bool FirstMove = true;

    public delegate void OnPieceCreated(Piece piece);
    public static event OnPieceCreated OnPieceCreatedEvent;
    public delegate void OnPieceSetupComplete();
    public static event OnPieceSetupComplete OnPieceSetupCompleteEvent;
    public delegate void OnValidMove(Piece piece, Vector2 targetPosition);
    public static event OnValidMove OnValidMoveEvent;

    void Awake()
    {
        GameController.OnTurnStartEvent += GeneratePossibleMoves;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnPieceCreatedEvent?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    //  Public Methods
    //

    //  Loop through possible moves to see if move attempt is valid
    public void MoveAttempt(Vector2 targetPosition)
    {
        bool validMove = false;
        Debug.Log("MoveAttempt PossibleMovesList length " + PossibleMovesList.Count);
        foreach (General.PossibleMove move in PossibleMovesList)
        {
            if (targetPosition == move.TargetPosition)
            {
                BoardController.ExecuteMove(this, targetPosition);
                validMove = true;
                if (FirstMove) FirstMove = false;
                break; // To avoid list changing while executing - it would return after events finish and error
            }
        }
        if (validMove) OnValidMoveEvent?.Invoke(this, targetPosition); // Trigger event here instead (I AM A GENIUS)
    }

    //
    //  Virtual Methods
    //

    //  Evaluates all 64 tiles and determines which are possible moves for this piece
    //  Adds each move to the possibleMovesList
    public virtual void GeneratePossibleMoves()
    {
        
    }

    //
    //  Protected Methods
    //

    protected void TriggerSetupComplete()
    {
        OnPieceSetupCompleteEvent?.Invoke();
    }

    protected void EmptyMovesList()
    {
        PossibleMovesList.Clear();
    }

    protected bool IsRangeMoveBlocked(Vector2 targetPosition)
    {
        bool blockingBool = false;

        Vector2 moveUnitVector = CalculateMoveUnitVector(targetPosition);
        int distance = CalculateMoveDistance(targetPosition);

        for (int i = 1; i < Mathf.Abs(distance); i++)
        {
            Vector2 blockPosition = new Vector2((Position.x + (moveUnitVector.x * i)), 
                                                (Position.y + (moveUnitVector.y * i)));
            //Debug.Log("Position x " + Position.x + " y " + Position.y);
            //Debug.Log("IsRangeMoveBlocked() checking " + this + " at x " + (int)blockPosition.x + " y " + (int)blockPosition.y);
            if (TrackingHandler.pieceTracker[(int)blockPosition.x,(int)blockPosition.y] != null) 
            {
                blockingBool = true;
            }
            //Debug.Log("No piece in the way at x " + (int)blockPosition.x + " and y " + (int)blockPosition.y);
        }

        return blockingBool;
    }

    //  
    protected Vector2 CalculateMoveUnitVector(Vector2 targetPosition)
    {
        // Create unitary vector of the target relative to the piece
        Vector2 moveVector = new Vector2((int)targetPosition.x - (int)Position.x,
                                         (int)targetPosition.y - (int)Position.y);
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

    //  
    protected int CalculateMoveDistance(Vector2 targetPosition)
    {
        // Find the distance the piece is moving
        int distanceX = (int)targetPosition.x - (int)Position.x;
        int distanceY = (int)targetPosition.y - (int)Position.y;
        int distance = 0;

        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY)) distance = Mathf.Abs(distanceX);
        else distance = Mathf.Abs(distanceY);

        return distance;
    }

    //
    //  Private Methods
    //

    /*private void GenerateFirstMoves()
    {
        if (!_generatedFirstPossibleMoves)
        {
            GeneratePossibleMoves();
            _generatedFirstPossibleMoves = true;
        }
    }*/
}
