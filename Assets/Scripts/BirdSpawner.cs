using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
 
 public float spawnTime=.5f;
 public GameObject birdPrefab;
 public Collider2D target;
public LayerMask mask;
public float spawnRange=15;

public bool canSpawn=true;
 private void Update() {
  target= Physics2D.OverlapBox(transform.position,new Vector2(spawnRange,spawnRange),90,mask);
    if(target){
        if(canSpawn){
           float x= Random.Range(transform.position.x-spawnRange/2,+transform.position.x+spawnRange/2);
            float y= Random.Range(transform.position.y+ (spawnRange/4),transform.position.y+10+ (spawnRange / 4));
            GameObject obj=PoolManager.instance.Spawn("Bird",new Vector3(x,y,0),Quaternion.identity,true);
           // GameObject obj=Instantiate(birdPrefab,new Vector3(x,y,0),Quaternion.identity);
            canSpawn=false;
            StartCoroutine(ResetSpawn());
        }
        
    }
    else{
        StopAllCoroutines();
        canSpawn=true;
    }
 }
 private void OnDrawGizmos() {
    Gizmos.color=Color.cyan;
    Gizmos.DrawWireCube(transform.position,new Vector2(spawnRange,spawnRange/2));
 }
 IEnumerator ResetSpawn(){
    yield return new WaitForSeconds(spawnTime);
    canSpawn=true;

 }
}
