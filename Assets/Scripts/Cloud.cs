using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cloud : MonoBehaviour
{
    public Transform[] points;
    public float spawnTime = .5f;
    public GameObject rainPrefab;
    public Transform target;
    public bool canMove = true;
    public bool boss=false;
    public bool canRain = true;

    void Start()
    {
        if (canMove)
            transform.DOMoveX(target.position.x, 5, false).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnEnable()
    {
        StartCoroutine(LifeCircle());
    }
    IEnumerator LifeCircle()
    {
        yield return new WaitForSeconds(spawnTime);
        if (canRain)
        {
            int rand = Random.Range(0, points.Length - 1);
            GameObject obj = PoolManager.instance.Spawn("Rain", points[rand].position, Quaternion.identity, true);
            if(boss)
                obj.GetComponent<Rain>().boosFight = true;
            PoolManager.instance.Despawn(obj, .5f);
        }
        StartCoroutine(LifeCircle());
    }
}
