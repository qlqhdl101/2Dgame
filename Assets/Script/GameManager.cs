using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


// �Ŵ�����"����"�� "��������"�� �����Ѵ�.
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
        //����ǥ��
        UIPoint.text = (Point).ToString();
    }


    
    public void NextStage()
    {
        //Change Stage
        if(stageIndex < Stages.Length-1) // stage�� Stages.Length(���������� ���̺���) ���ٸ� ����
        {
            Stages[stageIndex].SetActive(false); // ���� stage�� false�Ͽ� �Ⱥ��̰� �����Ѵ�.
            stageIndex++;                        // stage�� 1 ���Ѵ�.
            Stages[stageIndex].SetActive(true);  // 1 ������ stage�� true�� �����Ͽ� ���� stage�� ���̰� �����.
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex); // Stage�� text�� "STAGE"��� �ϸ�, stageIndex�� 0���� �����ϱ⿡ 1�� ���ؼ� �����ش�.
        }
        else // Game Clear!!         stage�� Stages.Length(���������� ���̺���) ���� �ʴٸ� ����
        {
            //Player Contol Lock
            Time.timeScale = 0; // �����ϰ� �Ǹ� timeScale = 0���� �ð��� �����
            //Result UI
            Debug.Log("Clear!!");
            //Restart Button UI
            Text btnText = UIRestartBtn.GetComponentInChildren<Text>(); //��ư �ؽ�Ʈ�� �ڽĿ�����Ʈ�̹Ƿ� InChildren�� �� �ٿ��� �� 
            btnText.text = "��!";
            UIRestartBtn.SetActive(true); // ����� ��ư�� ������ ������ �� Ȱ��ȭ
            ViewBtn();
        }

        //Calculate Point
        
    }

    public void HealthDown()
    {
        /*
        if (health > 1) // HP1 �ʰ��� �����
        {
            health--; // HP1�� �����Ѵ�.
            UIhealth[health].color = new Color(1, 0, 0, 0.2f); // UIhealth�� ������ ������ Color�� �ٲ��.
        }
        
        else // HP�� 1������ �����
        {
            //All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.2f);// UIhealth�� ������ ������ Color�� �ٲ��.

            //Player Die Effect
            player.OnDie(); // O

            //Result UI
            Debug.Log("���");

            //Retry Button UI 
            UIRestartBtn.SetActive(true); // ����� ��ư�� ������ ������ �� Ȱ��ȭ
        }
        */

        health--;

        if (health == 2) // HP�� 3�� ���
        {
            UIhealth1.color = new Color(1, 0, 0, 0.2f);
        }
        if (health == 1) // HP�� 2�� ���
        {
            UIhealth2.color = new Color(1, 0, 0, 0.2f);
        }
        if (health == 0) // HP�� 1�� ���
        {
            UIhealth3.color = new Color(1, 0, 0, 0.2f);

            //Player Die Effect
            player.OnDie(); // O

            //Retry Button UI 
            UIRestartBtn.SetActive(true); // ����� ��ư�� ������ ������ �� Ȱ��ȭ
        }
    }

    public void HealthUp()
    {            
        if (health == 2)
        {
            health++; // HP1�� ����Ѵ�.
            UIhealth1.color = new Color(1, 1, 1, 1); // �� ���·� ���ƿ������� UI ����
        }
        if (health == 1)
        {
            health++; // HP1�� ����Ѵ�.
            UIhealth2.color = new Color(1, 1, 1, 1);
        }
            
    }

    public void HealthFull()
    {
        health = 3; // HP�� 3�� �ȴ�.

        // Health UI ���� ������ ���� 
        UIhealth2.color = new Color(1, 1, 1, 1);
        UIhealth1.color = new Color(1, 1, 1, 1);

    }


    //�浹�� ����
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player") {
            //Player Reposition
            if (health > 1)
            {// ������ ü�¿��� ���������� ��, ����ġ ���� �ʱ�
                PlayerReposition();
            }
            //Health Down
            HealthDown();
        }

    }
    
    // ����Ʈ ĭ�� ���� ��ȭ
    public void LowPoint()
    {
        UIPoint.color = new Color(1, 0, 0, 1); // ����
        Invoke("InvokeColor", 0.05f); // ���� ���� ����
    }

    void InvokeColor()
    {
        UIPoint.color = new Color(1, 1, 1, 1); // ������� ���¿��� �� ���·� ���ƿ������� ĳ���� ����
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
        // Restart�� OnClick() �̺�Ʈ �Լ�
        Time.timeScale = 1; // ������ϰ� �Ǹ� timeScale�� 1�� �ð��� ����
        SceneManager.LoadScene(0);
    }

}
