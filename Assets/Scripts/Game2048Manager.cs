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

    [Header("UI引用")]

    /// <summary>
    /// 棋盘显示
    /// </summary>
    [SerializeField]
    private BoardView boardView;

    /// <summary>
    /// 游戏HUD
    /// </summary>
    [SerializeField]
    private GameHUD gameHUD;

    /// <summary>
    /// 当前棋盘
    /// </summary>
    public BoardModel Board => board;

    /// <summary>
    /// 当前分数
    /// </summary>
    public int Score => score;

    /// <summary>
    /// 是否游戏结束
    /// </summary>
    public bool IsGameOver => isGameOver;

    /// <summary>
    /// 是否已经达到2048
    /// </summary>
    public bool HasWon => hasWon;

    [SerializeField]
    private GameResultUI gameResultUI;

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

        gameResultUI?.ResetUI();

        boardView?.Refresh();

        gameHUD?.SetScore(score);

        PrintBoard();
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame()
    {
        StartGame();
    }

    /// <summary>
    /// 执行移动
    /// </summary>
    /// <param name="direction">移动方向</param>
    public void Move(MoveDirection direction)
    {
        if (board == null)
            return;

        //游戏结束后禁止继续移动
        if (isGameOver)
            return;

        //执行棋盘移动
        BoardMoveResult result = board.Move(direction);

        //无效移动
        if (!result.Moved)
        {
            Debug.Log($"无效移动：{direction}");

            //即使移动失败，也检查是否已经Game Over
            CheckGameState();

            return;
        }

        //增加本次合并获得的分数
        score += result.Score;

        //有效移动后生成一个新数字
        board.GenerateRandomTile();

        //检查胜利或失败状态
        CheckGameState();

        //刷新棋盘UI
        boardView?.Refresh();

        //刷新分数
        gameHUD?.SetScore(score);

        //输出测试数据
        PrintBoard();
    }

    /// <summary>
    /// 检查当前游戏状态
    /// </summary>
    private void CheckGameState()
    {
        if (board == null)
            return;

        //第一次达到2048
        if (!hasWon && board.HasValue(2048))
        {
            hasWon = true;

            Debug.Log("Victory！已经合成2048！");

            gameResultUI?.ShowVictory();
        }

        //Game Over
        if (board.IsGameOver())
        {
            isGameOver = true;

            Debug.Log("Game Over！");

            gameResultUI?.ShowGameOver(score);
        }
    }

    /// <summary>
    /// 输出当前棋盘数据
    /// </summary>
    private void PrintBoard()
    {
        if (board == null)
            return;

        StringBuilder builder = new StringBuilder();

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

    /// <summary>
    /// 达到2048后继续游戏
    /// </summary>
    public void ContinueGame()
    {
        gameResultUI?.HideVictory();
    }

    /// <summary>
    /// 关闭胜利弹窗
    /// </summary>
    public void CloseVictoryPanel()
    {
        gameResultUI?.HideVictory();
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}