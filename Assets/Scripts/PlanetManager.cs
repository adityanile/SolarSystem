using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    private static float G = 6.67f;

    private GameObject sun;

    private Rigidbody2D rb;
    public float force;

    private float radius = 0;
    private float m1;
    private float m2;

    // Start is called before the first frame update
    void Start()
    {
        // First find around which planet this palnet will rotate
        sun = GameObject.FindGameObjectWithTag("Sun");
        rb = gameObject.GetComponent<Rigidbody2D>();

        Vector3 dir = (sun.transform.position - transform.position);
        radius = dir.magnitude;

        m1 = rb.mass;
        m2 = sun.GetComponent<Rigidbody2D>().mass;
    }

    // Update is called once per frame
    void Update()
    { 
        force = (G * m1 * m2) / (radius * radius);
        transform.RotateAround(sun.transform.position, sun.transform.forward, force * Time.deltaTime);
    }

    Vector3 CalculateVelDir()
    {
        Vector3 dir = (sun.transform.position - transform.position);
        radius = dir.magnitude;

        float x = (-dir.y) / (dir.magnitude);
        float y = (dir.x) / (dir.magnitude);

        return new Vector3(-x, -y, dir.z).normalized;
    }
}
