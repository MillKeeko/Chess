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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    //  Override methods
    //

    public override void MoveAttempt(Vector2 targetPosition)
    {
        if (IsValidForwardMove(targetPosition) || 
            IsValidDiagonalAttack(targetPosition) || 
            IsValidFirstMoveDouble(targetPosition))
        {
            MoveExecutor(targetPosition);
        }   
    }

    public override void GeneratePossibleMoves()
    {
        Vector2 targetPosition;
        EmptyMovesList();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                targetPosition = new Vector2(x, y);
                
                if (IsValidForwardMove(targetPosition) || 
                    IsValidDiagonalAttack(targetPosition) || 
                    IsValidFirstMoveDouble(targetPosition))
                {
                    General.PossibleMove possibleMove = new General.PossibleMove(targetPosition, this);
                    PossibleMovesList.Add(possibleMove);
                }
            }
        }
        //Debug.Log(this + " has " + possiblePieceMovesList.Count + " possible moves.");
    }

    //
    //  Private Methods
    //

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
        if (GetFirstMove() && 
            (targetPosition.y == (Position.y + (2 * _forwardMove))) &&
            targetPosition.x == Position.x &&
            TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y] == null)
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
