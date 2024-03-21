using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
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
        if (KnightMoves(targetPiecePosition) == true) return true;
        else return false;
    }

    public override bool MovePiece(Vector3 targetSquarePosition)
    {
        if (KnightMoves(targetSquarePosition) == true) return true;
        else return false;
    }

    private bool KnightMoves(Vector3 target)
    {
        int pieceX = (int)transform.position.x;
        int pieceY = (int)transform.position.y;
        int targetX = (int)target.x;
        int targetY = (int)target.y;

        if (targetX == pieceX - 2 || targetX == pieceX + 2)
        {
            if (targetY == pieceY + 1 || targetY == pieceY - 1) return true;
        }
        else if (targetY == pieceY - 2 || targetY == pieceY + 2)
        { 
            if (targetX == pieceX + 1 || targetX == pieceX - 1) return true;
        }

        return false;
    }
}
