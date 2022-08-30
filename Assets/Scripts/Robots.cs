using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robots : MonoBehaviour
{
    Rigidbody rb;
    public int speed = 8;
    Box box, finalBox;
    Stand finalStand;
    Transform finalBoxPosition;
    Vector3 direction, newLocation;
    bool moveToStand, depositBox, aroundBox, aroundBoxRight, aroundBoxLeft, goForBox, foundBox, once, once2;
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation *= Quaternion.Euler(0, -90, 0);
        once = false;
        once2 = false;
        speed = UnityEngine.Random.Range(3,7);
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
        //Cuando se mueve hacia el estante o hacia la caja cambia su rotacion 
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



        //Si ya no ha encontrado cajas (Ya no hay cajas) entonces se va hasta arriba del cuarto para no estorbar
        if (foundBox == false) {
            if (transform.position.z <=  7f) {
                direction = (new Vector3(transform.position.x, transform.position.y, 8f) - transform.position).normalized;
                rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            }
        }

        //Si la caja esta en estatus de ser agarrada y el robot tiene que ir por la caja
        if (finalBox.grabed == true && goForBox == true)
        {
            if (finalBox != null)
            {
                //Ir hacia la caja a agarrar
                direction = (finalBox.transform.position - transform.position).normalized;
                rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            }
        }
        else if (depositBox == false)
        {
            //Si no esta dejando una caja y no esta llendo por una caja entonces encuentra otra caja
            FindObject();
        }
        else {
            //Si no es nada de lo anterior entonces busca un estante para poner la caja
            FindStand();
        }

        //Si choca contra algo entonces se mueve hacia la izquierda o hacia la derecha
        if (aroundBox == true) {
            transform.position = Vector3.MoveTowards(transform.position, newLocation, speed / 2 * Time.deltaTime);
            //Si ya se movio hasta la derecho o izquierda entonces regresa todos los valores a falso y regresa a su movimiento normal
            if (transform.position == newLocation)
            {
                aroundBox = false;
                aroundBoxRight = false;
                aroundBoxLeft = false;
            }
        }

        //Si el robot tiene que moverse hacia un estante entonces se dirige hacia ese estante
        if (moveToStand == true && aroundBox == false) {
            direction = (finalStand.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        }

    }

  
    //Encontrar el estante mas cercano
    void FindStand() {
        //Buscar por todos los estantes
        GameObject[] stands = GameObject.FindGameObjectsWithTag("Stand");
        float min = Mathf.Infinity;
        //Interar por cada estante
        for (int i = 0; i < stands.Length; i++)
        {
            //Sacar la distancia entre el robot y cada estante
            float distance = (stands[i].transform.position - transform.position).sqrMagnitude;
            //Si la distancia es menor que la distancia anterior y el estante tiene lugar
            if (distance < min && stands[i].GetComponent<Stand>().amountOfBoxes < 5)
            {
                //El estante a donde se debe de dirigir cambia
                finalStand = stands[i].GetComponent<Stand>();
                min = distance;
            }
        }
        moveToStand = true;
    }

    //Encontrar la caja mas cercana
    void FindObject() {
        //Encontrar todas las cajas
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        float min = Mathf.Infinity;
        //Iterar por todos las cajas
        for (int i = 0; i < boxes.Length; i++) {
            //Encontrar la distancia entre la caja y el robot
            float distance = (boxes[i].transform.position - transform.position).sqrMagnitude;
            //Si la caja todavia no es agarrada y la distancia es menor que la distancia anterior
            if (distance < min && boxes[i].GetComponent<Box>().grabed == false)
            {
                //Caja anterior ya no es agarrada
                box.grabed = false;
                //Caja final se guarda
                finalBox = boxes[i].GetComponent<Box>();
                //La posicion de la ultima caja se da a conocer
                finalBoxPosition = boxes[i].transform;
                //La caja a donde se va a ir el robot se pone como si un robot ya va por una
                finalBox.grabed = true;
                //La caja anterior ahora es la caja final
                box = finalBox;
                //La distancia cambia
                min = distance;
                //Da a conocer que el robot si encontro una caja
                foundBox = true;
            }
        }
        //Da a conocer que esta llendo por una caja
        goForBox = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        //Distancia random que el robot se mueve para evadir un objeto
        float randomDistanceX = 0.1f;
        randomDistanceX = UnityEngine.Random.Range(0.1f, .5f);

        //Si colisiona con una caja o un robot
        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "Robot")
        {
            //Da a conocer que va a rodear un objeto
            aroundBox = true;
            //Dependiendo si el robot esta a la izquierda o derecha del objeto es a donde se mueve.
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

    //Si entra en colision
    private void OnCollisionEnter(Collision collision)
    {
        //Si hay una caja
        if(finalBox != null) {
            //Si la colision fue con una caja y esta en la posicion de la caja hacia donde va el robot
            if (collision.gameObject.tag == "Box" && collision.gameObject.transform.position == finalBox.transform.position)
            {
                //Renueva la variable
                goForBox = false;
                finalBox = new Box();
                //Avisa que ahora tiene que ir a depositar la caja al estante
                depositBox = true;
                //Destruye la caja
                Destroy(collision.gameObject);
            }
        }
        //Si hay un estante
        if(finalStand != null) { 
            //Si colisiona con el estante correcto
            if (collision.gameObject.tag == "Stand" && finalStand.transform.position == collision.gameObject.transform.position) {
                //Agrega a la cantidad de cajas en el estante y renueva variables
                finalStand.amountOfBoxes += 1;
                //Haz que el estante ponga otra caja
                finalStand.UpdateStand();
                finalStand = new Stand();
                depositBox = false;
                moveToStand = false;
                foundBox = false;
            }
        }
    }
}
