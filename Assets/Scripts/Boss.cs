using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
  public int Healt =3;
  public int stage=0;
  public GameObject birdSpawner;
  public GameObject [] clouds;
  public Transform [] spawnPointVelet;
  public float veletSpawnTime=3;

  
  int veletCont=0;
 public int activeCloudIndex=0;
 public int activeCloudIndex1=0;

private void Update() {
    if(Input.GetKeyDown(KeyCode.L)){
        GetDamage();
    }
}
private void Start() {
    StageAttack(stage);
}
    public void StageAttack (int stage) {
        switch(stage){
            case 0:
        
           StartCoroutine(SpawnVelet());
            break;
           case 1:
           birdSpawner.SetActive(true);
            break;
            case 2:
            birdSpawner.SetActive(false);
             foreach(var obj in clouds) {
                obj.SetActive(true);
                obj.GetComponent<Cloud>().canMove=false;
                obj.GetComponent<Cloud>().canRain=false;

            }
            
              StartCoroutine(waitCloud());
            break;
           
            
        }
    }
IEnumerator CloudWorkCnahnge(){
       
 

     yield return new WaitForSeconds(veletSpawnTime);

     activeCloudIndex=Random.Range(0,clouds.Length);
      activeCloudIndex1=Random.Range(0,clouds.Length);
      while(activeCloudIndex==activeCloudIndex1)
        activeCloudIndex1=Random.Range(0,clouds.Length);
        clouds[activeCloudIndex].GetComponent<Cloud>().canRain=true;
        clouds[activeCloudIndex1].GetComponent<Cloud>().canRain=true;

      
      
     StartCoroutine(waitCloud());
}
IEnumerator waitCloud(){
    yield return new WaitForSeconds(2);
    foreach(var obj in clouds) {
              
                obj.GetComponent<Cloud>().canRain=false;

            }
      
     StartCoroutine(CloudWorkCnahnge());
}

IEnumerator SpawnVelet(){
    yield return new WaitForSeconds(veletSpawnTime);
    GameObject velet=PoolManager.instance.Spawn("Velet",true);
    int rand=Random.Range(0,spawnPointVelet.Length);
    velet.transform.position=spawnPointVelet[rand].position;
    veletCont++;
    if(veletCont<5)
    StartCoroutine(SpawnVelet());
  
}
  public void GetDamage () {
   
    if(Healt>0){
         Healt--;
         stage++;
         StopAllCoroutines();
         StageAttack(stage);
    }   
    else{
          foreach(var obj in clouds) {
                obj.SetActive(false);
            }

    }
  }
}
