using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBonuses : MonoBehaviour
{
    public static PieceBonuses instance { get; private set; }

    public static readonly int[,] PawnWhiteValues, PawnBlackValues;
    public static readonly int[,] KnightWhiteValues, KnightBlackValues;
    public static readonly int[,] BishopWhiteValues, BishopBlackValues;
    public static readonly int[,] RookWhiteValues, RookBlackValues;
    public static readonly int[,] QueenWhiteValues, QueenBlackValues;
    public static readonly int[,] KingWhiteValues, KingBlackValues;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            enabled = false;
        }
        else
        {
            instance = this;
        }
    }

    static PieceBonuses()
    {
        PawnWhiteValues = new int[8,8]
        {
            {0,  0,  0,  0,  0,  0,  0,  0},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {5,  5, 10, 25, 25, 10,  5,  5},
            {0,  0,  0, 20, 20,  0,  0,  0},
            {5, -5,-10,  0,  0,-10, -5,  5},
            {5, 10, 10,-20,-20, 10, 10,  5},
            {0,  0,  0,  0,  0,  0,  0,  0}
        };

        PawnBlackValues = new int[8,8]
        {
            {0,  0,  0,  0,  0,  0,  0,  0},
            {5, 10, 10,-20,-20, 10, 10,  5},
            {5, -5,-10,  0,  0,-10, -5,  5},
            {0,  0,  0, 20, 20,  0,  0,  0},
            {5,  5, 10, 25, 25, 10,  5,  5},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {0,  0,  0,  0,  0,  0,  0,  0}
        };

        KnightWhiteValues = new int[8,8]
        {
            {-50,-40,-30,-30,-30,-30,-40,-50},
            {-40,-20,  0,  0,  0,  0,-20,-40},
            {-30,  0, 10, 15, 15, 10,  0,-30},
            {-30,  5, 15, 20, 20, 15,  5,-30},
            {-30,  0, 15, 20, 20, 15,  0,-30},
            {-30,  5, 10, 15, 15, 10,  5,-30},
            {-40,-20,  0,  5,  5,  0,-20,-40},
            {-50,-40,-30,-30,-30,-30,-40,-50}
        };

        KnightBlackValues = new int[8,8]
        {
            {-50,-40,-30,-30,-30,-30,-40,-50},
            {-40,-20,  0,  5,  5,  0,-20,-40},
            {-30,  5, 10, 15, 15, 10,  5,-30},
            {-30,  0, 15, 20, 20, 15,  0,-30},
            {-30,  5, 15, 20, 20, 15,  5,-30},
            {-30,  0, 10, 15, 15, 10,  0,-30},
            {-40,-20,  0,  0,  0,  0,-20,-40},
            {-50,-40,-30,-30,-30,-30,-40,-50}
        };

        BishopWhiteValues = new int[8,8]
        {
            {-20,-10,-10,-10,-10,-10,-10,-20},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-10,  0,  5, 10, 10,  5,  0,-10},
            {-10,  5,  5, 10, 10,  5,  5,-10},
            {-10,  0, 10, 10, 10, 10,  0,-10},
            {-10, 10, 10, 10, 10, 10, 10,-10},
            {-10,  5,  0,  0,  0,  0,  5,-10},
            {-20,-10,-10,-10,-10,-10,-10,-20}
        };

        BishopBlackValues = new int[8,8]
        {
            {-20,-10,-10,-10,-10,-10,-10,-20},
            {-10,  5,  0,  0,  0,  0,  5,-10},
            {-10, 10, 10, 10, 10, 10, 10,-10},
            {-10,  0, 10, 10, 10, 10,  0,-10},
            {-10,  5,  5, 10, 10,  5,  5,-10},
            {-10,  0,  5, 10, 10,  5,  0,-10},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-20,-10,-10,-10,-10,-10,-10,-20}
        };

        RookWhiteValues = new int[8,8]
        {
            {0,  0,  0,  0,  0,  0,  0,  0},
            {5, 10, 10, 10, 10, 10, 10,  5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {0,  0,  0,  5,  5,  0,  0,  0}
        };

        RookBlackValues = new int[8,8]
        {
            {0,  0,  0,  0,  0,  0,  0,  0},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {5, 10, 10, 10, 10, 10, 10,  5},
            {0,  0,  0,  5,  5,  0,  0,  0}
        };

        QueenWhiteValues = new int[8,8]
        {
            {-20,-10,-10, -5, -5,-10,-10,-20},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-10,  0,  5,  5,  5,  5,  0,-10},
            {-5,  0,  5,  5,  5,  5,  0, -5},
            {0,  0,  5,  5,  5,  5,  0, -5},
            {-10,  5,  5,  5,  5,  5,  0,-10},
            {-10,  0,  5,  0,  0,  0,  0,-10},
            {-20,-10,-10, -5, -5,-10,-10,-20}
        };

        QueenBlackValues = new int[8,8]
        {
            {-20,-10,-10, -5, -5,-10,-10,-20},
            {-10,  0,  5,  0,  0,  0,  0,-10},
            {-10,  5,  5,  5,  5,  5,  0,-10},
            {0,  0,  5,  5,  5,  5,  0, -5},
            {-5,  0,  5,  5,  5,  5,  0, -5},
            {-10,  0,  5,  5,  5,  5,  0,-10},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-20,-10,-10, -5, -5,-10,-10,-20}
        };

        KingWhiteValues = new int[8,8]
        {
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-20,-30,-30,-40,-40,-30,-30,-20},
            {-10,-20,-20,-20,-20,-20,-20,-10},
            {20, 20,  0,  0,  0,  0, 20, 20},
            {20, 30, 10,  0,  0, 10, 30, 20}
        };

        KingBlackValues = new int[8,8]
        {
            {20, 30, 10,  0,  0, 10, 30, 20},
            {20, 20,  0,  0,  0,  0, 20, 20},
            {-10,-20,-20,-20,-20,-20,-20,-10},
            {-20,-30,-30,-40,-40,-30,-30,-20},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30}
        };
    }

    public static int GetPieceBonus(Piece piece, int row, int col)
    {
        int bonus = 0;

        if (piece is Pawn)
        {
            if (GameController.BotTag == Constants.WHITE_TAG)
            {
                bonus = PawnWhiteValues[row,col];
            }
            else if (GameController.BotTag == Constants.BLACK_TAG)
            {
                bonus = PawnBlackValues[row,col];
            }
        }
        else if (piece is Knight)
        {
            if (GameController.BotTag == Constants.WHITE_TAG)
            {
                bonus = KnightWhiteValues[row,col];
            }
            else if (GameController.BotTag == Constants.BLACK_TAG)
            {
                bonus = KnightBlackValues[row,col];
            }
        }
        else if (piece is Bishop)
        {
            if (GameController.BotTag == Constants.WHITE_TAG)
            {
                bonus = BishopWhiteValues[row,col];
            }
            else if (GameController.BotTag == Constants.BLACK_TAG)
            {
                bonus = BishopBlackValues[row,col];
            }
        }
        else if (piece is Rook)
        {
            if (GameController.BotTag == Constants.WHITE_TAG)
            {
                bonus = RookWhiteValues[row,col];
            }
            else if (GameController.BotTag == Constants.BLACK_TAG)
            {
                bonus = RookBlackValues[row,col];
            }
        }
        else if (piece is Queen)
        {
            if (GameController.BotTag == Constants.WHITE_TAG)
            {
                bonus = QueenWhiteValues[row,col];
            }
            else if (GameController.BotTag == Constants.BLACK_TAG)
            {
                bonus = QueenBlackValues[row,col];
            }
        }
        else if (piece is King)
        {
            if (GameController.BotTag == Constants.WHITE_TAG)
            {
                bonus = KingWhiteValues[row,col];
            }
            else if (GameController.BotTag == Constants.BLACK_TAG)
            {
                bonus = KingBlackValues[row,col];
            }
        }

        return bonus;
    }
}
