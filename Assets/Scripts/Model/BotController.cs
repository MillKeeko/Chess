using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class BotController : MonoBehaviour
{
    void Awake()
    {
        GameController.OnBotMoveEvent += MakeMove;
    }

    private void MakeMove()
    {
        MakeRandomMove();
    }

    private void MakeRandomMove()
    {
        //Debug.Log("Bot MakeRandomMove Start");
        int randomIndex = Random.Range(0, GameController.PossibleBotMovesList.Count);
        //Debug.Log("Random index " + randomIndex);
        PossibleMove move = GameController.PossibleBotMovesList[randomIndex];

        Piece piece = move.SelectedPiece;
        Vector3 targetPosition = new Vector3(move.TargetPosition.x, move.TargetPosition.y, Constants.PIECE_Z_INDEX);

        MoveController.PrepareExecuteMove(piece, targetPosition);
        //Debug.Log("Bot MakeRandomMove End");
    }
}
