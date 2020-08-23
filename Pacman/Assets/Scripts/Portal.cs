using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField]
    private GameObject otherPortal;

    private Transform spawnPoint;

    private void Start()
    {
        spawnPoint = otherPortal.transform.Find("Spawn").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = spawnPoint.position;
    }
}
