using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public int damage = 1;
    public bool boosFight = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.player.GetComponent<PlayerMovement>().ChangeHealt(-damage);
        }
        if(!boosFight) 
            PoolManager.instance.Despawn(gameObject);
    }
}