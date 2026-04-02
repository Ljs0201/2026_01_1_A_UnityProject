using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefabs;          // 동전 프리팹
    public GameObject MissilePrefabs;

    [Header("스폰 타이밍 설정")]
    public float minSpawnInterval = 0.5f;   // 최소 생성 간격(초)
    public float maxSpawnInterval = 2.0f;   // 최대 생성 간격(초)

    [Header("동전 스폰확률 설정")]
    [Range (0, 100)]
    public int coinSpawnChance = 50;        // 동전이 생성될 확률 (0~100)

    public float timer = 0.0f;              // 사용할 타이머 설정
    public float nextSpawnTime;             // 다음 생성 시간 변수 선언

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNextSpawnTime();                 // 함수 호출을 한다.
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;            // 시간이 0에서 점점 증가

        // 생성 시간이 되면 오브젝트 생성
        if (timer > nextSpawnTime)
        {
            SpawnObject();                   // 오브젝트 생성 함수 호출s
            timer = 0.0f;                   // 시간을 초기화 시켜준다.
            SetNextSpawnTime();             // 다시 함수 실행
        }
    }

    void SpawnObject()                            // 오브젝트 생성 함수
    {
        Transform spawntransform = transform;

        int RandomValue = Random.Range(0, 100);

        if (RandomValue < coinSpawnChance)
        {
            Instantiate(coinPrefabs, spawntransform.position, spawntransform.rotation); 
        }
        else
        {
            Instantiate(MissilePrefabs, spawntransform.position, spawntransform.rotation);
        }

         
    }

    void SetNextSpawnTime()                 // 최소 ~ 최대 사이의 랜덤한 시간 설정
    {
        // 랜덤 함수를 통해 nextSpawnTime에 시간 설정
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}