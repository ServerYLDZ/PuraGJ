using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour
{

public GameObject sceneOpen;
private void OnTriggerEnter2D(Collider2D other) {
    sceneOpen.SetActive(true);

}

}
