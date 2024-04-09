using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class King : Piece
{
    //
    //  Override Methods
    //

    public override bool IsBasicMoveValid(Vector2 targetPosition)
    {
        bool returnBool = false;
        if (IsValidKingMove(targetPosition))
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
    private bool IsValidKingMove(Vector2 targetPosition)
    {
        bool returnBool = false;
        int targetX = (int)targetPosition.x;
        int targetY = (int)targetPosition.y;
        Piece target = TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y];

        if (target != null && target.CompareTag(this.tag))
        {
            returnBool = false;
        }
        else if (targetX == Position.x + 1)
        {
            if (targetY == Position.y || targetY == Position.y + 1 || targetY == Position.y - 1) returnBool = true;
        }
        else if (targetX == Position.x)
        {
            if (targetY == Position.y + 1 || targetY == Position.y - 1) returnBool = true;
        }
        else if (targetX == Position.x - 1)
        {
            if (targetY == Position.y || targetY == Position.y + 1 || targetY == Position.y - 1) returnBool = true;
        }

        return returnBool;
    }
}
