using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // 목표 : 다음 씬을 비동기 방식으로 로드하고 싶다.
    //        또한 로딩 진행률을 시작적으로 표현하고 싶다.
    //                             ㄴ % 프로그레스 바와 %별 텍스트

    // 속성 :
    // - 다음 씬 번호 (인덱스)
    public int NextSceneIndex = 2;

    // - 프로그레스 슬라이더 바
    public Slider ProgressSlider;

    // - 프로그레스 텍스트
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(LoadNextScene_Coroutine());
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        // 지정된 씬을 비동기로 로드한다.
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false; // 비동기로 로드되는 씬의 모습이 화면에 보이지 않게 한다.

        // 로딩이 되는 동안 계속해서 반복문
        while(ao.isDone == false)
        {
            ProgressSlider.value = ao.progress;
            ProgressText.text = $"{ao.progress * 100f}%";

            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
