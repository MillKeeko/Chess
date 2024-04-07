using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class TrackingHandler : MonoBehaviour
{
    public static TrackingHandler instance { get; private set; }

    public GameObject circle;
    public static Piece[,] pieceTracker;

    private int _trackerCount = 0;

    public delegate void OnTrackerUpdated();
    public static event OnTrackerUpdated OnTrackerUpdatedEvent;
    public delegate void OnTrackerReady();
    public static event OnTrackerReady OnTrackerReadyEvent;

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
        Piece.OnPieceCreatedEvent += AddToTracker;
        Piece.OnValidMoveEvent += UpdateTracker;
        BotController.OnValidBotMoveEvent += UpdateTracker;
    }

    // Start is called before the first frame update
    void Start()
    {
   
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

    private void UpdateTracker(Piece piece, Vector2 targetPosition)
    {
        pieceTracker[(int)piece.Position.x, (int)piece.Position.y] = null;
        pieceTracker[(int)targetPosition.x, (int)targetPosition.y] = piece;
        piece.Position = targetPosition;
        if (piece.FirstMove) piece.FirstMove = false;
        OnTrackerUpdatedEvent?.Invoke();
    }

    // _trackerCount only useful on initialization. I forsee this being redone for game restart.
    private void AddToTracker (Piece piece)
    {
        pieceTracker[(int)piece.Position.x, (int)piece.Position.y] = piece;
        _trackerCount++;
        if (_trackerCount >=32)
        {
            OnTrackerReadyEvent?.Invoke();
            _trackerCount = 0;
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
