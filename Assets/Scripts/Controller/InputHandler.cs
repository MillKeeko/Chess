using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller
//  ---------- Can I take the transform logic out of the targetPosition???????
public class InputHandler : MonoBehaviour
{
    public static InputHandler instance { get; private set; }

    private Piece _selectedPiece;
    private Piece _targetPiece;
    private Square _targetSquare;

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
    }

    void Update()
    {
        HandleMouseClick();
    }

    private void HandleMouseClick()
    {
        Vector2 targetPosition;

         // Handle mouse click when it's the player's turn
        if (GameController.Turn == GameController.PlayerTag && Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null)
            {
                //  Check player is selecting their piece
                if (hit.collider.CompareTag(GameController.PlayerTag) && _selectedPiece == null)
                {
                    _selectedPiece = hit.collider.GetComponent<Piece>();
                    Debug.Log("Piece selected " + _selectedPiece);
                }
                //  Check player is trying to take a piece
                else if (hit.collider.CompareTag(GameController.BotTag) && _selectedPiece != null)
                {
                    Debug.Log("Move Attempted.");
                    _targetPiece = hit.collider.GetComponent<Piece>();
                    targetPosition = new Vector2 (_targetPiece.Position.x, _targetPiece.Position.y);
                    _selectedPiece.MoveAttempt(targetPosition);
                    _selectedPiece = null; // reset selected piece
                }
                //  Check player is trying to move a piece
                else if (hit.collider.CompareTag(Constants.SQUARE_TAG) && _selectedPiece != null)
                {
                    Debug.Log("Move Attempted.");
                    _targetSquare = hit.collider.GetComponent<Square>();
                    targetPosition = new Vector2 (_targetSquare.Position.x, _targetSquare.Position.y);
                    _selectedPiece.MoveAttempt(targetPosition);
                    _selectedPiece = null; // reset selected piece
                }
                // Else player not selecting or moving anything
                else 
                {
                    Debug.Log("Nothing Selected.");
                    _selectedPiece = null; // reset selected piece
                }
            }
        }
    }
}
