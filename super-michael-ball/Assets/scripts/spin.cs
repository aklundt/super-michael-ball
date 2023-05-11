using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    public bool active;
    public Vector3 spinAxis;
    public float spinSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.Rotate(Vector3.Normalize(spinAxis), spinSpeed * Time.deltaTime);
        }
    }
}
