using UnityEngine;

/// <summary>
/// 2048玩家输入控制
/// </summary>
public class Game2048Input : MonoBehaviour
{
    [SerializeField]
    private Game2048Manager gameManager;

    private void Update()
    {
        if (gameManager == null)
            return;

        HandleKeyboardInput();
    }

    /// <summary>
    /// 处理键盘输入
    /// </summary>
    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A))
        {
            gameManager.Move(MoveDirection.Left);
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            gameManager.Move(MoveDirection.Right);
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.W))
        {
            gameManager.Move(MoveDirection.Up);
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.S))
        {
            gameManager.Move(MoveDirection.Down);
        }
    }
}