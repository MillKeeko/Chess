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
    
    public override void MoveValidator(Vector3 targetPosition)
    {
        Debug.Log("MoveValidator Start.");
        MoveExecutor(targetPosition);
    }
}
