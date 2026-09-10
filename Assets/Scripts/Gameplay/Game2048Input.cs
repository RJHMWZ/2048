using UnityEngine;

/// <summary>
/// 将玩家输入转换为棋盘移动指令。
/// </summary>
public sealed class Game2048Input : MonoBehaviour
{
    [SerializeField] private Game2048Manager gameManager;

    private void Update()
    {
        if (gameManager == null || !gameManager.CanAcceptInput)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            gameManager.Move(MoveDirection.Left);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            gameManager.Move(MoveDirection.Right);
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            gameManager.Move(MoveDirection.Up);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            gameManager.Move(MoveDirection.Down);
    }
}
