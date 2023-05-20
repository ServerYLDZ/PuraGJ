using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
   [SerializeField] Transform attackPoint;
   [SerializeField] GameObject prefab;
     float speed=10;

   public void Attack () {
    GameObject obj=Instantiate(prefab,attackPoint);
    Vector3 direction= -(obj.transform.position- Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
    obj.GetComponent<Rigidbody2D>().velocity=direction*speed;
   }
}
