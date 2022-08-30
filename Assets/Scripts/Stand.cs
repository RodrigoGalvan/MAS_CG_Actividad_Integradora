using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    //Cantida de cajas que tiene el estante
    public int amountOfBoxes = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStand() {
        //Ir desplegando las cajas conforme los robots dejan las cajas
        if (amountOfBoxes == 1)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        } else if (amountOfBoxes == 2)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        } else if (amountOfBoxes == 3)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        } else if (amountOfBoxes == 4)
        {
            transform.GetChild(3).gameObject.SetActive(true);
        } else if (amountOfBoxes == 5)
        {
            transform.GetChild(4).gameObject.SetActive(true);
        }
    }
}
