using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System.Threading.Tasks;
using System.Threading;

public static class TagTypes
{
    public const string BOARD_BOX = "BoardBox";
}

enum GameResult
{
    TIE = 0,
    PLAYER_X_WON = 1,
    PLAYER_O_WON = -1
}

public class GameManager : MonoBehaviour
{
    Board board;

    Mark currentMark;

    bool isGameOver;

    public GameObject playerX;

    public GameObject playerO;

    [SerializeField]
    VisualEffect spawnEffect;

    readonly Dictionary<string, int> scores = new Dictionary<string, int>()
    {
        { "X", 1 },
        { "O", -1 },
        { "Tie", 0}
    };

    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();

        isGameOver = false;

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
                if (hit.transform.CompareTag(TagTypes.BOARD_BOX))
                {
                    var boxIndex = hit.transform.GetComponent<Box>().Index;

                    MakeMove(boxIndex);
                }
            }

        }
    }

    async void MakeMove(int boxIndex)
    {
        if (!board.BoardBoxes[boxIndex].IsMarked)
        {
            board.BoardBoxes[boxIndex].SetAsMarked(currentMark == Mark.X ? playerX : playerO, currentMark);
            if (spawnEffect != null)
            {
                Instantiate(spawnEffect, board.BoardBoxes[boxIndex].transform);
            }

            String winner = CheckWinCondition(board);

            if (winner != null)
            {
                Debug.Log(scores[winner]);

                ShowWinnerScreen(winner);

                isGameOver = true;
                return;
            }

            await AIMove();
        }
    }

    async Task AIMove()
    {
        await Task.Delay(2000);

        int bestScore = int.MaxValue;

        int bestMove = 0;

        foreach (Box box in board.BoardBoxes)
        {
            if (!box.IsMarked)
            {
                box.SetAsMarked(playerO, Mark.O);

                int score = Minimax(board, 0, true);

                box.Clear();

                if (score < bestScore)
                {
                    bestScore = score;

                    bestMove = box.Index;
                }
            }
        }

        board.BoardBoxes[bestMove].SetAsMarked(playerO, Mark.O);

        String winner = CheckWinCondition(board);

        if (winner != null)
        {
            Debug.Log(scores[winner]);

            ShowWinnerScreen(winner);

            isGameOver = true;
            return;
        }
    }

    int Minimax(Board boardState, int depth, bool isMaximizing)
    {
        var winner = CheckWinCondition(boardState);

        if (winner != null)
        {
            return scores[winner];
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;

            foreach (Box box in boardState.BoardBoxes)
            {
                if (!box.IsMarked)
                {
                    box.SetAsMarked(playerX, Mark.X);

                    bestScore = Math.Max(bestScore, Minimax(boardState, depth + 1, !isMaximizing));

                    box.Clear();
                }
            }

            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            foreach (Box box in boardState.BoardBoxes)
            {
                if (!box.IsMarked)
                {
                    box.SetAsMarked(playerO, Mark.O);

                    bestScore = Math.Min(bestScore, Minimax(boardState, depth + 1, !isMaximizing));

                    box.Clear();
                }
            }

            return bestScore;
        }
    }

    void ShowWinnerScreen(String winner)
    {
        switch (winner)
        {
            case "X":
                Debug.Log("X won");
                break;
            case "O":
                Debug.Log("O won");
                break;
            case "Tie":
                Debug.Log("It's a tie!");
                break;
            default:
                break;
        }
    }

    String CheckWinCondition(Board boardState)
    {
        int boardSize = 3;

        for (int i = 0; i < boardSize; i++)
        {
            if (Equals3((i * boardSize), (i * boardSize) + 1, (i * boardSize) + 2))
            {
                return boardState.BoardBoxes[i * boardSize].Mark.ToString();
            }
        }

        for (int i = 0; i < boardSize; i++)
        {
            if (Equals3(i, (1 * boardSize) + i, (2 * boardSize) + i))
            {
                return boardState.BoardBoxes[i].Mark.ToString();
            }
        }

        if (Equals3(2, 4, 6))
        {
            return boardState.BoardBoxes[2].Mark.ToString();
        }

        if (Equals3(0, 4, 8))
        {
            return boardState.BoardBoxes[0].Mark.ToString();
        }

        if (!board.BoardBoxes.Any(box => box.Mark == Mark.None))
        {
            return "Tie";
        }

        return null;
    }

    bool Equals3(int a, int b, int c)
    {
        return board.BoardBoxes[a].Mark != Mark.None &&
            board.BoardBoxes[a].Mark == board.BoardBoxes[b].Mark &&
            board.BoardBoxes[b].Mark == board.BoardBoxes[c].Mark &&
            board.BoardBoxes[c].Mark == board.BoardBoxes[a].Mark;
    }
}
