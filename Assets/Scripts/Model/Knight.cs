using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class Knight : Piece
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
                if (IsValidKnightMove(targetPosition))
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
    private bool IsValidKnightMove(Vector2 targetPosition)
    {
        bool returnBool = false;
        int targetX = (int)targetPosition.x;
        int targetY = (int)targetPosition.y;
        Piece target = TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y];

        if (target != null && target.CompareTag(this.tag)) 
        {
            returnBool = false;
        }
        else if (targetX == Position.x - 2 || targetX == Position.x + 2)
        {
            if (targetY == Position.y + 1 || targetY == Position.y - 1) returnBool = true;
        }
        else if (targetY == Position.y - 2 || targetY == Position.y + 2)
        { 
            if (targetX == Position.x + 1 || targetX == Position.x - 1) returnBool = true;
        }

        return returnBool;
    }
}
