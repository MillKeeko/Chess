using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class King : Piece
{
    void Awake()
    {
        Value = 10000;
    }

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

        if (GameController.PlayerTag == Constants.WHITE_TAG) 
        {
            kingSideOffset = 2;
            queenSideOffset = -2;
            kingSideRookFile = 7;
            queenSideRookFile = 0;   
        }
        else 
        {
            kingSideOffset = -2;
            queenSideOffset = 2;
            kingSideRookFile = 0;
            queenSideRookFile = 7;
        }

        if (this.CompareTag(GameController.BotTag))
        {
            rank = 7;
        }
        else
        {
            rank = 0;
        }

        if (FirstMove && 
            (int)targetPosition.y == (int)Position.y &&
            !CheckHandler.IsInCheck &&
            this.CompareTag(GameController.Turn))
        {
            if ((int)targetPosition.x == (int)(Position.x + kingSideOffset) &&
                !IsCastleBlocked(kingSideOffset, 0))
            {
                rook = TrackingHandler.pieceTracker[kingSideRookFile,rank];
                if (rook != null && rook.FirstMove && rook is Rook)
                {
                    returnBool = true;
                }
            }
            else if ((int)targetPosition.x == (int)(Position.x + queenSideOffset) &&
                     !IsCastleBlocked(queenSideOffset, 1))
            {
                rook = TrackingHandler.pieceTracker[queenSideRookFile,rank];
                if (rook != null && rook.FirstMove && rook is Rook)
                {
                    returnBool = true;
                }
            }
        }
        
        return returnBool;
    }

    //  Offset is either -2 or 2
    //  int side: 0 = kingSide, 1 = queenSide 
    private bool IsCastleBlocked(int offset, int side)
    {
        bool returnBool = false;
        int halfOffset = offset/2;
        Vector2 positionOne = new Vector2(this.Position.x + halfOffset, this.Position.y);
        Vector2 positionTwo = new Vector2(this.Position.x + offset, this.Position.y);
        Vector2 positionThree = new Vector2(this.Position.x + offset + halfOffset, this.Position.y);

        foreach (PossibleMove move in GameController.PossibleEnemyAttackList)
        {
            if ((move.TargetPosition.x == positionOne.x && move.TargetPosition.y == positionOne.y) ||
                (move.TargetPosition.x == positionTwo.x && move.TargetPosition.y == positionTwo.y) ||
                TrackingHandler.pieceTracker[(int)positionOne.x, (int)positionTwo.y] != null ||
                TrackingHandler.pieceTracker[(int)positionTwo.x, (int)positionTwo.y] != null)
            {
                returnBool = true;
                break;
            }
            else if (side == 1)
            {
                if (move.TargetPosition.x == positionThree.x && move.TargetPosition.y == positionThree.y ||
                    TrackingHandler.pieceTracker[(int)positionThree.x, (int)positionThree.y] != null)
                {
                    returnBool = true;
                    break;
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
