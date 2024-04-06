using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

//  Controls game start
//  Controls board setup
//  Controls changing turns
//  Controls game end
public class GameController : MonoBehaviour
{
    //  Static Variables
    public static string PlayerTag { get; private set; }
    public static string BotTag { get; private set; }
    public static string Turn { get; private set; }


    //  
    //  Events & Delegates
    //  
    public delegate void OnBotMove();
    public static event OnBotMove OnBotMoveEvent;
    public delegate void OnChangeTurn(string turn);
    public static event OnChangeTurn OnChangeTurnEvent;

    void Awake()
    {
        Turn = Constants.WHITE_TAG;
        RandomTeams();
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckBotFirstTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckBotFirstTurn()
    {
        if (Turn == BotTag)
        {
            //Debug.Log("Bot goes first.");
            OnBotMoveEvent?.Invoke();
        } 
    }

    private void ChangeTurn()
    {
        if (Turn == Constants.WHITE_TAG) Turn = Constants.BLACK_TAG;
        else Turn = Constants.WHITE_TAG;
        OnChangeTurnEvent?.Invoke(Turn);
        if (Turn == BotTag) 
        {
            OnBotMoveEvent?.Invoke();
        }
    }

    private void RandomTeams()
    {
        int playerSide = Random.Range(0,2); 
        if (playerSide == 0) 
        {   
            PlayerTag = Constants.WHITE_TAG;
            BotTag = Constants.BLACK_TAG;
        }
        else if (playerSide == 1) 
        {
            PlayerTag = Constants.BLACK_TAG;
            BotTag = Constants.WHITE_TAG;
        }
    }
}
