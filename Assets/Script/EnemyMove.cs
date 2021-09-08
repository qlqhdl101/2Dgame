using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public PlayerMove playerMove;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxcollider;
    public int nextMove; // �ൿ��ǥ�� ������ ���� �ϳ��� ����
    public int health; // ����



    public AudioClip audioHit; // !!!!! Coin Hit Sound !!!!!
    AudioSource audioSource; // !!!!! Coin Hit Sound !!!!!
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxcollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>(); // !!!!! Coin Hit Sound !!!!!

        // Invoke() : �־��� �ð��� ���� ��, ������ �Լ��� �����ϴ� �Լ�
        Invoke("Think", 2); // Think��� �Լ��� 5�ʸ��� ȣ��
    }

    public void PlaySound(string action) // !!!!! Coin Hit Sound !!!!!
    {
        switch (action)
        {
            case "HIT":
                audioSource.clip = audioHit;
                break;
        }
    }


    void FixedUpdate()
    {

        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y); // Think�� �ִ� nextMove�� ���� -1,0,1�� �������� �޾� �̵��Ѵ�.

        // Platrom Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null) // ray�������� collider�� null ���̶��
            Turn();
    }

    // ��� �Լ� : �ڽ��� ������ ȣ���ϴ� �Լ� / ������ ���� ��� �Լ��� ����ϴ� ���� ���� ����
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2); // random : ���� ���� �����ϴ� ���� ���� Ŭ����
                                            // Range() : �ּ� ~ �ִ� ������ ���� �� ���� (�ִ� ����)
        //Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);

        //Flip Sprite (����)
        if(nextMove != 0)
        spriteRenderer.flipX = nextMove == 1;

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void Turn()
    {
        nextMove *= -1; // +��-�� -��+�� ����
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke(); // CancleInvoke() : ���� �۵� ���� ��� Invoke�Լ��� ���ߴ� �Լ�
        Invoke("Think", 2); // �ٽ� ����

    }

    public void OnDamaged() // �������� �޴´ٸ�~
    {
        
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // �� ���� ���°� �ȴ�
        //Sprite Flip Y
        spriteRenderer.flipY = true; //��������
        //Collider Disable
        boxcollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //Destroy
        Invoke("DeActive", 2);

    }

    // ���� ���� ����
    void OnHit(int dmg)
    {
        health -= dmg; // dmg��ŭ helth�� ��ġ�� ������

        if (health <= 0) //health�� 0���϶��
        {
            OnDamaged(); // OnDamaged �޼ҵ带 �ҷ���
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>(); // Bullet ��ũ��Ʈ(������Ʈ) �ҷ�����
            OnHit(bullet.dmg);

            // Sound
            PlaySound("HIT");
            audioSource.Play();
        }

    }

    void DeActive()
    {
        gameObject.SetActive(false); // ������Ʈ�� Ȱ��ȭ ��Ȱ��ȭ
    }

}
