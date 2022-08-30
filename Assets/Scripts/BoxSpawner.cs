using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{

    [SerializeField]
    public int k = 5; // Cantidad de cajas
    [Range(1,12)]
    public int n = 12; //Row
    [Range(1, 14)] 
    public int m = 14; //Column
    [SerializeField] 
    public GameObject prefabBox;
    [SerializeField]
    public GameObject prefabRobot;
    float m1, m2, n1, n2;
    int optionX, optionY;
    string pos;
    // Start is called before the first frame update
    void Start()
    {
        //Posiciones donde ya esta una caja
        List<string> places = new List<string>();
        //Posiciones donde ya esta un robot
        List<string> placesRobots = new List<string>();
        
        //Instanciar 5 robot en lugares random en la parte de abajo
        for (int i = 0; i < 5; i++) {
            
            m1 = 0.5f - 14 / 2;
            m2 = 0.5f + 14 / 2;
            optionY = Random.Range(-7, -9);
            optionX = (int)Random.Range(m1, m2);

            pos = optionX + "" + optionY;
            if (placesRobots.Contains(pos) == true)
            {
                i--;
            }
            else
            {
                //Si no existe la posicion se agrega al arreglo de lugares
                placesRobots.Add(pos);
                //Se instancia la caja
                Instantiate(prefabRobot, new Vector3(optionX, 0.1f, optionY), Quaternion.identity);

            }
        }

        //Por la cantidad de cajas
        for (int i = 0; i < k; i++) {
            //Dividir m y n para escoger una x random y una y random
            m1 = 0.5f - m / 2;
            m2 = 0.5f + m / 2;
            n1 = 0.5f - n / 2;
            n2 = 0.5f + n / 2;

            //Escoger random
            optionX = (int)Random.Range(m1, m2);
            optionY = (int)Random.Range(n1, n2);
            pos = optionX + "" + optionY;

            //Si el lugar existe entonces no cuenta esta iteracion
            if (places.Contains(pos) == true)
            {
                i--;
            }
            else {
                //Si no existe la posicion se agrega al arreglo de lugares
                places.Add(pos);
                //Se instancia la caja
                Instantiate(prefabBox, new Vector3(optionX, 0.1f, optionY), Quaternion.identity);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
