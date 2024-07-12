using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public int index;
    public void Load()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(index);
    }
}
