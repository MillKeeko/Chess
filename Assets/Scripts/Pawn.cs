using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool firstMove = true;
    private int forwardMove = 1;
    private const int zIndex = -1;

    private int forwardDirection(Piece piece)
    {
        string tag = this.tag;

        if (tag == "PieceBlack")
        {
            forwardMove = -1;
        }
        else if (tag == "PieceWhite")
        {
            forwardMove = 1;
        }

        return forwardMove;
    }

    public override bool TakePiece(Vector3 targetPiecePosition)
    {
        forwardMove = forwardDirection(this);
        
        if (targetPiecePosition.x == transform.position.x + 1 || targetPiecePosition.x == transform.position.x -1)
        {
            if (targetPiecePosition.y == transform.position.y + forwardMove)
            {
                //firstMove = false;
                return true;
            }
        }

        return false;
    }

    public override void SetFirstMove(bool set)
    {
        firstMove = set;
    }

    public override bool MovePiece(Vector3 targetSquarePosition)
    {
        forwardMove = forwardDirection(this);

        if (targetSquarePosition.x == transform.position.x)
        {
            // Move 1 square forward
            if (targetSquarePosition.y == (transform.position.y + forwardMove))
            {
                //firstMove = false;
                return true;
            }
            // First move can be 2 squares forward
            else if (firstMove && (targetSquarePosition.y == (transform.position.y + (2 * forwardMove))))
            {
                firstMove = false;
                return true;
            }
        }

        return false;
    }
}
