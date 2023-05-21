using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sound : MonoBehaviour
{
    public void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void gamestart()
    {
        SceneManager.LoadScene(1);
    }
}
