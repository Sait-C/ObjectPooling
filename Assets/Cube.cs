using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IPooledObject
{
    public float upForce = .1f;
    public float sideForce = .1f;

    // Start is called before the first frame update
    public void OnObjectSpawn() //it add force to object
    {
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(-upForce, upForce);
        float zForce = Random.Range(-sideForce, sideForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().velocity = force;
    }
}
