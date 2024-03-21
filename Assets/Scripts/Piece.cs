using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{ 
    public GameObject pieceObject = null;

    public virtual bool TakePiece(Vector3 targetPiecePosition)
    {
        return false;
    }

    public virtual bool MovePiece(Vector3 targetSquarePosition)
    {
        return false;
    }

    public virtual bool GetFirstMove()
    {
        return false;
    }

    public virtual void SetFirstMove(bool set)
    {
        return;
    }
    
    public void DestroyInstance()
    {
        Destroy(gameObject);
    }
}
