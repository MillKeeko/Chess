using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class King : Piece
{
    public delegate void CastleKingSide(Piece piece);
    public static event CastleKingSide CastleKingSideEvent;
    public delegate void CastleQueenSide(Piece piece);
    public static event CastleQueenSide CastleQueenSideEvent;

    //
    //  Override Methods
    //

    public override bool IsBasicMoveValid(Vector2 targetPosition)
    {
        bool returnBool = false;
        if (IsValidKingMove(targetPosition) ||
            IsValidCastleMove(targetPosition))
        {
            returnBool = true;
        }
        return returnBool;
    }

    //
    //  Private Methods
    //

    private bool IsValidCastleMove(Vector2 targetPosition)
    {
        bool returnBool = false;
        Piece rook;
        int rank;
        int kingSideOffset;
        int queenSideOffset;
        int kingSideRookFile;
        int queenSideRookFile;

        if (this.CompareTag(GameController.BotTag)) 
        {
            kingSideOffset = -2;
            queenSideOffset = 2;
            rank = 7;
            kingSideRookFile = 0;
            queenSideRookFile = 7;
        }
        else 
        {
            kingSideOffset = 2;
            queenSideOffset = -2;
            rank = 0;
            kingSideRookFile = 7;
            queenSideRookFile = 0;
        }

        if (FirstMove && (int)targetPosition.y == (int)Position.y)
        {
            if ((int)targetPosition.x == (int)(Position.x + kingSideOffset))
            {
                rook = TrackingHandler.pieceTracker[kingSideRookFile,rank];
                if (rook != null && rook.FirstMove)
                {
                    CastleKingSideEvent?.Invoke(this);
                    returnBool = true;
                }
            }
            else if ((int)targetPosition.x == (int)(Position.x - queenSideOffset))
            {
                rook = TrackingHandler.pieceTracker[queenSideRookFile,rank];
                if (rook != null && rook.FirstMove)
                {
                    CastleQueenSideEvent?.Invoke(this);
                    returnBool = true;
                }
            }
        }
        
        return returnBool;
    }

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
