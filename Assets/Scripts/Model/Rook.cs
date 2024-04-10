using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class Rook : Piece
{
    //
    //  Override Methods
    //

    public override bool IsBasicMoveValid(Vector2 targetPosition, Piece[,] pieceArray)
    {
        bool returnBool = false;
        if (IsValidRookMove(targetPosition, pieceArray))
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
    private bool IsValidRookMove(Vector2 targetPosition, Piece[,] pieceArray)
    {
        bool returnBool = false;
        Piece target = pieceArray[(int)targetPosition.x, (int)targetPosition.y];
        
        // if the difference between the x == the difference between the y, or the negative of that difference
        if ((targetPosition.x == transform.position.x  || targetPosition.y == transform.position.y) &&
            (target == null || !target.CompareTag(this.tag)) &&
            !IsRangeMoveBlocked(targetPosition))
        {
            returnBool = true;
        }

        return returnBool;
    }
}
