using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//  Controls all bot move decision making
public class BotController : MonoBehaviour
{
    private List<Piece> pieceList;
    private List<General.PossibleMove> possibleBotMovesList = new List<General.PossibleMove>();

    void Awake()
    {
        pieceList = new List<Piece>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameController.onBotTurn += MakeMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MakeMove()
    {
        //Debug.Log("MakeMove start");
        FillPieceList();
        GenerateAllPossibleMoves();
        MakeRandomMove();
        EmptyLists();
    }

    private void EmptyLists()
    {
        pieceList.Clear(); // Can probably create an event for destroyed pieces
        possibleBotMovesList.Clear(); 

        Debug.Log("Total possible bot moves after clear " + possibleBotMovesList.Count);
    }

    private void MakeRandomMove()
    {
        int randomIndex = Random.Range(0, possibleBotMovesList.Count);
        General.PossibleMove move = possibleBotMovesList[randomIndex];

        Piece piece = move.MovePiece;
        Vector3 targetPosition = new Vector3(move.X, move.Y, Constants.PIECE_Z_INDEX);

        piece.MoveAttempt(targetPosition);
    }

    // Method to generate a list of possible moves
    private void GenerateAllPossibleMoves()
    {
        foreach (Piece piece in pieceList)
        {
            possibleBotMovesList.AddRange(piece.GeneratePossibleMoves());
        }
        Debug.Log("Total possible bot moves " + possibleBotMovesList.Count);
    }

    private void FillPieceList()
    {
        Piece[] pieceArray = GameObject.FindObjectsOfType<Piece>();

        foreach (Piece piece in pieceArray)
        {
            if (piece.CompareTag(GameController.botTag))
            {
                pieceList.Add(piece);
                //Debug.Log(piece);
            }
        }
        //Debug.Log(pieceList.Count);
    } 
}
