using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Handles all piece movement input from player
//  Handles all UI input from player
public class InputHandler : MonoBehaviour
{
    public GameController gameController;

    private Piece selectedPiece, square;

    void Update()
    {
        HandleMouseClick();
    }

    private void HandleMouseClick()
    {
        Vector3 targetPosition;

         // Handle mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null)
            {
                // Check player selecting their piece
                if (hit.collider.CompareTag(GameController.playerTag) && selectedPiece == null)
                {
                    selectedPiece = hit.collider.GetComponent<Piece>();
                    Debug.Log("Piece selected " + selectedPiece);
                }
                // Check player making a move
                else if ((hit.collider.CompareTag(GameController.botTag) && selectedPiece != null) ||
                         (hit.collider.CompareTag(Constants.SQUARE_TAG) && selectedPiece != null))
                {
                    targetPosition = new Vector3 (hit.collider.transform.position.x, hit.collider.transform.position.y, Constants.PIECE_Z_INDEX);
                    selectedPiece.MoveValidator(hit.collider.transform.position);
                    selectedPiece = null; // reset selected piece
                    Debug.Log("Move Attempted.");
                }
                // Else player not selecting or moving anything
                else 
                {
                    Debug.Log("Nothing Selected.");
                    selectedPiece = null;
                }
            }
        }
    }
}
