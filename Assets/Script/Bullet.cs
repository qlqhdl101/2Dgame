using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;

    void OnTriggerEnter2D(Collider2D collision) // OnCollisionEnter2D : Trigger 체크가 안되어 있는 물체끼리만 반응
    {                                           // OnTriggerEnter2D : Trigger가 한쪽만 체크 되어있으면 반응

        // 몬스터에 닿는다면
        if (collision.gameObject.tag == "Enemy")
        {            
            Debug.Log("몬스터 닿음");
            Destroy(gameObject); // 매개변수 오브젝트를 삭제하는 함수
        }

        // 길에 닿는다면
        if (collision.gameObject.tag == "Road")
        {
            Destroy(gameObject); // 매개변수 오브젝트를 삭제하는 함수
            Debug.Log("길에 닿음");
        }

    }
}
