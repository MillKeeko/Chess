using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class Queen : Piece
{
    //
    //  Override Methods
    //

    public override void GeneratePossibleMoves()
    {
        Vector2 targetPosition;
        EmptyMovesList();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                targetPosition = new Vector2(x, y);
                if (IsValidQueenMove(targetPosition))
                { 
                    General.PossibleMove possibleMove = new General.PossibleMove(targetPosition, this);
                    PossibleMovesList.Add(possibleMove);
                    //Debug.Log(this + " from x " + this.transform.position.x + " y " + this.transform.position.y + " to x " + targetPosition.x + " y " + targetPosition.y);
                }
            }
        }
        //Debug.Log(this + " has " + PossibleMovesList.Count + " possible moves.");
        TriggerSetupComplete();
    }
    
    //
    //  Private Methods
    //

    //  Take vector2 representing move target location in TrackingHandler.pieceTracker
    //  Returns bool if given position of piece, the move follows the rules
    private bool IsValidQueenMove(Vector2 targetPosition)
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
