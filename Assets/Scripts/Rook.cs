using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
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

    public override bool GetFirstMove()
    {
        return firstMove;
    }

    public override bool TakePiece(Vector3 targetPiecePosition)
    {
        if (RookMoves(targetPiecePosition) == true) return true;
        return false;
    }

    public override bool MovePiece(Vector3 targetSquarePosition)
    {
        if (RookMoves(targetSquarePosition) == true) return true;
        return false;
    }

    public override void SetFirstMove(bool set)
    {
        firstMove = set;
    }

    private bool RookMoves(Vector3 newPosition)
    {
        // if the difference between the x == the difference between the y, or the negative of that difference
        if (newPosition.x == transform.position.x  || newPosition.y == transform.position.y)
        {
            return true;
        }
        else return false;
    }
}
