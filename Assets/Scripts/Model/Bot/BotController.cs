using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

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
        int testBoardValue = 0;
        List<PossibleMove> bestMoveList = new List<PossibleMove>();
                
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

            if (testBoardValue > maxBoardValue) 
            {
                bestMoveList.Clear();
                bestMoveList.Add(move);
                maxBoardValue = testBoardValue;
            }
            else if (testBoardValue == maxBoardValue)
            {
                bestMoveList.Add(move);
            }

            testBoardValue = 0; // Reset test value

            OnRevertTestBoardValueEvent?.Invoke(move.SelectedPiece, move.TargetPosition);
        }

        Debug.Log("Black board value = " + maxBoardValue);

        MakeRandomMove(bestMoveList);
    }   

    //  Intended to replace MakeMove()
    private void MakeBetterMove()
    {
        
    }

    private void SearchMoves()
    {
        //  Generate Player response Moves
    }

    private void MakeRandomMove(List<PossibleMove> moveList)
    {
        int randomIndex = Random.Range(0, moveList.Count);
        PossibleMove move = moveList[randomIndex];

        Debug.Log("Best move list count = " + moveList.Count);

        Piece piece = move.SelectedPiece;
        Vector2 targetPosition = new Vector3(move.TargetPosition.x, move.TargetPosition.y);

        MoveController.PrepareExecuteMove(piece, targetPosition);
    }
}
