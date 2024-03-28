using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  MoveValidator
//      IsCastle
//      IsNormalRookMove
public class Rook : Piece
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<General.PossibleMove> GeneratePossibleMoves()
    {
        return possiblePieceMovesList;
    }
    
    public override void MoveAttempt(Vector3 targetPosition)
    {
        Debug.Log("MoveAttempt Start.");
        MoveExecutor(targetPosition);
    }
}
