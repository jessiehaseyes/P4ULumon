using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public List<GameObject> cubeList = new List<GameObject>();
    public int maxObjects = 5;
    public int score = 0;
    private GameObject cubeHolder;


    void Start()
    {
        cubeHolder = new GameObject("Cube Holder");

        for (int i = 0; i < maxObjects; i++)
        {
            SpawnCube();
        }
        InvokeRepeating(nameof(SpawnCube), 5, 5);

     
    }

    void Update()
    {
        
    }

    public void SpawnCube()
    {
        if (cubeList.Count >= maxObjects)
        {
            return;
        }

        GameObject newCube = Instantiate(cubePrefab,
            new Vector3(
                Random.Range(-10f, 10f),
                0.5f,
                Random.Range(-10f, 10f)),
            Quaternion.identity,
            cubeHolder.transform);

      
     
        cubeList.Add(newCube);
    }

    


}
