using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class deathPlane : MonoBehaviour
{
    public GameObject gameManager;
    public int deaths;
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
        deaths++;
        StartCoroutine(gameManager.GetComponent<gameManager>().transitionStatic(true, true, true));
    }
}
