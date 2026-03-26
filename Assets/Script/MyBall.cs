using UnityEngine;

public class MyBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + " 와 충돌함");
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("땅과 출동");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리어 안으로 들어옴");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("트리어 밖으론 나감");
    }

}
