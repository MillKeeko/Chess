using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------- TO DO LIST ----------
// - Allow pawns to perform an en passant
// - Allow promotions for pawns
// - Allow player to restart game on checkmate
// - Create an AI that makes random moves
// - Allow player to choose whether they are playing white, black, or random

public class GameController : MonoBehaviour
{   
    // Events
    public delegate bool OnCheckPieceBlocking(Piece piece, Vector3 target);
    public static event OnCheckPieceBlocking onCheckPieceBlocking;
    public delegate Piece OnNeedPieceAtLocation(int x, int y);
    public static event OnNeedPieceAtLocation onNeedPieceAtLocation;

    public static GameController gameController { get; private set; }

    private int turn = 0; // 0 = White, 1 = Black

    private const int zIndex = -1;
    private Vector2 pawnDouble; 
    private bool isEnPassant = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    //
    // ---------- Public Methods ----------
    //

    public void TryToTakePiece(Piece piece, Piece target)
    {
        Vector3 kingPosition = FindKing();

        // Only try to move piece if it's their turn and moves the piece in the right direction
        if (IsYourTurn(piece) && piece.TakePiece(target.transform.position) == true)
        {
            // Only try to move piece if King isn't in check, or it is but the move removes check
            if (!IsKingInCheck(kingPosition) || DoesMoveRemoveCheck(piece, target.transform.position))
            {
                // Bishops, Rooks, and Queens need to ensure nothing blocks their move
                if (piece is Bishop || piece is Rook || piece is Queen)
                {
                    Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, zIndex);
                    if (onCheckPieceBlocking?.Invoke(piece, targetPosition) == true) return;
                }

                ExecuteMove(piece, target.transform.position, true);
                target.DestroyInstance();
            }
        }
    }

    public void TryToMovePiece(Piece piece, Piece target)
    {
        Vector3 kingPosition = FindKing();

        if (IsYourTurn(piece))
        {
            // Only try to move piece if it's their turn and moves the piece in the right direction
            if (piece.MovePiece(target.transform.position) == true)
            {
                // Only try to move piece if King isn't in check, or it is but the move removes check
                if (!IsKingInCheck(kingPosition) || DoesMoveRemoveCheck(piece, target.transform.position))
                {
                    // Bishops, Rooks, and Queens need to ensure nothing blocks their move
                    if (piece is Bishop || piece is Rook || piece is Queen)
                    {
                        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, zIndex);
                        if (onCheckPieceBlocking?.Invoke(piece, targetPosition) == true) return;
                    }

                    ExecuteMove(piece, target.transform.position, true);
                }
            }
            // Check King Castle
            else if (piece is King && !IsKingInCheck(kingPosition)) 
            {
                // A little redundant to check for check on king in IsCastleBlocked() through TryCastle() because this already checked
                TryCastle(piece, target);
            }
            // Check En Passant
            else if (piece is Pawn && IsEnPassant(piece, target.transform.position))
            {
                Debug.Log("En Passant.");
                Piece targetPiece = onNeedPieceAtLocation?.Invoke((int)target.transform.position.x, (int)target.transform.position.y);
                targetPiece.DestroyInstance(); 
                ExecuteMove(piece, target.transform.position, true);
            }
        }
    }

    //
    // ---------- GameController Methods ----------
    //

    private bool IsEnPassant(Piece piece, Vector3 targetPosition)
    {
        bool returnValue = false;
        if (piece.TakePiece(targetPosition) && (piece.transform.position.x == (pawnDouble.x + 1) || piece.transform.position.x == (pawnDouble.x - 1)))
        {
            if (turn == 0 && piece.transform.position.y == 4) returnValue = true;
            else if (turn == 1 && piece.transform.position.y == 3) returnValue = true;
        }

        return returnValue;
    }

    private void ExecuteMove(Piece piece, Vector3 targetPosition, bool changeTurn)
    {
        Vector3 kingPosition;
        int yDistance = Mathf.Abs((int)targetPosition.y - (int)piece.transform.position.y);

        ChangeTrackerPosition(piece.transform.position, targetPosition, false);
        piece.transform.position = new Vector3(targetPosition.x, targetPosition.y, zIndex);
        
        if (changeTurn) 
        {
            ChangeTurn();

            // Record Pawn's first move if double for en passant and destroy taken pawn
            if (piece is Pawn && yDistance == 2)
            {
                //Debug.Log("Setting pawnDouble x " + (int)targetPosition.x + " y " + (int)targetPosition.y);
                pawnDouble.x = (int)targetPosition.x;
                pawnDouble.y = (int)targetPosition.y;
            }
            else
            {
                pawnDouble.x = -1;
                pawnDouble.y = -1;
            }

            // Check for Checkmate
            kingPosition = FindKing();
            if (IsKingInCheck(kingPosition)) 
            {
                IsCheckmate();
            }
            if (piece is King || piece is Rook || piece is Pawn) piece.SetFirstMove(false);
        }
    }

    private string FindEnemyTag()
    {
        string enemyTag = null;
        if (turn == 0) enemyTag = "PieceBlack";
        else if (turn == 1) enemyTag = "PieceWhite";
        return enemyTag;
    }

    private string FindAllyTag()
    {
        string allyTag = null;
        if (turn == 0) allyTag = "PieceWhite";
        else if (turn == 1) allyTag = "PieceBlack";
        return allyTag;
    }

    private void ChangeTurn()
    {
        if (turn == 0) turn = 1;
        else turn = 0;
    }

    private bool IsYourTurn (Piece piece)
    {
        if (piece.tag == "PieceWhite" && turn == 0) return true;
        else if (piece.tag == "PieceBlack" && turn == 1) return true;
        else return false;
    }


    //
    // ---------- PieceTracker Methods ----------
    //

    private bool IsCheckmate()
    {
        Piece allyPiece = null;
        Piece targetPiece = null;
        Vector3 move;
        // Loop through all possible pieces
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                
                allyPiece = onNeedPieceAtLocation?.Invoke(x,y);
                if (allyPiece != null && !allyPiece.CompareTag(FindEnemyTag()))
                {
                    // Loop through all possible moves
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            move = new Vector3 (i, j, zIndex);
                            // Check for a valid move, skip invalid
                            if ((allyPiece.TakePiece(move) || allyPiece.MovePiece(move)) && (allyPiece is Bishop || allyPiece is Rook || allyPiece is Queen || allyPiece is King))
                            {
                                if (onCheckPieceBlocking?.Invoke(allyPiece, move) == true) continue;
                            }
                            else if (allyPiece is Pawn)
                            {
                                targetPiece = onNeedPieceAtLocation?.Invoke((int)move.x,(int)move.y);
                                if ((allyPiece.TakePiece(move) && targetPiece == null) ||
                                    (allyPiece.MovePiece(move) && targetPiece != null)) continue;
                            }
                            // Check if the valid move does remove the check
                            if (allyPiece.TakePiece(move) || allyPiece.MovePiece(move))
                            {
                                if(DoesMoveRemoveCheck(allyPiece, move)) 
                                {
                                    //Debug.Log(allyPiece + " from x " + allyPiece.transform.position.x + " y " + allyPiece.transform.position.y + " to x " + move.x + " y " + move.y);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Checkmate!");
        return false;
    }

     private Vector3 FindKing()
    {
        Piece kingSearch = null;
        Vector3 kingPosition = new Vector3 (-1, -1, -1);
        
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                kingSearch = onNeedPieceAtLocation?.Invoke(x,y);
                if (kingSearch != null && kingSearch.CompareTag(FindAllyTag()) && kingSearch is King)
                {
                    kingPosition = new Vector3 (kingSearch.transform.position.x, kingSearch.transform.position.y, zIndex);
                    //Debug.Log("King Position x " + kingPosition.x + " y " + kingPosition.y);
                    break;
                }
            }
        }

        return kingPosition;
    }

    private bool IsKingInCheck(Vector3 kingPosition)
    {
        Piece enemyPiece = null;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                enemyPiece = onNeedPieceAtLocation?.Invoke(x,y);
                if (enemyPiece != null && enemyPiece.CompareTag(FindEnemyTag()))
                {
                    if (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook)
                    {
                        if (enemyPiece.TakePiece(kingPosition) == true)
                        {
                            if (onCheckPieceBlocking?.Invoke(enemyPiece, kingPosition) == false) 
                            {
                                //Debug.Log("King in Check! From " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                //Debug.Log("King position x " + kingPosition.x + " at y " + kingPosition.y);
                                return true;
                            }
                        } 
                    }
                    else if (enemyPiece is Knight || enemyPiece is Pawn)
                    {
                        if (enemyPiece.TakePiece(kingPosition)) 
                        {
                            //Debug.Log("King in Check! From " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                            //Debug.Log("King position x " + kingPosition.x + " at y " + kingPosition.y);
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private bool IsCastleBlocked(Piece piece, Vector3 targetPosition, int castleSide)
    {
        string enemyTag = null;
        Piece enemyPiece = null;
        Vector3 kingPosition = new Vector3 (piece.transform.position.x, piece.transform.position.y, zIndex);
        Vector3 middlePosition;

        if (piece.CompareTag("PieceBlack")) enemyTag = "PieceWhite";
        else if (piece.CompareTag("PieceWhite")) enemyTag = "PieceBlack";

        // Look for Black Rooks, Queens, and Bishops
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                enemyPiece = onNeedPieceAtLocation?.Invoke(x,y);
                if (enemyPiece != null && enemyPiece.CompareTag(enemyTag))
                {
                    // Check for a valid move, skip invalid
                    // King Position Check
                    if ((enemyPiece.TakePiece(kingPosition) || enemyPiece.MovePiece(kingPosition)) && (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                    {
                        if(onCheckPieceBlocking?.Invoke(enemyPiece, kingPosition) == false)
                        {
                            //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                            return true;
                        }
                    }
                    else if (castleSide == 0)
                    {
                        middlePosition = new Vector3 (targetPosition.x + 1, targetPosition.y, zIndex);
                        if ((enemyPiece.TakePiece(middlePosition) || enemyPiece.MovePiece(middlePosition)) && (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            if(onCheckPieceBlocking?.Invoke(enemyPiece, middlePosition) == false) 
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                return true;
                            }
                        }
                        if ((enemyPiece.TakePiece(targetPosition) || enemyPiece.MovePiece(targetPosition)) && (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            if(onCheckPieceBlocking?.Invoke(enemyPiece, targetPosition) == false) 
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                return true;
                            }
                        }
                    }
                    else if (castleSide == 1)
                    {
                        middlePosition = new Vector3 (targetPosition.x - 1, targetPosition.y, zIndex);
                        if ((enemyPiece.TakePiece(middlePosition) || enemyPiece.MovePiece(middlePosition)) && (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            if(onCheckPieceBlocking?.Invoke(enemyPiece, middlePosition) == false) 
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                return true;
                            }
                        }
                        if ((enemyPiece.TakePiece(targetPosition) || enemyPiece.MovePiece(targetPosition)) && (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            if(onCheckPieceBlocking?.Invoke(enemyPiece, targetPosition) == false) 
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    private void TryCastle(Piece piece, Piece target)
    {
        King king = (King)piece;
        Piece rookWhiteKSide = onNeedPieceAtLocation?.Invoke(7,0);
        Piece rookBlackKSide = onNeedPieceAtLocation?.Invoke(7,7);
        Piece rookWhiteQSide = onNeedPieceAtLocation?.Invoke(0,0);
        Piece rookBlackQSide = onNeedPieceAtLocation?.Invoke(0,7);
        Vector3 rookTarget;
        int castleSide = king.Castle(target.transform.position);
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, zIndex);

        if (castleSide == 0)
        {
            if (piece.tag == "PieceWhite" && rookWhiteKSide != null && rookWhiteKSide.GetFirstMove() && !IsCastleBlocked(piece, targetPosition, castleSide))
            {   
                rookTarget = new Vector3(target.transform.position.x - 1, target.transform.position.y, zIndex);
                ExecuteMove(king, target.transform.position, false);
                ExecuteMove(rookWhiteKSide, rookTarget, true);
            }
            else if (piece.tag == "PieceBlack" && rookBlackKSide != null && rookBlackKSide.GetFirstMove() && !IsCastleBlocked(piece, targetPosition, castleSide))
            {
                rookTarget = new Vector3(target.transform.position.x - 1, target.transform.position.y, zIndex);
                ExecuteMove(piece, target.transform.position, false);
                ExecuteMove(rookBlackKSide, rookTarget, true);
            }
        }
        else if (castleSide == 1)
        {
            if (piece.tag == "PieceWhite" && rookWhiteQSide != null && rookWhiteQSide.GetFirstMove() && !IsCastleBlocked(piece, targetPosition, castleSide))
            {
                rookTarget = new Vector3(target.transform.position.x + 1, target.transform.position.y, zIndex);
                ExecuteMove(piece, target.transform.position, false);
                ExecuteMove(rookWhiteQSide, rookTarget, true);
            }
            else if (piece.tag == "PieceBlack" && rookBlackQSide != null && rookBlackQSide.GetFirstMove() && !IsCastleBlocked(piece, targetPosition, castleSide))
            {
                rookTarget = new Vector3(target.transform.position.x + 1, target.transform.position.y, zIndex);
                ExecuteMove(piece, target.transform.position, false);
                ExecuteMove(rookBlackQSide, rookTarget, true);
            }
        }
    }
}
