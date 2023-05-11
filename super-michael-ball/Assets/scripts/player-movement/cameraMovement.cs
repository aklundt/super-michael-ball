using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{

    public gameManager gameManager;

    GameObject cameraOBJ;

    // Start is called before the first frame update
    void Start()
    {
        cameraOBJ = gameManager.GetComponent<gameManager>().cameraOBJ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator resetCamera(float slerpSpeed) {
        float time = 0;
        while (time < 3)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.zero, slerpSpeed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(Vector3.zero), slerpSpeed);
            yield return null;
            time += Time.deltaTime;
        }
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        yield return null;
    }

    public IEnumerator moveCameraTo(GameObject destination, float slerpSpeed) {
        float time = 0;
        while (time < 3) {
            transform.position = Vector3.Slerp(transform.position, destination.transform.position, slerpSpeed);
            yield return null;
            time += Time.deltaTime;
        }
        transform.position = destination.transform.position;
        yield return null;
    }
    public IEnumerator rotateTo(GameObject destination, float slerpSpeed) { 
        Quaternion lookDirection = Quaternion.LookRotation(destination.transform.position - transform.position);
        float time = 0;
        while (time < 3) {
            lookDirection = Quaternion.Euler(Quaternion.LookRotation(destination.transform.position - transform.position).eulerAngles + new Vector3(0, 0, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, slerpSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = lookDirection;
        yield return null;
    }

    public void makeSeeThrough(float opacity, float speed) { 
        
    }
}
