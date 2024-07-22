using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Slider s1;
    public Slider s2;

    [SerializeField]
    PlanetManager currPlanet;

    public float scaleOffset = 1;

    public float velOffset = 1;

    public void Init(GameObject obj)
    {
        Time.timeScale = 0;

        currPlanet = obj.GetComponent<PlanetManager>();

        s1.value = currPlanet.s1Scale;
        s2.value = currPlanet.s2Scale;

        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnClickDone()
    {
        currPlanet.transform.localScale = new Vector3((s2.value + 1), (s2.value + 1), (s2.value + 1));
        currPlanet.GetComponent<Rigidbody2D>().mass = currPlanet.origMass * (scaleOffset + s2.value);

        if (s1.value > 0.6f)
        {
            currPlanet.GetComponent<Rigidbody2D>().velocity = currPlanet.transform.right * velOffset;
        }
        else
        {
            currPlanet.enabled = false;

            Vector3 dir = (currPlanet.transform.position - currPlanet.sun.transform.position).normalized;
            Vector3 pos = new Vector3(dir.x +  velOffset + s2.value, dir.y +velOffset+ s2.value, dir.z);
            currPlanet.gameObject.transform.localPosition += pos;
            currPlanet.CalculateRadius();

            currPlanet.enabled = true;
        }

        currPlanet.s1Scale = s1.value;
        currPlanet.s2Scale = s2.value;

        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
