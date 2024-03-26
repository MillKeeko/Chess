using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------- TO DO LIST ----------
// - Allow promotions for pawns
// - Allow player to restart game on checkmate
// - Create an AI that makes random moves
// - Allow player to choose whether they are playing white, black, or random

public class GameController : MonoBehaviour
{   
    // Delegates/Events to remove
    public delegate Piece OnNeedPieceAtLocation(int x, int y);
    public static event OnNeedPieceAtLocation onNeedPieceAtLocation;

    // Delegates
    public delegate bool OnCheckMoveRemovesCheck(Piece piece, Vector3 target);
    public delegate void OnChangeTrackerPosition(Vector3 piecePosition, Vector3 targetPosition, bool saveTargetPiece);
    public delegate void OnChangeTurn();
    public delegate void OnAttemptCastle(Piece piece, Vector3 targetPosition);

    // Events
    public static event OnCheckMoveRemovesCheck onCheckMoveRemovesCheck;
    public static event OnChangeTrackerPosition onChangeTrackerPosition;
    public static event OnChangeTurn onChangeTurn;
    public static event OnAttemptCastle onAttemptCastle;


    // In place of Event Variables
    private bool isPieceBlocking = false;
    private bool isCastleBlocked = false;
    private bool isKingInCheck = false;
    private bool isCheckmate = false;
 
    // Public variables
    public static GameController gameController { get; private set; }

    // Private variables
    private int turn = 0; // 0 = White, 1 = Black
    private const int zIndex = -1;
    private Vector2 pawnDouble; 

    // Start is called before the first frame update
    void Start()
    {
        PieceTracker.onPieceBlockingChange += SetIsPieceBlocking;
        PieceTracker.onKingInCheckChange += SetIsKingInCheck;
        PieceTracker.onCastleBlockedChange += SetIsCastleBlocked;
        PieceTracker.onCheckmate += SetIsCheckmate;
        PieceTracker.onCastleNeedsExecute += ExecuteMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    // ---------- Mutators ----------
    //

    private void SetIsPieceBlocking (bool blockStatus)
    {
        isPieceBlocking = blockStatus;
    }

    private void SetIsCastleBlocked (bool blockStatus)
    {
        Debug.Log("GameController - Setting castleblock is " + blockStatus);
        isCastleBlocked = blockStatus;
    }

    private void SetIsKingInCheck (bool checkStatus)
    {
        isKingInCheck = checkStatus;
    }

    private void SetIsCheckmate ()
    {
        isCheckmate = true;
    }

    //
    // ---------- Public Methods ----------
    //

    public void TryToTakePiece(Piece piece, Piece target)
    {
        // Only try to move piece if it's their turn and moves the piece in the right direction
        if (IsYourTurn(piece) && piece.TakePiece(target.transform.position) == true)
        {
            // Only try to move piece if King isn't in check, or it is but the move removes check
            if (isKingInCheck == false || onCheckMoveRemovesCheck?.Invoke(piece, target.transform.position) == true)
            {
                // Bishops, Rooks, and Queens need to ensure nothing blocks their move
                if (piece is Bishop || piece is Rook || piece is Queen)
                {
                    Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, zIndex);
                    if (isPieceBlocking == true) return;
                }

                ExecuteMove(piece, target.transform.position, true);
                target.DestroyInstance();
            }
        }
    }

    public void TryToMovePiece(Piece piece, Piece target)
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, zIndex);

        if (IsYourTurn(piece))
        {
            // Only try to move piece if it's their turn and moves the piece in the right direction
            if (piece.MovePiece(target.transform.position) == true)
            {
                // Only try to move piece if King isn't in check, or it is but the move removes check
                if (isKingInCheck == false || onCheckMoveRemovesCheck?.Invoke(piece, target.transform.position) == true)
                {
                    // Bishops, Rooks, and Queens need to ensure nothing blocks their move
                    if (piece is Bishop || piece is Rook || piece is Queen)
                    {
                        if (isPieceBlocking == true) return;
                    }

                    ExecuteMove(piece, target.transform.position, true);
                }
            }
            // Check King Castle
            else if (piece is King && isKingInCheck == false) 
            {
                // A little redundant to check for check on king in IsCastleBlocked() through TryCastle() because this already checked
                onAttemptCastle!.Invoke(piece, targetPosition);
                //TryCastle(piece, target);
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
        int yDistance = Mathf.Abs((int)targetPosition.y - (int)piece.transform.position.y);

        onChangeTrackerPosition?.Invoke(piece.transform.position, targetPosition, false);
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
            if (isCheckmate == true) 
            {
                Debug.Log("Checkmate!");
            }
            if (piece is King || piece is Rook || piece is Pawn) piece.SetFirstMove(false);
        }
    }

    private void ChangeTurn()
    {
        if (turn == 0) turn = 1;
        else turn = 0;
        onChangeTurn!.Invoke();
    }

    private bool IsYourTurn (Piece piece)
    {
        if (piece.tag == "PieceWhite" && turn == 0) return true;
        else if (piece.tag == "PieceBlack" && turn == 1) return true;
        else return false;
    }
}
