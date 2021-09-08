using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public EnemyMove enemyMove;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public AudioClip audioNoPoint;
    public AudioClip audioToss;
    public AudioClip audioPotion;
    //public AudioClip audioHit2;
    public int stopper;

    public float maxSpeed;
    public float jumpPower;
    public Text AlarmText;

    public GameObject bulletObjGold;//총알 오브젝트 / 프리펩
    public GameObject alarm; // 알람 오브젝트 / 프리펩
    
    Rigidbody2D rigid; // Unity의 Rigidbody2D 기능을 rigid라는 변수명에 설정
    SpriteRenderer spriteRenderer; // Unity의 SpriteRenderer 기능을 spriteRenderer라는 변수명에 설정
    CapsuleCollider2D capuleCollider;
    Animator anim;
    AudioSource audioSource;

    void Awake()
    {
        // 초기화 작업
        rigid = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        capuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
            case "NOPOINT":
                audioSource.clip = audioNoPoint;
                break;
            case "TOSS":
                audioSource.clip = audioToss;
                break;
            case "POTION":
                audioSource.clip = audioPotion;
                break;

        }
    }

    private void Update() // 단발적인 키 입력은 Update
                          // 디폴트 값이 1초에 60회(프레임)
    {
        // Coin Attack
        Fire();

        // Jump
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            Debug.Log("Alt누름"); //ButtonDown("Jump")
            if (Input.GetKeyDown(KeyCode.LeftAlt) && !anim.GetBool("isJumping")) {  // !aim.GetBool("isJumping") : 파라미터의 값을 불러와 애니메이션이 진행되지 않을때만 실행 하라는 조건
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);

            // Sound
            PlaySound("JUMP");
            audioSource.Play();
            }
        }


        // Stop Speed(멈춤 속도)
        if (Input.GetButtonUp("Horizontal")) // 버튼은 땔때 (누르는건 Down)
        {
            //rigid.velocity.normalized // 단위(방향)를 구할때 사용 (normalized), 벡터 크기를 1로 만든 상태 (단위벡터) 
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite(방향 전환)
        if (Input.GetButton("Horizontal")) { // 버튼을 누를때
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
           }
        
        // Animation(행동에 따른 애니메이션 변경)
        if (Mathf.Abs(rigid.velocity.x) < 0.2) // 좌우 이동속도가 0(멈춤)과 같다면
            anim.SetBool("isWalking", false); // 설정해놓은 매개변수 타입("설정한 이름", false)
        else // 좌우 이동속도가 0(멈춤)과 같지 않다면
            anim.SetBool("isWalking", true); // 설정해놓은 매개변수 타입("설정한 이름", true)

    }

    void FixedUpdate() // 지속적인 키 입력은 FixedUpdate
                       // 설정에 따라 다르지만 디폴트 값이 1초에 약 50회 정도 돈다.
    {
        // Move Speed(이동속도)
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        // 이동 버튼을 꾹 누르면 AddForce에 힘을 1초당 50씩 준 것이다.

        // Max Speed(최대속도)
        if (rigid.velocity.x > maxSpeed) //Right Max Speed 오른쪽 최대 입력값
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        // velocity : 리지드바디의 현재 속도
        else if (rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed 왼쪽 최대 입력값
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        // Landing Platform
        // DrawRay() : 에디터 상에서만 Ray를 그려주는 함수
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); // 리지드바디의 위치에서 시작 아래에서 받고 색상은 0,1,0이다.

        // RayCast : 오브젝트 검색을 위해 Ray를 쏘는 방식

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform")); // 빔은 물리기반이기에 Physics2D를 활용해야한다.
        // RayCastHit : Ray에 닿은 오브젝트  
        // GetMask() : 레이어 이름에 해당하는 정수값을 리턴하는 함수
        // rayHit : 빔을 쏘고 거기에 맞은 오브젝트의 정보

        // RayCast가 인지하고 있는 물체의 이름 출력
        if (rayHit.collider != null) // rayHit.collider는 충돌이 감지 되지 않으면 null 값을 갖게 된다.
        {
            Debug.Log(rayHit.collider.name);
        }
        else // rayHit.collider가 아무것도 감지되지 않는다면 실행
        {
            Debug.Log("null 감지");
        }

        // 점프를 할때 y의 값이 마이너스일 경우
        if (rigid.velocity.y < 0)
        {   // 만약 빔을 맞지 않았다면
            if (rayHit.collider != null)
            {// RayCastHit변수의 콜라이더로 검색 확인 가능
                if (rayHit.distance < 0.5f) //distance : Ray에 닿았을 때의 거리 
                    anim.SetBool("isJumping", false);
            }
        }
    }
    //코인(총) 발사
    void Fire()
    {
        // GetButton : 버튼을 꾹 누르면 총알이 계속 나가고 Down, Up은 딱 1프레임으로 한번만 반응
        if (!Input.GetButtonDown("Fire1"))  // Fire1이라는 버튼을 누르지 않았다면(!) 
            return; // 리턴(실행하지않는다)한다.

        if (Input.GetButtonDown("Fire1"))
        {
            if (gameManager.Point >= 100) // Point가 100 이상이라면 
            {
                gameManager.Point -= 100; // Point 100 차감

                // 코인 발사 애니메이터 true
                anim.SetBool("isCoin_Attack", true);
                // Invoke_CoinAttackAni을 0.4초 뒤 실행
                Invoke("Invoke_CoinAttackAni", 0.4f);

                // Instantiate() : 매개변수 오브젝트를 생성하는 함수  // 위치, 회전 매개변수는 플레이어 transform을 사용
                GameObject bullet = Instantiate(bulletObjGold, transform.position, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>(); //리지드바디를 가져와 Addforce()로 총알 발사 로직 작성
                                                                        

                if (spriteRenderer == spriteRenderer.flipX) // flipX인 상태라면 실행하여라 
                {
                    rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
                }



                else // flipX이 아니라면 실행하여라
                    rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                // Sound
                PlaySound("TOSS");
                audioSource.Play();

            }
            else // Point가 100 이상이 아니라면
            {
                //alarm의 위치, 회전 매개변수는 플레이어 transform을 사용
                Instantiate(alarm, transform.position, transform.rotation); // 총알을 발사할 포인트가 없다고 알림
                
                gameManager.LowPoint(); // Point의 색이 빨간색으로 0.05초 동안 됐다가 다시 돌아온다.
                
                // Sound
                PlaySound("NOPOINT");
                audioSource.Play();
            }
        }
    }
    void Invoke_CoinAttackAni() // Invoke coinAttack 애니메이터 실행시간
    {
        anim.SetBool("isCoin_Attack", false); // 공격모션 종료
    }


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy") // 만약 테그가 Enemy일 경우
        {
            // 밟기
            // Enemy가 Player보다 낮은곳에 있다면  velocity : 상 하 를 나타낸다.
            if (rigid.velocity.y < 1 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);

                // Sound
                PlaySound("ATTACK");
                audioSource.Play();
            }
            
            else
            {//Damaged
                OnDamaged(collision.transform.position);

                // Sound
                PlaySound("DAMAGED");
                audioSource.Play();
            }
        }

        if (collision.gameObject.tag == "Spike") // 만약 테그가 Spike일 경우
        {
            OnDamaged(collision.transform.position); // OnDamaged함수를 호출한다.

            // Sound
            PlaySound("DAMAGED");
            audioSource.Play();

            //Tutorial Stage
            Tutorialstopper();
        }

    }

    // 충돌시 실행!
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Stage1 Text
        if (collision.gameObject.tag == "Arrow")
        {

            bool isArrow1 = collision.gameObject.name.Contains("Arrow1");
            if (isArrow1)
            {
                collision.gameObject.SetActive(false);
                AlarmText.text = "Alt를 눌러 뛰어볼까요?";
                
            }

            bool isArrow2 = collision.gameObject.name.Contains("Arrow2");
            if (isArrow2)
            {
                collision.gameObject.SetActive(false); // 안보이게 만듦
                AlarmText.text = "좋아요! 이번엔 앞에 있는 동전을 얻어 점수를 획득하세요!";
            }

            bool isArrow3 = collision.gameObject.name.Contains("Arrow3");
            if (isArrow3)
            {
                AlarmText.text = "동전을 놓쳤다간 클리어 못할 수 있으니 주의하십쇼!";

                if (stopper == 3)
                {
                    collision.gameObject.SetActive(false);
                    AlarmText.text = "Ctrl키를 눌러 Coin Attack 사용해 보십시오!";
                }

                
            }

            bool isArrow4 = collision.gameObject.name.Contains("Arrow4");
            if (isArrow4)
            {
                AlarmText.text = "Ctrl을 눌러야 Coin Attack이 사용 됩니다!";

                if (gameManager.Point <= 280)
                {
                    collision.gameObject.SetActive(false);
                    AlarmText.text = "Coin Attack은 100 Point를 사용합니다! ";
                }
                
            }

            bool isArrow5 = collision.gameObject.name.Contains("Arrow5");
            if (isArrow5)
            {
                collision.gameObject.SetActive(false);
                AlarmText.text = "가시를 한번 밟아보세요!";
                
            }
            bool isArrow6 = collision.gameObject.name.Contains("Arrow6");
            if (isArrow6)
            {
                AlarmText.text = "가시 밟으라구요 밟아!!";

                if (stopper == 4)
                {
                    AlarmText.text = "가시를 밟으니 피가 1감소 되었습니다!";
                    collision.gameObject.SetActive(false);
                }
               
            }

            bool isArrow7 = collision.gameObject.name.Contains("Arrow7");
            if (isArrow7)
            {
                collision.gameObject.SetActive(false);
                AlarmText.text = "포션을 한번 먹어 볼까요?";
                
            }

            bool isArrow8 = collision.gameObject.name.Contains("Arrow8");
            if (isArrow8)
            {
                AlarmText.text = "포션을 안먹으면 결국 죽는겁니다 ^^?";

                if (stopper == 5)
                {
                    collision.gameObject.SetActive(false);
                    AlarmText.text = "목숨이 1회복 되었습니다!";
                }
            }

            bool isArrow9 = collision.gameObject.name.Contains("Arrow9");
            if (isArrow9)
            {
                collision.gameObject.SetActive(false);
                AlarmText.text = "좋아요! 이상으로 게임 설명을 마치겠습니다! 앞에 깃발로 가서 튜토리얼을 끝내주세요!";
            }
        }

        if (collision.gameObject.tag == "Potion")
        {
            // Sound
            PlaySound("POTION");
            audioSource.Play(); 
        }

        if (collision.gameObject.tag == "Item")
        {
            // Point
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            if (isBronze)
            {
                gameManager.Point += 50;

                //Tutorial Stage
                Tutorialstopper();
            }
            else if (isSilver)
            {
                gameManager.Point += 100;

                //Tutorial Stage
                Tutorialstopper();
            }
            else if (isGold)
            {
                gameManager.Point += 200;

                //Tutorial Stage
                Tutorialstopper();
            }
            // Deactive Item
            collision.gameObject.SetActive(false);

            // Sound
            PlaySound("ITEM");
            audioSource.Play();
        }
        else if(collision.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();

            // Sound
            PlaySound("FINISH");
            audioSource.Play();
        }

        else if (collision.gameObject.tag == "Potion")
        {
            collision.gameObject.SetActive(false); // 안보이게 바뀜

            gameManager.HealthUp();

        }

        else if (collision.gameObject.tag == "PotionFull")
        {
            collision.gameObject.SetActive(false); // 안보이게 바뀜

            gameManager.HealthFull();

            //Tutorial Stage
            Tutorialstopper();

        }
    }


    // 죽음관련 함수를 호출
    void OnAttack(Transform enemy) 
    {
        //Point
        gameManager.Point += 20;

        //Reaction Force
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        //Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    // 무적상태, 피격
    void OnDamaged(Vector2 targetPos)
    {
        // Health Down
        gameManager.HealthDown(); // 데미지를 받았을경우 HealthDown 함수를 불러낸다.

        // Change Layer (Immortal Active)
        gameObject.layer = 11; // Layer 11번으로 변경(PlayerDamaged)

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 캐릭터 피격 후 무적모드 색상

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse); // 피격 받았을때 튕겨나감

        // Animation
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 0.8f); // 무적시간
    }

    // 무적해제
    void OffDamaged()
    {
        gameObject.layer = 10; // Layer 10번으로 변경(Player)
        spriteRenderer.color = new Color(1, 1, 1, 1); // 무적모드 상태에서 본 상태로 돌아왔을때의 캐릭터 색상

    }

    public void OnDie()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 반 투명 상태가 된다
        //Sprite Flip Y
        spriteRenderer.flipY = true; //뒤집어짐
        //Collider Disable
        capuleCollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Sound
        PlaySound("DIE");
        audioSource.Play();
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    public void Tutorialstopper()
    {
        //Tutorial Stage
        stopper++;
    }

}
