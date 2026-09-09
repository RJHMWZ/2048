// using System.Text;
// using UnityEngine;

// /// <summary>
// /// 2048棋盘移动测试
// /// </summary>
// public class BoardDebugTest : MonoBehaviour
// {
//     private BoardModel board;

//     private void Start()
//     {
//         TestMoveLeft();

//         TestMoveRight();

//         TestMoveUp();

//         TestMoveDown();
//     }

//     /// <summary>
//     /// 测试向左移动
//     /// </summary>
//     private void TestMoveLeft()
//     {
//         CreateTestBoard();

//         Debug.Log("向左移动前：");
//         PrintBoard();

//         bool moved = board.Move(MoveDirection.Left);

//         Debug.Log($"向左移动结果：{moved}");
//         PrintBoard();
//     }

//     /// <summary>
//     /// 测试向右移动
//     /// </summary>
//     private void TestMoveRight()
//     {
//         CreateTestBoard();

//         Debug.Log("向右移动前：");
//         PrintBoard();

//         bool moved = board.Move(MoveDirection.Right);

//         Debug.Log($"向右移动结果：{moved}");
//         PrintBoard();
//     }

//     /// <summary>
//     /// 测试向上移动
//     /// </summary>
//     private void TestMoveUp()
//     {
//         CreateTestBoard();

//         Debug.Log("向上移动前：");
//         PrintBoard();

//         bool moved = board.Move(MoveDirection.Up);

//         Debug.Log($"向上移动结果：{moved}");
//         PrintBoard();
//     }

//     /// <summary>
//     /// 测试向下移动
//     /// </summary>
//     private void TestMoveDown()
//     {
//         CreateTestBoard();

//         Debug.Log("向下移动前：");
//         PrintBoard();

//         bool moved = board.Move(MoveDirection.Down);

//         Debug.Log($"向下移动结果：{moved}");
//         PrintBoard();
//     }

//     /// <summary>
//     /// 创建固定测试棋盘
//     /// </summary>
//     private void CreateTestBoard()
//     {
//         board = new BoardModel();

//         board.Clear();

//         board.SetValue(0, 0, 2);
//         board.SetValue(0, 2, 2);
//         board.SetValue(0, 3, 2);

//         board.SetValue(1, 0, 4);
//         board.SetValue(1, 1, 4);

//         board.SetValue(2, 0, 2);
//         board.SetValue(2, 1, 2);
//         board.SetValue(2, 2, 2);
//         board.SetValue(2, 3, 2);

//         board.SetValue(3, 0, 2);
//         board.SetValue(3, 1, 4);
//         board.SetValue(3, 2, 8);
//         board.SetValue(3, 3, 16);
//     }

//     /// <summary>
//     /// 打印棋盘
//     /// </summary>
//     private void PrintBoard()
//     {
//         StringBuilder builder = new StringBuilder();

//         builder.AppendLine("----------------");

//         for (int row = 0; row < BoardModel.Size; row++)
//         {
//             for (int column = 0;
//                  column < BoardModel.Size;
//                  column++)
//             {
//                 builder.Append(
//                     board.GetValue(row, column)
//                 );

//                 builder.Append("\t");
//             }

//             builder.AppendLine();
//         }

//         builder.AppendLine("----------------");

//         Debug.Log(builder.ToString());
//     }
// }