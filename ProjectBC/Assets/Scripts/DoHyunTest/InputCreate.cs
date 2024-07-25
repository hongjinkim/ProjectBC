using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCreate : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
