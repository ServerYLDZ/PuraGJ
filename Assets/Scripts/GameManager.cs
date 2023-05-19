using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
   private int gold;

    public int Gold
 {
     get { return gold; }
     set
     {
         if(value < 0)
             gold = 0;
         else
             gold = value;
     }
 }
}
