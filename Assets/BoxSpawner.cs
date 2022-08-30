using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{

    [SerializeField]
    public int k = 5;
    [Range(1,12)]
    public int n = 12;
    [Range(1, 14)] 
    public int m = 14;
    [SerializeField] 
    public GameObject prefabBox;
    // Start is called before the first frame update
    void Start()
    {
        List<string> places = new List<string>();

        for (int i = 0; i < k; i++) {
            //Instantiate(prefabBox, new Vector3(x_Start + x_Space * (i % Random.Range(1,m)),0.1f, y_Start + y_Space * (i / Random.Range(1, m))), Quaternion.identity);
            float m1 = 0.5f - m / 2;
            float m2 = 0.5f + m / 2;
            float n1 = 0.5f - n / 2;
            float n2 = 0.5f + n / 2;

            int optionX = (int)Random.Range(m1, m2);
            int optionY = (int)Random.Range(n1, n2);
            string pos = optionX + "" + optionY;

            if (places.Contains(pos) == true)
            {
                i--;
            }
            else {
                places.Add(pos);
                Instantiate(prefabBox, new Vector3(optionX, 0.1f, optionY), Quaternion.identity);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
