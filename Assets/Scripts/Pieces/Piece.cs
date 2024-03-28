using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Virtual MoveValidator for child classes to override
//  MoveExecutor moves the piece and can't be overridden
//  DestroyInstance which destroys the piece and can't be overridden
public class Piece : MonoBehaviour
{
    protected bool firstMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void MoveValidator(Vector3 targetPosition)
    {
        
    }

    protected void DestroyInstance()
    {
        Destroy(gameObject);
    }

    protected void MoveExecutor(Vector3 targetPosition)
    {
        Debug.Log("MoveExecutor Start.");
        this.transform.position = targetPosition;
    }

    //
    // Accessors
    //

    protected bool GetFirstMove()
    {
        return firstMove;
    }

    //
    // Mutators
    //

    protected void SetFirstMove(bool firstMoveSetter)
    {
        firstMove = firstMoveSetter;
    }
}
