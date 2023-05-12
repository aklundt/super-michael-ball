using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleScreenUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveToScene(int sceneNum) {
        if (PlayerPrefs.GetInt("gameState") == 0)
        {
            SceneManager.LoadScene("RealWorldHouse");
        }
        else {
            SceneManager.LoadScene("DreamLobby");
        }
        
    }
}
