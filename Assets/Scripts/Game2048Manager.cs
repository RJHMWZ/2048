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
    /// 当前棋盘
    /// </summary>
    public BoardModel Board => board;

    /// <summary>
    /// 当前分数
    /// </summary>
    public int Score => score;

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

        //重置分数
        score = 0;

        //开局生成两个数字
        board.GenerateRandomTile();
        board.GenerateRandomTile();

        PrintBoard();
    }

    /// <summary>
    /// 根据玩家输入移动
    /// </summary>
    public void Move(MoveDirection direction)
    {
        if (board == null)
            return;

        BoardMoveResult result =
            board.Move(direction);

        //无效移动
        if (!result.Moved)
        {
            Debug.Log($"无效移动：{direction}");
            return;
        }

        //累加分数
        score += result.Score;

        //生成新数字
        board.GenerateRandomTile();

        PrintBoard();
    }

    /// <summary>
    /// 打印当前棋盘
    /// </summary>
    private void PrintBoard()
    {
        StringBuilder builder =
            new StringBuilder();

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