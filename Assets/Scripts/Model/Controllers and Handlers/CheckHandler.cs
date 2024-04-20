using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHandler : MonoBehaviour
{
    public static CheckHandler instance { get; private set; }
    public static bool IsInCheck;

    public delegate void OnTestRemoveCheck(Piece piece, Vector2 targetPosition);
    public static event OnTestRemoveCheck OnTestRemoveCheckEvent;
    public delegate void OnRevertTestRemoveCheck(Piece piece, Vector2 targetPosition);
    public static event OnRevertTestRemoveCheck OnRevertTestRemoveCheckEvent;

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

        IsInCheck = false;
    }

    public static void SetIsInCheck()
    {
        IsInCheck = IsKingInCheck();
    }

    public static bool DoesMoveEndInCheck(Piece piece, Vector2 targetPosition)
    {
        bool returnBool = false;
        OnTestRemoveCheckEvent?.Invoke(piece, targetPosition);

        GameController.GenerateEnemyAttackList();
        returnBool = !IsKingInCheck();
        
        OnRevertTestRemoveCheckEvent?.Invoke(piece, targetPosition);

        return returnBool;
    }

    // On remove check .... this isn't using updated move lists 
    private static bool IsKingInCheck()
    {
        bool returnBool = false;
        Vector2 kingPosition = FindKing();
        
        foreach (PossibleMove attack in GameController.PossibleEnemyAttackList)
        {
            if ((int)attack.TargetPosition.x == (int)kingPosition.x && 
                (int)attack.TargetPosition.y == (int)kingPosition.y)
            {
                returnBool = true;
                break;
            }
        }

        return returnBool;
    }

    private static Vector2 FindKing()
    {
        Vector2 kingPosition = new Vector2(-1,-1);

        Piece piece = null;

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file <8; file++)
            {
                piece = TrackingHandler.pieceTracker[file, rank];
                if (piece is King && piece.CompareTag(GameController.Turn))
                {
                    kingPosition.x = file;
                    kingPosition.y = rank;
                    return kingPosition;
                }
            }
        }

        return kingPosition;
    }
}
