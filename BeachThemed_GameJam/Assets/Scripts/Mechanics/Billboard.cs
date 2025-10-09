using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(mainCameraTransform.position);

        //transform.Rotate(0, 180, 0);
    }
}
