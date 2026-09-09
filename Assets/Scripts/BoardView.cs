using UnityEngine;

/// <summary>
/// 2048棋盘显示控制
/// </summary>
public class BoardView : MonoBehaviour
{
    [SerializeField]
    private Game2048Manager gameManager;

    [SerializeField]
    private TileView[] tiles;

    /// <summary>
    /// 刷新整个棋盘
    /// </summary>
    public void Refresh()
    {
        if (gameManager == null)
            return;

        if (tiles == null ||
            tiles.Length != BoardModel.Size * BoardModel.Size)
        {
            Debug.LogError(
                "BoardView必须配置16个TileView"
            );

            return;
        }

        BoardModel board = gameManager.Board;

        if (board == null)
            return;

        int index = 0;

        for (int row = 0; row < BoardModel.Size; row++)
        {
            for (int column = 0;
                 column < BoardModel.Size;
                 column++)
            {
                int value =
                    board.GetValue(row, column);

                tiles[index].SetValue(value);

                index++;
            }
        }
    }
}