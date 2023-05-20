using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneePass : MonoBehaviour
{   
  public Image image;
  [SerializeField] Sprite [] images;
 [SerializeField] private int imageIndex;
 
 private void Start() {
    image.sprite=images[imageIndex];
 }
  private void Update() {
    if(Input.GetMouseButtonDown(0)){
          imageIndex++;
        if(imageIndex<images.Length){
            image.sprite=images[imageIndex];
        }
        else{
            enabled=false;
            image.enabled=false;
        }
    }
  }
}
