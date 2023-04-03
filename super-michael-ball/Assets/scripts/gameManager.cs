using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public bool movementEnabled;
    public int frameRate;

    // Start is called before the first frame update
    void Start()
    {
        movementEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = frameRate;
    }
}
