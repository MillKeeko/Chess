using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  View

public class BoardController : MonoBehaviour
{
    public static BoardController instance { get; private set; }

    public GameObject SquareBlack, SquareWhite;
    public GameObject PawnWhite, RookWhite, KnightWhite, BishopWhite, QueenWhite, KingWhite;
    public GameObject PawnBlack, RookBlack, KnightBlack, BishopBlack, QueenBlack, KingBlack;

    private int _blackPawnRank, _blackPieceRank, _whitePawnRank, _whitePieceRank, _queenFile, _kingFile;
    private Vector2 _promotionPosition;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        _promotionPosition = new Vector2 (-1, -1);

        PromotionMenuController.OnPromotionEvent += PromotePawn;
        TrackingHandler.OnPromotionNeedPositionEvent += SetPromotionPosition;
    }

    void Start()
    {
        SetupBoard();
    }

    //
    //  Public Methods
    //
    public static void ExecuteMove(Piece piece, Vector2 targetPosition)
    {
        Vector3 realTarget = new Vector3 (targetPosition.x, targetPosition.y, Constants.PIECE_Z_INDEX);
        piece.transform.position = realTarget;
    }

    //
    //  Private Methods
    //

    private void PromotePawn(Pieces pieceType)
    {
        Piece piece = null;
        switch (pieceType)
        {
            case Pieces.Queen:
                if (GameController.Turn == Constants.WHITE_TAG) piece = MakeChessPiece(QueenWhite, _promotionPosition);
                else piece = MakeChessPiece(QueenBlack, _promotionPosition);
                break;
            case Pieces.Knight:
                if (GameController.Turn == Constants.WHITE_TAG) piece = MakeChessPiece(KnightWhite, _promotionPosition);
                else piece = MakeChessPiece(KnightBlack, _promotionPosition);
                break;
            case Pieces.Rook:
                if (GameController.Turn == Constants.WHITE_TAG) piece = MakeChessPiece(RookWhite, _promotionPosition);
                else piece = MakeChessPiece(RookBlack, _promotionPosition);
                break;
            case Pieces.Bishop:
                if (GameController.Turn == Constants.WHITE_TAG) piece = MakeChessPiece(BishopWhite, _promotionPosition);
                else piece = MakeChessPiece(BishopBlack, _promotionPosition);
                break;
        }
        piece.Position = _promotionPosition; // *********Is this good VIEW logic???
    }

    private void SetPromotionPosition(Vector2 targetPosition)
    {
        _promotionPosition.x = targetPosition.x;
        _promotionPosition.y = targetPosition.y;
    }

    // Create board and place pieces
    private void SetupBoard()
    {
        DetermineSides();
        PlaceSquares();
        PlacePieces();
    }

    // Iterate through 64 squares and place pieces
    private void PlacePieces()
    {
        Vector3 bottomLeftBoard = new Vector2 (0,0);
        Vector3 currentBoardPosition = bottomLeftBoard;
        Piece instantiatedPiece = null;

        for (int rank = 0; rank < 8; rank++)
        {
            currentBoardPosition.y = rank;

            for (int file = 0; file < 8; file++)
            {
                currentBoardPosition.x = file;

                if (rank == _whitePieceRank || rank == _blackPieceRank)
                {
                    if (file == 0 || file == 7)
                    {
                        if (rank == _whitePieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(RookWhite, currentBoardPosition);
                        }
                        if (rank == _blackPieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(RookBlack, currentBoardPosition);
                        }
                    }
                    else if (file == 1 || file == 6)
                    {
                        if (rank == _whitePieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(KnightWhite, currentBoardPosition);
                        }
                        if (rank == _blackPieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(KnightBlack, currentBoardPosition);
                        }
                    }
                    else if (file == 2 || file == 5)
                    {
                        if (rank == _whitePieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(BishopWhite, currentBoardPosition);
                        }
                        if (rank == _blackPieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(BishopBlack, currentBoardPosition);
                        }
                    }
                    else if (file == _queenFile)
                    {
                        if (rank == _whitePieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(QueenWhite, currentBoardPosition);
                        }
                        if (rank == _blackPieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(QueenBlack, currentBoardPosition);
                        }
                    }
                    else if (file == _kingFile)
                    {
                        if (rank == _whitePieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(KingWhite, currentBoardPosition);
                        }
                        if (rank == _blackPieceRank) 
                        {
                            instantiatedPiece = MakeChessPiece(KingBlack, currentBoardPosition);
                        }
                    }
                }
                else if (rank == _whitePawnRank) 
                {
                    instantiatedPiece = MakeChessPiece(PawnWhite, currentBoardPosition);
                }
                else if (rank == _blackPawnRank) 
                {
                    instantiatedPiece = MakeChessPiece(PawnBlack, currentBoardPosition);
                }
                
                //  Send rank and file to the instantiated piece (separated from transform)
                if (instantiatedPiece != null) 
                {
                    instantiatedPiece.Position = currentBoardPosition;
                    instantiatedPiece = null;
                }
            }
        }
    }

    //  Iterate through 64 squares and create black and white squares
    private void PlaceSquares()
    {
        Vector3 bottomLeftBoard = new Vector2 (0,0);
        Vector3 currentBoardPosition = bottomLeftBoard;
        Square instantiatedSquare = null;

        for (int rank = 0; rank < 8; rank++)
        {
            currentBoardPosition.y = rank;

            for (int file = 0; file < 8; file++)
            {
                currentBoardPosition.x = file;

                if (rank % 2 == 0)
                {
                    if (file % 2 == 0) 
                    {
                        instantiatedSquare = MakeSquare(SquareBlack, currentBoardPosition);
                    }
                    else 
                    {
                        instantiatedSquare = MakeSquare(SquareWhite, currentBoardPosition);
                    }
                }
                else
                {
                    if (file % 2 == 0) 
                    {
                        instantiatedSquare = MakeSquare(SquareWhite, currentBoardPosition);
                    }
                    else 
                    {
                        instantiatedSquare = MakeSquare(SquareBlack, currentBoardPosition);
                    }
                }

                //  Send rank and file to the instantiated square (separated from transform)
                if (instantiatedSquare != null) 
                {
                    instantiatedSquare.Position = currentBoardPosition;
                    instantiatedSquare.SetDefaultColour();
                    instantiatedSquare = null;
                }
            }
        }
    }

    //  Set which side the pieces are on
    private void DetermineSides()
    {
        if (GameController.PlayerTag == Constants.WHITE_TAG)
        {
            _blackPawnRank = 6;
            _blackPieceRank = 7;
            _whitePawnRank = 1;
            _whitePieceRank = 0;
            _queenFile = 3;
            _kingFile = 4;
        }
        else
        {
            _blackPawnRank = 1;
            _blackPieceRank = 0;
            _whitePawnRank = 6;
            _whitePieceRank = 7;
            _queenFile = 4;
            _kingFile = 3;
        }
    }

    //  Instantiate piece at currentBoardPosition in SetupBoard() with the piece z index
    //  Return piece so PlacePieces() can set position separate from transform position
    private Piece MakeChessPiece(GameObject piece, Vector2 position)
    {
        Vector3 pieceVector = new Vector3 (position.x, position.y, Constants.PIECE_Z_INDEX);
        Piece instantiatedPiece = Instantiate(piece, pieceVector, transform.rotation).GetComponent<Piece>();
        return instantiatedPiece;
    }

    //  Instantiate piece at currentBoardPosition in SetupBoard() with the square z index
    //  Return piece so PlaceSquares() can set position separate from transform position
    private Square MakeSquare(GameObject square, Vector2 position)
    {
        Vector3 squareVector = new Vector3 (position.x, position.y, Constants.SQUARE_Z_INDEX);
        Square instantiatedSquare = Instantiate(square, squareVector, transform.rotation).GetComponent<Square>();
        return instantiatedSquare;
    }
}
