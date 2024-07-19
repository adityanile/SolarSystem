using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHoleManager : MonoBehaviour
{
    public List<GameObject> celectialObjects = new List<GameObject>();
    SolarSystem solarSystem;

    Rigidbody2D rb;
    public float scaleDown = 0.1f;

    public float offset = 4;
    public float time = 3f;

    public bool start = true;

    // Start is called before the first frame update
    void Start()
    {
        solarSystem = GameObject.Find("SolarSystem").GetComponent<SolarSystem>();
        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < solarSystem.transform.childCount; i++)
        {
            GameObject p = solarSystem.transform.GetChild(i).gameObject;
            celectialObjects.Add(p.gameObject);
        }
       InitialVelocity();
        StartCoroutine(scaleFactor());
    }

    private void FixedUpdate()
    {
        PullForce();

        if (start)
        {
            if (celectialObjects.Count == 0)
            {
                start = false;
            }
        }
    }

    IEnumerator scaleFactor()
    {
        while (start) {
            yield return new WaitForSeconds(time);
            scaleDown += 0.2f;
        }
    }

    void PullForce()
    {
        float m1 = rb.mass * scaleDown;

        foreach (var p in celectialObjects)
        {
            p.GetComponent<PlanetManager>().enabled = false;

            Rigidbody2D rb2 = p.GetComponent<Rigidbody2D>();
            float m2 = rb2.mass;

            float r = Vector2.Distance(p.transform.position, transform.position);

            Debug.Log(r);

            if (r > offset)
            {
                Vector2 dir = (transform.position - rb2.transform.position).normalized;
                float force = (solarSystem.G * m1 * m2) / Mathf.Pow(r, 2);
                rb2.AddForce(dir *force);
            }
            else
            {
                rb2.velocity = Vector2.zero;
                celectialObjects.Remove(p.gameObject);
                Destroy(p.gameObject);
            }
        }

    }

    void InitialVelocity()
    {
        float m1 = rb.mass;

        foreach (var p in celectialObjects)
        {

            Rigidbody2D rb2 = p.GetComponent<Rigidbody2D>();
            float m2 = rb2.mass;

            float r = Vector2.Distance(p.transform.position, transform.position);
            
            Vector2 vdir = CalculatePerpendicular(p.transform.position);

            rb2.velocity += vdir * Mathf.Sqrt((solarSystem.G * m1) / r);
        }

    }

    Vector3 CalculatePerpendicular(Vector3 sun)
    {
        Vector3 dir = (sun - transform.position);
        float radius = dir.magnitude;

        float x = (-dir.y) / (dir.magnitude);
        float y = (dir.x) / (dir.magnitude);

        return new Vector3(x, y, dir.z).normalized;
    }

}
