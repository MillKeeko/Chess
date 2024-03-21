using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private bool firstMove = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool TakePiece(Vector3 targetPiecePosition)
    {
        if (KingMoves(targetPiecePosition) == true) 
        {
            //firstMove = false;
            return true;
        }
        else return false;
    }

    public override bool MovePiece(Vector3 targetSquarePosition)
    {
        if (KingMoves(targetSquarePosition) == true)
        {
            //firstMove = false;
            return true;
        }
        else return false;
    }

    public override void SetFirstMove(bool set)
    {
        firstMove = set;
    }

    // Return -1 for failure
    // Return 0 for king side castle
    // Return 1 for Queen side castle
    public int Castle (Vector3 target)
    {
        int pieceX = (int)transform.position.x;
        int pieceY = (int)transform.position.y;
        int targetX = (int)target.x;
        int targetY = (int)target.y;

        if (firstMove == true && pieceY == targetY)
        {
            if (targetX == pieceX + 2) return 0;
            else if (targetX == pieceX - 2) return 1;
        }

        return -1;
    }

    private bool KingMoves(Vector3 target)
    {
        int pieceX = (int)transform.position.x;
        int pieceY = (int)transform.position.y;
        int targetX = (int)target.x;
        int targetY = (int)target.y;

        if (targetX == pieceX + 1)
        {
            if (targetY == pieceY || targetY == pieceY + 1 || targetY == pieceY - 1) return true;
        }
        else if (targetX == pieceX)
        {
            if (targetY == pieceY + 1 || targetY == pieceY - 1) return true;
        }
        else if (targetX == pieceX - 1)
        {
            if (targetY == pieceY || targetY == pieceY + 1 || targetY == pieceY - 1) return true;
        }

        return false;
    }
}
