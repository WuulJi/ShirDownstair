using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
     [SerializeField] float moveSpeed = 5f;
     GameObject currentFloor;
     [SerializeField] int hp;
     [SerializeField] GameObject hpBar;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
          hp = 10;
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
                    currentFloor = collision.gameObject;
                    ModifyHp(1);
               }
          }
          else if(collision.gameObject.tag == "Nails")
          {
               if (collision.contacts[0].normal == Vector2.up)
               {
                    Debug.Log("hit Nails");
                    currentFloor = collision.gameObject;
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
}
