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

    public override void MoveAttempt(Vector2 targetPosition)
    {
        //Debug.Log("MoveAttempt Start.");
        if (IsValidKnightMove(targetPosition))
        {
            MoveExecutor(targetPosition);
        }  
        
    }

    public override void GeneratePossibleMoves()
    {
        Vector3 targetPosition;
        EmptyMovesList();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                targetPosition = new Vector3(x, y, Constants.PIECE_Z_INDEX);
                if (IsValidKnightMove(targetPosition))
                {
                    General.PossibleMove possibleMove = new General.PossibleMove(x, y, this);
                    PossibleMovesList.Add(possibleMove);
                    //Debug.Log(this + " from x " + this.transform.position.x + " y " + this.transform.position.y + " to x " + targetPosition.x + " y " + targetPosition.y);  
                }
            }
        }
        //Debug.Log(this + " has " + possiblePieceMovesList.Count + " possible moves.");
    }

    private bool IsValidKnightMove(Vector2 targetPosition)
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
        else if (targetX == pieceX - 2 || targetX == pieceX + 2)
        {
            if (targetY == pieceY + 1 || targetY == pieceY - 1) returnBool = true;
        }
        else if (targetY == pieceY - 2 || targetY == pieceY + 2)
        { 
            if (targetX == pieceX + 1 || targetX == pieceX - 1) returnBool = true;
        }

        return returnBool;
    }
}
