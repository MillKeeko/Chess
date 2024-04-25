using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pieces
{
    King,
    Queen,
    Rook,
    Bishop,
    Knight,
    Pawn
}

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
