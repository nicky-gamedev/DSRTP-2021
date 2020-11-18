using System.Collections;
using UnityEngine;

public class LightBlink : MonoBehaviour
{
    public float interval;
    public MeshRenderer lightMaterial;

    public void Setup()
    {
        StopCoroutine("Blink");
        StartCoroutine("Blink");
    }

    IEnumerator Blink()
    {
        lightMaterial.material.color = Color.white;
        yield return new WaitForSeconds(interval);
        lightMaterial.material.color = Color.green;
        yield return new WaitForSeconds(interval);
        StartCoroutine("Blink");
    }
}
