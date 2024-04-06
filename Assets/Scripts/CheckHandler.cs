using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckHandler : MonoBehaviour
{
    public static bool isKingInCheck;

    //  Delegates
    public delegate void OnDoesMoveRemoveCheck ();

    //  Events
    public static event OnDoesMoveRemoveCheck onDoesMoveRemoveCheck;

    // Start is called before the first frame update
    void Start()
    {
        GameController.onChangeTurn += SetIsKingInCheck;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool DoesMoveRemoveCheck(General.PossibleMove move)
    {
        bool returnBool = false;
        Vector3 targetPosition = new Vector3(move.X, move.Y, Constants.PIECE_Z_INDEX);
        List<Piece> pieceList = new List<Piece>();
        List<General.PossibleMove> possibleMoveList = new List<General.PossibleMove>();
        string enemyTag = General.GetEnemyTag();
        int oldX = (int)move.MovePiece.transform.position.x;
        int oldY = (int)move.MovePiece.transform.position.y;
        Vector3  originalPosition = new Vector3 (oldX, oldY, Constants.PIECE_Z_INDEX);
        
        // Temporarily move piece for calculations
        move.MovePiece.transform.position = targetPosition;

        onDoesMoveRemoveCheck?.Invoke();

        returnBool = IsKingInCheck(GameController.turn);

        // Move piece back
        move.MovePiece.transform.position = originalPosition;

        return returnBool; 
    }

    // See if the king is in check of a specific side (white or black)
    public static bool IsKingInCheck(string tag)
    {
        bool returnBool = false;
        List<General.PossibleMove> possibleMoveList = new List<General.PossibleMove>();
        List<Piece> pieceList = new List<Piece>();
        Piece king = FindKing(tag);
        string enemyTag = General.GetEnemyTag();

        // Generate Enemy possible moves and find King
        pieceList = General.GeneratePieceList(enemyTag);
        possibleMoveList = General.CompilePossibleMoves(pieceList);

        foreach (General.PossibleMove move in possibleMoveList)
        {
            if (move.X == king.transform.position.x && move.Y == king.transform.position.y)
            {
                returnBool = true;
                break;
            }
        }

        Debug.Log("King is in Check? " + tag + " " + returnBool);

        return returnBool;
    }

    private void SetIsKingInCheck(string tag)
    {
        isKingInCheck = IsKingInCheck(tag);
    }

    // Find the king of a specific side (white or black)
    private static Piece FindKing(string tag)
    {
        King[] kingArray = GameObject.FindObjectsOfType<King>();
        Piece king = null;

        foreach (King kingCheck in kingArray)
        {
            if (kingCheck.CompareTag(tag))
            {
                king = kingCheck;
                break;
            }
        }

        return king;
    }
}
