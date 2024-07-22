using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public GameObject planet;
    public Transform SolarSystem;

    private Vector3 screenPoint;
    private Vector3 offset;

    public bool spawnAsMoon = false;
    private GameObject trig;

    float moonOffset = 3;

    private void Start()
    {
        SolarSystem = GameObject.Find("SolarSystem").transform;
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    private void OnMouseUp()
    {
        if (gameObject.CompareTag("BlackHole"))
        {
            Instantiate(planet, transform.position, planet.transform.rotation);
            Destroy(gameObject);
            return;
        }   

        if (spawnAsMoon)
        {
            float m1 = planet.GetComponent<Rigidbody2D>().mass;
            float m2 = trig.GetComponent<Rigidbody2D>().mass;
                Vector3 pos = new Vector3(moonOffset, moonOffset, 0);

            if (m1 < m2)
            {
                GameObject moon = Instantiate(planet, pos, planet.transform.rotation, trig.transform);
                moon.transform.localPosition = pos;

                PlanetManager pm = moon.GetComponent<PlanetManager>();
                pm.sun = trig;

                pm.CalculateRadius();
            }
            else
            {
                // Make other planet moon of this planet
                GameObject pl = Instantiate(planet, transform.position, planet.transform.rotation, SolarSystem);
                pl.GetComponent<PlanetManager>().areuPlanet = true;

                trig.transform.parent = pl.transform;
                trig.transform.localPosition = pos;
                
                PlanetManager pm = trig.GetComponent<PlanetManager>();
                pm.sun = pl;
                pm.areuPlanet = false;
                
                pm.CalculateRadius();
            }

            Destroy(gameObject);
                return;
        }

        GameObject plnt = Instantiate(planet, transform.position, planet.transform.rotation, SolarSystem);
        plnt.GetComponent<PlanetManager>().areuPlanet = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlanetManager>().areuPlanet)
        {
            spawnAsMoon = true;
            trig = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlanetManager>().areuPlanet)
        {
            spawnAsMoon = false;
            trig = null;
        }
    }
}
