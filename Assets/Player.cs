using UnityEngine;

public class Player : MonoBehaviour
{
     [SerializeField] float moveSpeed = 5f;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
          Debug.Log(123);
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
}
