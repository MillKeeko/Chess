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

    public static void ShowAttackingSquares()
    {
        List<General.PossibleMove> enemyMoveList = new List<General.PossibleMove>();
        SpriteRenderer renderer;

        if (GameController.Turn == GameController.BotTag) enemyMoveList = GameController.PossiblePlayerMovesList;
        else enemyMoveList = GameController.PossibleBotMovesList;
        Debug.Log("Number of enemy moves " + enemyMoveList.Count);

        Square[] allSquares = FindObjectsOfType<Square>();
        Debug.Log("Number of squares " + allSquares.Length);

        foreach (Square square in allSquares)
        {
            foreach (General.PossibleMove move in enemyMoveList)
            {
                Debug.Log("Square position x " + square.Position.x + " y " + square.Position.y);
                //  Found square that is attacked
                if (move.TargetPosition.x == square.Position.x && move.TargetPosition.y == square.Position.y)
                {
                    Debug.Log("Lighting up square.");
                    renderer = square.GetComponent<SpriteRenderer>();
                    renderer.color = Color.red;
                }
                break;
            }
        }
    }

}
