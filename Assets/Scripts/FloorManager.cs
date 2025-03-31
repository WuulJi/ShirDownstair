using UnityEngine;

public class FloorManager : MonoBehaviour 
{
     [SerializeField] GameObject[] floorPrefabs;
     [SerializeField] Transform[] wallPosition;
     [SerializeField] Transform deathlinePos;

     public void SpawnFloor()
     {
          int r = Random.Range(0, floorPrefabs.Length);
          float leftWallX = wallPosition[0].transform.position.x;
          float RightWallX = wallPosition[1].transform.position.x;
          float deathlineY = deathlinePos.transform.position.y;

          GameObject floor = Instantiate(floorPrefabs[r], transform);
          floor.transform.position = new Vector3(Random.Range(leftWallX, RightWallX), deathlineY, 0f);
     }
}
