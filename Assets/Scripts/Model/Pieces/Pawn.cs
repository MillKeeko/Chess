using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class Pawn : Piece
{
    private int _forwardMove;

    void Awake()
    {
        SetForwardMove();
        GameController.AddDiagonalPawnAttacksEvent += AddDiagonalPawnAttacks;
    }

    //
    //  Override methods
    //

    public override bool IsBasicMoveValid(Vector2 targetPosition)
    {
        bool returnBool = false;
        if (IsValidDiagonalAttack(targetPosition) || 
            IsValidFirstMoveDouble(targetPosition) || 
            IsValidForwardMove(targetPosition) ||
            IsValidEnPassant(targetPosition))
        {
            returnBool = true;
        }
        return returnBool;
    }

    public override void DestroyPiece()
    {
        GameController.AddDiagonalPawnAttacksEvent -= AddDiagonalPawnAttacks;
        Destroy(gameObject);
    }

    //
    //  Private Methods
    //

    private bool IsValidEnPassant(Vector2 targetPosition)
    {
        if (targetPosition.x == Position.x + 1 || targetPosition.x == Position.x - 1)
        {
            if (targetPosition.y == (Position.y + _forwardMove) &&
                targetPosition == MoveController.EnPassantTargetPosition)
            {
                return true;
            }
        }
        return false;
    }

    private void AddDiagonalPawnAttacks()
    {
        Vector2 targetPosition;
        PossibleMove attack;

        if (!this.CompareTag(GameController.Turn))
        {
            targetPosition = new Vector2 (Position.x + 1, Position.y + (_forwardMove));
            attack = new PossibleMove(targetPosition, this);

            if (!GameController.PossibleEnemyAttackList.Contains(attack))
            {
                GameController.PossibleEnemyAttackList.Add(attack);
            }

            targetPosition = new Vector2 (Position.x - 1, Position.y + (_forwardMove));
            attack = new PossibleMove(targetPosition, this);

            if (!GameController.PossibleEnemyAttackList.Contains(attack))
            {
                GameController.PossibleEnemyAttackList.Add(attack);
            }
        } 
    }

    //  Take vector2 representing move target location in TrackingHandler.pieceTracker
    //  Returns bool if given position of piece, the move follows the rules
    private bool IsValidDiagonalAttack(Vector2 targetPosition)
    {
        bool returnBool = false;

        //  Attack moves have to be forward diagonal
        //      IF      (target file is +1 OR -1 of piece file) AND
        //              target is one rank forward              AND
        //              target tile has a piece                 AND
        //              that piece is the opposite colour
        //      THEN    return true
        if (targetPosition.x == Position.x + 1 || targetPosition.x == Position.x - 1)
        {
            if (targetPosition.y == (Position.y + _forwardMove) &&
                TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y] != null &&
                !TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y].CompareTag(this.tag))
            {
                //Debug.Log("See diagonal move");
                returnBool = true;
            }
        }

        return returnBool;
    }

    //  Take vector2 representing move target location in TrackingHandler.pieceTracker
    //  Returns bool if given position of piece, the move follows the rules
    private bool IsValidForwardMove(Vector2 targetPosition)
    {
        bool returnBool = false;

        //  Normal forward pawn move 1 rank
        //      IF      target is one rank forward          AND
        //              target is on same file              AND
        //              No piece on target tile
        //      THEN    return true
        if (targetPosition.x == Position.x)
        {
            if (targetPosition.y == (Position.y + _forwardMove) &&
                TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y] == null)
            {
                //Debug.Log("See normal move");
                returnBool = true;
            }
        }

        return returnBool;
    }

    //  Take vector2 representing move target location in TrackingHandler.pieceTracker
    //  Returns bool if given position of piece, the move follows the rules
    private bool IsValidFirstMoveDouble(Vector2 targetPosition)
    {
        bool returnBool = false;

        //  First move can be 2 squares forward
        //      If      first move                          AND
        //              target is two ranks forward         AND
        //              target is on same file              AND
        //              No piece in either 2 ranks forward
        //      THEN    return true
        if (FirstMove && 
            (targetPosition.y == (Position.y + (2 * _forwardMove))) &&
            targetPosition.x == Position.x &&
            TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y] == null &&
            !IsRangeMoveBlocked(targetPosition))
        {
            //Debug.Log("See double move");
            returnBool = true;
        }

        return returnBool;
    }

    //  Player is always at the bottom of the screen so forward is always position
    //  Bot is always at the top of the screen so forward is always negative
    //  Runs on Awake()
    private void SetForwardMove()
    {
        if (this.tag == GameController.BotTag) _forwardMove = -1;
        else if (this.tag == GameController.PlayerTag) _forwardMove = 1;
    }
}
