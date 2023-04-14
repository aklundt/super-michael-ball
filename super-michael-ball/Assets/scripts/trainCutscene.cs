using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainCutscene : MonoBehaviour
{

    public gameManager gameManager;
    public GameObject player;
    public GameObject cameraObj;
    public GameObject cutsceneCameraObj;
    public GameObject trainObj;
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
    }

    private IEnumerator trainMoving() {
        while (sceneOngoing) {
            trainObj.transform.Translate(0, 0, 50 * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
}
