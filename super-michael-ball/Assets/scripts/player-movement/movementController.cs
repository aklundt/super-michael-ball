using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{

    float horizontalInput;
    float verticalInput;

    // start is called before the first frame update
    void Start()
    {
        
    }

    // update is called once per frame
    void Update()
    {
        checkInput();
    }

    // reads horizontal and vertical inputs and update values.
    void checkInput() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
}
