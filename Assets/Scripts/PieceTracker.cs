using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceTracker : MonoBehaviour
{
    private const int zIndex = -1;
    private Piece[,] pieceTracker = new Piece[8,8];

    // Start is called before the first frame update
    void Start()
    {
        GameController.onCheckPieceBlocking += IsPieceBlocking;
        GameController.onNeedPieceAtLocation += GetPiece;
        GameController.onNeedSetPieceAtLocation += SetPiece;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IsPieceBlocking(Piece piece, Vector3 target)
    {
        // Find the distance the piece is moving
        int distanceX = (int)target.x - (int)piece.transform.position.x;
        int distanceY = (int)target.y - (int)piece.transform.position.y;
        int distance = 0;
        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY)) distance = Mathf.Abs(distanceX);
        else distance = Mathf.Abs(distanceY);

        // Create unitary vector of the target relative to the piece
        Vector3 moveVector = new Vector3((int)target.x - (int)piece.transform.position.x,
                                         (int)target.y - (int)piece.transform.position.y, zIndex);
        Vector3 unitMoveVector = moveVector.normalized;

        // Simplify unit vector
        int x, y = 0;
        if (unitMoveVector.x > 0) x = 1;
        else if (unitMoveVector.x < 0) x = -1;
        else x = 0;
        if (unitMoveVector.y > 0) y = 1;
        else if (unitMoveVector.y < 0) y = -1;
        else y = 0;

        // Check if any pieces are in the way
        //Debug.Log("Distance " + distance);
        //Debug.Log("Target x " + target.x + " y " + target.y);
        for (int i = 1; i < Mathf.Abs(distance); i++)
        {
            Vector3 checkPosition = new Vector3(piece.transform.position.x + (x * i), piece.transform.position.y + (y * i), zIndex);
            // Debug.Log("IsPieceBlocking() checking " + piece + " at x " + piece.transform.position.x + " y " + piece.transform.position.y);
            if (pieceTracker[(int)checkPosition.x,(int)checkPosition.y] != null) 
            {
                return true;
            }
            //Debug.Log("No piece in the way at x " + (int)checkPosition.x + " and y " + (int)checkPosition.y);
        }

        // If piece is king check 1 move
        if (piece is King)
        {
            Vector3 checkPosition = new Vector3(piece.transform.position.x + x, piece.transform.position.y + y, zIndex);
            if (pieceTracker[(int)checkPosition.x,(int)checkPosition.y] != null) return true;
        }

        return false;
    }

    private Piece GetPiece(int x, int y)
    {
        return pieceTracker[x,y];
    }

    private void SetPiece(Piece piece, int x, int y)
    {
        pieceTracker[x,y] = piece;
    }
}
