using System.Collections;
using System.Text;
using UnityEngine;

/// <summary>
/// 协调棋盘模型、显示、音效和游戏状态。
/// </summary>
public sealed class Game2048Manager : MonoBehaviour
{
    [Header("UI引用")]
    [SerializeField] private BoardView boardView;
    [SerializeField] private GameHUD gameHUD;
    [SerializeField] private GameResultUI gameResultUI;

    private BoardModel board;
    private int score;
    private int bestScore;
    private bool isGameOver;
    private bool hasWon;
    private bool isPaused;
    private bool isAnimating;
    private bool isAwaitingVictoryChoice;

    public BoardModel Board => board;
    public int Score => score;
    public int BestScore => bestScore;
    public bool IsGameOver => isGameOver;
    public bool HasWon => hasWon;
    public bool CanAcceptInput =>
        board != null &&
        !isGameOver &&
        !isPaused &&
        !isAnimating &&
        !isAwaitingVictoryChoice;

    private void Start()
    {
        bestScore = GameSaveManager.GetBestScore();
        StartGame();
    }

    public void StartGame()
    {
        StopAllCoroutines();

        board = new BoardModel();
        score = 0;
        isGameOver = false;
        hasWon = false;
        isPaused = false;
        isAnimating = false;
        isAwaitingVictoryChoice = false;

        gameResultUI?.ResetUI();
        boardView?.ResetView();

        board.GenerateRandomTile();
        board.GenerateRandomTile();

        RefreshView();
        GameAudioManager.Instance.PlaySpawn();
        PrintBoard();
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void Move(MoveDirection direction)
    {
        if (!CanAcceptInput)
            return;

        isAnimating = true;
        StartCoroutine(ProcessMove(direction));
    }

    private IEnumerator ProcessMove(MoveDirection direction)
    {
        BoardMoveResult result = board.Move(direction);

        if (!result.Moved)
        {
            isAnimating = false;
            CheckGameState();
            yield break;
        }

        if (result.Score > 0)
            GameAudioManager.Instance.PlayMerge();
        else
            GameAudioManager.Instance.PlayMove();

        if (boardView != null)
        {
            boardView.HideTiles();
            yield return boardView.PlayMoveAnimation(result);
        }

        score += result.Score;
        UpdateBestScore();

        Vector2Int? spawnedPosition = null;

        if (board.TryGenerateRandomTile(out Vector2Int position))
        {
            spawnedPosition = position;
            GameAudioManager.Instance.PlaySpawn();
        }

        RefreshAfterMove(result, spawnedPosition);
        isAnimating = false;
        CheckGameState();
        PrintBoard();
    }

    public void ShowPause()
    {
        if (!CanAcceptInput)
            return;

        isPaused = true;
        gameResultUI?.ShowPause();
    }

    public void ClosePausePanel()
    {
        gameResultUI?.HidePause();
        isPaused = false;
    }

    public void ContinueGame()
    {
        gameResultUI?.HideVictory();
        isAwaitingVictoryChoice = false;
        CheckGameState();
    }

    public void CloseVictoryPanel()
    {
        ContinueGame();
    }

    public void BackToMenu()
    {
        GameManager.BackToMenu();
    }

    private void RefreshView()
    {
        boardView?.Refresh();
        gameHUD?.SetScore(score, bestScore);
    }

    private void RefreshAfterMove(BoardMoveResult result, Vector2Int? spawnedPosition)
    {
        boardView?.RefreshAfterMove(result, spawnedPosition);
        gameHUD?.SetScore(score, bestScore);
    }

    private void CheckGameState()
    {
        if (board == null)
            return;

        if (!hasWon && board.HasValue(2048))
        {
            hasWon = true;
            isAwaitingVictoryChoice = true;
            GameAudioManager.Instance.PlayVictory();
            gameResultUI?.ShowVictory();
            return;
        }

        if (!board.IsGameOver())
            return;

        isGameOver = true;
        GameAudioManager.Instance.PlayGameOver();
        gameResultUI?.ShowGameOver(score);
    }

    private void UpdateBestScore()
    {
        if (score <= bestScore)
            return;

        bestScore = score;
        GameSaveManager.SaveBestScore(bestScore);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void PrintBoard()
    {
        if (board == null)
            return;

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("========== 2048 ==========");
        builder.AppendLine($"Score: {score}");

        for (int row = 0; row < BoardModel.Size; row++)
        {
            for (int column = 0; column < BoardModel.Size; column++)
                builder.Append(board.GetValue(row, column)).Append('\t');

            builder.AppendLine();
        }

        builder.AppendLine("==========================");
        Debug.Log(builder.ToString());
    }
}
