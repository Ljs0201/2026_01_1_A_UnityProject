using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;          // 이동 속도 변수 설정
    public float jumpForce = 5.0f;          // 점프 힘 변수 설정

    public Rigidbody rb;                    // 플레이어 강체 선언

    public bool isGrounded = true;                   // 플레이어가 땅에 닿아있는지 여부를 나타내는 변수

    public int coinCount = 0;                   // 플레이어가 획득한 코인 수를 저장하는 변수

    void Start()
    {

    }

    void Update()
    {
        // 움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");    // 수평 이동 (키값을 받아온다, -1 ~ 1)
        float moveVertical = Input.GetAxis("Vertical");        // 수직 이동 (키값을 받아온다, -1 ~ 1)

        // 강체에 속도의 값을 변경해서 캐릭터를 이동 시킨다.
        rb.linearVelocity = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);

        // 점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded) // && 두 값을 만족할 때 -> (스페이스 버튼을 눌렀을 때와 땅 위에 있을 때)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);    // 위쪽 방향으로 설정한 점프수치만큼 순간적으로 힘을 가한다.
            isGrounded = false;                                       // 점프를 하는 순간 땅에서 떨어졌기 때문에 false로 한다.
        }

    }
    void OnCollisionEnter(Collision collision) // 유니티에서 지원해주는 충돌 체크 함수
    {
        if (collision.gameObject.tag == "Ground") // 충돌이 일어난 물체의 tag가 Ground 인 경우
        {
            isGrounded = true; // 땅과 충돌하면 true로 만들어 준다.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin")) // 충돌한 물체의 tag가 Coin 인 경우
        {
            coinCount++; // 코인과 충돌하면 코인 수를 1 증가시킨다.
            Destroy(other.gameObject); // 충돌한 코인 오브젝트를 제거한다.
        }
    }

}
