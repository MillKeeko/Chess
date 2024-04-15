using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public static MoveController instance { get; private set; }

    public delegate void OnMove(Piece piece, Vector2 targetPosition);
    public static event OnMove OnMoveEvent;
    public delegate void OnEnPassant(Piece piece, Vector2 targetPosition);
    public static event OnEnPassant OnEnPassantEvent;


    public static bool IsEnPassantPossible { get; private set; }
    public static Vector2 EnPassantTargetPosition { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            enabled = false;
        }
        else
        {
            instance = this;
        }

        IsEnPassantPossible = false;
        EnPassantTargetPosition = new Vector2 (-1, -1);
    }

    public static void PrepareExecuteMove(Piece piece, Vector2 targetPosition)
    {
        if (IsCastleMove(piece, targetPosition)) MoveRookForCastle(piece, targetPosition);
        TriggerMoveExecution(piece, targetPosition);
    }

    private static void TriggerMoveExecution (Piece piece, Vector2 targetPosition)
    {
        if (piece.FirstMove) piece.FirstMove = false;
        SetIsEnPassantVariables(piece, targetPosition);
        BoardController.ExecuteMove(piece, targetPosition);
        if (IsEnPassantMove(piece, targetPosition)) OnEnPassantEvent?.Invoke(piece, targetPosition);
        else OnMoveEvent?.Invoke(piece, targetPosition);
    }

    private static bool IsEnPassantMove(Piece piece, Vector2 targetPosition)
    {
        if (piece is Pawn && targetPosition == EnPassantTargetPosition) return true;
        else return false;
    }

    private static void SetIsEnPassantVariables(Piece piece, Vector2 targetPosition)
    {
        if (piece is Pawn)
        {
            if (piece.Position.y - targetPosition.y == 2)
            {
                IsEnPassantPossible = true;
                Debug.Log("EnPassant possible.");
                EnPassantTargetPosition = new Vector2 (piece.Position.x, piece.Position.y - 1);
            }
            else if (piece.Position.y - targetPosition.y == -2)
            {
                IsEnPassantPossible = true;
                Debug.Log("EnPassant possible.");
                EnPassantTargetPosition = new Vector2 (piece.Position.x, piece.Position.y + 1);
            }
            else
            {
                IsEnPassantPossible = false;
                Debug.Log("EnPassant impossible.");
            }
        }
        else 
        {
            IsEnPassantPossible = false;
            Debug.Log("EnPassant impossible.");
        }
    }

    private static void MoveRookForCastle(Piece piece, Vector2 targetPosition)
    {
        int rookFile;
        int rookTargetFile;
        if (targetPosition.x > piece.Position.x) 
        {
            rookFile = 7;
            if (targetPosition.x == 6) rookTargetFile = 5;
            else rookTargetFile = 4;
        }
        else 
        {
            rookFile = 0;
            if (targetPosition.x == 1) rookTargetFile = 2;
            else rookTargetFile = 3;
        }
        
        Piece rook = TrackingHandler.pieceTracker[rookFile, (int)piece.Position.y];
        Vector2 rookTargetPosition = new Vector2(rookTargetFile, piece.Position.y);
        TriggerMoveExecution(rook, rookTargetPosition);
    }

    private static bool IsCastleMove(Piece piece, Vector2 targetPosition)
    {
        bool returnBool = false;
        if (piece is King && 
            Mathf.Abs((int)piece.Position.x - (int)targetPosition.x) == Mathf.Abs(2))
        {
            returnBool = true;
        }
        return returnBool;
    }
}
