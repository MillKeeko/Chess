using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHandler : MonoBehaviour
{
    private Piece[,] tempPieceTracker;
    public static bool isKingInCheck = false;

    void Awake()
    {
        tempPieceTracker = new Piece [8,8];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RemoveFromTempTracker(int x, int y)
    {
        tempPieceTracker[x,y] = null;
    }

    private void AddToTempTracker(Piece piece, int x, int y)
    {
        tempPieceTracker[x,y] = piece;
    }
    
    // This is done differently than the bot, I wonder which is better???
    public static void IsKingInCheck()
    {
        Vector3 kingPosition = FindKing();
        Piece enemyPiece = null;
        List<General.PossibleMove> possibleEnemyPieceMovesList = new List<General.PossibleMove>();

        //Debug.Log("Checking king in check at x " + kingPosition.x + " y " + kingPosition.y);
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                enemyPiece = TrackingHandler.pieceTracker[x,y];
                if (enemyPiece != null && !enemyPiece.CompareTag(GameController.turn))
                {
                    possibleEnemyPieceMovesList.AddRange(enemyPiece.GeneratePossibleMoves());
                }
            }
        }

        int kingX = (int)kingPosition.x;
        int kingY = (int)kingPosition.y;
        General.PossibleMove checkMove = new General.PossibleMove (-1, -1, null);

        // Retrieve the item with the specified X and Y values
        foreach (General.PossibleMove move in possibleEnemyPieceMovesList)
        {
            if (move.X == kingX && move.Y == kingY)
            {
                Debug.Log("Check from " + move.MovePiece + " at x " + move.X + " y " + move.Y);
                checkMove = move;
            }
        }

        if (checkMove.MovePiece == null)
        {
            isKingInCheck = false;
            Debug.Log("King not in Check.");
        }
        else 
        {
            isKingInCheck = true;
            Debug.Log("King in Check."); 
        }
    }

    private static Vector3 FindKing() //
    {
        Piece kingSearch = null;
        Vector3 kingPosition = new Vector3 (-1, -1, Constants.PIECE_Z_INDEX);// Wish I didn't need to do this

        //Debug.Log("Move attempt piece is " + moveAttemptPiece);
        /*if (moveAttemptPiece is King || hypotheticalMoveAttemptPiece is King)
        {
            kingPosition = moveAttemptTargetPosition;
        }
        else
        {*/
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    kingSearch = TrackingHandler.pieceTracker[x,y];
                    
                    if (kingSearch != null)
                    {
                        if (kingSearch.CompareTag(GameController.turn) && kingSearch is King)
                        {
                            kingPosition = new Vector3 (kingSearch.transform.position.x, kingSearch.transform.position.y, Constants.PIECE_Z_INDEX);
                            //Debug.Log("King Position x " + kingPosition.x + " y " + kingPosition.y);
                            break;
                        }
                    }
                }
            }
        //}

        return kingPosition;
    }
}
