using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  View
//  Not so important, and not yet implemented. Script does not work

public class SquareHighlighter : MonoBehaviour
{
    public static Vector2 LastPiecePosition; // Used for last move highlighting
    public static Vector2 LastMoveTarget;

    void Awake()
    {
        LastPiecePosition = new Vector2 (-1, -1);
        LastMoveTarget = new Vector2 (-1, -1);
    }

    public static void SetSquaresDefault()
    {
        Square[] allSquares = FindObjectsOfType<Square>();

        for (int i = 0; i < 64; i++)
        {
            Square square = allSquares[i];
            SpriteRenderer renderer = square.GetComponent<SpriteRenderer>();
            if (square.Position == LastPiecePosition || 
                square.Position == LastMoveTarget)
            {
                renderer.color = Color.green;
            }
            else
            {
                square.SetDefaultColour();
            }
        }
    }

    public static void ShowMoveSquares(Piece piece)
    {
        Square[] allSquares = FindObjectsOfType<Square>();

        for (int i = 0; i < 64; i++)
        {
            Square square = allSquares[i];
            SpriteRenderer renderer = square.GetComponent<SpriteRenderer>();

            foreach (PossibleMove move in piece.PossibleMovesList)
            {
                if (move.SelectedPiece == piece &&
                    (int)move.TargetPosition.x == (int)square.Position.x && 
                    (int)move.TargetPosition.y == (int)square.Position.y)
                {
                    if (renderer.color == Color.red) renderer.color = Color.blue;
                    else if (renderer.color == Color.blue) renderer.color = Color.blue;
                    else renderer.color = Color.yellow;
                    break;
                }
                else if (square.Position == LastPiecePosition || 
                         square.Position == LastMoveTarget)
                {
                    renderer.color = Color.green;
                }
                /*else if (renderer.color != Color.red && renderer.color != Color.blue)
                {
                    square.SetDefaultColour();
                }*/
            }
        }
    }

    /*public static void ShowAttackingSquares(List<PossibleMove> attackList)
    {
        //Debug.Log("Number of enemy moves " + attackList.Count);

        Square[] allSquares = FindObjectsOfType<Square>();
        //Debug.Log("Number of squares " + allSquares.Length);

        for (int i = 0; i < 64; i++)
        {
            Square square = allSquares[i];
            //Debug.Log("Square position x " + square.Position.x + " y " + square.Position.y);
            foreach (PossibleMove move in attackList)
            {
                SpriteRenderer renderer = square.GetComponent<SpriteRenderer>();
                //  Found square that is attacked
                if ((int)move.TargetPosition.x == (int)square.Position.x && (int)move.TargetPosition.y == (int)square.Position.y)
                {
                    //Debug.Log(move.SelectedPiece + " at x " + move.SelectedPiece.Position.x + " y " + move.SelectedPiece.Position.y + " is attacking x " + move.TargetPosition.x + " y " + move.TargetPosition.y);
                    renderer.color = Color.red;
                    break;
                }
                else
                {
                    square.SetDefaultColour();
                }
            }
        }
    }*/
}
