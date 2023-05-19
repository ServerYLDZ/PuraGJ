using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEnemy : MonoBehaviour
{
   
    private Transform target;
    private void Awake() {
        target=GameManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
