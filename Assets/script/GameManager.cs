using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 单例模式

    public GameState currentState = GameState.MainMenu;
    private PlayerController playerController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 保持 GameManager 跨场景不销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        ExitState(currentState); // 退出当前状态的处理
        currentState = newState;
        EnterState(newState); // 进入新状态的处理
    }

    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                EnterMainMenu();
                break;
            case GameState.Playing:
                EnterPlaying();
                break;
            case GameState.Paused:
                EnterPaused();
                break;
            case GameState.GameOver:
                EnterGameOver();
                break;
        }
    }

    private void ExitState(GameState state)
    {
        switch (state)
        {
            case GameState.Paused:
                ExitPaused();
                break;
        }
    }

    void EnterMainMenu()
    {
        // 加载主菜单场景
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("进入主菜单");
    }

    void HandleMainMenu()
    {
        // 处理主菜单逻辑
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButton("Fire1"))
        {
            ChangeState(GameState.Playing);
        }
    }

    public void EnterPlaying()
    {
        // 检查当前场景是否是 SampleScene
        if (SceneManager.GetActiveScene().name != "SampleScene")
        {
            // 加载游戏场景
            Debug.Log("开始游戏");
            SceneManager.LoadScene("SampleScene");
            StartCoroutine(InitializeGameScene());
        }
        else
        {
            // 从暂停状态恢复
            Time.timeScale = 1; // 确保时间恢复正常
            currentState = GameState.Playing;
            Debug.Log("恢复游戏");
        }
    }


    public IEnumerator InitializeGameScene()
    {
        yield return new WaitForSeconds(0.1f); // 等待场景加载完成

        // 获取 playerController 实例
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.StartGame();
            }
            else
            {
                Debug.LogError("未找到 PlayerController 实例。");
            }
        }
        else
        {
            Debug.LogError("未找到 Player 对象。");
        }
    }

    void HandlePlaying()
    {
        // 处理游戏中的逻辑
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(GameState.Paused);
        }

        // 检查游戏结束条件
        if (playerController != null && playerController.IsDead())
        {
            ChangeState(GameState.GameOver);
        }
    }

    void EnterPaused()
    {
        // 初始化暂停菜单
        Debug.Log("游戏暂停");
        Time.timeScale = 0;
    }

    private void ExitPaused()
    {
        // 恢复游戏
        Time.timeScale = 1;
    }

    void HandlePaused()
    {
        // 处理暂停菜单逻辑
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(GameState.Playing);
        }
    }

    void EnterGameOver()
    {
        // 初始化游戏结束界面
        Debug.Log("游戏结束");
        Time.timeScale = 1; // 确保时间恢复正常
        UIManager uiManager = FindObjectOfType<UIManager>();

        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
    }


    void HandleGameOver()
    {

        // 处理游戏结束逻辑
        Time.timeScale = 0;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            ChangeState(GameState.MainMenu);
        }
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }
}

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}
