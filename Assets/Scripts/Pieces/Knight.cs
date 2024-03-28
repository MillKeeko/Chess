using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  MoveValidator
//      IsNormalKnightMove
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

    public override void MoveAttempt(Vector3 targetPosition)
    {
        //Debug.Log("MoveAttempt Start.");
        if (IsValidNormalKnightMove(targetPosition))
        {
            MoveExecutor(targetPosition);
        }  
    }

    public override List<General.PossibleMove> GeneratePossibleMoves()
    {
        Vector3 targetPosition;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                targetPosition = new Vector3(x, y, Constants.PIECE_Z_INDEX);
                if (IsValidNormalKnightMove(targetPosition))
                {
                    General.PossibleMove possibleMove = new General.PossibleMove(x, y, this);
                    possiblePieceMovesList.Add(possibleMove);
                    //Debug.Log(this + " from x " + this.transform.position.x + " y " + this.transform.position.y + " to x " + targetPosition.x + " y " + targetPosition.y);
                }
            }
        }

        //Debug.Log(this + " has " + possiblePieceMovesList.Count + " possible moves.");
        return possiblePieceMovesList;
    }

    private bool IsValidNormalKnightMove(Vector3 target)
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
