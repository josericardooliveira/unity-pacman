using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PillScript : MonoBehaviour
{
    private Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        RemoveTile(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RemoveTile(collision);
    }

    private void RemoveTile(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3Int cell = tilemap.WorldToCell(collision.transform.position);
            tilemap.SetTile(cell, null);
        }
    }
}
