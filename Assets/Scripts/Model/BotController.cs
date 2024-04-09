using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class BotController : MonoBehaviour
{
    private List<Piece> pieceList;
    private List<General.PossibleMove> possibleBotMovesList;

    public delegate void OnValidBotMove(Piece piece, Vector2 targetPosition);
    public static event OnValidBotMove OnValidBotMoveEvent;

    void Awake()
    {
        pieceList = new List<Piece>();
        possibleBotMovesList = new List<General.PossibleMove>();
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
        //Debug.Log("Bot MakeMove start");
        pieceList = GeneratePieceList(GameController.BotTag);
        possibleBotMovesList = CompilePossibleMoves(pieceList);
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

        Piece piece = move.SelectedPiece;
        Vector3 targetPosition = new Vector3(move.TargetPosition.x, move.TargetPosition.y, Constants.PIECE_Z_INDEX);

        BoardController.ExecuteMove(piece, targetPosition);
        OnValidBotMoveEvent?.Invoke(piece, targetPosition);
        //Debug.Log("Bot MakeRandomMove End");
    }

    private List<General.PossibleMove> CompilePossibleMoves(List<Piece> pieceList)
    {
        List<General.PossibleMove> possibleMoveList = new List<General.PossibleMove>();

        foreach (Piece piece in pieceList)
        {
            possibleMoveList.AddRange(piece.PossibleMovesList);
        }

        //Debug.Log("Possible bot move list length " + possibleMoveList.Count);
        return possibleMoveList;
    }

    private List<Piece> GeneratePieceList(string tag)
    {
        Piece piece = null;
        List<Piece> pieceList = new List<Piece>();

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                piece = TrackingHandler.pieceTracker[rank, file];
                if (piece != null && piece.CompareTag(tag))
                {
                    pieceList.Add(piece);
                }

            }
        }
        //Debug.Log("Bot piece list length = " + pieceList.Count);
        return pieceList;
    } 
}
