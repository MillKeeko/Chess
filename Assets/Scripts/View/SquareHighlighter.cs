using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  View
//  Not so important, and not yet implemented. Script does not work

public class SquareHighlighter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ShowMoveSquares(Piece piece, List<General.PossibleMove> moveList)
    {
        Square[] allSquares = FindObjectsOfType<Square>();

        for (int i = 0; i < 64; i++)
        {
            Square square = allSquares[i];
            foreach (General.PossibleMove move in moveList)
            {
                SpriteRenderer renderer = square.GetComponent<SpriteRenderer>();
                if (move.SelectedPiece == piece &&
                    (int)move.TargetPosition.x == (int)square.Position.x && 
                    (int)move.TargetPosition.y == (int)square.Position.y)
                {
                    if (renderer.color == Color.red)
                    {
                        renderer.color = Color.blue;
                    }
                    else renderer.color = Color.yellow;
                    break;
                }
                else if (renderer.color != Color.red)
                {
                    square.SetDefaultColour();
                }
            }
        }
    }

    public static void ShowAttackingSquares(List<General.PossibleMove> attackList)
    {
        //Debug.Log("Number of enemy moves " + attackList.Count);

        Square[] allSquares = FindObjectsOfType<Square>();
        //Debug.Log("Number of squares " + allSquares.Length);

        for (int i = 0; i < 64; i++)
        {
            Square square = allSquares[i];
            //Debug.Log("Square position x " + square.Position.x + " y " + square.Position.y);
            foreach (General.PossibleMove move in attackList)
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
    }
}
