using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileMapProcedural : MonoBehaviour
{
    Tilemap tilemap;
    [SerializeField]
    TileBase tile;
    void Start()
    { 
        tilemap = GetComponent<Tilemap>();
        tilemap.SetTile(new Vector3Int(128, 32, 0),tile);

    }
    void DeleteTile(Vector3Int position,float delay)
    {
        StartCoroutine(DeleteTileCoroutine(position,delay));
    }
    IEnumerator DeleteTileCoroutine(Vector3Int position,float delay)
    {
        yield return new WaitForSeconds(delay);
        tilemap.SetTile(position, null);
    }
}
