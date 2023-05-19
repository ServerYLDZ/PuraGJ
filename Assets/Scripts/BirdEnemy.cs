using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEnemy : MonoBehaviour
{
   
   public int damage;
    public Vector3 target;
    public float speed=2;
     float distace;
     private Rigidbody2D rb;
    
    private void Start() {
          target=GameManager.instance.player.transform.position;
          rb=GetComponent<Rigidbody2D>();
          Vector2 direction =-(transform.position - GameManager.instance.player.transform.position).normalized;
          rb.velocity = -(transform.position - GameManager.instance.player.transform.position).normalized * speed;
          float angle=Mathf.Atan2(direction.y,direction.x) *Mathf.Rad2Deg;

          transform.rotation=Quaternion.AngleAxis(angle,Vector3.forward);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
           GameManager.instance.player.GetComponent<PlayerMovement>().ChangeHealt(-damage);   
    }
    
    private void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

}
