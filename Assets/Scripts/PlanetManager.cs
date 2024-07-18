using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public GameObject sun;
    public bool areuSun = false;
    public bool areuPlanet = false;

    private SolarSystem SolarSystem;

    private Rigidbody2D rb;

    public float forceDueToStar;
    private float radius = 0;
    private float m1;
    private float m2;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        SolarSystem = GameObject.Find("SolarSystem").GetComponent<SolarSystem>();

        if (areuPlanet)
        {
            // First find around which planet this palnet will rotate
            // If sun is not present there then the first planet to enter will be the sun
            sun = SolarSystem.sun;

            if (!sun)
            {
                SolarSystem.sun = gameObject;
                sun = gameObject;
                areuSun = true;

                return;
            }

            SolarSystem.planets.Add(gameObject);
            CalculateRadius();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!areuSun)
        {
            m1 = rb.mass;
            m2 = sun.GetComponent<Rigidbody2D>().mass;

            if (areuPlanet)
            {
                // If the planet has mass more than the sun then all planets will revolve around new sun
                if (m1 > m2)
                {
                    SolarSystem.ChangeSun(gameObject);
                }
                else
                {
                    forceDueToStar = (SolarSystem.G * m1 * m2) / Mathf.Pow(radius, 2);
                    transform.RotateAround(sun.transform.position, sun.transform.forward, forceDueToStar * Time.deltaTime);
                }
            }
            else
            {
                forceDueToStar = (SolarSystem.G * m1 * m2) / Mathf.Pow(radius, 2);
                transform.RotateAround(sun.transform.position, sun.transform.forward, forceDueToStar * Time.deltaTime);
            }
        }
    }

    public void CalculateRadius(float compression = 0)
    {
        Vector3 dir = (sun.transform.position - transform.position);
        radius = dir.magnitude - compression;
    }
}
