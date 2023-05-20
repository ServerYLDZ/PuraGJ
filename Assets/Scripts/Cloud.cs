using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cloud : MonoBehaviour
{
    public Transform [] points;
     public float spawnTime=.5f;
      public GameObject rainPrefab;
     public Transform target;
public bool canSpawn=true;
public bool canMove=true;
public bool isBossScene=false;
public bool canRain=true;

    void Start()
    {
        if(canMove)
        transform.DOMoveX(target.position.x,5,false).SetLoops(-1,LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
         if(canSpawn ){
            if( canRain){
                int rand=Random.Range(0,points.Length-1);
             GameObject obj=PoolManager.instance.Spawn("Rain",true);
             obj.transform.position=points[rand].position;
             if(isBossScene){
                obj.GetComponent<Rain>().boosFight=true;   
                PoolManager.instance.Despawn(obj,2);
             }
            
           // GameObject obj=Instantiate(rainPrefab,points[rand].position,Quaternion.identity);
            canSpawn=false;
            StartCoroutine(ResetSpawn());
            }
            
        }
        
    }

    IEnumerator ResetSpawn(){
    yield return new WaitForSeconds(spawnTime);
    canSpawn=true;

 }
}
