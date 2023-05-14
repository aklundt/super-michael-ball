using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makiBody : MonoBehaviour
{
    public gameManager gameManager;
    public Material makiTexture;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nextFrameStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator nextFrameStart() {
        yield return new WaitForEndOfFrame();
        if (gameManager.gameStatus > 1)
        {
            transform.Find("backrightleg").gameObject.GetComponent<MeshRenderer>().material = makiTexture;
            if (gameManager.gameStatus > 2)
            {
                transform.Find("frontleftleg").GetComponent<MeshRenderer>().material = makiTexture;
                if (gameManager.gameStatus > 3)
                {
                    transform.Find("tail").GetComponent<MeshRenderer>().material = makiTexture;
                    if (gameManager.gameStatus > 4)
                    {
                        transform.Find("backleftleg").GetComponent<MeshRenderer>().material = makiTexture;
                        if (gameManager.gameStatus > 5)
                        {
                            transform.Find("frontrightleg").GetComponent<MeshRenderer>().material = makiTexture;
                            if (gameManager.gameStatus > 6)
                            {
                                transform.Find("body").GetComponent<MeshRenderer>().material = makiTexture;
                                if (gameManager.gameStatus > 7)
                                {
                                    transform.Find("collar").GetComponent<MeshRenderer>().material = makiTexture;
                                    if (gameManager.gameStatus > 8)
                                    {
                                        transform.Find("head").GetComponent<MeshRenderer>().material = makiTexture;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
