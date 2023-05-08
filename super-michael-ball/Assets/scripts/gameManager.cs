using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraOBJ;

    public float primaryHorizontalInput;
    public float primaryVerticalInput;
    public float secondaryHorizontalInput;
    public float secondaryVerticalInput;

    public bool movementEnabled;
    public bool NPCTalking;
    public int frameRate;

    // Start is called before the first frame update
    void Start()
    {
        NPCTalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        Application.targetFrameRate = frameRate;
    }

    void checkInput()
    {
        primaryHorizontalInput = Input.GetAxis("Horizontal");
        primaryVerticalInput = Input.GetAxis("Vertical");
        secondaryHorizontalInput = Input.GetAxis("Secondary Horizontal");
        secondaryVerticalInput = Input.GetAxis("Secondary Vertical");
    }
}
