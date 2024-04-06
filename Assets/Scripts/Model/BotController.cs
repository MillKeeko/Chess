using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Model

//  Controls all bot move decision making
public class BotController : MonoBehaviour
{
    private List<Piece> pieceList;
    private List<General.PossibleMove> possibleBotMovesList;

    void Awake()
    {
        pieceList = new List<Piece>();
        possibleBotMovesList = new List<General.PossibleMove>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //GameController.OnBotMoveEvent += MakeMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MakeMove()
    {
        //Debug.Log("MakeMove start");
        pieceList = General.GeneratePieceList(GameController.BotTag);
        possibleBotMovesList = General.CompilePossibleMoves(pieceList);
        MakeRandomMove();
        EmptyLists();
    }

    private void EmptyLists()
    {
        pieceList.Clear(); // Can probably create an event for destroyed pieces
        possibleBotMovesList.Clear(); 
    }

    private void MakeRandomMove()
    {
        int randomIndex = Random.Range(0, possibleBotMovesList.Count);
        General.PossibleMove move = possibleBotMovesList[randomIndex];

        Piece piece = move.MovePiece;
        Vector3 targetPosition = new Vector3(move.X, move.Y, Constants.PIECE_Z_INDEX);

        piece.MoveAttempt(targetPosition);
    }
}
