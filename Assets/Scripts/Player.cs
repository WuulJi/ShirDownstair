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
     [SerializeField] TextMeshProUGUI scoreText; // �ϥ� TextMeshPro �ե�
     int score = 0;
     GameObject lastFloor;
     Animator anim;
     SpriteRenderer sprR;
     AudioSource deathSound;
     [SerializeField] GameObject replayButton;
     int damage = -2; // ���ˮɪ��ˮ`��
     int heal = 1; // �I��a�O�ɪ��^���
     float gameAcc = 0.05f; // �[�ֹC���t��

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          hp = 10;
          score = 0;
          anim = GetComponent<Animator>();
          sprR = GetComponent<SpriteRenderer>();
          deathSound = GetComponent<AudioSource>();
          replayButton.SetActive(false);
          Time.timeScale = 1; // �T�O�C���}�l�ɮɶ����`�y�u
     }

     // Update is called once per frame
     void Update()
     {
          if (Time.timeScale != 0) // �Ȱ��N���[�t
          {
               Time.timeScale += gameAcc * Time.deltaTime; //�[�ֹC���t��
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

          // �~��C�������s�X�{�~�ˬd�ť��䪺Ĳ�o�A�]�N�O���F�~�ˬd
          if (replayButton.activeSelf == true)
          {
               // �ˬd�ť���O�_�Q���U
               if (Input.GetKeyDown(KeyCode.Space))
               {
                    // ��ʽեΫ��s�� onClick �ƥ�
                    replayButton.GetComponent<Button>().onClick.Invoke();
                    // �\�൥�P�󪽱��I�s Replay()
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

                    if (currentFloor == null)// �p�G currentFloor ���šA��ܬO�Ĥ@���I��a�O�A��l�� currentFloor �M lastFloor
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
                    collision.gameObject.GetComponent<Collider2D>().isTrigger = true; // �� Normal ���I�����ܦ�Ĳ�o���A�������k�a�O���I��
               }
          }
          else if (collision.gameObject.tag == "Nails")
          {
               if (collision.contacts[0].normal == Vector2.up)
               {
                    //Debug.Log("hit Nails");

                    if (currentFloor == null) // �p�G currentFloor ���šA��ܬO�Ĥ@���I��a�O�A��l�� currentFloor �M lastFloor
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
                    collision.gameObject.GetComponent<Collider2D>().isTrigger = true; // �� Normal ���I�����ܦ�Ĳ�o���A�������k�a�O���I��
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
          scoreText.text = "�a�U " + score + " �h";
          //��score.ToString()���G�@�ˡAC#�|�۰���r��
     }

     void Die()
     {
          deathSound.Play();
          Time.timeScale = 0; //�Ȱ��C��
          replayButton.SetActive(true);          
     }

     public void Replay()
     {
          Time.timeScale = 1; //��_�C��
          SceneManager.LoadScene(SceneManager.GetActiveScene().name);
          //Debug.Log(SceneManager.GetActiveScene().name);
          //SceneManager.GetActiveScene().name == ��e���� == "SampleScene"
     }
}
