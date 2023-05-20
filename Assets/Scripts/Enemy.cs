using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float distace;
    public int damage=20;
    public float folowDis=6;
    public float attackDis=2;
    public float speed=5;
    public bool canAttack=true;
    public float attackTime=2;
    private bool isDead=false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        GetHit();
        distace=Vector2.Distance(transform.position,GameManager.instance.player.transform.position);
        Vector2 direction =GameManager.instance.player.transform.position-transform.position;
        direction.Normalize();
        if(direction.x>0)
        GetComponent<SpriteRenderer>().flipX=true;
        else if(direction.x<0)
          GetComponent<SpriteRenderer>().flipX=false;
        if(distace<attackDis){
            if(canAttack&&!isDead){
                Attack();
            }
        }
        else if(distace<folowDis && !isDead){
             Vector2 newPos=GameManager.instance.player.transform.position;
            newPos.y=transform.position.y;
            transform.position=Vector2.MoveTowards(transform.position,newPos,speed*Time.deltaTime);
        }
        
    }
    public void Attack () {
        Debug.Log("hiii");
        GameManager.instance.player.GetComponent<PlayerMovement>().ChangeHealt(-damage);
        canAttack=false;
        StartCoroutine(ResetAttack());
    }
    IEnumerator ResetAttack(){
        yield return new WaitForSeconds(attackTime);
        canAttack=true;
    }

    public void GetHit () {
        
            // animasyon girer
            isDead=true;
            GetComponent<Rigidbody2D>().gravityScale=0;
            GetComponent<Collider2D>().enabled=false;
            GetComponent<Animator>().SetTrigger("Dead");
            
        
    }
   
}
