using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveblePlatform : MonoBehaviour
{
    public bool canMove;
    public Transform target;
    private void Start() {
         if(canMove)
        transform.DOMoveX(target.position.x,5,false).SetLoops(-1,LoopType.Yoyo);
    }
}
