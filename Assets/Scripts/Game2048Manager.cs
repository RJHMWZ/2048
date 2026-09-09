using System.Text;
using UnityEngine;

/// <summary>
/// 2048游戏流程管理器
/// </summary>
public class Game2048Manager : MonoBehaviour
{
    /// <summary>
    /// 当前棋盘
    /// </summary>
    private BoardModel board;

    /// <summary>
    /// 当前分数
    /// </summary>
    private int score;

    /// <summary>
    /// 是否游戏结束
    /// </summary>
    private bool isGameOver;

    /// <summary>
    /// 是否已经达到2048
    /// </summary>
    private bool hasWon;

    public BoardModel Board => board;

    public int Score => score;

    public bool IsGameOver => isGameOver;

    public bool HasWon => hasWon;

    private void Start()
    {
        StartGame();
    }

    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void StartGame()
    {
        board = new BoardModel();

        board.Clear();

        score = 0;

        isGameOver = false;
        hasWon = false;

        board.GenerateRandomTile();
        board.GenerateRandomTile();

        PrintBoard();
    }

    /// <summary>
    /// 执行移动
    /// </summary>
    public void Move(MoveDirection direction)
    {
        if (board == null)
            return;

        //游戏结束后禁止继续移动
        if (isGameOver)
            return;

        BoardMoveResult result =
            board.Move(direction);

        //无效移动
        if (!result.Moved)
        {
            Debug.Log($"无效移动：{direction}");

            CheckGameState();

            return;
        }

        //增加分数
        score += result.Score;

        //有效移动后生成新数字
        board.GenerateRandomTile();

        //检查胜负状态
        CheckGameState();

        PrintBoard();
    }

    /// <summary>
    /// 检查当前游戏状态
    /// </summary>
    private void CheckGameState()
    {
        //第一次达到2048
        if (!hasWon && board.HasValue(2048))
        {
            hasWon = true;

            Debug.Log("Victory！已经合成2048！");
        }

        //判断Game Over
        if (board.IsGameOver())
        {
            isGameOver = true;

            Debug.Log("Game Over！");
        }
    }

    /// <summary>
    /// 输出棋盘
    /// </summary>
    private void PrintBoard()
    {
        StringBuilder builder =new StringBuilder();
        builder.AppendLine("========== 2048 ==========");
        builder.AppendLine($"Score：{score}");

        for (int row = 0;
             row < BoardModel.Size;
             row++)
        {
            for (int column = 0;
                 column < BoardModel.Size;
                 column++)
            {
                builder.Append(
                    board.GetValue(row, column)
                );

                builder.Append("\t");
            }

            builder.AppendLine();
        }

        builder.AppendLine("==========================");

        Debug.Log(builder.ToString());
    }
}