using UnityEngine;
using UnityEngine.Serialization;

public class ChaseMovement : MonoBehaviour
{
    public float speed = 1f;

    public Vector3 startPos;
    public Vector3 endPos;

    public Transform targetTrans;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
  
        endPos = targetTrans.position;

        Vector3 direction = endPos - transform.position;
        direction.Normalize();

        transform.position += direction * (speed * Time.deltaTime);

    }
}
