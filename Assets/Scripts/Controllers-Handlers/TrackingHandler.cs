using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Handles all write access to the pieceTracker array
//  Handles all write access to the tempPieceTracker array
public class TrackingHandler : MonoBehaviour
{
    public GameObject circle;
    public static Piece[,] pieceTracker;

    void Awake()
    {
        pieceTracker = new Piece [8,8];
    }

    // Start is called before the first frame update
    void Start()
    {
        GameController.onPieceCreated += AddToTracker;
        Piece.onPieceMoved += PieceMovedPosition;
        Piece.onPieceDestroyed += RemoveFromTracker;
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

    private void PieceMovedPosition(Piece piece, int oldX, int oldY)
    {
        int newX = (int)piece.transform.position.x;
        int newY = (int)piece.transform.position.y;

        RemoveFromTracker(oldX, oldY);
        AddToTracker(piece, newX, newY);
    }

    private void RemoveFromTracker(int x, int y)
    {
        pieceTracker[x,y] = null;
    }

    private void AddToTracker(Piece piece, int x, int y)
    {
        pieceTracker[x,y] = piece;
    }
}
