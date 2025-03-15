using UnityEngine;

public class WASDControl : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        //W key forward on Z
        if(Input.GetKey(KeyCode.W))
        {
            position += new Vector3(0, 0, moveSpeed);
        }
        transform.position = position;

        //S key backwards on Z
        if (Input.GetKey(KeyCode.S))
        {
            position += new Vector3(0, 0, moveSpeed * -1);
        }
        transform.position = position;

        //A key left on X
        if (Input.GetKey(KeyCode.A))
        {
            position += new Vector3(moveSpeed * -1, 0, 0);
        }
        transform.position = position;

        //D key left on X
        if (Input.GetKey(KeyCode.D))
        {
            position += new Vector3(moveSpeed, 0, 0);
        }
        transform.position = position;
    }
}
