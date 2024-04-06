using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  MoveValidator
//      IsNormalKingMove
//      IsCastle
public class King : Piece
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GeneratePossibleMoves()
    {
        Vector3 targetPosition;
        EmptyMovesList();
        Debug.Log("Generating moves for " + this.tag);

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                targetPosition = new Vector3(x, y, Constants.PIECE_Z_INDEX);
                if (IsValidKingMove(targetPosition))
                {
                    General.PossibleMove possibleMove = new General.PossibleMove(x, y, this);
                    PossibleMovesList.Add(possibleMove);
                    //Debug.Log(this + " from x " + this.transform.position.x + " y " + this.transform.position.y + " to x " + targetPosition.x + " y " + targetPosition.y);   
                }
            }
        }
        //Debug.Log(this + " has " + possiblePieceMovesList.Count + " possible moves.");
    }
    
    public override void MoveAttempt(Vector2 targetPosition)
    {
        //Debug.Log("MoveAttempt Start.");
        if (IsValidKingMove(targetPosition)) MoveExecutor(targetPosition);
    }

    private bool IsValidKingMove(Vector2 targetPosition)
    {
        bool returnBool = false;
        int pieceX = (int)transform.position.x;
        int pieceY = (int)transform.position.y;
        int targetX = (int)targetPosition.x;
        int targetY = (int)targetPosition.y;
        Piece target = TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y];

        if (target != null && target.CompareTag(this.tag))
        {
            returnBool = false;
        }
        else if (targetX == pieceX + 1)
        {
            if (targetY == pieceY || targetY == pieceY + 1 || targetY == pieceY - 1) returnBool = true;
        }
        else if (targetX == pieceX)
        {
            if (targetY == pieceY + 1 || targetY == pieceY - 1) returnBool = true;
        }
        else if (targetX == pieceX - 1)
        {
            if (targetY == pieceY || targetY == pieceY + 1 || targetY == pieceY - 1) returnBool = true;
        }

        return returnBool;
    }
}
