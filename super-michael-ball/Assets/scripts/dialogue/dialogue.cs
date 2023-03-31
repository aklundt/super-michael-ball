using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogue : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraHolder;
    public GameObject gameManager;
    public int dialogueStartLine;
    public int dialogueEndLine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && (player.transform.position - transform.position).magnitude <= 6) {
            gameManager.GetComponent<gameManager>().movementEnabled = false;
        }
    }
}
