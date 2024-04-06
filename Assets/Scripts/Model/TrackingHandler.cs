using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model
//  Singleton

public class TrackingHandler : MonoBehaviour
{
    public static TrackingHandler instance { get; private set; }

    public GameObject circle;
    public static Piece[,] pieceTracker;

    
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
        
    }

    // Start is called before the first frame update
    void Start()
    {
        GameController.OnGameStartEvent += UpdateTracker;
        //Piece.onPieceMoved += PieceMovedPosition;
        //Piece.onPieceDestroyed += RemoveFromTracker;
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

    private void UpdateTracker()
    {
        PurgeTracker();
        AddToTracker();
    }

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

    private void AddToTracker()
    {
        Piece[] pieceArray = GameObject.FindObjectsOfType<Piece>();

        Debug.Log(pieceArray.Length);

        foreach (Piece piece in pieceArray)
        {
            pieceTracker[(int)piece.Position.x, (int)piece.Position.y] = piece;
            Debug.Log(piece + " added to pieceTracker at x " + (int)piece.Position.x + " y " + (int)piece.Position.y);
        }   
    }
}
