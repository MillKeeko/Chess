using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

//  We have 1 move list, the possible bot move list, for this specific turn
//  How does the bot decide which move is best? 
//  -   Assign a value to the board. 
//      -   Add all pieces' values + bonus for their position on the board
//  -   Iterate all available moves and attach total value
//  -   Make move which maximizes value

public class BotController : MonoBehaviour
{
    public delegate void OnTestBoardValue(Piece piece, Vector2 targetPosition);
    public static event OnTestBoardValue OnTestBoardValueEvent;
    public delegate void OnRevertTestBoardValue(Piece piece, Vector2 targetPosition);
    public static event OnRevertTestBoardValue OnRevertTestBoardValueEvent;

    void Awake()
    {
        GameController.OnBotMoveEvent += MakeMove;
    }

    private void MakeMove()
    {
        int maxBoardValue = 0;
        PossibleMove bestMove = GameController.PossibleBotMovesList[0]; // initializing to first move
        int testBoardValue = 0;
                
        foreach (PossibleMove move in GameController.PossibleBotMovesList)
        {
            OnTestBoardValueEvent?.Invoke(move.SelectedPiece, move.TargetPosition);

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = TrackingHandler.pieceTracker[row, col];
                    if (piece != null && piece.CompareTag(GameController.BotTag))
                    {
                        testBoardValue += piece.Value;
                        testBoardValue += PieceBonuses.GetPieceBonus(piece, row, col);
                    }
                }
            }

            Debug.Log(testBoardValue);

            if (testBoardValue > maxBoardValue) 
            {
                maxBoardValue = testBoardValue;
                bestMove = move;
            }

            testBoardValue = 0; // Reset test value

            OnRevertTestBoardValueEvent?.Invoke(move.SelectedPiece, move.TargetPosition);
        }

        MoveController.PrepareExecuteMove(bestMove.SelectedPiece, bestMove.TargetPosition);
    }   

    //  Only using this from now on when MakeMove is not working and need to test something unrelated
    private void MakeRandomMove()
    {
        int randomIndex = Random.Range(0, GameController.PossibleBotMovesList.Count);
        PossibleMove move = GameController.PossibleBotMovesList[randomIndex];

        Piece piece = move.SelectedPiece;
        Vector3 targetPosition = new Vector3(move.TargetPosition.x, move.TargetPosition.y, Constants.PIECE_Z_INDEX);

        MoveController.PrepareExecuteMove(piece, targetPosition);
    }
}
