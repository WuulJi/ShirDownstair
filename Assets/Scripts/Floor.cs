using UnityEngine;

public class Floor : MonoBehaviour
{
     [SerializeField] float moveSpeed = 2f;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, moveSpeed * Time.deltaTime, 0);
        if (transform.position.y > transform.parent.GetComponent<FloorManager>().GetClearline())
         {
               Destroy(gameObject);
               transform.parent.GetComponent<FloorManager>().SpawnFloor();
         }        
     }
}
