using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  BEGINNING OF CHECK ACTUALLY WORKING!!!!!!
//  Known bugs:
//      -   King can move itself to "block" check from itself
//          -   Interesting that this is only true for the bot.... hmm...
//      -   Player can put themselves into check
//      -   

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        if (piece is King && piece.CompareTag(GameController.PlayerTag)) Debug.Log("TESTING KING CHECK");
        returnBool = !IsKingInCheck();
        if (piece is King && piece.CompareTag(GameController.PlayerTag)) Debug.Log("REMOVES CHECK: " + returnBool);

        OnRevertTestRemoveCheckEvent?.Invoke(piece, targetPosition);

        return returnBool;
    }

    // On remove check .... this isn't using updated move lists 
    private static bool IsKingInCheck()
    {
        bool returnBool = false;
        Vector2 kingPosition = FindKing();
        
        foreach (General.PossibleMove attack in GameController.PossibleEnemyAttackList)
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
                    if (piece.CompareTag(GameController.PlayerTag)) Debug.Log("Player King Position x " + kingPosition.x + " y " + kingPosition.y);
                    return kingPosition;
                }
            }
        }

        return kingPosition;
    }
}
