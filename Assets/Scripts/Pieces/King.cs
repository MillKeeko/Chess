using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  MoveValidator
//      IsNormalKingMove
//      IsCastle
public class King : Piece
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
