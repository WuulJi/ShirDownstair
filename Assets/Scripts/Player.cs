using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
     [SerializeField] float moveSpeed = 5f;
     GameObject currentFloor;
     [SerializeField] int hp;
     [SerializeField] GameObject hpBar;
     [SerializeField] TextMeshProUGUI scoreText; // 使用 TextMeshPro 組件
     int score = 0;
     GameObject lastFloor;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
          hp = 10;
          score = 0;
     }

    // Update is called once per frame
    void Update()
    {
          if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
          {
               transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
          }
          else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
          {
               transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
          }          
     }

     private void OnCollisionEnter2D(Collision2D collision)
     {
          if (collision.gameObject.tag == "Normal") 
          {
               if (collision.contacts[0].normal == Vector2.up)
               {
                    Debug.Log("hit Normal");

                    if (currentFloor == null)// 如果 currentFloor 為空，表示是第一次碰到地板，初始化 currentFloor 和 lastFloor
                    {
                         currentFloor = collision.gameObject;
                         lastFloor = currentFloor;
                    }
                    else
                    {
                         lastFloor = currentFloor;
                         currentFloor = collision.gameObject;
                         int floorsJumped = Mathf.Abs(Mathf.FloorToInt(currentFloor.transform.position.y - lastFloor.transform.position.y));
                         Debug.Log("floorsJumped: " + floorsJumped);
                         UpdateScore(floorsJumped);
                    }

                    ModifyHp(1);                    
               }
          }
          else if(collision.gameObject.tag == "Nails")
          {
               if (collision.contacts[0].normal == Vector2.up)
               {
                    Debug.Log("hit Nails");

                    if (currentFloor == null) // 如果 currentFloor 為空，表示是第一次碰到地板，初始化 currentFloor 和 lastFloor
                    {
                         currentFloor = collision.gameObject;
                         lastFloor = currentFloor;
                    }
                    else
                    {
                         lastFloor = currentFloor;
                         currentFloor = collision.gameObject;
                         int floorsJumped = Mathf.Abs(Mathf.FloorToInt(currentFloor.transform.position.y - lastFloor.transform.position.y));
                         Debug.Log("floorsJumped: " + floorsJumped);
                         UpdateScore(floorsJumped);
                    }

                    ModifyHp(-1);                    
               }
          }
          else if(collision.gameObject.tag == "Ceiling")
          {
               Debug.Log("hit Ceiling");
               currentFloor.GetComponent<BoxCollider2D>().enabled = false;
               ModifyHp(-1);
          }
     }

     private void OnTriggerEnter2D(Collider2D collision)
     {
          if (collision.gameObject.tag == "DeathLine")
          {
               Debug.Log("hit Dead");
          }
     }

     void ModifyHp (int num)
     {
          hp += num;
          if (hp < 0) hp = 0;
          else if (hp > 10) hp = 10;
          UpdateHp();
     }

     void UpdateHp()
     {
          for (int i = 0; i < hpBar.transform.childCount; i++)
          {
               if (i < hp)
               {
                    hpBar.transform.GetChild(i).gameObject.SetActive(true);
               }
               else
               {
                    hpBar.transform.GetChild(i).gameObject.SetActive(false);
               }
          }
     }

     public void UpdateScore(int floorsJumped)
     {
          score += floorsJumped;
          scoreText.text = "地下 " +score+ " 層"; 
          //用score.ToString()結果一樣，C#會自動轉字串
     }
}
