using System;
using System.Linq;
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

    Mark currentMark;

    public GameObject playerX;
    public GameObject playerO;
    //public GameObject[] players;

    bool isGameOver;
    bool isBoardAvailable;

    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();

        isGameOver = false; 
        isBoardAvailable = true;

        currentMark = Mark.X;
    }

    void Update()
    {

        if (!isGameOver && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag == TagTypes.BOARD_BOX)
                {
                    MakeMove(hit.transform.GetComponent<Box>());
                }
            }

        }
    }

    void MakeMove(Box box)
    {
        if (!box.IsMarked)
        {
            box.SetAsMarked(currentMark == Mark.X ? playerX : playerO, currentMark);

            isBoardAvailable = board.BoardBoxes.Any(element => element.Mark == Mark.None);

            var winner = CheckWinCondition();

            if (winner != null)
            {
                if (winner == 1)
                {
                    Debug.Log("winner is X");
                }
                else if (winner == -1)
                {
                    Debug.Log("winner is O");
                }
                else
                {
                    Debug.Log("Tie");
                }

                isGameOver = true;
                return;
            }

            currentMark = currentMark == Mark.X ? Mark.O : Mark.X;

            AutoTurn();
        }
    }

    void AutoTurn()
    {
        Box[] availableBoxes = Array.FindAll(board.BoardBoxes, box => box.IsMarked == false);

        System.Random rnd = new();

        Box randomBox = availableBoxes[rnd.Next(1, availableBoxes.Length)];

        randomBox.SetAsMarked(playerO, currentMark);

        var winner = CheckWinCondition();

        if (winner != null)
        {
            if (winner == 1)
            {
                Debug.Log("winner is X");
            }
            else if (winner == -1)
            {
                Debug.Log("winner is O");
            }
            else
            {
                Debug.Log("Tie");
            }

            isGameOver = true;
            return;
        }

        currentMark = currentMark == Mark.X ? Mark.O : Mark.X;
    }

    int Minimax(Box[] boardState, int depth, bool isMaximizing)
    {
        return 1;
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
            board.BoardBoxes[a].Mark == currentMark &&
            board.BoardBoxes[b].Mark == currentMark &&
            board.BoardBoxes[c].Mark == currentMark;

        return isEqualsToCurrentMark;
    }
}
