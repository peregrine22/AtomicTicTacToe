using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class TagTypes
{
    public const string BOARD_BOX = "BoardBox";
}

enum GameResult { 
    TIE = 0,
    PLAYER_X_WON = 1,
    PLAYER_O_WON = -1
}

public class GameManager : MonoBehaviour
{
    Board board;
    Box[] boardBoxes;

    Mark currentMark;

    public GameObject playerX;
    public GameObject playerO;
    //public GameObject[] players;

    bool isGameOver;
    bool isBoardAvailable;

    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        boardBoxes = board.GetBoardBoxes();

        isGameOver = false; 
        isBoardAvailable = true;

        currentMark = Mark.X;
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.tag == TagTypes.BOARD_BOX)
                    {
                        MakeMove(hit.transform.GetComponent<Box>());
                        var winner = CheckWinCondition();

                        if (winner != null)
                        {
                            isGameOver = true;
                        }

                    }
                }

                currentMark = currentMark == Mark.X ? Mark.O : Mark.X;

                NextTurn();
            }
        }
    }

    void MakeMove(Box box)
    {
        if (!box.IsMarked)
        {
            box.SetAsMarked(currentMark == Mark.X ? playerX : playerO, currentMark);

            isBoardAvailable = boardBoxes.Any(element => element.Mark == Mark.None);




        }

    }

    int? CheckWinCondition()
    {
        bool winner = Equals3(0, 1, 2)
            || Equals3(0, 3, 6)
            || Equals3(0, 4, 8)
            || Equals3(3, 4, 5)
            || Equals3(1, 4, 7)
            || Equals3(2, 4, 6)
            || Equals3(6, 7, 8)
            || Equals3(2, 5, 8);

        bool tie = !winner && !isBoardAvailable;

        if (winner && currentMark == Mark.X)
            return 1;
        if (winner && currentMark == Mark.O)
            return -1;
        if (tie)
            return 0;

        return null;
    }

    bool Equals3(int a, int b, int c)
    {
        bool isEqualsToCurrentMark = 
            boardBoxes[a].Mark == currentMark && 
            boardBoxes[b].Mark == currentMark && 
            boardBoxes[c].Mark == currentMark;

        return isEqualsToCurrentMark;
    }

    void NextTurn()
    {
        Debug.Log("Next turn");
        Box[] availableBoxes = Array.FindAll(boardBoxes, box => box.IsMarked == false);

        System.Random rnd = new();
        Box randomBox = availableBoxes[rnd.Next(1, availableBoxes.Length)];

        MakeMove(randomBox);

        //randomBox.SetAsMarked(playerO, currentMark);
        //if (CheckWinCondition())
        //{
        //    isGameOver = true;
        //    return;
        //}

        //currentMark = currentMark == Mark.X ?
        //Mark.O : Mark.X;
    }

    int Minimax(Box[] boardState, int depth, bool isMaximizing)
    {
        return 1;
    }
}
