using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    public Vector2 Position;
    public List<PossibleMove> PossibleMovesList = new List<PossibleMove>();
    public bool FirstMove = true;
    public int Value;

    public delegate void OnPieceCreated(Piece piece);
    public static event OnPieceCreated OnPieceCreatedEvent;

    void Awake()
    {
        
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

    //  Evaluates all 64 tiles and determines which are possible moves for this piece
    //  Adds each move to the possibleMovesList
    public void GeneratePossibleMoves()
    {
        Vector2 targetPosition;
        PossibleMovesList.Clear();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                targetPosition = new Vector2(x, y);
                if (IsBasicMoveValid(targetPosition))
                {
                    if (this.CompareTag(GameController.Turn))
                    {
                        if (CheckHandler.DoesMoveEndInCheck(this, targetPosition))
                        {
                            PossibleMove possibleMove = new PossibleMove(targetPosition, this);
                            PossibleMovesList.Add(possibleMove);
                        }
                    }
                    else
                    {
                        PossibleMove possibleMove = new PossibleMove(targetPosition, this);
                        PossibleMovesList.Add(possibleMove);
                    }
                }
            }
        }
    }

    //
    //  Virtual Methods
    //

    public virtual void DestroyPiece()
    {
        Destroy(gameObject);
    }

    public virtual bool IsBasicMoveValid(Vector2 targetPosition)
    {
        return false;
    }

    //
    //  Protected Methods
    //

    protected bool IsRangeMoveBlocked(Vector2 targetPosition)
    {
        bool blockingBool = false;

        Vector2 moveUnitVector = CalculateMoveUnitVector(targetPosition);
        int distance = CalculateMoveDistance(targetPosition);
        for (int i = 1; i < Mathf.Abs(distance); i++)
        {
            Vector2 blockPosition = new Vector2((Position.x + (moveUnitVector.x * i)), 
                                                (Position.y + (moveUnitVector.y * i)));
            if (TrackingHandler.pieceTracker[(int)blockPosition.x,(int)blockPosition.y] != null) 
            {
                blockingBool = true;
            }
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
}
