using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header ("기본 이동 설정")]
    public float moveSpeed = 5.0f;          // 이동 속도 변수 설정
    public float jumpForce = 5.0f;          // 점프 힘 변수 설정
    public float turnSpeed = 0.0f;           // 회전속도

    [Header("점프 개선 설정")]
    public float fallMultiplier = 2.5f;          //하강 중력 배율
    public float lowJumpMultiplier = 2.0f;       //짧은 점프 배율

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;           //지면 관성 시간
    public float coyoteTimeCounter;            //관성 타이머
    public bool realGrounded = true;          //실제 지면 상태

    [Header("글라이더 설정")]
    public GameObject gliderObject;           //글라이더 오브젝트
    public float gliderFallSpeed = 1.0f;      //글라이더 낙하 속도
    public float gliderFMoveSpeed = 7.0f;     //글라이더 이동 속도
    public float gliderMaxTime = 5.0f;        //최대 사용 시간
    public float gliderTimerLeft;             //남은 사용 시간
    public bool isGliding = false;            //글라이딩 중인지 여부



    public Rigidbody rb;                    // 플레이어 강체 선언

    public bool isGrounded = true;                   // 플레이어가 땅에 닿아있는지 여부를 나타내는 변수

    public int coinCount = 0;                   // 플레이어가 획득한 코인 수를 저장하는 변수

    void Start()
    {
        coyoteTimeCounter = 0;

        if (gliderObject != null)                    //글라이더 오브젝트 초기화
        {
            gliderObject.SetActive(false);           //시작 시 비활성화
        }

        gliderTimerLeft = gliderMaxTime;             //글라이더 시간 초기화

    }

    void Update()
    {
        UpdateGroundedState();

        // 움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");    // 수평 이동 (키값을 받아온다, -1 ~ 1)
        float moveVertical = Input.GetAxis("Vertical");        // 수직 이동 (키값을 받아온다, -1 ~ 1)

        //이동 방향 벡터
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (movement.magnitude > 0.1f)                      //이동 방향 감지
        {                                                   //벡터의 길이를 감지해서 입력을 확인함
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            //이동 방향을 바라보도록 부드럽게 회전
        }

        //G키로 글라이더 제어 (누르는 동안 활성화)
        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimerLeft > 0)     //G키를 누르면서 땅에 있지 않고 글라이더 남은 시간이 있을때 (3가지 조건)
        {
            if (!isGliding)                         //글라이딩이 아닐경우 함수를 통해서 활성화
            {
                EnableGlider();
            }

            gliderTimerLeft -= Time.deltaTime;       //글라이더 사용 시간 감소

            if (gliderTimerLeft <= 0)                //사용 시간이 다 되면 비활성화
            {
                DisableGlider();
            }
        }
        else if (isGliding)
        {
            DisableGlider();                        //키를 떼면 글라이더 비활성화
        }

        if (isGliding)                                            //글라이딩 움직임 처리
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);    //글라이더 사용 중 이동
        }

        else
        {
            //강체에 속도의 값을 변경해서 캐릭터를 이동 시킨다.
            rb.linearVelocity = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);

            //착시 점프 높이 구현
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;         //하강 시 중력 강화
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))                                           //상승 중 점프 버튼을 떼면 낮게 점프
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

        }


        // 강체에 속도의 값을 변경해서 캐릭터를 이동 시킨다.
        rb.linearVelocity = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);

        //착시 점프 높이 구현
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;         //하강 시 중력 강화
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))                                           //상승 중 점프 버튼을 떼면 낮게 점프
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }


        // 점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded) // && 두 값을 만족할 때 -> (스페이스 버튼을 눌렀을 때와 땅 위에 있을 때)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);    // 위쪽 방향으로 설정한 점프수치만큼 순간적으로 힘을 가한다.
            isGrounded = false;                                       // 점프를 하는 순간 땅에서 떨어졌기 때문에 false로 한다.
            realGrounded = false;
            coyoteTimeCounter = 0;                                        //점프 시 코요테 타임 초기화
        }

        //지면에 있으면 글라이더 시간 회복 및 글라이더 비활성화
        if (isGrounded)
        {
            if (isGliding)
            {
                DisableGlider();
            }

            gliderTimerLeft = gliderMaxTime;            //지상에 있을때 시간 회복
        }


    }
    void OnCollisionEnter(Collision collision)                  //유니티에서 지원해주는 충돌 체크 함수
    {
        if (collision.gameObject.tag == "Ground")               //충돌이 일어난 물체의 Tag 가 Ground 인 경우
        {
            realGrounded = true;                                //땅과 충돌하면 true로 만들어 준다.
        }
    }

    void OnCollisionStay(Collision collision)                   //지면과의 충돌이 유지 되는지 확인
    {
        if (collision.gameObject.tag == "Ground")               //충돌이 일어난 물체의 Tag 가 Ground 인 경우
        {
            realGrounded = true;                                //땅과 충돌하면 true로 만들어 준다.
        }
    }

    void OnCollisionExit(Collision collision)                   //지면에서 떨어졌는지 확인
    {
        if (collision.gameObject.tag == "Ground")               //지면에서 떨어졌기 때문에 false
        {
            realGrounded = false;
        }
    }


    //지면 상태 업데이트 함수
    void UpdateGroundedState()
    {
        if (realGrounded)                               //실제 지면에 있으면 코요테 타임 리셋
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            //실제로 지면에 없지만 코요테 타임 내에 있으면 여전히 지면으로 판단
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;    //시간을 지속적으로 감소 시킨다.
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    //글라이더 활성화 함수
    void EnableGlider()
    {
        isGliding = true;

        //글라이더 오브젝트 표시
        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        //하강 속도 초기화
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, -gliderFallSpeed, rb.linearVelocity.z);
    }

    //글라이더 이동 적용
    void ApplyGliderMovement(float horizontal, float vertical)      //수평과 수직을 함수의 인수로 받는다
    {
        //글라이더 효과 : 천천히 떨어지고 수평 방향으로 빠르게 이동
        Vector3 gliderVelocity = new Vector3(horizontal * gliderFMoveSpeed, -gliderFallSpeed, vertical * gliderFMoveSpeed);

        rb.linearVelocity = gliderVelocity;
    }


    //글라이더 비활성화 함수
    void DisableGlider()
    {
        isGliding = false;

        //글라이더 오브젝트 숨기기
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        //즉시 착하하도록 중력 적용
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    }


}
