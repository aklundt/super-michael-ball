using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class dialogue : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraHolder;
    public GameObject gameManager;
    public int dialogueCurrentLine;
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
            player.GetComponent<Rigidbody>().drag = 300;
            returnLine();
        }
    }

    private string returnLine()
    {
        using (FileStream fs = File.OpenRead(@"Assets\scripts\dialogue\dialogueLines.txt"))
        {
            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);
            int readLen;
            while ((readLen = fs.Read(b, 0, b.Length)) > 0)
            {
                Debug.Log(temp.GetString(b, 0, readLen));
                return temp.GetString(b, 0, readLen);
            }
        }
        return null;
    }
}
