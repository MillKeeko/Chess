using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Handles all write access to the pieceTracker array
//  Handles all write access to the tempPieceTracker array
public class TrackingHandler : MonoBehaviour
{
    private Piece[,] pieceTracker;
    private Piece[,] tempPieceTracker;

    public delegate void OnUpdatePieceTracker(Piece[,] updatedPieceTracker);
    public static event OnUpdatePieceTracker onUpdatePieceTracker;

    void Awake()
    {
        pieceTracker = new Piece[8,8];
        tempPieceTracker = new Piece[8,8];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AnalyzeBoard()
    {
        //  Read the entire board and add to piece tracker
        //  Trigger onUpdatePieceTracker event once
        onUpdatePieceTracker!.Invoke(pieceTracker);
    }
}
