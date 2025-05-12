using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.SceneManagement;

[Serializable]
public class UI_InputFields
{
    public TextMeshProUGUI ResultText;
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordConfirmInputField;

    public Button ConfirmButton;
}

public class UI_LoginScene : MonoBehaviour
{
    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject SignupPanel;

    [Header("로그인")]
    public UI_InputFields LoginInputFields;

    [Header("회원가입")]
    public UI_InputFields SignupInputFields;

    private const string PREFIX = "ID_";
    private const string SALT = "1006247";

    private void Start()
    {
        OnClickGoToLoginButton();
        LoginCheck();
    }

    public void OnClickGoToSignupButton()
    {
        LoginPanel.SetActive(false);
        SignupPanel.SetActive(true);
    }

    public void OnClickGoToLoginButton()
    {
        LoginPanel.SetActive(true);
        SignupPanel.SetActive(false);
    }

    // 회원가입
    public void Signup()
    {
        // 1. 아이디 입력 확인
        string id = SignupInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            SignupInputFields.ResultText.text = "아이디를 입력해주세요.";
            SignupInputFields.ResultText.transform.DOShakePosition(0.5f, 15);
            return;
        }

        // 2. 비밀번호 입력을 확인한다.
        string pwd = SignupInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(pwd))
        {
            SignupInputFields.ResultText.text = "비밀번호를 입력해주세요.";
            SignupInputFields.ResultText.transform.DOShakePosition(0.5f, 15);
            return;
        }

        // 3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인한다.
        string confirmPwd = SignupInputFields.PasswordConfirmInputField.text;
        if (string.IsNullOrEmpty(confirmPwd))
        {
            SignupInputFields.ResultText.text = "확인 비밀번호를 입력해주세요.";
            SignupInputFields.ResultText.transform.DOShakePosition(0.5f, 15);
            return;
        }
        else if(confirmPwd.Equals(pwd) == false)
        {
            SignupInputFields.ResultText.text = "확인 비밀번호가 다릅니다.";
            SignupInputFields.ResultText.transform.DOShakePosition(0.5f, 15);
            return;
        }

        // 4. PlayerPrefs를 이용해서 아이디와 비밀번호를 저장한다.
        PlayerPrefs.SetString(PREFIX + id, Encryption(pwd + SALT));

        // 5. 로그인 창으로 돌아간다. (이때 아이디는 자동 입력되어 있다.)
        LoginInputFields.IDInputField.text = id;
        OnClickGoToLoginButton();
    }

    public string Encryption(string text)
    {
        // 해시 암호화 알고리즘 인스턴스를 생성한다.
        SHA256 sha256 = SHA256.Create();

        // 운영체제 혹은 프로그래밍 언어별로 string을 표현하는 방식이 다 다르므로
        // UTF8버전 바이트 배열로 바꿔야 한다.
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = string.Empty;
        foreach (byte b in hash) resultText += b.ToString("X2");

        return resultText;
    }

    public void Login()
    {
        // 1. 아이디 입력을 확인한다.
        string id = LoginInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            LoginInputFields.ResultText.text = "아이디를 입력해주세요.";
            LoginInputFields.ResultText.transform.DOShakePosition(0.5f, 15);
            return;
        }

        // 2. 비밀번호 입력을 확인한다.
        string pwd = LoginInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(pwd))
        {
            LoginInputFields.ResultText.text = "아이디와 비밀번호를 확인해주세요.";
            LoginInputFields.ResultText.transform.DOShakePosition(0.5f, 15);
            return;
        }

        // 3. PlayerPrefs.Get을 이용해서 아이디와 비밀번호가 맞는지 확인한다.
        if(PlayerPrefs.HasKey(PREFIX + id) == false)
        {
            LoginInputFields.ResultText.text = "아이디와 비밀번호를 확인해주세요.";
            LoginInputFields.ResultText.transform.DOShakePosition(0.5f, 15);
            return;
        }

        string hashedPassword = PlayerPrefs.GetString(PREFIX + id);
        if(hashedPassword != Encryption(pwd + SALT))
        {
            LoginInputFields.ResultText.text = "아이디와 비밀번호를 확인해주세요.";
            LoginInputFields.ResultText.transform.DOShakePosition(0.5f, 15);
            return;
        }

        // 4. 맞다면 로그인

        LoginInputFields.ResultText.text = "로그인 성공!";
        SceneManager.LoadScene(1);
    }

    public void LoginCheck()
    {
        string id = LoginInputFields.IDInputField.text;
        string password = LoginInputFields.PasswordInputField.text;

        LoginInputFields.ConfirmButton.enabled = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password);
    }
}
