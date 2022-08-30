using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    public GameObject redLight;
    public float waitTime = 1f;

    private void Start()
    {
        StartCoroutine(Siren());
    }

    IEnumerator Siren()
    {
        yield return new WaitForSeconds(waitTime);
        redLight.SetActive(false);

        yield return new WaitForSeconds(waitTime);
        redLight.SetActive(true);

        StartCoroutine(Siren());
    }
}
