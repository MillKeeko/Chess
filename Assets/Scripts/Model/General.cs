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
}
