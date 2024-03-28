using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static int RandomSide() 
    {
        return Random.Range(0,2); 
    }
}
