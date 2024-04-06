using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  MoveValidator
//      IsNormalBishopMove
public class Bishop : Piece
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    //  Override Methods
    //

    public override void GeneratePossibleMoves()
    {
        Vector2 targetPosition;
        EmptyMovesList();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                targetPosition = new Vector2(x, y);
                if (IsValidBishopMove(targetPosition))
                {
                    General.PossibleMove possibleMove = new General.PossibleMove(targetPosition, this);
                    PossibleMovesList.Add(possibleMove);
                    //Debug.Log(this + " from x " + this.transform.position.x + " y " + this.transform.position.y + " to x " + targetPosition.x + " y " + targetPosition.y);
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
    private bool IsValidBishopMove(Vector2 targetPosition)
    {
        bool returnBool = false;
        Piece target = TrackingHandler.pieceTracker[(int)targetPosition.x, (int)targetPosition.y];

        // if the difference between the x == the difference between the y, or the negative of that difference
        if ((targetPosition.x - transform.position.x == targetPosition.y - transform.position.y ||
            targetPosition.x - transform.position.x == -1 * (targetPosition.y - transform.position.y)) &&
            (target == null || !target.CompareTag(this.tag)) &&
            !IsRangeMoveBlocked(targetPosition))
        {
            returnBool = true;
        }
        
        return returnBool;
    }
}
