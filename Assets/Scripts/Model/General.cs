using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Model

public class General
{
    public struct PossibleMove
    {
        public int X;
        public int Y;
        public Piece MovePiece;

        // Constructor
        public PossibleMove (int x, int y, Piece movePiece)
        {
            X = x;
            Y = y;
            MovePiece = movePiece;
        }
    };

    public static List<PossibleMove> CompilePossibleMoves(List<Piece> pieceList)
    {
        List<PossibleMove> possibleMoveList = new List<PossibleMove>();

        foreach (Piece piece in pieceList)
        {
            possibleMoveList.AddRange(piece.PossibleMovesList);
        }
        return possibleMoveList;
    }

    public static List<Piece> GeneratePieceList(string tag)
    {
        Piece[] pieceArray = GameObject.FindObjectsOfType<Piece>();
        List<Piece> pieceList = new List<Piece>();

        foreach (Piece piece in pieceArray)
        {
            if (piece.CompareTag(tag))
            {
                pieceList.Add(piece);
            }
        }
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
