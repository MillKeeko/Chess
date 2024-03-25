using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupController : MonoBehaviour
{
    public GameController gameController;

    public GameObject SquareBlack, SquareWhite;
    public GameObject PawnWhite, RookWhite, KnightWhite, BishopWhite, QueenWhite, KingWhite;
    public GameObject PawnBlack, RookBlack, KnightBlack, BishopBlack, QueenBlack, KingBlack;

    private Vector3 bottomLeftBoard = new Vector3(0, 0, 0f);
    private const int zIndex = -1;
    public delegate void OnPieceCreated (Piece piece, int x, int y);
    public static event OnPieceCreated onPieceCreated;

    // Start is called before the first frame update
    void Start()
    {
        SetupBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Instantiate chess pieces and place them in the array
    private void MakeChessPiece(GameObject piece, Vector3 position)
    {
        Vector3 pieceVector = new Vector3 (position.x, position.y, zIndex);
        Piece instantiatedPiece = Instantiate(piece, pieceVector, transform.rotation).GetComponent<Piece>();
        onPieceCreated?.Invoke(instantiatedPiece, (int)position.x, (int)position.y);
    }

    // Instantiate squares and place them in the array
    private void MakeSquare(GameObject square, Vector3 position)
    {
        Instantiate(square, position, transform.rotation);
    }

    // Create board and place pieces, done at the beginning of the game
    public void SetupBoard()
    {
        int row = 0;
        int col = 0;

        Vector3 currentBoardPosition = bottomLeftBoard;

        for (row = 0; row < 8; row++)
        {
            for (col = 0; col < 8; col++)
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
                if (row == 0 || row == 7)
                {
                    switch (col)
                    {
                        case 0: case 7:
                            if (row == 0) MakeChessPiece(RookWhite, currentBoardPosition);
                            if (row == 7) MakeChessPiece(RookBlack, currentBoardPosition);
                            break;
                        case 1: case 6:
                            if (row == 0) MakeChessPiece(KnightWhite, currentBoardPosition);
                            if (row == 7) MakeChessPiece(KnightBlack, currentBoardPosition);
                            break;
                        case 2: case 5:
                            if (row == 0) MakeChessPiece(BishopWhite, currentBoardPosition);
                            if (row == 7) MakeChessPiece(BishopBlack, currentBoardPosition);
                            break;
                        case 3:
                            if (row == 0) MakeChessPiece(QueenWhite, currentBoardPosition);
                            if (row == 7) MakeChessPiece(QueenBlack, currentBoardPosition);
                            break;
                        case 4:
                            if (row == 0) MakeChessPiece(KingWhite, currentBoardPosition);
                            if (row == 7) MakeChessPiece(KingBlack, currentBoardPosition);
                            break;
                    }
                }
                else if (row == 1) MakeChessPiece(PawnWhite, currentBoardPosition);
                else if (row == 6) MakeChessPiece(PawnBlack, currentBoardPosition);
            }
        }
    }
}
