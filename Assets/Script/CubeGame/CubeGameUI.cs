using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CubeGameUI : MonoBehaviour
{
    public TextMeshProUGUI TimerText;    // UI 선언
    public float Timer;                  // 타이머 선언

    void Update()
    {
        Timer += Time.deltaTime;                                    // 타이머 시간을 늘려준다.
        TimerText.text = "생존 시간 : " + Timer.ToString("0.00");    // 문자열 형태로 변환해서 보여준다.
    }
}