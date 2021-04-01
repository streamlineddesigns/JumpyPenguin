using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (ZPlayerPrefs.GetInt("InGameFTUE") == 1) {
            SceneManager.LoadScene("Game");
        } else {
            SceneManager.LoadScene("FTUE");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
