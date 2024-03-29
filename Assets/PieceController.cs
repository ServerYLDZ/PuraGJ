using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Velet")
        {
            Debug.Log("velettt");
            collision.GetComponent<Enemy>().GetHit();
            PoolManager.instance.Despawn(gameObject);
        }
        if(collision.tag == "Bird")
        {
            collision.GetComponent<BirdEnemy>().GetHit();
            PoolManager.instance.Despawn(gameObject);
        }
    }
}
