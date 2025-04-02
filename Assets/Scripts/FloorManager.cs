using UnityEngine;

public class FloorManager : MonoBehaviour 
{
     [SerializeField] GameObject[] floorPrefabs;
     [SerializeField] Transform[] wallPosition;
     [SerializeField] Transform deathlinePos;
     [SerializeField] Transform ceilingPos;

     public void SpawnFloor()
     {
          int r = Random.Range(0, floorPrefabs.Length);
          float leftWallX = wallPosition[0].position.x;
          float RightWallX = wallPosition[1].position.x;
          float deathlineY = deathlinePos.position.y;

          GameObject floor = Instantiate(floorPrefabs[r], transform);
          floor.transform.position = new Vector3(Random.Range(leftWallX, RightWallX), deathlineY, 0f);
     }

     public float GetClearline()
     {
          return ceilingPos.position.y+2f;
     }
}
