using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : BehaviourSingleton<GameManager>
{
    public CanvasGroup ReadyScreen;
    public CanvasGroup GoScreen;
    public CanvasGroup GameOverScreen;

    public void Start()
    {
        StartCoroutine(Start_Coroutine());
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
