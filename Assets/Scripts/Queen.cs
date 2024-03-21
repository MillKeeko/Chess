using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
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
        if (QueenMoves(targetPiecePosition) == true) return true;
        return false;
    }

    public override bool MovePiece(Vector3 targetSquarePosition)
    {
        if (QueenMoves(targetSquarePosition) == true) return true;
        return false;
    }

    private bool QueenMoves(Vector3 newPosition)
    {
        // if the difference between the x == the difference between the y, or the negative of that difference
        if (newPosition.x == transform.position.x  || newPosition.y == transform.position.y)
        {
            return true;
        }
        else if (newPosition.x - transform.position.x == newPosition.y - transform.position.y ||
                 newPosition.x - transform.position.x == -1 * (newPosition.y - transform.position.y))
        {
            return true;
        }
        else return false;
    }
}
