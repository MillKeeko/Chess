using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class BotController : MonoBehaviour
{
    private Piece[,] _botPieceTracker;

    private Piece _pieceRemovedTest = null;

    void Awake()
    {
        _botPieceTracker = new Piece [8,8];

        TrackingHandler.OnTrackerReadyEvent += SetupTracker;

        MoveController.OnMoveEvent += UpdateTracker;

        GameController.OnBotMoveEvent += MakeMove;
    }

     private void RevertTestPosition(Piece piece, Vector2 targetPosition)
    {
        _botPieceTracker[(int)targetPosition.x, (int)targetPosition.y] = _pieceRemovedTest;
        _botPieceTracker[(int)piece.Position.x, (int)piece.Position.y] = piece;
        _pieceRemovedTest = null;
        piece.BotTestPosition = piece.Position;
    }

    private void TestPosition(Piece piece, Vector2 targetPosition)
    {
        _botPieceTracker[(int)piece.Position.x, (int)piece.Position.y] = null;
        _pieceRemovedTest = _botPieceTracker[(int)targetPosition.x, (int)targetPosition.y];
        _botPieceTracker[(int)targetPosition.x, (int)targetPosition.y] = piece;
        piece.BotTestPosition = targetPosition;
    }

    private void UpdateTracker(Piece piece, Vector2 targetPosition)
    {
        _botPieceTracker[(int)piece.Position.x, (int)piece.Position.y] = null;
        _botPieceTracker[(int)targetPosition.x, (int)targetPosition.y] = piece;
        piece.BotTestPosition = targetPosition;
    }

    private void SetupTracker()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Vector2 targetPosition = new Vector2(rank, file);
                _botPieceTracker[rank,file] = TrackingHandler.pieceTracker[rank,file];

                if (_botPieceTracker[rank,file] != null)
                {
                    _botPieceTracker[rank,file].BotTestPosition = targetPosition;
                }
            }
        }
    }

    private void MakeMove()
    {
        int maxBoardValue = 0;
        int testBoardValue = 0;
        List<PossibleMove> bestMoveList = new List<PossibleMove>();
                
        foreach (PossibleMove move in GameController.PossibleBotMovesList)
        {
            TestPosition(move.SelectedPiece, move.TargetPosition);

            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    Piece piece = _botPieceTracker[rank, file];
                    if (piece != null && piece.CompareTag(GameController.BotTag))
                    {
                        testBoardValue += piece.Value;
                        testBoardValue += PieceBonuses.GetPieceBonus(piece, rank, file);
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

            RevertTestPosition(move.SelectedPiece, move.TargetPosition);
        }

        Debug.Log("Bot board value = " + maxBoardValue);

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
