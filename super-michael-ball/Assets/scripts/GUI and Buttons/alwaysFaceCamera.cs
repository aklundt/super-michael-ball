using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alwaysFaceCamera : MonoBehaviour
{
    void Update()
    {
        try
        { 
            transform.LookAt(Camera.main.transform);
        } catch { }
    }
}
