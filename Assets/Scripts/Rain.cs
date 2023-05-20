using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public int damage=1;
   public float speed=10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector3(transform.localPosition.x,transform.localPosition.y-speed*Time.deltaTime,0);
    }
     private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
           GameManager.instance.player.GetComponent<PlayerMovement>().ChangeHealt(-damage);   
          PoolManager.instance.Despawn(gameObject);
    }
}