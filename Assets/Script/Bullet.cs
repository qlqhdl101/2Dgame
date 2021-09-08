using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;

    void OnTriggerEnter2D(Collider2D collision) // OnCollisionEnter2D : Trigger üũ�� �ȵǾ� �ִ� ��ü������ ����
    {                                           // OnTriggerEnter2D : Trigger�� ���ʸ� üũ �Ǿ������� ����

        // ���Ϳ� ��´ٸ�
        if (collision.gameObject.tag == "Enemy")
        {            
            Debug.Log("���� ����");
            Destroy(gameObject); // �Ű����� ������Ʈ�� �����ϴ� �Լ�
        }

        // �濡 ��´ٸ�
        if (collision.gameObject.tag == "Road")
        {
            Destroy(gameObject); // �Ű����� ������Ʈ�� �����ϴ� �Լ�
            Debug.Log("�濡 ����");
        }

    }
}
