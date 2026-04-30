using UnityEngine;

public class FruitGame : MonoBehaviour
{
    [Header("Fruit Settings")]
    public GameObject[] fruitPrefabs;        // 과일 프리팹 배열
    public float[] fruitSize = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f }; // 과일 크기

    [Header("Game State")]
    public GameObject currentFruit;         // 현재 조종 중인 과일
    public int currentFruitType;            // 현재 과일의 타입 index
    public float fruitStartHeight = 6.0f;   // 과일 생성 높이
    public float gameWidth = 6.0f;          // 게임판 너비 제한
    public bool isGameOver = false;         // 게임 종료 여부
    public Camera mainCamera;               // 메인 카메라

    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewFruit();                    // 첫 번째 과일 생성
    }

    void Update()
    {
        if (isGameOver) return;

        if (currentFruit != null)
        {
            // 1. 마우스 위치를 월드 좌표로 변환
            Vector3 mousePosition = Input.mousePosition;
            // 카메라와의 거리를 고려하여 좌표 변환
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -mainCamera.transform.position.z));

            // 2. 새로운 위치 계산 및 벽 제한(Clamp)
            float halfFruitSize = fruitSize[currentFruitType] / 2f;
            float leftLimit = -gameWidth / 2 + halfFruitSize;
            float rightLimit = gameWidth / 2 - halfFruitSize;

            float xPos = Mathf.Clamp(worldPosition.x, leftLimit, rightLimit);
            currentFruit.transform.position = new Vector3(xPos, fruitStartHeight, 0);

            // 3. 마우스 왼쪽 클릭 시 과일 투하
            if (Input.GetMouseButtonDown(0))
            {
                DropFruit();
            }
        }
    }

    // 과일 생성 로직
    public void SpawnNewFruit()
    {
        if (isGameOver) return;

        // 랜덤하게 초기 과일 타입 결정 (보통 0~2단계 사이)
        currentFruitType = Random.Range(0, 3);

        // 과일 생성
        currentFruit = Instantiate(fruitPrefabs[currentFruitType], new Vector3(0, fruitStartHeight, 0), Quaternion.identity);

        // 이미지 수정 사항 반영: 크기 설정
        currentFruit.transform.localScale = new Vector3(fruitSize[currentFruitType], fruitSize[currentFruitType], 1);

        // 물리 효과 일시 정지 (들고 있을 때는 떨어지지 않게)
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0.0f;
        }
    }

    // 과일 떨어뜨리기
    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1.0f; // 중력 적용
        }

        currentFruit = null; // 제어권 해제

        // 1초 후 다음 과일 생성
        Invoke("SpawnNewFruit", 1.0f);
    }

    // 과일 합성 로직 (Fruit.cs에서 충돌 시 이 함수를 호출하게 됩니다)
    public void Merge(int type, Vector3 position)
    {
        if (isGameOver) return;

        // 마지막 단계 과일이 아니면 다음 단계 생성
        if (type < fruitPrefabs.Length - 1)
        {
            int nextType = type + 1;
            GameObject nextFruit = Instantiate(fruitPrefabs[nextType], position, Quaternion.identity);

            // 크기 및 물리 설정
            nextFruit.transform.localScale = new Vector3(fruitSize[nextType], fruitSize[nextType], 1);
            Rigidbody2D rb = nextFruit.GetComponent<Rigidbody2D>();
            if (rb != null) rb.gravityScale = 1.0f;
        }
    }
}