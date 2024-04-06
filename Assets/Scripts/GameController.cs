using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

//  Controls game start
//  Controls board setup
//  Controls changing turns
//  Controls game end
public class GameController : MonoBehaviour
{
    public GameObject SquareBlack, SquareWhite;
    public GameObject PawnWhite, RookWhite, KnightWhite, BishopWhite, QueenWhite, KingWhite;
    public GameObject PawnBlack, RookBlack, KnightBlack, BishopBlack, QueenBlack, KingBlack;

    //  Static Variables
    public static string playerTag;
    public static string botTag;
    public static string turn;


    //  Delegates
    public delegate void  OnPieceCreated (Piece piece, int x, int y);
    public delegate void OnBotTurn ();
    public delegate void OnChangeTurn(string tag);
    public delegate void OnGameStart ();

    //  Events
    public static event OnPieceCreated onPieceCreated;
    public static event OnBotTurn onBotTurn;
    public static event OnChangeTurn onChangeTurn;
    public static event OnGameStart onGameStart;

    void Awake()
    {
        turn = Constants.WHITE_TAG;
    }

    // Start is called before the first frame update
    void Start()
    {
        Piece.onMoveExecuted += ChangeTurn;

        RandomTeams();
        SetupBoard();

        onGameStart?.Invoke();

        CheckBotFirstTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckBotFirstTurn()
    {
        if (turn == botTag)
        {
            //Debug.Log("Bot goes first.");
            onBotTurn?.Invoke();
        } 
    }

    private void ChangeTurn()
    {
        if (turn == Constants.WHITE_TAG) turn = Constants.BLACK_TAG;
        else turn = Constants.WHITE_TAG;
        onChangeTurn?.Invoke(turn);
        if (turn == botTag) 
        {
            onBotTurn?.Invoke();
        }
    }

    private void RandomTeams()
    {
        int playerSide = General.RandomSide();
        if (playerSide == 0) 
        {   
            playerTag = Constants.WHITE_TAG;
            botTag = Constants.BLACK_TAG;
        }
        else if (playerSide == 1) 
        {
            playerTag = Constants.BLACK_TAG;
            botTag = Constants.WHITE_TAG;
        }
    }

    // Needs to move to view
    // Create board and place pieces, done at the beginning of the game
    private void SetupBoard()
    {
        Vector3 bottomLeftBoard = new Vector3 (0,0,0);
        Vector3 currentBoardPosition = bottomLeftBoard;

        int blackPawnRow, blackPieceRow, whitePawnRow, whitePieceRow, queenCol, kingCol;

        // Set which side the pieces are on
        if (playerTag == Constants.WHITE_TAG)
        {
            blackPawnRow = 6;
            blackPieceRow = 7;
            whitePawnRow = 1;
            whitePieceRow = 0;
            queenCol = 3;
            kingCol = 4;
        }
        else
        {
            blackPawnRow = 1;
            blackPieceRow = 0;
            whitePawnRow = 6;
            whitePieceRow = 7;
            queenCol = 4;
            kingCol = 3;
        }

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                currentBoardPosition = new Vector3(bottomLeftBoard.x + col, bottomLeftBoard.y + row, bottomLeftBoard.z);

                // Make the chess board
                if (row % 2 == 0)
                {
                    if (col % 2 == 0) MakeSquare(SquareBlack, currentBoardPosition);
                    else MakeSquare(SquareWhite, currentBoardPosition);
                }
                else
                {
                    if (col % 2 == 0) MakeSquare(SquareWhite, currentBoardPosition);
                    else MakeSquare(SquareBlack, currentBoardPosition);
                }
                
                // Place chess pieces in starting positions
                if (row == whitePieceRow || row == blackPieceRow)
                {
                    if (col == 0 || col == 7)
                    {
                        if (row == whitePieceRow) MakeChessPiece(RookWhite, currentBoardPosition);
                        if (row == blackPieceRow) MakeChessPiece(RookBlack, currentBoardPosition);
                    }
                    else if (col == 1 || col == 6)
                    {
                        if (row == whitePieceRow) MakeChessPiece(KnightWhite, currentBoardPosition);
                        if (row == blackPieceRow) MakeChessPiece(KnightBlack, currentBoardPosition);
                    }
                    else if (col == 2 || col == 5)
                    {
                        if (row == whitePieceRow) MakeChessPiece(BishopWhite, currentBoardPosition);
                        if (row == blackPieceRow) MakeChessPiece(BishopBlack, currentBoardPosition);
                    }
                    else if (col == queenCol)
                    {
                        if (row == whitePieceRow) MakeChessPiece(QueenWhite, currentBoardPosition);
                        if (row == blackPieceRow) MakeChessPiece(QueenBlack, currentBoardPosition);
                    }
                    else if (col == kingCol)
                    {
                        if (row == whitePieceRow) MakeChessPiece(KingWhite, currentBoardPosition);
                        if (row == blackPieceRow) MakeChessPiece(KingBlack, currentBoardPosition);
                    }
                }
                else if (row == whitePawnRow) MakeChessPiece(PawnWhite, currentBoardPosition);
                else if (row == blackPawnRow) MakeChessPiece(PawnBlack, currentBoardPosition);
            }
        }
    }

    // Needs to move to view
    // Instantiate piece and trigger event to be added to pieceTracker
    private void MakeChessPiece(GameObject piece, Vector3 position)
    {
        Vector3 pieceVector = new Vector3 (position.x, position.y, Constants.PIECE_Z_INDEX);
        Piece instantiatedPiece = Instantiate(piece, pieceVector, transform.rotation).GetComponent<Piece>();
        onPieceCreated?.Invoke(instantiatedPiece, (int)position.x, (int)position.y);
    }

    // Needs to move to view
    // Instantiate squares and place them in the array
    private void MakeSquare(GameObject square, Vector3 position)
    {
        Instantiate(square, position, transform.rotation);
    }
}
