using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  MoveValidator
//      IsEnPassant
//      IsDoubleMove
//      IsDiagonalMove
//      IsNormalPawnMove
public class Pawn : Piece
{
    private int forwardMove;

    void Awake()
    {
        FindForwardDirection();
    }

    // Start is called before the first frame update
    void Start()
    {
        GeneratePossibleMoves();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void MoveAttempt(Vector3 targetPosition)
    {
        //Debug.Log("MoveAttempt Start.");
        if (IsValidNormalPawnMove(targetPosition) || 
            IsValidDiagonalAttack(targetPosition) || 
            IsValidFirstMoveDouble(targetPosition))
        {
            MoveExecutor(targetPosition);
        }
    }

    public override List<General.PossibleMove> GeneratePossibleMoves()
    {
        Vector3 targetPosition;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                targetPosition = new Vector3(x, y, Constants.PIECE_Z_INDEX);
                if (IsValidNormalPawnMove(targetPosition) || 
                    IsValidDiagonalAttack(targetPosition) || 
                    IsValidFirstMoveDouble(targetPosition))
                {
                    General.PossibleMove possibleMove = new General.PossibleMove(x, y, this);
                    possiblePieceMovesList.Add(possibleMove);
                    //Debug.Log(this + " from x " + this.transform.position.x + " y " + this.transform.position.y + " to x " + targetPosition.x + " y " + targetPosition.y);
                }
            }
        }

        //Debug.Log(this + " has " + possiblePieceMovesList.Count + " possible moves.");
        return possiblePieceMovesList;
    }

    private bool IsValidDiagonalAttack(Vector3 targetPosition)
    {
        bool returnBool = false;

        // Move 1 square diagonal forward if attacking a piece
        if (targetPosition.x == transform.position.x + 1 || targetPosition.x == transform.position.x - 1)
        {
            if (targetPosition.y == (transform.position.y + forwardMove) &&
                TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y] != null &&
                !TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y].CompareTag(this.tag))
            {
                //Debug.Log("See diagonal move");
                returnBool = true;
            }
        }

        return returnBool;
    }

    private bool IsValidNormalPawnMove(Vector3 targetPosition)
    {
        bool returnBool = false;

        if (targetPosition.x == transform.position.x)
        {
            // Move 1 square forward
            if (targetPosition.y == (transform.position.y + forwardMove) &&
                TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y] == null)
            {
                //Debug.Log("See normal move");
                returnBool = true;
            }
        }

        return returnBool;
    }

    private bool IsValidFirstMoveDouble(Vector3 targetPosition)
    {
        bool returnBool = false;

        // First move can be 2 squares forward
        if (GetFirstMove() && 
            (targetPosition.y == (transform.position.y + (2 * forwardMove))) &&
            targetPosition.x == transform.position.x &&
            TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y] == null)
        {
            //Debug.Log("See double move");
            returnBool = true;
        }

        return returnBool;
    }

    private int FindForwardDirection()
    {
        if (this.tag == GameController.botTag) forwardMove = -1;
        else if (this.tag == GameController.playerTag) forwardMove = 1;

        return forwardMove;
    }
}
