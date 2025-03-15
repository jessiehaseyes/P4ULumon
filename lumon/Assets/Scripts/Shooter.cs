using System;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Rigidbody prefabCup;
    public Vector3 speed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody newCup = Instantiate<Rigidbody>(prefabCup);

            newCup.transform.position = new Vector3(
                transform.position.x,
                transform.position.y ,
                transform.position.z);
            speed = new Vector3(0, 0, -30);
            newCup.linearVelocity = speed;


        }

    }
    void OnTriggerEnter(Collider other)
    {
        Destroy(other);
    }
}
    
