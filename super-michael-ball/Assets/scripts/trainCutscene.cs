using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class trainCutscene : MonoBehaviour
{

    public gameManager gameManager;
    public GameObject player;
    public GameObject cameraObj;
    public GameObject cutsceneCameraObj;
    public GameObject trainObj;
    public GameObject twinklePrefab;
    public GameObject whiteboxObj;
    private bool sceneOngoing;

    // Start is called before the first frame update
    void Start()
    {
        sceneOngoing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneOngoing) {
            cutsceneCameraObj.transform.LookAt(player.transform, Vector3.up);
            if (player.transform.position.x > -174) { player.transform.position = new Vector3(-174, player.transform.position.y, player.transform.position.z); }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.movementEnabled = false;
        cutsceneCameraObj.SetActive(true);
        cameraObj.SetActive(false);
        sceneOngoing = true;
        StartCoroutine(trainMoving());
        StartCoroutine(endCutscene());
    }

    private IEnumerator trainMoving() {
        while (sceneOngoing) {
            trainObj.transform.Translate(0, 0, 50 * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
    private IEnumerator endCutscene() {
        bool fadingOut = false;
        MeshRenderer fadingWhiteMaterial = whiteboxObj.GetComponent<MeshRenderer>();
        while (sceneOngoing)
        {
            yield return new WaitForSeconds(2);
            Instantiate(twinklePrefab, player.transform.position, Quaternion.Euler(-120, 0, 0)); 
            player.SetActive(false);
            sceneOngoing = false;
            yield return new WaitForSeconds(2);
            new WaitForSeconds(1);
            fadingOut = true;
        }
        while (fadingOut) {
            fadingWhiteMaterial.material.color = new Color(1f, 1f, 1f, fadingWhiteMaterial.material.color.a + (1f * Time.deltaTime));
            if (fadingWhiteMaterial.material.color.a >= 1) { fadingOut = false; }
            new WaitForSeconds(1);
            // Scene transition?
            yield return null;
        }
    }
}
