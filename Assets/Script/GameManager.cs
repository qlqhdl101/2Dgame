using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


// 매니저는"점수"와 "스테이지"를 관리한다.
public class GameManager : MonoBehaviour
{
    public int Point;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    //public Image[] UIhealth;
    public Image UIhealth3;
    public Image UIhealth2;
    public Image UIhealth1;

    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartBtn;

    private void Update( )
    {
        //점수표시
        UIPoint.text = (Point).ToString();
    }


    
    public void NextStage()
    {
        //Change Stage
        if(stageIndex < Stages.Length-1) // stage가 Stages.Length(스테이지의 길이보다) 적다면 실행
        {
            Stages[stageIndex].SetActive(false); // 현재 stage를 false하여 안보이게 설정한다.
            stageIndex++;                        // stage를 1 더한다.
            Stages[stageIndex].SetActive(true);  // 1 더해진 stage를 true로 설정하여 다음 stage가 보이게 만든다.
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex); // Stage의 text는 "STAGE"라고 하며, stageIndex는 0부터 시작하기에 1을 더해서 적어준다.
        }
        else // Game Clear!!         stage가 Stages.Length(스테이지의 길이보다) 적지 않다면 실행
        {
            //Player Contol Lock
            Time.timeScale = 0; // 완주하게 되면 timeScale = 0으로 시간을 멈춰둠
            //Result UI
            Debug.Log("Clear!!");
            //Restart Button UI
            Text btnText = UIRestartBtn.GetComponentInChildren<Text>(); //버튼 텍스트는 자식오브젝트이므로 InChildren을 더 붙여야 함 
            btnText.text = "끝!";
            UIRestartBtn.SetActive(true); // 재시작 버튼은 게임이 끝났을 때 활성화
            ViewBtn();
        }

        //Calculate Point
        
    }

    public void HealthDown()
    {
        /*
        if (health > 1) // HP1 초과일 경우라면
        {
            health--; // HP1이 감소한다.
            UIhealth[health].color = new Color(1, 0, 0, 0.2f); // UIhealth의 색상이 설정한 Color로 바뀐다.
        }
        
        else // HP가 1이하일 경우라면
        {
            //All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.2f);// UIhealth의 색상이 설정한 Color로 바뀐다.

            //Player Die Effect
            player.OnDie(); // O

            //Result UI
            Debug.Log("사망");

            //Retry Button UI 
            UIRestartBtn.SetActive(true); // 재시작 버튼은 게임이 끝났을 때 활성화
        }
        */

        health--;

        if (health == 2) // HP가 3일 경우
        {
            UIhealth1.color = new Color(1, 0, 0, 0.2f);
        }
        if (health == 1) // HP가 2일 경우
        {
            UIhealth2.color = new Color(1, 0, 0, 0.2f);
        }
        if (health == 0) // HP가 1일 경우
        {
            UIhealth3.color = new Color(1, 0, 0, 0.2f);

            //Player Die Effect
            player.OnDie(); // O

            //Retry Button UI 
            UIRestartBtn.SetActive(true); // 재시작 버튼은 게임이 끝났을 때 활성화
        }
    }

    public void HealthUp()
    {            
        if (health == 2)
        {
            health++; // HP1이 상승한다.
            UIhealth1.color = new Color(1, 1, 1, 1); // 본 상태로 돌아왔을때의 UI 색상
        }
        if (health == 1)
        {
            health++; // HP1이 상승한다.
            UIhealth2.color = new Color(1, 1, 1, 1);
        }
            
    }

    public void HealthFull()
    {
        health = 3; // HP가 3이 된다.

        // Health UI 색상 원상태 복귀 
        UIhealth2.color = new Color(1, 1, 1, 1);
        UIhealth1.color = new Color(1, 1, 1, 1);

    }


    //충돌시 실행
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player") {
            //Player Reposition
            if (health > 1)
            {// 마지막 체력에서 낭떨어지일 땐, 원위치 하지 않기
                PlayerReposition();
            }
            //Health Down
            HealthDown();
        }

    }
    
    // 포인트 칸의 색상 변화
    public void LowPoint()
    {
        UIPoint.color = new Color(1, 0, 0, 1); // 부족
        Invoke("InvokeColor", 0.05f); // 원래 색상 복귀
    }

    void InvokeColor()
    {
        UIPoint.color = new Color(1, 1, 1, 1); // 무적모드 상태에서 본 상태로 돌아왔을때의 캐릭터 색상
    }

    void PlayerReposition()
    {
        //collision.attachedRigidbody.velocity = Vector2.zero;
        //collision.transform.position = new Vector3(0, 0, -1);
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    void ViewBtn()
    {
        UIRestartBtn.SetActive(true);
    }

    public void Restart()
    {
        // Restart의 OnClick() 이벤트 함수
        Time.timeScale = 1; // 재시작하게 되면 timeScale을 1로 시간을 복구
        SceneManager.LoadScene(0);
    }

}
