using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class Queen : Piece
{
    //
    //  Override Methods
    //

    public override bool IsBasicMoveValid(Vector2 targetPosition, Piece[,] pieceArray)
    {
        bool returnBool = false;
        if (IsValidQueenMove(targetPosition, pieceArray))
        {
            returnBool = true;
        }
        return returnBool;
    }
    
    //
    //  Private Methods
    //

    //  Take vector2 representing move target location in TrackingHandler.pieceTracker
    //  Returns bool if given position of piece, the move follows the rules
    private bool IsValidQueenMove(Vector2 targetPosition, Piece[,] pieceArray)
    {
        bool returnBool = false;
        Piece target = pieceArray[(int)targetPosition.x, (int)targetPosition.y];
        
        // Combined logic for Rook and Bishop
        if ((targetPosition.x - transform.position.x == targetPosition.y - transform.position.y ||
            targetPosition.x - transform.position.x == -1 * (targetPosition.y - transform.position.y) ||
            targetPosition.x == transform.position.x  || 
            targetPosition.y == transform.position.y) &&
            (target == null || !target.CompareTag(this.tag)) &&
            !IsRangeMoveBlocked(targetPosition))
        {
            //Debug.Log("Valid " + this.tag + " Queen move to x " + targetPosition.x + " to y " + targetPosition.y);
            returnBool = true;
        }

        return returnBool;
    }
}
