using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int Healt = 2;
    public int stage = 0;
    public GameObject[] healtbars;
    public GameObject birdSpawner;
    public GameObject[] clouds;
    public Transform[] spawnPointVelet;
    public float veletSpawnTime = 3;
    public float rainTime = 3;

    public GameObject[] spawnObjects;
    public Transform[] spawnTransform;
    public int spawnCount=0;
    public GameObject cuurentObj;
    public GameObject WinScene;

    int veletCont = 0;

    public int cloudstage;
    public Vector2[] cloudIndexs;

    private void Update()
    {
        if (!cuurentObj)
        {
            healtbars[Healt].GetComponent<Image>().fillAmount = 1 - ((float)spawnCount* 0.33f);
            cuurentObj = Instantiate(spawnObjects[stage], spawnTransform[Random.Range(0, spawnTransform.Length)].position, Quaternion.identity);
            spawnCount++;
            if (spawnCount >= 4)
            {
                GetDamage();
                spawnCount = 0;
                stage++;
            }
        }
    }
    private void Start()
    {
        StageAttack(stage);
    }
    public void StageAttack(int stage)
    {
        switch (stage)
        {
            case 0:

                StartCoroutine(SpawnVelet());
                break;
            case 1:
                birdSpawner.SetActive(true);
                break;
            case 2:
                birdSpawner.SetActive(false);
                foreach (var obj in clouds)
                {
                    obj.SetActive(false);
                    obj.GetComponent<Cloud>().canMove = false;
                    obj.GetComponent<Cloud>().canRain = false;
                }
                StartCoroutine(waitCloud());
                break;
        }
    }
    IEnumerator CloudWorkCnahnge()
    {
        clouds[(int)cloudIndexs[cloudstage].x].GetComponent<Cloud>().canRain = true;
        clouds[(int)cloudIndexs[cloudstage].y].GetComponent<Cloud>().canRain = true;
        yield return new WaitForSeconds(rainTime);

        foreach (var obj in clouds)
        {
            obj.SetActive(false);
        }

        StartCoroutine(waitCloud());
    }
    IEnumerator waitCloud()
    {
        int last = cloudstage;

        while (last == cloudstage)
            cloudstage = Random.Range(0, 3);

        clouds[(int)cloudIndexs[cloudstage].x].SetActive(true);
        clouds[(int)cloudIndexs[cloudstage].y].SetActive(true);
        clouds[(int)cloudIndexs[cloudstage].x].GetComponent<Cloud>().canRain = false;
        clouds[(int)cloudIndexs[cloudstage].y].GetComponent<Cloud>().canRain = false;

        yield return new WaitForSeconds(.5f);

        StartCoroutine(CloudWorkCnahnge());
    }

    IEnumerator SpawnVelet()
    {
        yield return new WaitForSeconds(veletSpawnTime);

        int rand = Random.Range(0, spawnPointVelet.Length);
        GameObject velet = PoolManager.instance.Spawn("Velet", spawnPointVelet[veletCont].position, Quaternion.identity, true);

        veletCont++;
        if (veletCont < 5)
            StartCoroutine(SpawnVelet());

    }
    public void GetDamage()
    {

        if (Healt > 0)
        {
            StopAllCoroutines();
            if (stage < 3)
                StageAttack(stage); 
            Healt--;
        }
        else
        {
            foreach (var obj in clouds)
            {
                obj.SetActive(false);
            }
            Time.timeScale = 0;
            WinScene.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}
