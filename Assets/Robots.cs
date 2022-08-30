using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robots : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    public int speed = 8;
    bool goForBox;
    Box box;
    Box finalBox;
    Stand finalStand;
    Transform finalBoxPosition;
    Vector3 direction;
    bool moveToStand;
    bool depositBox;
    bool aroundBox;
    bool aroundBoxRight;
    bool aroundBoxLeft;
    Vector3 newLocation;
    bool foundBox;
    bool once;
    bool once2;
    // Start is called before the first frame update
    void Start()
    {
        once = false;
        once2 = false;
        speed = UnityEngine.Random.Range(2,7);
        aroundBox = false;
        moveToStand = false;
        depositBox = false;
        box = new Box();
        finalBox = new Box();
        goForBox = false;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (moveToStand == true && once == false)
        {
            transform.localRotation *= Quaternion.Euler(0, 180, 0);
            once = true;
            once2 = false;
        }
        else if(moveToStand == false && once2 == false){
            transform.localRotation *= Quaternion.Euler(0, 180, 0);
            once = false;
            once2 = true;
        }




        if (foundBox == false) {
            if (transform.position.z <=  7f) {
                direction = (new Vector3(transform.position.x, transform.position.y, 8f) - transform.position).normalized;
                rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            }
        }

        if (finalBox.grabed == true && goForBox == true)
        {
            if (finalBox != null)
            {
                direction = (finalBox.transform.position - transform.position).normalized;
                rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            }
        }
        else if (depositBox == false)
        {
            FindObject();
        }
        else {
            FindStand();
        }

        if (aroundBox == true) {
            if (aroundBoxLeft == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, newLocation, speed / 2 * Time.deltaTime);
            }
            else {
                transform.position = Vector3.MoveTowards(transform.position, newLocation, speed / 2 * Time.deltaTime);              
            }
            if (transform.position == newLocation)
            {
                aroundBox = false;
                aroundBoxRight = false;
                aroundBoxLeft = false;
            }
        }

        if (moveToStand == true && aroundBox == false) {
            direction = (finalStand.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        }

    }

  

    void FindStand() {
        GameObject[] stands = GameObject.FindGameObjectsWithTag("Stand");
        float min = Mathf.Infinity;
        for (int i = 0; i < stands.Length; i++)
        {
            float distance = (stands[i].transform.position - transform.position).sqrMagnitude;
            if (distance < min && stands[i].GetComponent<Stand>().amountOfBoxes < 5)
            {
                finalStand = stands[i].GetComponent<Stand>();
                min = distance;
            }
        }
        moveToStand = true;
    }

    void FindObject() {
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        float min = Mathf.Infinity;
        for (int i = 0; i < boxes.Length; i++) {
            float distance = (boxes[i].transform.position - transform.position).sqrMagnitude;
            if (distance < min && boxes[i].GetComponent<Box>().grabed == false)
            {
                box.grabed = false;
                finalBox = boxes[i].GetComponent<Box>();
                finalBoxPosition = boxes[i].transform;
                finalBox.grabed = true;
                box = finalBox;
                min = distance;
                foundBox = true;
            }
        }
        goForBox = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        float randomDistanceX = 0.1f;
        randomDistanceX = UnityEngine.Random.Range(0.1f, .5f);

        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "Robot")
        {
            aroundBox = true;
            if (collision.gameObject.transform.position.x - transform.position.x > 0)
            {
                newLocation = transform.position - new Vector3(randomDistanceX, 0f, 0f);
                aroundBoxLeft = true;
            }
            else {
                randomDistanceX = UnityEngine.Random.Range(0.1f, .5f);
                newLocation = transform.position + new Vector3(randomDistanceX, 0f, 0f);
                aroundBoxRight = true;
            }

        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        if(finalBox != null) { 
            if (collision.gameObject.tag == "Box" && collision.gameObject.transform.position == finalBox.transform.position)
            {
                goForBox = false;
                finalBox = new Box();
                depositBox = true;
                Destroy(collision.gameObject);
            }
        }
        if(finalStand != null) { 
            if (collision.gameObject.tag == "Stand" && finalStand.transform.position == collision.gameObject.transform.position) {
                finalStand.amountOfBoxes += 1;
                finalStand = new Stand();
                depositBox = false;
                moveToStand = false;
                foundBox = false;
            }
        }
    }
}
