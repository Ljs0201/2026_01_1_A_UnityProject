using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;    // 생성할 큐브 프리팹
    public int totalCubes = 10;      // 총 생성할 큐브 개수
    public float cubeSpacing = 1.0f; // 큐브 간격

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenCube();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenCube()
    {
        Vector3 myPosition = transform.position;    // 스크립트가 붙은 오브젝트의 위치(x, y, z)

        for (int i = 0; i < totalCubes; i++)
        {
            Vector3 position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cubeSpacing));
            Instantiate(cubePrefab, position, Quaternion.identity);    // 큐브 생성
        }
    }
}