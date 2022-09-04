using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField]
    GameObject grid;
    public TextMeshProUGUI text;
    int total;
    int totalBoxes;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        totalBoxes = grid.transform.GetComponent<BoxSpawner>().k;
    }

    // Update is called once per frame
    void Update()
    {
        total = 0;
        for (int i = 0; i < transform.childCount; i++) {
            total += transform.GetChild(i).GetComponent<Stand>().amountOfBoxes;
        }
        if (total != totalBoxes)
        {
            time += Time.deltaTime;
            text.text = "Tiempo total para organizar cajas: " + time.ToString("0.00");
        }
    }
}
