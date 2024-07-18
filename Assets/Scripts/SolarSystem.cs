using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SolarSystem : MonoBehaviour
{
    public float G = 6.67f;
    public GameObject sun;

    public List<GameObject> planets = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void ChangeSun(GameObject newSun)
    {
        float dimFactor = newSun.GetComponent<Rigidbody2D>().mass/sun.GetComponent<Rigidbody2D>().mass;
        sun.GetComponent<PlanetManager>().areuSun = false;
        planets.Add(sun);

        planets.Remove(newSun);
        newSun.GetComponent<PlanetManager>().areuSun = true;

        sun = newSun;

        foreach (var p in planets)
        {
            var pm = p.GetComponent<PlanetManager>();
            pm.sun = sun;
            pm.CalculateRadius(dimFactor);
        }

    }
}
