using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defaultCheckpoint : MonoBehaviour
{
    public gameManager gameManager;
    public GameObject location;
    bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && gameManager.checkpointsEnabled) {
            triggered = true;
            gameManager.resetPosition = location.transform.position;
            gameManager.resetRotationY = location.transform.rotation.eulerAngles.y;
        }
    }
}
