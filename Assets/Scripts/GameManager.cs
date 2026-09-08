using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingtonMgr<GameManager>
{
    private GameManager() { }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
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
