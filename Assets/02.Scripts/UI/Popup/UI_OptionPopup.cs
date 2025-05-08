using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_OptionPopup : UI_Popup
{
    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnClickContinueButton()
    {
        GameManager.Instance.Continue();
        gameObject.SetActive(false);
    }

    public void OnClickRetryButton()
    {
        GameManager.Instance.Restart();
        gameObject.SetActive(false);
    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickCreditButton()
    {
        UI_PopupManager.Instance.Open(EPopupType.UI_CreditPopup);
    }
}
