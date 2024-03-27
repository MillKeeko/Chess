using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Virtual MoveValidator for child classes to override
//  MoveExecutor moves the piece and can't be overridden
//  DestroyInstance which destroys the piece and can't be overridden
public class Piece : MonoBehaviour
{
    protected Piece[,] pieceTracker;
    protected Piece[,] tempPieceTracker;

    void Awake()
    {
        pieceTracker = new Piece[8,8];
        tempPieceTracker = new Piece[8,8];
    }

    // Start is called before the first frame update
    void Start()
    {
        TrackingHandler.onUpdatePieceTracker += UpdatePieceTracker;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void MoveValidator()
    {
        
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
    }

    protected void MoveExecutor(Piece piece, Vector3 targetPosition)
    {
        
    }

    private void UpdatePieceTracker(Piece[,] updatedPieceTracker)
    {
        pieceTracker = updatedPieceTracker;
    }
}
