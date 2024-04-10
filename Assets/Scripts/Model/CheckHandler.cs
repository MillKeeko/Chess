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
    public static Piece[,] RemoveCheckTracker;
    public static bool IsInCheck { get; private set; }

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

        RemoveCheckTracker = new Piece [8,8];
        IsInCheck = false;
        GameController.OnSetIsInCheckEvent += SetIsInCheck;
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
        //if (IsInCheck) Debug.Log(GameController.Turn + " is in check.");
        //else Debug.Log(GameController.Turn + " is not in check.");
    }

    //  Everything seems to be working down to this function. 
    //   
    public static bool DoesMoveRemoveCheck(Piece piece, Vector2 targetPosition)
    {
        //Debug.Log(piece + " DoesMoveRemoveCheck() to x " + targetPosition.x + " y " + targetPosition.y);
        bool returnBool = false;
        //FillRemoveCheckTracker(piece, targetPosition);
        OnTestRemoveCheckEvent?.Invoke(piece, targetPosition);

        GameController.GenerateEnemyTestMovesList();
        returnBool = !IsKingInCheck();

        OnRevertTestRemoveCheckEvent?.Invoke(piece, targetPosition);

        //if (returnBool) Debug.Log("Move removes check.");
        //else Debug.Log("Move does not remove check."); 

        return returnBool;
    }

    // On remove check .... this isn't using updated move lists 
    private static bool IsKingInCheck()
    {
        bool returnBool = false;
        string enemyTag = General.GetEnemyTag();
        List<General.PossibleMove> possibleEnemyMoveList = new List<General.PossibleMove>();
        Vector2 kingPosition = FindKing();

        if (GameController.Turn == GameController.BotTag) possibleEnemyMoveList = GameController.PossiblePlayerMovesList;
        else possibleEnemyMoveList = GameController.PossibleBotMovesList;

        //Debug.Log("Enemy is " + enemyTag + " and MoveList is count " + possibleEnemyMoveList.Count);
        
        foreach (General.PossibleMove move in possibleEnemyMoveList)
        {
            //Debug.Log("Target x " + move.TargetPosition.x + " king x " + kingPosition.x + " target y " + move.TargetPosition.y + " king y " + kingPosition.y);
            if (move.TargetPosition.x == kingPosition.x && move.TargetPosition.y == kingPosition.y)
            {
                //Debug.Log("King in check from " + move.SelectedPiece + " to king position x " + kingPosition.x + " y " + kingPosition.y);
                returnBool = true;
                break;
            }
            else 
            {
                //Debug.Log("King not in check when " + move.SelectedPiece + " and king position is x " + kingPosition.x + " y " + kingPosition.y);
            }
        }

        return returnBool;
    }

    private static Vector2 FindKing()
    {
        Vector2 kingPosition = new Vector2(-1,-1);

        /*foreach (Piece piece in TrackingHandler.pieceTracker)
        {
            if (piece is King && piece.CompareTag(GameController.Turn))
            {
                kingPosition = new Vector2(piece.Position.x, piece.Position.y);
                //Debug.Log(piece + " is king at x " + kingPosition.x + " y " + kingPosition.y);
                break;
            }
        }*/

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
                    Debug.Log("King Position x " + kingPosition.x + " y " + kingPosition.y);
                    break;
                }
            }
        }

        return kingPosition;
    }

}
