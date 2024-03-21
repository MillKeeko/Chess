using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameController gameController;

    private Piece selectedPiece, square;

    void Update()
    {
        // Handle mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null)
            {
                // If player clicks a piece, select it
                if (hit.collider.CompareTag("PieceBlack") || hit.collider.CompareTag("PieceWhite"))
                {
                    // Select Piece
                    if (selectedPiece == null) selectedPiece = hit.collider.GetComponent<Piece>();
                    // If piece is already selected and a second piece is selected, try to attack
                    else
                    {
                        // Black attacking White
                        if (selectedPiece.tag == "PieceBlack" && hit.collider.CompareTag("PieceWhite"))
                        {
                            gameController.TryToTakePiece(selectedPiece, hit.collider.GetComponent<Piece>());
                            selectedPiece = null;
                        }
                        // White attacking Black
                        else if (selectedPiece.tag == "PieceWhite" && hit.collider.CompareTag("PieceBlack"))
                        {
                            gameController.TryToTakePiece(selectedPiece, hit.collider.GetComponent<Piece>());
                            selectedPiece = null;
                        }
                        // Select new piece
                        else selectedPiece = hit.collider.GetComponent<Piece>();
                    }
                }
                // If a piece is already selected and a square is selected, move piece to that square
                else if (hit.collider.CompareTag("Square") && selectedPiece != null)
                {
                    square = hit.collider.GetComponent<Piece>();
                    gameController.TryToMovePiece(selectedPiece, square);
                    selectedPiece = null; 
                }
            }
        }
    }
}
