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

    public static List<General.PossibleMove> CompilePossibleMoves(string tag)
    {
        List<Piece> pieceList = GeneratePieceList(tag);
        List<General.PossibleMove> possibleMoveList = new List<General.PossibleMove>();

        foreach (Piece piece in pieceList)
        {
            piece.GeneratePossibleMoves();
            possibleMoveList.AddRange(piece.PossibleMovesList);
        }

        //Debug.Log("PossibleMove count " + possibleMoveList.Count);
        return possibleMoveList;
    }

    public static List<Piece> GeneratePieceList(string tag)
    {
        List<Piece> pieceList = new List<Piece>();

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Piece piece = TrackingHandler.pieceTracker[rank, file];
                if (piece != null && piece.CompareTag(tag))
                {
                    pieceList.Add(piece);
                }
            }
        }

        //Debug.Log("Piece list count " + pieceList.Count);
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
