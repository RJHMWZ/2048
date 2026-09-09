using System.Text;
using UnityEngine;

/// <summary>
/// 2048棋盘测试
/// </summary>
public class BoardDebugTest : MonoBehaviour
{
    private BoardModel board;

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

    #region 测试代码
    private void Start()
    {
        TestMerge(new int[] { 2, 0, 2, 2 });
        TestMerge(new int[] { 2, 2, 2, 2 });
        TestMerge(new int[] { 2, 2, 4, 4 });
        TestMerge(new int[] { 4, 4, 4, 0 });
        TestMerge(new int[] { 2, 4, 8, 16 });
        TestMerge(new int[] { 0, 0, 0, 0 });
    }

    /// <summary>
    /// 测试单行合并
    /// </summary>
    private void TestMerge(int[] line)
    {
        int[] result = LineMergeUtility.MergeLeft(line);

        Debug.Log(
            $"原始：{ArrayToString(line)} " +
            $"→ 合并：{ArrayToString(result)}"
        );
    }

    /// <summary>
    /// 将数组转换成字符串
    /// </summary>
    private string ArrayToString(int[] array)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("[");

        for (int i = 0; i < array.Length; i++)
        {
            builder.Append(array[i]);

            if (i < array.Length - 1)
            {
                builder.Append(", ");
            }
        }

        builder.Append("]");
        return builder.ToString();
    }
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