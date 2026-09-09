using System.Text;
using UnityEngine;

/// <summary>
/// 2048游戏流程管理器
/// </summary>
public class Game2048Manager : MonoBehaviour
{
    /// <summary>
    /// 当前棋盘数据
    /// </summary>
    private BoardModel board;

    /// <summary>
    /// 当前棋盘
    /// </summary>
    public BoardModel Board => board;

    private void Start()
    {
        StartGame();
    }

    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void StartGame()
    {
        //创建棋盘
        board = new BoardModel();

        //清空棋盘
        board.Clear();

        //2048开局生成两个数字
        board.GenerateRandomTile();
        board.GenerateRandomTile();

        //当前阶段暂时使用Console查看棋盘
        PrintBoard();
    }

    /// <summary>
    /// 根据玩家输入移动棋盘
    /// </summary>
    /// <param name="direction">移动方向</param>
    public void Move(MoveDirection direction)
    {
        if (board == null)
            return;

        //执行移动
        bool moved = board.Move(direction);

        //无效移动不生成新数字
        if (!moved)
        {
            Debug.Log($"无效移动：{direction}");
            return;
        }

        //有效移动后生成一个新数字
        board.GenerateRandomTile();

        //输出当前棋盘
        PrintBoard();
    }

    /// <summary>
    /// 输出当前棋盘
    /// </summary>
    private void PrintBoard()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("========== 2048 ==========");

        for (int row = 0; row < BoardModel.Size; row++)
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