using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  MoveValidator
//      IsNormalQueenMove
public class Queen : Piece
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<General.PossibleMove> GeneratePossibleMoves()
    {
        Vector3 targetPosition;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (!CheckHandler.isKingInCheck)
                {
                    targetPosition = new Vector3(x, y, Constants.PIECE_Z_INDEX);
                    if (IsValidQuuenMove(targetPosition))
                    {
                        General.PossibleMove possibleMove = new General.PossibleMove(x, y, this);
                        possiblePieceMovesList.Add(possibleMove);
                        //Debug.Log(this + " from x " + this.transform.position.x + " y " + this.transform.position.y + " to x " + targetPosition.x + " y " + targetPosition.y);
                    }
                }
            }
        }

        //Debug.Log(this + " has " + possiblePieceMovesList.Count + " possible moves.");
        return possiblePieceMovesList;
    }
    
    public override void MoveAttempt(Vector3 targetPosition)
    {
        if (!CheckHandler.isKingInCheck)
        {
            //Debug.Log("MoveAttempt Start.");
            if (IsValidQuuenMove(targetPosition)) MoveExecutor(targetPosition);
            EmptyMovesList();
        }
    }

    private bool IsValidQuuenMove(Vector3 targetPosition)
    {
        bool returnBool = false;
        Piece target = TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y];
        
        // Combined logic for Rook and Bishop
        if ((targetPosition.x - transform.position.x == targetPosition.y - transform.position.y ||
            targetPosition.x - transform.position.x == -1 * (targetPosition.y - transform.position.y) ||
            targetPosition.x == transform.position.x  || 
            targetPosition.y == transform.position.y) &&
            (target == null || !target.CompareTag(this.tag)) &&
            !IsRangeMoveBlocked(targetPosition))
        {
            returnBool = true;
        }

        return returnBool;
    }
}
