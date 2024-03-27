using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the read and write access to the PieceTracker array

public class PieceTracker : MonoBehaviour
{
    // Delegates
    public delegate void OnPieceBlockingChange(bool blockStatus);
    public delegate void OnKingInCheckChange(bool checkStatus);
    public delegate void OnCastleBlockedChange(bool blockStatus);
    public delegate void OnCheckmate();
    public delegate void OnCastleNeedsExecute(Piece piece, Vector3 targetPosition, bool changeTurn);

    // Events
    public static event OnPieceBlockingChange onPieceBlockingChange;
    public static event OnKingInCheckChange onKingInCheckChange;
    public static event OnCastleBlockedChange onCastleBlockedChange;
    public static event OnCheckmate onCheckmate;
    public static event OnCastleNeedsExecute onCastleNeedsExecute;

    public GameObject circle;
    private const int zIndex = -1;
    private Piece[,] pieceTracker = new Piece[8,8];
    private Piece savedPiece = null;

    // In place of Event variables
    private Piece hypotheticalMoveAttemptPiece = null;
    private Piece moveAttemptPiece = null;
    private Vector3 moveAttemptTargetPosition;
    private bool isPieceBlocking = false;
    private bool isCastleBlocked = false;
    private bool isKingInCheck = false;
    private int turn = 0; // 0 = White, 1 = Black

    void Awake()
    {
        SetupController.onPieceCreated += AddToTracker;
    }

    // Start is called before the first frame update
    void Start()
    {
        InputHandler.onAttemptMove += SetMoveAttempt;
        InputHandler.onAttemptMove += IsPieceBlocking;
        InputHandler.onAttemptMove += DoesMoveRemoveCheck;
        
        GameController.onAttemptCastle += TryCastle;

        GameController.onExecuteMove += ChangeTrackerPosition;

        GameController.onChangeTurn += UpdateTurn;
        GameController.onChangeTurn += IsKingInCheck;
        GameController.onChangeTurn += IsCheckmate;
        

        onPieceBlockingChange += SetIsPieceBlocking;
        onKingInCheckChange += SetIsKingInCheck;
        onCastleBlockedChange += SetIsCastleBlocked;

        GameController.onNeedPieceAtLocation += GetFromTracker;
    }

    private float timer = 0;
    private Vector3 circlePosition;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Decrease timer by time passed since last frame
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
        }
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
        //Debug.Log("PieceTracker - Setting castleblock is " + blockStatus);
        isCastleBlocked = blockStatus;
    }

    private void SetIsKingInCheck (bool checkStatus)
    {
        isKingInCheck = checkStatus;
    }

    private void SetMoveAttempt (Piece piece, Vector3 targetPosition)
    {
        moveAttemptPiece = piece;
        moveAttemptTargetPosition = targetPosition;
    }

    //
    // --------- General Methods ---------
    //

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

    private void UpdateTurn()
    {
        if (turn == 0) turn = 1;
        else turn = 0;
    }


    private void IsPieceBlocking(Piece piece, Vector3 target)
    {
        bool blockingBool = false;
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
        //Debug.Log("Unit vector x " + x + " y " + y);

        if (piece is Bishop || piece is Rook || piece is Queen)
        {
            // Check if any pieces are in the way
            //Debug.Log("Distance " + distance);
            //Debug.Log("Target x " + target.x + " y " + target.y);
            for (int i = 1; i < Mathf.Abs(distance); i++)
            {
                Vector3 checkPosition = new Vector3((piece.transform.position.x + (x * i)), 
                                                    (piece.transform.position.y + (y * i)), zIndex);
                //Debug.Log("x * i " + (x * i));
                //Debug.Log("y * i " + (y * i));
                //Debug.Log("IsPieceBlocking() checking " + piece + " at x " + (int)checkPosition.x + " y " + (int)checkPosition.y);
                if (pieceTracker[(int)checkPosition.x,(int)checkPosition.y] != null) 
                {
                    blockingBool = true;
                }
                //Debug.Log("No piece in the way at x " + (int)checkPosition.x + " and y " + (int)checkPosition.y);
            }
        }
        // If piece is king check 1 move
        else if (piece is King)
        {
            Vector3 checkPosition = new Vector3(piece.transform.position.x + x, piece.transform.position.y + y, zIndex);
            if (pieceTracker[(int)checkPosition.x,(int)checkPosition.y] != null) 
            {
                Debug.Log("King move blocked at x " + (int)checkPosition.x + " y " + (int)checkPosition.y);
                blockingBool = true;
            }
        }

        onPieceBlockingChange?.Invoke(blockingBool);
    }

    private Piece GetFromTracker(int x, int y)
    {
        return pieceTracker[x,y];
    }

    private void AddToTracker(Piece piece, int x, int y)
    {
        pieceTracker[x,y] = piece;
    }

    // Change piece position in tracker and delete piece from old position
    private void ChangeTrackerPosition(Vector3 piecePosition, Vector3 targetPosition, bool saveTargetPiece)
    {
        if (saveTargetPiece) savedPiece = pieceTracker[(int)targetPosition.x, (int)targetPosition.y];
        pieceTracker[(int)targetPosition.x, (int)targetPosition.y] = pieceTracker[(int)piecePosition.x, (int)piecePosition.y];
        pieceTracker[(int)piecePosition.x, (int)piecePosition.y] = null;
    }

    // Need to fix how king position is dynamic in this function. I do not want IsKingInCheck to take an argument
    private void DoesMoveRemoveCheck(Piece piece, Vector3 target)
    {
        hypotheticalMoveAttemptPiece = piece;
        //Debug.Log("Checking " + piece);
        ChangeTrackerPosition(piece.transform.position, target, true); // Save piece
        //Debug.Log("Changed tracker for move check");
        IsKingInCheck();
        ChangeTrackerPosition(target, piece.transform.position, false); 
        if (savedPiece != null) 
        {
            pieceTracker[(int)target.x, (int)target.y] = savedPiece;
            savedPiece = null;
            //Debug.Log("Changed tracker back to normal");
        }
        hypotheticalMoveAttemptPiece = null;
    }

     private void IsCheckmate()
    {
        Piece allyPiece = null;
        Piece targetPiece = null;
        Vector3 move;
        bool checkmateBool = true;
        //Debug.Log("IsCheckmate() Start.");
        IsKingInCheck();
        //Debug.Log("Is King in check? " + isKingInCheck);
        if (isKingInCheck)
        {
            // Loop through all possible pieces
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    
                    allyPiece = pieceTracker[x,y];
                    //Debug.Log("Is Checkmate check - " + FindAllyTag());
                    if (allyPiece != null && allyPiece.CompareTag(FindAllyTag())) 
                    {
                        // Loop through all possible moves
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                move = new Vector3 (i, j, zIndex);
                                // Check for a valid move, skip invalid
                                if ((allyPiece.TakePiece(move) || allyPiece.MovePiece(move)) && 
                                    (allyPiece is Bishop || allyPiece is Rook || allyPiece is Queen || allyPiece is King))
                                {
                                    IsPieceBlocking(allyPiece, move);
                                    if (isPieceBlocking == true) continue;
                                }
                                else if (allyPiece is Pawn)
                                {
                                    targetPiece = pieceTracker[(int)move.x,(int)move.y];
                                    if ((allyPiece.TakePiece(move) && targetPiece == null) ||
                                        (allyPiece.MovePiece(move) && targetPiece != null)) 
                                    {
                                        //Debug.Log("Pawn move not valid from x " + allyPiece.transform.position.x + " y " + allyPiece.transform.position.y + " to x " + move.x + " y " + move.y);
                                        continue;
                                    }
                                }
                                // Check if the valid move does remove the check
                                if (allyPiece.TakePiece(move) || allyPiece.MovePiece(move))
                                {
                                    DoesMoveRemoveCheck(allyPiece, move);
                                    if(isKingInCheck == false) 
                                    {
                                        Debug.Log(allyPiece + " from x " + allyPiece.transform.position.x + " y " + allyPiece.transform.position.y + " to x " + move.x + " y " + move.y);
                                        checkmateBool = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else checkmateBool = false;
        
        //Debug.Log("Checked whether " + FindAllyTag() + " is in Checkmate.");
        
        if (checkmateBool == true)
        {
            Debug.Log("Checkmate!");
            onCheckmate?.Invoke();
        }
        else IsKingInCheck(); // Reset from calls that were looped in DoesMoveRemoveCheck()
    }

    private Vector3 FindKing()
    {
        Piece kingSearch = null;
        Vector3 kingPosition = new Vector3 (-1, -1, -1);

        //Debug.Log("Move attempt piece is " + moveAttemptPiece);
        if (moveAttemptPiece is King || hypotheticalMoveAttemptPiece is King)
        {
            kingPosition = moveAttemptTargetPosition;
        }
        else
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    kingSearch = pieceTracker[x,y];
                    //Debug.Log("Ally King - " + FindAllyTag());
                    if (kingSearch != null && kingSearch.CompareTag(FindAllyTag()) && kingSearch is King)
                    {
                        kingPosition = new Vector3 (kingSearch.transform.position.x, kingSearch.transform.position.y, zIndex);
                        //Debug.Log("King Position x " + kingPosition.x + " y " + kingPosition.y);
                        break;
                    }
                }
            }
        }

        return kingPosition;
    }

    private void IsKingInCheck()
    {
        Vector3 kingPosition = FindKing();
        Piece enemyPiece = null;
        bool checkBool = false;

        //Debug.Log("Checking king in check at x " + kingPosition.x + " y " + kingPosition.y);
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
                            IsPieceBlocking(enemyPiece, kingPosition);
                            if (isPieceBlocking == false) 
                            {
                                //Debug.Log("King in Check! From " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                //Debug.Log("King position x " + kingPosition.x + " at y " + kingPosition.y);
                                checkBool = true;
                            }
                        } 
                    }
                    else if (enemyPiece is Knight || enemyPiece is Pawn)
                    {
                        if (enemyPiece.TakePiece(kingPosition)) 
                        {
                            //Debug.Log("King in Check! From " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                            //Debug.Log("King position x " + kingPosition.x + " at y " + kingPosition.y);
                            checkBool = true;
                        }
                    }
                }
            }
        }

        onKingInCheckChange?.Invoke(checkBool); 
    }

    private void IsCastleBlocked(Piece piece, Vector3 targetPosition, int castleSide)
    {
        //Debug.Log("Trying is castle blocked.");
        string enemyTag = null;
        Piece enemyPiece = null;
        Vector3 kingPosition = new Vector3 (piece.transform.position.x, piece.transform.position.y, zIndex);
        Vector3 middlePosition;
        bool blockedBool = false;

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
                    if ((enemyPiece.TakePiece(kingPosition) || enemyPiece.MovePiece(kingPosition)) && 
                        (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                    {
                        IsPieceBlocking(enemyPiece, kingPosition);
                        if (isPieceBlocking == false)
                        {
                            //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                            blockedBool = true;
                        }
                    }
                    else if (castleSide == 0)
                    {
                        middlePosition = new Vector3 (targetPosition.x + 1, targetPosition.y, zIndex);
                        if ((enemyPiece.TakePiece(middlePosition) || enemyPiece.MovePiece(middlePosition)) && 
                            (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            IsPieceBlocking(enemyPiece, middlePosition);
                            if (isPieceBlocking == false)
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                blockedBool = true;
                            }
                        }
                        if ((enemyPiece.TakePiece(targetPosition) || enemyPiece.MovePiece(targetPosition)) && 
                            (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            IsPieceBlocking(enemyPiece, targetPosition);
                            if(isPieceBlocking == false) 
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                blockedBool = true;
                            }
                        }
                    }
                    else if (castleSide == 1)
                    {
                        middlePosition = new Vector3 (targetPosition.x - 1, targetPosition.y, zIndex);
                        if ((enemyPiece.TakePiece(middlePosition) || enemyPiece.MovePiece(middlePosition)) && 
                            (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            IsPieceBlocking(enemyPiece, middlePosition);
                            if(isPieceBlocking == false) 
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                blockedBool = true;
                            }
                        }
                        if ((enemyPiece.TakePiece(targetPosition) || enemyPiece.MovePiece(targetPosition)) && 
                            (enemyPiece is Bishop || enemyPiece is Queen || enemyPiece is Rook))
                        {
                            IsPieceBlocking(enemyPiece, targetPosition);
                            if(isPieceBlocking == false) 
                            {
                                //Debug.Log("Castle blocked by " + enemyPiece + " at x " + enemyPiece.transform.position.x + " y " + enemyPiece.transform.position.y);
                                blockedBool = true;
                            }
                        }
                    }
                }
            }
        }

        onCastleBlockedChange?.Invoke(blockedBool);
    }

     private void TryCastle(Piece piece, Vector3 targetPosition)
    {
        //Debug.Log("Trying Castle.");
        King king = (King)piece;
        Piece rookWhiteKSide = pieceTracker[7,0];
        Piece rookBlackKSide = pieceTracker[7,7];
        Piece rookWhiteQSide = pieceTracker[0,0];
        Piece rookBlackQSide = pieceTracker[0,7];
        Vector3 rookTarget;
        int castleSide = king.Castle(targetPosition);

        IsCastleBlocked(piece, targetPosition, castleSide);
        
        //Debug.Log("TryCastle - castleblocked? " + isCastleBlocked);
        if (castleSide == 0)
        {
            if (piece.tag == "PieceWhite" && rookWhiteKSide != null && rookWhiteKSide.GetFirstMove() && isCastleBlocked == false)
            {   
                rookTarget = new Vector3(targetPosition.x - 1, targetPosition.y, zIndex);
                onCastleNeedsExecute.Invoke(king, targetPosition, false);
                onCastleNeedsExecute.Invoke(rookWhiteKSide, rookTarget, true);
            }
            else if (piece.tag == "PieceBlack" && rookBlackKSide != null && rookBlackKSide.GetFirstMove() && isCastleBlocked == false)
            {
                rookTarget = new Vector3(targetPosition.x - 1, targetPosition.y, zIndex);
                onCastleNeedsExecute?.Invoke(piece, targetPosition, false);
                onCastleNeedsExecute?.Invoke(rookBlackKSide, rookTarget, true);
            }
        }
        else if (castleSide == 1)
        {
            if (piece.tag == "PieceWhite" && rookWhiteQSide != null && rookWhiteQSide.GetFirstMove() && isCastleBlocked == false)
            {
                rookTarget = new Vector3(targetPosition.x + 1, targetPosition.y, zIndex);
                onCastleNeedsExecute?.Invoke(piece, targetPosition, false);
                onCastleNeedsExecute?.Invoke(rookWhiteQSide, rookTarget, true);
            }
            else if (piece.tag == "PieceBlack" && rookBlackQSide != null && rookBlackQSide.GetFirstMove() && isCastleBlocked == false)
            {
                rookTarget = new Vector3(targetPosition.x + 1, targetPosition.y, zIndex);
                onCastleNeedsExecute?.Invoke(piece, targetPosition, false);
                onCastleNeedsExecute?.Invoke(rookBlackQSide, rookTarget, true);
            }
        }
    }
}
