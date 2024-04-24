using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

//  Purpose of this class is to be used both by GameController and BotController.
//  -   GameController will use it to generate possible move for the player
//  -   BotController will use it to search many moves into the future and return best move

public class MoveGenerator : MonoBehaviour
{
    //
    //  Public Methods
    //

    //  Input
    //      -   Piece[8,8], representation of the board
    //      -   String, tag represententing white or black
    //  Output
    //      -   List<PossibleMove>, all possible moves for either white or black
    //  Description
    //      -   Used to output a list of possible moves given a specific board position
    public static List<PossibleMove> CompilePossibleMoves(Piece[,] boardPosition, string tag)
    {
        List<Piece> pieceList = GeneratePieceList(boardPosition, tag);
        List<PossibleMove> possibleMoveList = new List<PossibleMove>();

        foreach (Piece piece in pieceList)
        {
            piece.GeneratePossibleMoves(boardPosition);
            possibleMoveList.AddRange(piece.PossibleMovesList);
        }

        //Debug.Log("PossibleMove count " + possibleMoveList.Count);
        return possibleMoveList;
    }

    //
    //  Private Methods
    //

    //  Input
    //      -   Piece[8,8], representation of the board
    //      -   String, tag represententing white or black
    //  Output
    //      -   List<Piece>, list of pieces on the board
    //  Description
    //      -   Used as a helper function for CompilePossibleMoves()
    private static List<Piece> GeneratePieceList(Piece[,] boardPosition, string tag)
    {
        List<Piece> pieceList = new List<Piece>();

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Piece piece = boardPosition[rank, file];
                if (piece != null && piece.CompareTag(tag))
                {
                    pieceList.Add(piece);
                }
            }
        }

        //Debug.Log("Piece list count " + pieceList.Count);
        return pieceList;
    } 
}
