using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class TrackingHandler : MonoBehaviour
{
    public static TrackingHandler instance { get; private set; }

    public static Piece[,] pieceTracker;
    public static int TrackerCount = 0;

    public delegate void OnTrackerUpdated();
    public static event OnTrackerUpdated OnTrackerUpdatedEvent;
    public delegate void OnTrackerReady();
    public static event OnTrackerReady OnTrackerReadyEvent;
    public delegate void OnPromotionSelect();
    public static event OnPromotionSelect OnPromotionSelectEvent;
    public delegate void OnPromotionNeedPosition(Vector2 targetPosition);
    public static event OnPromotionNeedPosition OnPromotionNeedPositionEvent;

    private Piece _pieceRemovedTestCheck;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            enabled = false;
        }
        else
        {
            instance = this;
        }

        pieceTracker = new Piece [8,8];
        _pieceRemovedTestCheck = null;
        
        Piece.OnPieceCreatedEvent += AddToTracker;

        MoveController.OnMoveEvent += UpdateTracker;
        MoveController.OnEnPassantEvent += UpdateTrackerEnPassant;

        CheckHandler.OnTestRemoveCheckEvent += TestRemoveCheck;
        CheckHandler.OnRevertTestRemoveCheckEvent += RevertTestRemoveCheck;
    }

    private static bool IsPawnPromotion(Piece piece, Vector2 targetPosition)
    {
        bool returnBool = false;
        if (piece is not Pawn) return returnBool;

        if (piece.CompareTag(GameController.BotTag))
        {
            if (targetPosition.y == 0) returnBool = true;
        }
        else if (piece.CompareTag(GameController.PlayerTag))
        {
            if (targetPosition.y == 7) returnBool = true;
        }

        return returnBool;
    }

    private void RevertTestRemoveCheck(Piece piece, Vector2 targetPosition)
    {
        pieceTracker[(int)targetPosition.x, (int)targetPosition.y] = _pieceRemovedTestCheck;
        pieceTracker[(int)piece.Position.x, (int)piece.Position.y] = piece;
        _pieceRemovedTestCheck = null;
    }

    private void TestRemoveCheck(Piece piece, Vector2 targetPosition)
    {
        pieceTracker[(int)piece.Position.x, (int)piece.Position.y] = null;
        _pieceRemovedTestCheck = pieceTracker[(int)targetPosition.x, (int)targetPosition.y];
        pieceTracker[(int)targetPosition.x, (int)targetPosition.y] = piece;
    }

    private void UpdateTracker(Piece piece, Vector2 targetPosition)
    {
        pieceTracker[(int)piece.Position.x, (int)piece.Position.y] = null;
        if (pieceTracker[(int)targetPosition.x, (int)targetPosition.y] != null)
        {
            TrackerCount--;
            pieceTracker[(int)targetPosition.x, (int)targetPosition.y].DestroyPiece();
        }
        pieceTracker[(int)targetPosition.x, (int)targetPosition.y] = piece;
        piece.Position = targetPosition;

        if (IsPawnPromotion(piece, targetPosition)) 
        {
            OnPromotionSelectEvent?.Invoke(); 
            OnPromotionNeedPositionEvent?.Invoke(targetPosition);
        }
        else
        {
            OnTrackerUpdatedEvent?.Invoke();
        }
    }

    private void UpdateTrackerEnPassant(Piece piece, Vector2 targetPosition)
    {
        pieceTracker[(int)piece.Position.x, (int)piece.Position.y] = null;
        if (targetPosition.y == 5)
        {
            TrackerCount--;
            pieceTracker[(int)targetPosition.x, ((int)targetPosition.y - 1)].DestroyPiece();
        }
        else if (targetPosition.y == 2)
        {
            TrackerCount--;
            pieceTracker[(int)targetPosition.x, ((int)targetPosition.y + 1)].DestroyPiece();
        }
        pieceTracker[(int)targetPosition.x, (int)targetPosition.y] = piece;
        piece.Position = targetPosition;
        OnTrackerUpdatedEvent?.Invoke();
    }

    // _trackerCount only useful on initialization. I forsee this being redone for game restart.
    private void AddToTracker (Piece piece)
    {
        // This is for pawn promotions
        if (pieceTracker[(int)piece.Position.x, (int)piece.Position.y] != null)
        {
            pieceTracker[(int)piece.Position.x, (int)piece.Position.y].DestroyPiece();
            pieceTracker[(int)piece.Position.x, (int)piece.Position.y] = piece;
            OnTrackerUpdatedEvent?.Invoke();
        }
        else // This is for board setup
        {
            pieceTracker[(int)piece.Position.x, (int)piece.Position.y] = piece;
            TrackerCount++;
            if (TrackerCount >= 32) 
            {
                OnTrackerReadyEvent?.Invoke();
            }
        }
        
    }

    //  Could be useful on game restart?
    private void PurgeTracker()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                pieceTracker[rank, file] = null;
            }
        }
    }
}

//public GameObject circle;

//private float timer = 0;
//private Vector3 circlePosition;

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