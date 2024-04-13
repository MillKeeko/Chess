using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class BotController : MonoBehaviour
{
    public delegate void OnValidBotMove(Piece piece, Vector2 targetPosition);
    public static event OnValidBotMove OnValidBotMoveEvent;

    void Awake()
    {
        GameController.OnBotMoveEvent += MakeMove;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        BoardController.ExecuteMove(piece, targetPosition);
        OnValidBotMoveEvent?.Invoke(piece, targetPosition);
        //Debug.Log("Bot MakeRandomMove End");
    }
}
