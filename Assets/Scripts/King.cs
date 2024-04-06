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
        Vector2 targetPosition;
        EmptyMovesList();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                targetPosition = new Vector2(x, y);
                if (IsValidKingMove(targetPosition))
                {
                    General.PossibleMove possibleMove = new General.PossibleMove(targetPosition, this);
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
        int targetX = (int)targetPosition.x;
        int targetY = (int)targetPosition.y;
        Piece target = TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y];

        if (target != null && target.CompareTag(this.tag))
        {
            returnBool = false;
        }
        else if (targetX == Position.x + 1)
        {
            if (targetY == Position.y || targetY == Position.y + 1 || targetY == Position.y - 1) returnBool = true;
        }
        else if (targetX == Position.x)
        {
            if (targetY == Position.y + 1 || targetY == Position.y - 1) returnBool = true;
        }
        else if (targetX == Position.x - 1)
        {
            if (targetY == Position.y || targetY == Position.y + 1 || targetY == Position.y - 1) returnBool = true;
        }

        return returnBool;
    }
}
