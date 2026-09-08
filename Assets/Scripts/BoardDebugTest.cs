using System.Text;
using UnityEngine;

/// <summary>
/// 2048棋盘测试
/// </summary>
public class BoardDebugTest : MonoBehaviour
{
    private BoardModel board;

    private void Start() 
    {
        board = new BoardModel();

        //清空棋盘
        board.Clear();

        //开局生成两个数字
        board.GenerateRandomTile();
        board.GenerateRandomTile();

        PrintBoard();
    }

    #region 测试代码
  // private void Start()
    // {
    //     board = new BoardModel();

    //     board.Clear();

    //     for (int i = 0; i < 16; i++)
    //     {
    //         board.GenerateRandomTile();
    //     }

    //     bool result = board.GenerateRandomTile();

    //     Debug.Log($"第17次生成结果：{result}");

    //     PrintBoard();
    // }

    // private void Start()
    // {
    //     board = new BoardModel();

    //     board.Clear();

    //     for (int i = 0; i < 16; i++)
    //     {
    //         board.GenerateRandomTile();
    //     }

    //     PrintBoard();
    // }

    // private void Start()
    // {
    //     board = new BoardModel();

    //     //清空棋盘
    //     board.Clear();

    //     //开局生成两个数字
    //     board.GenerateRandomTile();
    //     board.GenerateRandomTile();

    //     PrintBoard();
    // }
    #endregion
  
    /// <summary>
    /// 输出当前棋盘数据
    /// </summary>
    private void PrintBoard()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("========== 2048 Board ==========");

        for (int row = 0; row < BoardModel.Size; row++)
        {
            for (int column = 0; column < BoardModel.Size; column++)
            {
                builder.Append(board.GetValue(row, column));
                builder.Append("\t");
            }
            builder.AppendLine();
        }
        builder.AppendLine("================================");
        Debug.Log(builder.ToString());
    }
}