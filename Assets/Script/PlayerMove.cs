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

    public GameObject bulletObjGold;//�Ѿ� ������Ʈ / ������
    public GameObject alarm; // �˶� ������Ʈ / ������
    
    Rigidbody2D rigid; // Unity�� Rigidbody2D ����� rigid��� ������ ����
    SpriteRenderer spriteRenderer; // Unity�� SpriteRenderer ����� spriteRenderer��� ������ ����
    CapsuleCollider2D capuleCollider;
    Animator anim;
    AudioSource audioSource;

    void Awake()
    {
        // �ʱ�ȭ �۾�
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

    private void Update() // �ܹ����� Ű �Է��� Update
                          // ����Ʈ ���� 1�ʿ� 60ȸ(������)
    {
        // Coin Attack
        Fire();

        // Jump
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            Debug.Log("Alt����"); //ButtonDown("Jump")
            if (Input.GetKeyDown(KeyCode.LeftAlt) && !anim.GetBool("isJumping")) {  // !aim.GetBool("isJumping") : �Ķ������ ���� �ҷ��� �ִϸ��̼��� ������� �������� ���� �϶�� ����
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);

            // Sound
            PlaySound("JUMP");
            audioSource.Play();
            }
        }


        // Stop Speed(���� �ӵ�)
        if (Input.GetButtonUp("Horizontal")) // ��ư�� ���� (�����°� Down)
        {
            //rigid.velocity.normalized // ����(����)�� ���Ҷ� ��� (normalized), ���� ũ�⸦ 1�� ���� ���� (��������) 
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite(���� ��ȯ)
        if (Input.GetButton("Horizontal")) { // ��ư�� ������
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
           }
        
        // Animation(�ൿ�� ���� �ִϸ��̼� ����)
        if (Mathf.Abs(rigid.velocity.x) < 0.2) // �¿� �̵��ӵ��� 0(����)�� ���ٸ�
            anim.SetBool("isWalking", false); // �����س��� �Ű����� Ÿ��("������ �̸�", false)
        else // �¿� �̵��ӵ��� 0(����)�� ���� �ʴٸ�
            anim.SetBool("isWalking", true); // �����س��� �Ű����� Ÿ��("������ �̸�", true)

    }

    void FixedUpdate() // �������� Ű �Է��� FixedUpdate
                       // ������ ���� �ٸ����� ����Ʈ ���� 1�ʿ� �� 50ȸ ���� ����.
    {
        // Move Speed(�̵��ӵ�)
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        // �̵� ��ư�� �� ������ AddForce�� ���� 1�ʴ� 50�� �� ���̴�.

        // Max Speed(�ִ�ӵ�)
        if (rigid.velocity.x > maxSpeed) //Right Max Speed ������ �ִ� �Է°�
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        // velocity : ������ٵ��� ���� �ӵ�
        else if (rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed ���� �ִ� �Է°�
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        // Landing Platform
        // DrawRay() : ������ �󿡼��� Ray�� �׷��ִ� �Լ�
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); // ������ٵ��� ��ġ���� ���� �Ʒ����� �ް� ������ 0,1,0�̴�.

        // RayCast : ������Ʈ �˻��� ���� Ray�� ��� ���

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform")); // ���� ��������̱⿡ Physics2D�� Ȱ���ؾ��Ѵ�.
        // RayCastHit : Ray�� ���� ������Ʈ  
        // GetMask() : ���̾� �̸��� �ش��ϴ� �������� �����ϴ� �Լ�
        // rayHit : ���� ��� �ű⿡ ���� ������Ʈ�� ����

        // RayCast�� �����ϰ� �ִ� ��ü�� �̸� ���
        if (rayHit.collider != null) // rayHit.collider�� �浹�� ���� ���� ������ null ���� ���� �ȴ�.
        {
            Debug.Log(rayHit.collider.name);
        }
        else // rayHit.collider�� �ƹ��͵� �������� �ʴ´ٸ� ����
        {
            Debug.Log("null ����");
        }

        // ������ �Ҷ� y�� ���� ���̳ʽ��� ���
        if (rigid.velocity.y < 0)
        {   // ���� ���� ���� �ʾҴٸ�
            if (rayHit.collider != null)
            {// RayCastHit������ �ݶ��̴��� �˻� Ȯ�� ����
                if (rayHit.distance < 0.5f) //distance : Ray�� ����� ���� �Ÿ� 
                    anim.SetBool("isJumping", false);
            }
        }
    }
    //����(��) �߻�
    void Fire()
    {
        // GetButton : ��ư�� �� ������ �Ѿ��� ��� ������ Down, Up�� �� 1���������� �ѹ��� ����
        if (!Input.GetButtonDown("Fire1"))  // Fire1�̶�� ��ư�� ������ �ʾҴٸ�(!) 
            return; // ����(���������ʴ´�)�Ѵ�.

        if (Input.GetButtonDown("Fire1"))
        {
            if (gameManager.Point >= 100) // Point�� 100 �̻��̶�� 
            {
                gameManager.Point -= 100; // Point 100 ����

                // ���� �߻� �ִϸ����� true
                anim.SetBool("isCoin_Attack", true);
                // Invoke_CoinAttackAni�� 0.4�� �� ����
                Invoke("Invoke_CoinAttackAni", 0.4f);

                // Instantiate() : �Ű����� ������Ʈ�� �����ϴ� �Լ�  // ��ġ, ȸ�� �Ű������� �÷��̾� transform�� ���
                GameObject bullet = Instantiate(bulletObjGold, transform.position, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>(); //������ٵ� ������ Addforce()�� �Ѿ� �߻� ���� �ۼ�
                                                                        

                if (spriteRenderer == spriteRenderer.flipX) // flipX�� ���¶�� �����Ͽ��� 
                {
                    rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
                }



                else // flipX�� �ƴ϶�� �����Ͽ���
                    rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                // Sound
                PlaySound("TOSS");
                audioSource.Play();

            }
            else // Point�� 100 �̻��� �ƴ϶��
            {
                //alarm�� ��ġ, ȸ�� �Ű������� �÷��̾� transform�� ���
                Instantiate(alarm, transform.position, transform.rotation); // �Ѿ��� �߻��� ����Ʈ�� ���ٰ� �˸�
                
                gameManager.LowPoint(); // Point�� ���� ���������� 0.05�� ���� �ƴٰ� �ٽ� ���ƿ´�.
                
                // Sound
                PlaySound("NOPOINT");
                audioSource.Play();
            }
        }
    }
    void Invoke_CoinAttackAni() // Invoke coinAttack �ִϸ����� ����ð�
    {
        anim.SetBool("isCoin_Attack", false); // ���ݸ�� ����
    }


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy") // ���� �ױװ� Enemy�� ���
        {
            // ���
            // Enemy�� Player���� �������� �ִٸ�  velocity : �� �� �� ��Ÿ����.
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

        if (collision.gameObject.tag == "Spike") // ���� �ױװ� Spike�� ���
        {
            OnDamaged(collision.transform.position); // OnDamaged�Լ��� ȣ���Ѵ�.

            // Sound
            PlaySound("DAMAGED");
            audioSource.Play();

            //Tutorial Stage
            Tutorialstopper();
        }

    }

    // �浹�� ����!
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Stage1 Text
        if (collision.gameObject.tag == "Arrow")
        {

            bool isArrow1 = collision.gameObject.name.Contains("Arrow1");
            if (isArrow1)
            {
                collision.gameObject.SetActive(false);
                AlarmText.text = "Alt�� ���� �پ���?";
                
            }

            bool isArrow2 = collision.gameObject.name.Contains("Arrow2");
            if (isArrow2)
            {
                collision.gameObject.SetActive(false); // �Ⱥ��̰� ����
                AlarmText.text = "���ƿ�! �̹��� �տ� �ִ� ������ ��� ������ ȹ���ϼ���!";
            }

            bool isArrow3 = collision.gameObject.name.Contains("Arrow3");
            if (isArrow3)
            {
                AlarmText.text = "������ ���ƴٰ� Ŭ���� ���� �� ������ �����Ͻʼ�!";

                if (stopper == 3)
                {
                    collision.gameObject.SetActive(false);
                    AlarmText.text = "CtrlŰ�� ���� Coin Attack ����� ���ʽÿ�!";
                }

                
            }

            bool isArrow4 = collision.gameObject.name.Contains("Arrow4");
            if (isArrow4)
            {
                AlarmText.text = "Ctrl�� ������ Coin Attack�� ��� �˴ϴ�!";

                if (gameManager.Point <= 280)
                {
                    collision.gameObject.SetActive(false);
                    AlarmText.text = "Coin Attack�� 100 Point�� ����մϴ�! ";
                }
                
            }

            bool isArrow5 = collision.gameObject.name.Contains("Arrow5");
            if (isArrow5)
            {
                collision.gameObject.SetActive(false);
                AlarmText.text = "���ø� �ѹ� ��ƺ�����!";
                
            }
            bool isArrow6 = collision.gameObject.name.Contains("Arrow6");
            if (isArrow6)
            {
                AlarmText.text = "���� �����󱸿� ���!!";

                if (stopper == 4)
                {
                    AlarmText.text = "���ø� ������ �ǰ� 1���� �Ǿ����ϴ�!";
                    collision.gameObject.SetActive(false);
                }
               
            }

            bool isArrow7 = collision.gameObject.name.Contains("Arrow7");
            if (isArrow7)
            {
                collision.gameObject.SetActive(false);
                AlarmText.text = "������ �ѹ� �Ծ� �����?";
                
            }

            bool isArrow8 = collision.gameObject.name.Contains("Arrow8");
            if (isArrow8)
            {
                AlarmText.text = "������ �ȸ����� �ᱹ �״°̴ϴ� ^^?";

                if (stopper == 5)
                {
                    collision.gameObject.SetActive(false);
                    AlarmText.text = "����� 1ȸ�� �Ǿ����ϴ�!";
                }
            }

            bool isArrow9 = collision.gameObject.name.Contains("Arrow9");
            if (isArrow9)
            {
                collision.gameObject.SetActive(false);
                AlarmText.text = "���ƿ�! �̻����� ���� ������ ��ġ�ڽ��ϴ�! �տ� ��߷� ���� Ʃ�丮���� �����ּ���!";
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
            collision.gameObject.SetActive(false); // �Ⱥ��̰� �ٲ�

            gameManager.HealthUp();

        }

        else if (collision.gameObject.tag == "PotionFull")
        {
            collision.gameObject.SetActive(false); // �Ⱥ��̰� �ٲ�

            gameManager.HealthFull();

            //Tutorial Stage
            Tutorialstopper();

        }
    }


    // �������� �Լ��� ȣ��
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

    // ��������, �ǰ�
    void OnDamaged(Vector2 targetPos)
    {
        // Health Down
        gameManager.HealthDown(); // �������� �޾������ HealthDown �Լ��� �ҷ�����.

        // Change Layer (Immortal Active)
        gameObject.layer = 11; // Layer 11������ ����(PlayerDamaged)

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // ĳ���� �ǰ� �� ������� ����

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse); // �ǰ� �޾����� ƨ�ܳ���

        // Animation
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 0.8f); // �����ð�
    }

    // ��������
    void OffDamaged()
    {
        gameObject.layer = 10; // Layer 10������ ����(Player)
        spriteRenderer.color = new Color(1, 1, 1, 1); // ������� ���¿��� �� ���·� ���ƿ������� ĳ���� ����

    }

    public void OnDie()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // �� ���� ���°� �ȴ�
        //Sprite Flip Y
        spriteRenderer.flipY = true; //��������
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
