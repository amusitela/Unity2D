using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // ����ģʽ

    public GameState currentState = GameState.MainMenu;
    private PlayerController playerController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� GameManager �糡��������
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
        ExitState(currentState); // �˳���ǰ״̬�Ĵ���
        currentState = newState;
        EnterState(newState); // ������״̬�Ĵ���
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
        // �������˵�����
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("�������˵�");
    }

    void HandleMainMenu()
    {
        // �������˵��߼�
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButton("Fire1"))
        {
            ChangeState(GameState.Playing);
        }
    }

    public void EnterPlaying()
    {
        // ��鵱ǰ�����Ƿ��� SampleScene
        if (SceneManager.GetActiveScene().name != "SampleScene")
        {
            // ������Ϸ����
            Debug.Log("��ʼ��Ϸ");
            SceneManager.LoadScene("SampleScene");
            StartCoroutine(InitializeGameScene());
        }
        else
        {
            // ����ͣ״̬�ָ�
            Time.timeScale = 1; // ȷ��ʱ��ָ�����
            currentState = GameState.Playing;
            Debug.Log("�ָ���Ϸ");
        }
    }


    public IEnumerator InitializeGameScene()
    {
        yield return new WaitForSeconds(0.1f); // �ȴ������������

        // ��ȡ playerController ʵ��
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
                Debug.LogError("δ�ҵ� PlayerController ʵ����");
            }
        }
        else
        {
            Debug.LogError("δ�ҵ� Player ����");
        }
    }

    void HandlePlaying()
    {
        // ������Ϸ�е��߼�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(GameState.Paused);
        }

        // �����Ϸ��������
        if (playerController != null && playerController.IsDead())
        {
            ChangeState(GameState.GameOver);
        }
    }

    void EnterPaused()
    {
        // ��ʼ����ͣ�˵�
        Debug.Log("��Ϸ��ͣ");
        Time.timeScale = 0;
    }

    private void ExitPaused()
    {
        // �ָ���Ϸ
        Time.timeScale = 1;
    }

    void HandlePaused()
    {
        // ������ͣ�˵��߼�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(GameState.Playing);
        }
    }

    void EnterGameOver()
    {
        // ��ʼ����Ϸ��������
        Debug.Log("��Ϸ����");
        Time.timeScale = 1; // ȷ��ʱ��ָ�����
        UIManager uiManager = FindObjectOfType<UIManager>();

        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
    }


    void HandleGameOver()
    {

        // ������Ϸ�����߼�
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
