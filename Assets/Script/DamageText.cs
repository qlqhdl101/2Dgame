using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro�� �������� TMPro ���ӽ����̽��� ����Ʈ ���ش�.

public class DamageText : MonoBehaviour
{

    public float textSpeed; // text �ö󰡴� �ӵ�
    public float alphaSpeed; // ���������� �ӵ�
    public float destroyTime; // DestroyObject �Լ� Invoke ���� �����ð�
    TextMeshPro text; // TextMeshPro�� Text Class�� ������Ʈ �����̶� ���� �ȴ�.
    Color alpha; //text�� color���� ���� color������ ����
    //public int alarmText;

    void Start()
    {
        // �ʱ�ȭ ����
        text = GetComponent<TextMeshPro>();
        //text.text = alarmText.ToString(); // ToString�� �̿��Ͽ� ���� damage�� string������ ��ȯ
        alpha = text.color;
        Invoke("DestroyObject",destroyTime); // DestroyObject �޼ҵ带 destroyTime�� �ڿ� ����
    }

    void Update()
    {
        // Translate : �ش� ���͸�ŭ �̵������ִ� �Լ�
        // Time.deltaTime : ������Ʈ�� �����Ӵ� 10���Ͱ� �ƴ� �ʴ� 10���� �����̰� ������츦 ��Ÿ����.
        // �� �Լ��� Update�� ������ �� �����Ӹ��� ���� �Ǵ� ���̹Ƿ� ������ ���͸�ŭ ��� �ö󰣴�.
        transform.Translate(new Vector3(0, textSpeed * Time.deltaTime, 0));
        // alpha.a ���� Mathf.Lerp�� �̿��Ͽ� 0�� ��������� ����
        // Mathf.Lerp(a,b,t) : t���� ���ؼ� a�� b ������ ���� ��ȯ�Ѵ�. ex) a=0,b=10,t=0.1�϶�, ��ȯ��:0.1
        alpha.a = Mathf.Lerp(alpha.a, 0, alphaSpeed * Time.deltaTime);
        text.color = alpha;
    }

    // Invoke�� �̿��Ͽ� N�� �Ŀ� ������Ʈ�� �ı��ϴ� �޼ҵ�
    private void DestroyObject()
    {
        Destroy(gameObject); // gameObject�� ���� �ϰڴ�.
    }



}
