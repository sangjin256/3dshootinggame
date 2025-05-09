using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameState
{
    Ready,
    Run,
    Pause,
    Over,
}

public class GameManager : BehaviourSingleton<GameManager>
{
    public CanvasGroup ReadyScreen;
    public CanvasGroup GoScreen;
    public CanvasGroup GameOverScreen;

    public PlayerController Player;

    private EGameState _gameState = EGameState.Ready;
    public EGameState GameState => _gameState;

    public void Start()
    {
        StartCoroutine(Start_Coroutine());
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UI_PopupManager.Instance.IsEmpty()) Pause();
            else
            {
                UI_PopupManager.Instance.Close();
                if (UI_PopupManager.Instance.IsEmpty())
                {
                    Continue();
                }
            }
        }
    }

    private void Pause()
    {
        _gameState = EGameState.Pause;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;

        UI_PopupManager.Instance.Open(EPopupType.UI_OptionPopup);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        _gameState = EGameState.Run;
    }

    public void Restart()
    {
        _gameState = EGameState.Run;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void SetCursorState(bool Lock)
    {
        if (Lock) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }

    private void Ready()
    {
        ReadyScreen.gameObject.SetActive(true);
        GameOverScreen.gameObject.SetActive(false);
    }

    private void Run()
    {
        GoScreen.gameObject.SetActive(true);
        ReadyScreen.gameObject.SetActive(false);
        GameOverScreen.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        ReadyScreen.gameObject.SetActive(false);
        GameOverScreen.gameObject.SetActive(true);
    }

    private IEnumerator HideScreen_Coroutine(CanvasGroup screen, float timer)
    {
        float elapsedtime = 0;
        while(elapsedtime <= timer)
        {
            elapsedtime += Time.unscaledDeltaTime;
            screen.alpha = (timer - elapsedtime) / timer;

            yield return null;
        }
        screen.alpha = 0f;
        screen.gameObject.SetActive(false);
    }

    private IEnumerator Start_Coroutine()
    {
        Time.timeScale = 0f;

        Ready();

        yield return new WaitForSecondsRealtime(2f);
        yield return HideScreen_Coroutine(ReadyScreen, 0.2f);
        
        Run();

        yield return new WaitForSecondsRealtime(0.5f);
        yield return HideScreen_Coroutine(GoScreen, 0.2f);

        Time.timeScale = 1f;
    }
}
