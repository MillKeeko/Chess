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
    public GameObject circle;
    public static GameController gameController { get; private set; }

    private Piece[,] pieceTracker = new Piece[8,8];
    private int turn = 0; // 0 = White, 1 = Black
    private Piece savedPiece = null;

    private const int zIndex = -1;
    private Vector2 pawnDouble; 
    private bool isEnPassant = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*private float timer = 0;
    private Vector3 circlePosition;*/
    // Update is called once per frame
    void Update()
    {
        /*timer += Time.deltaTime; // Decrease timer by time passed since last frame
        if (timer >= 0.5f)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (pieceTracker[x,y] != null)
                    {
                        circlePosition = new Vector3 (x, y, -2);
                        GameObject thisCircle = Instantiate(circle, circlePosition, transform.rotation);
                        timer = 0f;
                        Destroy(thisCircle, 0.4f);
                    }
                }
            }
        }*/
    }

    //
    // ---------- Public Methods ----------
    //

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
                        if (IsPieceBlocking(piece, targetPosition)) return;
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
                pieceTracker[(int)pawnDouble.x, (int)pawnDouble.y].DestroyInstance();
                ExecuteMove(piece, target.transform.position, true);
            }
        }
    }

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
                    if (IsPieceBlocking(piece, targetPosition)) return;
                }

                ExecuteMove(piece, target.transform.position, true);
                target.DestroyInstance();
            }
        }
    }

    //
    // ---------- Private Methods ----------
    //

    private bool IsEnPassant(Piece piece, Vector3 targetPosition) // These are not the right rules!!! Im an idiot!!!!
    {
        bool returnValue = false;
        if (piece.TakePiece(targetPosition) && (piece.transform.position.x == (pawnDouble.x + 1) || piece.transform.position.x == (pawnDouble.x - 1)))
        {
            if (turn == 0 && piece.transform.position.y == 4) returnValue = true;
            else if (turn == 1 && piece.transform.position.y == 3) returnValue = true;
        }

        return returnValue;
    }

    private bool IsPieceBlocking(Piece piece, Vector3 target)
    {
        // Find the distance the piece is moving
        int distanceX = (int)target.x - (int)piece.transform.position.x;
        int distanceY = (int)target.y - (int)piece.transform.position.y;
        int distance = 0;
        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY)) distance = Mathf.Abs(distanceX);
        else distance = Mathf.Abs(distanceY);

        // Create unitary vector of the target relative to the piece
        Vector3 moveVector = new Vector3((int)target.x - (int)piece.transform.position.x,
                                         (int)target.y - (int)piece.transform.position.y, zIndex);
        Vector3 unitMoveVector = moveVector.normalized;

        // Simplify unit vector
        int x, y = 0;
        if (unitMoveVector.x > 0) x = 1;
        else if (unitMoveVector.x < 0) x = -1;
        else x = 0;
        if (unitMoveVector.y > 0) y = 1;
        else if (unitMoveVector.y < 0) y = -1;
        else y = 0;

        // Check if any pieces are in the way
        //Debug.Log("Distance " + distance);
        //Debug.Log("Target x " + target.x + " y " + target.y);
        for (int i = 1; i < Mathf.Abs(distance); i++)
        {
            Vector3 checkPosition = new Vector3(piece.transform.position.x + (x * i), piece.transform.position.y + (y * i), zIndex);
            // Debug.Log("IsPieceBlocking() checking " + piece + " at x " + piece.transform.position.x + " y " + piece.transform.position.y);
            if (pieceTracker[(int)checkPosition.x,(int)checkPosition.y] != null) 
            {
                return true;
            }
            //Debug.Log("No piece in the way at x " + (int)checkPosition.x + " and y " + (int)checkPosition.y);
        }

        // If piece is king check 1 move
        if (piece is King)
        {
            Vector3 checkPosition = new Vector3(piece.transform.position.x + x, piece.transform.position.y + y, zIndex);
            if (pieceTracker[(int)checkPosition.x,(int)checkPosition.y] != null) return true;
        }

        return false;
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

    private bool IsCheckmate()
    {
        Piece allyPiece = null;
        Vector3 move;
        // Loop through all possible pieces
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                allyPiece = pieceTracker[x,y];
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
                                if (IsPieceBlocking(allyPiece, move)) continue;
                            }
                            else if (allyPiece is Pawn)
                            {
                                if ((allyPiece.TakePiece(move) && pieceTracker[(int)move.x, (int)move.y] == null) ||
                                    (allyPiece.MovePiece(move) && pieceTracker[(int)move.x, (int)move.y] != null)) continue;
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

    private bool DoesMoveRemoveCheck(Piece piece, Vector3 target)
    {
        bool returnValue = false;
        Vector3 kingPosition;
        
        if (piece is King)
        {
            //Debug.Log("Piece is King.");
            kingPosition = target;
        }
        else kingPosition = FindKing();

        //Debug.Log("Checking " + piece);
        ChangeTrackerPosition(piece.transform.position, target, true); // Save piece
        //Debug.Log("Changed tracker for move check");
        if (!IsKingInCheck(kingPosition))
        {
            returnValue = true;
        } 
        ChangeTrackerPosition(target, piece.transform.position, false); 
        if (savedPiece != null) 
        {
            pieceTracker[(int)target.x, (int)target.y] = savedPiece;
            savedPiece = null;
            //Debug.Log("Changed tracker back to normal");
        }

        return returnValue;
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

    private Vector3 FindKing()
    {
        Piece kingSearch = null;
        Vector3 kingPosition = new Vector3 (-1, -1, -1);
        
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                kingSearch = pieceTracker[x,y];
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
                enemyPiece = pieceTracker[x,y];
                if (enemyPiece != null && enemyPiece.CompareTag(FindEnemyTag()))
                {
                    if (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook)
                    {
                        if (enemyPiece.TakePiece(kingPosition) == true)
                        {
                            if (!IsPieceBlocking(enemyPiece, kingPosition)) 
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
                enemyPiece = pieceTracker[x,y];
                if (enemyPiece != null && enemyPiece.CompareTag(enemyTag))
                {
                    // Check for a valid move, skip invalid
                    // King Position Check
                    if ((enemyPiece.TakePiece(kingPosition) || enemyPiece.MovePiece(kingPosition)) && (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                    {
                        if(!IsPieceBlocking(enemyPiece, kingPosition))
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
                            if(!IsPieceBlocking(enemyPiece, middlePosition)) 
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                return true;
                            }
                        }
                        if ((enemyPiece.TakePiece(targetPosition) || enemyPiece.MovePiece(targetPosition)) && (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            if(!IsPieceBlocking(enemyPiece, targetPosition)) 
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
                            if(!IsPieceBlocking(enemyPiece, middlePosition)) 
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                return true;
                            }
                        }
                        if ((enemyPiece.TakePiece(targetPosition) || enemyPiece.MovePiece(targetPosition)) && (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            if(!IsPieceBlocking(enemyPiece, targetPosition)) 
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
        Piece rookWhiteKSide = pieceTracker[7,0];
        Piece rookBlackKSide = pieceTracker[7,7];
        Piece rookWhiteQSide = pieceTracker[0,0];
        Piece rookBlackQSide = pieceTracker[0,7];
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

    // Change piece position in tracker and delete piece from old position
    private void ChangeTrackerPosition(Vector3 piecePosition, Vector3 targetPosition, bool saveTargetPiece)
    {
        if (saveTargetPiece) savedPiece = pieceTracker[(int)targetPosition.x, (int)targetPosition.y];
        pieceTracker[(int)targetPosition.x, (int)targetPosition.y] = pieceTracker[(int)piecePosition.x, (int)piecePosition.y];
        pieceTracker[(int)piecePosition.x, (int)piecePosition.y] = null;
    }

    public void AddToTracker (Piece piece, int row, int col)
    {
        pieceTracker[col,row] = piece;
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
}
