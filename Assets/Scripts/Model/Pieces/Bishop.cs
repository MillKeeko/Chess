using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class Bishop : Piece
{
    void Awake()
    {
        Value = 300;
    }

    //
    //  Override Methods
    //

    public override bool IsBasicMoveValid(Piece[,] boardPosition, Vector2 targetPosition)
    {
        bool returnBool = false;
        if (IsValidBishopMove(boardPosition, targetPosition))
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
    private bool IsValidBishopMove(Piece[,] boardPosition, Vector2 targetPosition)
    {
        bool returnBool = false;
        Piece target = boardPosition[(int)targetPosition.x, (int)targetPosition.y];

        // if the difference between the x == the difference between the y, or the negative of that difference
        if ((targetPosition.x - transform.position.x == targetPosition.y - transform.position.y ||
            targetPosition.x - transform.position.x == -1 * (targetPosition.y - transform.position.y)) &&
            (target == null || !target.CompareTag(this.tag)) &&
            !IsRangeMoveBlocked(boardPosition, targetPosition))
        {
            returnBool = true;
        }
        
        return returnBool;
    }
}
