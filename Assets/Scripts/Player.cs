using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
     [SerializeField] float moveSpeed = 5f;
     GameObject currentFloor;
     [SerializeField] int hp;
     [SerializeField] GameObject hpBar;
     [SerializeField] TextMeshProUGUI scoreText; // 使用 TextMeshPro 組件
     int score = 0;
     GameObject lastFloor;
     Animator anim;
     SpriteRenderer sprR;
     AudioSource deathSound;
     [SerializeField] GameObject replayButton;
     int damage = -2; // 受傷時的傷害值
     int heal = 1; // 碰到地板時的回血值
     float gameAcc = 0.05f; // 加快遊戲速度

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          hp = 10;
          score = 0;
          anim = GetComponent<Animator>();
          sprR = GetComponent<SpriteRenderer>();
          deathSound = GetComponent<AudioSource>();
          replayButton.SetActive(false);
          Time.timeScale = 1; // 確保遊戲開始時時間正常流逝
     }

     // Update is called once per frame
     void Update()
     {
          if (Time.timeScale != 0) // 暫停就不加速
          {
               Time.timeScale += gameAcc * Time.deltaTime; //加快遊戲速度
               //Debug.Log("Time.timeScale: " + Time.timeScale);
          }
          
          if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
          {
               transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
               sprR.flipX = true;
               anim.SetBool("Move", true);
          }
          else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
          {
               transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
               sprR.flipX = false;
               anim.SetBool("Move", true);
          }
          else
          {
               anim.SetBool("Move", false);
          }

          // 繼續遊戲的按鈕出現才檢查空白鍵的觸發，也就是死了才檢查
          if (replayButton.activeSelf == true)
          {
               // 檢查空白鍵是否被按下
               if (Input.GetKeyDown(KeyCode.Space))
               {
                    // 手動調用按鈕的 onClick 事件
                    replayButton.GetComponent<Button>().onClick.Invoke();
                    // 功能等同於直接呼叫 Replay()
               }
          }          
     }

     private void OnCollisionEnter2D(Collision2D collision)
     {
          if (collision.gameObject.tag == "Normal")
          {
               if (collision.contacts[0].normal == Vector2.up)
               {
                    //Debug.Log("hit Normal");

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
                         //Debug.Log("floorsJumped: " + floorsJumped);
                         UpdateScore(floorsJumped);
                    }

                    ModifyHp(heal);
                    collision.gameObject.GetComponent<AudioSource>().Play();
               }
               else if (collision.contacts[0].normal == Vector2.left || collision.contacts[0].normal == Vector2.right)
               {
                    collision.gameObject.GetComponent<Collider2D>().isTrigger = true; // 讓 Normal 的碰撞器變成觸發器，取消左右地板的碰撞
               }
          }
          else if (collision.gameObject.tag == "Nails")
          {
               if (collision.contacts[0].normal == Vector2.up)
               {
                    //Debug.Log("hit Nails");

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
                         //Debug.Log("floorsJumped: " + floorsJumped);
                         UpdateScore(floorsJumped);
                    }

                    ModifyHp(damage);

                    anim.SetTrigger("Hurt");
                    collision.gameObject.GetComponent<AudioSource>().Play();
               }
               else if (collision.contacts[0].normal == Vector2.left || collision.contacts[0].normal == Vector2.right)
               {
                    collision.gameObject.GetComponent<Collider2D>().isTrigger = true; // 讓 Normal 的碰撞器變成觸發器，取消左右地板的碰撞
               }
          }
          else if (collision.gameObject.tag == "Ceiling")
          {
               //Debug.Log("hit Ceiling");

               currentFloor.GetComponent<BoxCollider2D>().enabled = false;
               ModifyHp(damage);

               anim.SetTrigger("Hurt");
               collision.gameObject.GetComponent<AudioSource>().Play();
          }
     }

     private void OnTriggerEnter2D(Collider2D collision)
     {
          if (collision.gameObject.tag == "DeathLine")
          {
               Die();
          }
     }

     void ModifyHp(int num)
     {
          hp += num;
          if (hp <= 0)
          {
               hp = 0;
               Die();
          }
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
          scoreText.text = "地下 " + score + " 層";
          //用score.ToString()結果一樣，C#會自動轉字串
     }

     void Die()
     {
          deathSound.Play();
          Time.timeScale = 0; //暫停遊戲
          replayButton.SetActive(true);          
     }

     public void Replay()
     {
          Time.timeScale = 1; //恢復遊戲
          SceneManager.LoadScene(SceneManager.GetActiveScene().name);
          //Debug.Log(SceneManager.GetActiveScene().name);
          //SceneManager.GetActiveScene().name == 當前場景 == "SampleScene"
     }
}
