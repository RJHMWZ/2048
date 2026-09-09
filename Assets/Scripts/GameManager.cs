using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingtonMgr<GameManager>
{
    private GameManager() { }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}