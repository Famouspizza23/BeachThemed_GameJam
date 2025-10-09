using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class WaterBalloon : MonoBehaviour
{
    PlayerController pc;

    public float destroyTimer = 5f;
    public float balloonSpeed = 50f;

    private bool launched = false;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, destroyTimer);
    }

    public void Launch(Vector3 direction)
    {
        if (rb != null)
        {
            rb.velocity = direction.normalized * balloonSpeed;
            launched = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print("Hit Something" + collision.gameObject.name);

        pc = collision.gameObject.GetComponent<PlayerController>();

        if(pc != null)
        {
            print("Player Hit");
            pc.DamagePlayers();
            Destroy(this.gameObject);
            return;
        }

        Destroy(this.gameObject);
    }
}
