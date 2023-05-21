using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait : MonoBehaviour
{
 private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.CompareTag("Player")){
        Destroy(this.gameObject);
    }
 }
}
