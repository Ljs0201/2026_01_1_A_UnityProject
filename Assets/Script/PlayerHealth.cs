using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가 필요

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;        // 최대 생명력
    public int currentLives;       // 현재 생명력

    void Start()
    {
        // 생명력 초기화
        currentLives = maxLives;
    }

    // 트리거 영역 안에 들어왔나를 검사하는 함수
    void OnTriggerEnter(Collider other)
    {
        // 미사일과 충돌 하면
        if (other.CompareTag("Missile"))
        {
            currentLives--;
            Destroy(other.gameObject); // 미사일 오브젝트를 없앤다.

            // 체력이 0 이하일 경우
            if (currentLives <= 0)
            {
                GameOver(); // 종료 함수를 호출 한다.
            }
        }
    }

    void GameOver()
    {
        gameObject.SetActive(false);          // 플레이어 비활성화
        Invoke("RestarGame", 3.0f);           // 3초 후 RestarGame 함수를 호출
    }

    void RestarGame()
    {
        // 현재 씬 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}