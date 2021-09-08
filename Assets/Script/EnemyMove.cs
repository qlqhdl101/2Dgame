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
    public int nextMove; // 행동지표를 결정할 변수 하나를 생성
    public int health; // 피통



    public AudioClip audioHit; // !!!!! Coin Hit Sound !!!!!
    AudioSource audioSource; // !!!!! Coin Hit Sound !!!!!
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxcollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>(); // !!!!! Coin Hit Sound !!!!!

        // Invoke() : 주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수
        Invoke("Think", 2); // Think라는 함수를 5초마다 호출
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
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y); // Think에 있는 nextMove에 따라 -1,0,1의 랜덤값을 받아 이동한다.

        // Platrom Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null) // ray기준으로 collider가 null 값이라면
            Turn();
    }

    // 재귀 함수 : 자신을 스스로 호출하는 함수 / 딜레이 없이 재귀 함수를 사용하는 것은 아주 위험
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2); // random : 랜덤 수를 생성하는 로직 관련 클래스
                                            // Range() : 최소 ~ 최대 범위의 랜덤 수 생성 (최대 제외)
        //Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);

        //Flip Sprite (방향)
        if(nextMove != 0)
        spriteRenderer.flipX = nextMove == 1;

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void Turn()
    {
        nextMove *= -1; // +면-로 -면+로 변경
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke(); // CancleInvoke() : 현재 작동 중인 모든 Invoke함수를 멈추는 함수
        Invoke("Think", 2); // 다시 실행

    }

    public void OnDamaged() // 데미지를 받는다면~
    {
        
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 반 투명 상태가 된다
        //Sprite Flip Y
        spriteRenderer.flipY = true; //뒤집어짐
        //Collider Disable
        boxcollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //Destroy
        Invoke("DeActive", 2);

    }

    // 몬스터 공격 받음
    void OnHit(int dmg)
    {
        health -= dmg; // dmg만큼 helth의 수치가 낮아짐

        if (health <= 0) //health가 0이하라면
        {
            OnDamaged(); // OnDamaged 메소드를 불러옴
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>(); // Bullet 스크립트(컴포넌트) 불러오기
            OnHit(bullet.dmg);

            // Sound
            PlaySound("HIT");
            audioSource.Play();
        }

    }

    void DeActive()
    {
        gameObject.SetActive(false); // 오브젝트의 활성화 비활성화
    }

}
