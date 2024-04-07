using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Model

public class General
{
    public struct PossibleMove
    {
        public Vector2 TargetPosition;
        public Piece SelectedPiece;

        // Constructor
        public PossibleMove (Vector2 targetPosition, Piece selectedPiece)
        {
            TargetPosition = targetPosition;
            SelectedPiece = selectedPiece;
        }
    };

    public static List<PossibleMove> CompilePossibleMoves(List<Piece> pieceList)
    {
        List<PossibleMove> possibleMoveList = new List<PossibleMove>();

        foreach (Piece piece in pieceList)
        {
            possibleMoveList.AddRange(piece.PossibleMovesList);
        }

        //Debug.Log("Possible bot move list length " + possibleMoveList.Count);
        return possibleMoveList;
    }

    public static List<Piece> GeneratePieceList(string tag)
    {
        Piece piece = null;
        List<Piece> pieceList = new List<Piece>();

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                piece = TrackingHandler.pieceTracker[rank, file];
                if (piece != null && piece.CompareTag(tag))
                {
                    pieceList.Add(piece);
                }

            }
        }
        //Debug.Log("Bot piece list length = " + pieceList.Count);
        return pieceList;
    } 

    public static string GetEnemyTag()
    {
        string enemyTag = null;

        if (GameController.Turn == Constants.WHITE_TAG) enemyTag = Constants.BLACK_TAG;
        else enemyTag = Constants.WHITE_TAG;

        return enemyTag;    
    }
}
