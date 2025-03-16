using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public class DayNightCycle : Singleton<DayNightCycle>
{
    public Light cycleLight;
    public Color dayColor;
    public Color nightColor;
    [HorizontalLine]
    public GameObject sun;
    public GameObject moon;
    public GameObject stars;
    public bool isNight = false;
    [HorizontalLine]
    public float cycleSpeed = 0.01f; 
    public float colorSpeed = 0.03f;
    
    [ReadOnly][SerializeField] Material skyDome;

    void Start()
    {
        sun.SetActive(true);
        moon.SetActive(false);
        stars.SetActive(false);
        isNight = false;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) skyDome = renderer.material;
        else Debug.LogError("Renderer component not found.");

        StartCoroutine(Cycle());
    }

    IEnumerator Cycle()
    {
        isNight = false;
        float currentOffset = 0;
        skyDome.mainTextureOffset = new Vector2(currentOffset, 0);

        while(currentOffset < 0.5f)
        {
            cycleLight.color = Color.Lerp(cycleLight.color, nightColor, Time.deltaTime * colorSpeed);
            currentOffset += Time.deltaTime * cycleSpeed;
            skyDome.mainTextureOffset = new Vector2(currentOffset, 0);
            isNight = false;
            yield return null;
        }
        while(currentOffset < 1)
        {
            cycleLight.color = Color.Lerp(cycleLight.color, dayColor, Time.deltaTime * colorSpeed);
            currentOffset += Time.deltaTime * cycleSpeed;
            skyDome.mainTextureOffset = new Vector2(currentOffset, 0);
            isNight = true;
            yield return null;
        }

        yield return null;
        StartCoroutine(Cycle());
    }

}
