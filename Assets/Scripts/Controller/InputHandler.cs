using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller
//  ---------- Can I take the transform logic out of the targetPosition???????
public class InputHandler : MonoBehaviour
{
    public static InputHandler instance { get; private set; }
    
    public delegate void OnValidMove(Piece piece, Vector2 targetPosition);
    public static event OnValidMove OnValidMoveEvent;

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
                if (hit.collider.CompareTag(GameController.PlayerTag))
                {
                    _selectedPiece = hit.collider.GetComponent<Piece>();
                    SquareHighlighter.ShowMoveSquares(_selectedPiece, GameController.PossiblePlayerMovesList);
                }
                //  Check player is trying to take a piece
                else if (hit.collider.CompareTag(GameController.BotTag) && _selectedPiece != null)
                {
                    _targetPiece = hit.collider.GetComponent<Piece>();
                    targetPosition = new Vector2 (_targetPiece.Position.x, _targetPiece.Position.y);
                    //Debug.Log("Trying to take piece.");
                    MoveAttempt(targetPosition);
                    _selectedPiece = null; // reset selected piece
                }
                //  Check player is trying to move a piece
                else if (hit.collider.CompareTag(Constants.SQUARE_TAG) && _selectedPiece != null)
                {
                    _targetSquare = hit.collider.GetComponent<Square>();
                    targetPosition = new Vector2 (_targetSquare.Position.x, _targetSquare.Position.y);
                    //Debug.Log("Trying to move piece.");
                    MoveAttempt(targetPosition);
                    _selectedPiece = null; // reset selected piece
                }
                // Else player not selecting or moving anything
                else
                {
                    _selectedPiece = null; // reset selected piece
                }
            }
        }
    }

    public void MoveAttempt(Vector2 targetPosition)
    {
        bool validMove = false;
        //Debug.Log("MoveAttempt PossibleMovesList length " + PossibleMovesList.Count);
        foreach (General.PossibleMove move in GameController.PossiblePlayerMovesList)
        {
            if (targetPosition == move.TargetPosition && _selectedPiece == move.SelectedPiece)
            {
                BoardController.ExecuteMove(_selectedPiece, targetPosition);
                validMove = true;
                if (_selectedPiece.FirstMove) _selectedPiece.FirstMove = false;
                break; // To avoid list changing while executing - it would return after events finish and error
            }
        }
        if (validMove) OnValidMoveEvent?.Invoke(_selectedPiece, targetPosition); // Trigger event here instead (I AM A GENIUS)
    }
}
