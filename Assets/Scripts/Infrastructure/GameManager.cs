using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 应用级场景导航。
/// </summary>
public static class GameManager
{
    private const string MenuSceneName = "MenuScene";
    private const string GameSceneName = "GameScene";

    public static void PlayGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public static void BackToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
