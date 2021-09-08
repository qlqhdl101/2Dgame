using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 쓰기위해 TMPro 네임스페이스를 임포트 해준다.

public class DamageText : MonoBehaviour
{

    public float textSpeed; // text 올라가는 속도
    public float alphaSpeed; // 투명해지는 속도
    public float destroyTime; // DestroyObject 함수 Invoke 실행 지연시간
    TextMeshPro text; // TextMeshPro는 Text Class의 업데이트 버전이라 봐도 된다.
    Color alpha; //text의 color값을 담을 color변수를 선언
    //public int alarmText;

    void Start()
    {
        // 초기화 선언
        text = GetComponent<TextMeshPro>();
        //text.text = alarmText.ToString(); // ToString을 이용하여 변수 damage를 string형으로 변환
        alpha = text.color;
        Invoke("DestroyObject",destroyTime); // DestroyObject 메소드를 destroyTime초 뒤에 실행
    }

    void Update()
    {
        // Translate : 해당 벡터만큼 이동시켜주는 함수
        // Time.deltaTime : 오브젝트를 프레임당 10미터가 아닌 초당 10미터 움직이고 싶은경우를 나타낸다.
        // 이 함수를 Update에 넣으면 매 프레임마다 실행 되는 것이므로 지정한 벡터만큼 계속 올라간다.
        transform.Translate(new Vector3(0, textSpeed * Time.deltaTime, 0));
        // alpha.a 값을 Mathf.Lerp를 이용하여 0에 가까워지게 만듦
        // Mathf.Lerp(a,b,t) : t값에 의해서 a와 b 사이의 값을 반환한다. ex) a=0,b=10,t=0.1일때, 반환값:0.1
        alpha.a = Mathf.Lerp(alpha.a, 0, alphaSpeed * Time.deltaTime);
        text.color = alpha;
    }

    // Invoke를 이용하여 N초 후에 오브젝트를 파괴하는 메소드
    private void DestroyObject()
    {
        Destroy(gameObject); // gameObject를 삭제 하겠다.
    }



}
