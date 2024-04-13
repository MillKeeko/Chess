using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public static MoveController instance { get; private set; }

    public delegate void OnMove(Piece piece, Vector2 targetPosition);
    public static event OnMove OnMoveEvent;

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
    }

    public static void PrepareExecuteMove(Piece piece, Vector2 targetPosition)
    {
        if (piece.FirstMove) piece.FirstMove = false;
        if (IsCastleMove(piece, targetPosition)) MoveRookForCastle(piece, targetPosition);
        BoardController.ExecuteMove(piece, targetPosition);
        OnMoveEvent?.Invoke(piece, targetPosition);
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
        BoardController.ExecuteMove(rook, rookTargetPosition);
        OnMoveEvent?.Invoke(rook, rookTargetPosition);
    }

    private static bool IsCastleMove(Piece piece, Vector2 targetPosition)
    {
        bool returnBool = false;
        if (piece is King && 
            Mathf.Abs((int)piece.Position.x - (int)targetPosition.x) == Mathf.Abs(2))
        {
            returnBool = true;
        }
        Debug.Log("IsCastleMove " + returnBool);
        return returnBool;
    }
}
