using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaterBalloon : MonoBehaviour
{
    public float destroyTimer = 5f;

    void Awake()
    {
        Destroy(this.gameObject, destroyTimer);
    }

    void OnCollisionEnter(Collision collision)
    {
        print("Hit Something" + collision.gameObject.name);
        Destroy(this.gameObject);
    }
}
