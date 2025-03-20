using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public KeyCode keyUp = KeyCode.W;
    public KeyCode keyDown = KeyCode.S;
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyRight = KeyCode.D;

   Rigidbody rb;
   public float speed = 10;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(keyUp))
        {
            rb.linearVelocity += Vector3.up * speed;
        }

        if (Input.GetKey(keyDown))
        {
            rb.linearVelocity += Vector3.down * speed;
        }

        if (Input.GetKey(keyLeft))
        {
            rb.linearVelocity += Vector3.left * speed;
        }

        if (Input.GetKey(keyRight))
        {
            rb.linearVelocity += Vector3.right * speed;
        }
    }
}
