using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWorld : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uv = new List<Vector2>();
    List<int> quads = new List<int>();
    Mesh mesh;
    [Header("ChunkSettings")]
    [SerializeField]
    int xSize, ySize;
    [Header("AtlasSettings")]
    [SerializeField]
    int tilesX, tilesY;
    int[,] map;
    static bool gen = false;
    static Vector2 offset;
    void Start()
    {
        if (gen == false)
        {
            offset = new Vector2(Random.Range(-10000, 10000), Random.Range(-10000, 10000));
            gen = true;
        }
        map = new int[xSize , ySize];
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                float xCoord = ((int)(transform.position.x)+i) / 18f + offset.x;
                float yCoord = (j+ (int)(transform.position.y)) / 18f + offset.y ;
                map[i, j] = 1;
                if((Mathf.PerlinNoise(xCoord, yCoord)) < 0.4f)
                {
                    map[i, j] = 0;
                }
            }
        }
        mesh = GetComponent<MeshFilter>().mesh;
        BuildChunk();
    }
    void BuildChunk()
    {
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if (map[x, y] > 0)
                {
                    CreateTile(new Vector3(x, y), map[x, y]);
                }
            }
        }
        if (vertices.Count > 3)
        {
            mesh.SetVertices(vertices);
            mesh.SetIndices(quads.ToArray(), MeshTopology.Quads, 0);
            mesh.SetUVs(0, uv);
        }
    }
    void CreateTile(Vector3 position,int blockId)
    {
        int temp = vertices.Count;
        
        vertices.Add(position + new Vector3(0, 0, 0));
        vertices.Add(position + new Vector3(1, 0, 0));
        vertices.Add(position + new Vector3(0, 1, 0));
        vertices.Add(position + new Vector3(1, 1, 0));

        quads.Add(temp + 2);
        quads.Add(temp + 3);
        quads.Add(temp + 1);
        quads.Add(temp + 0);

        blockId -= 1;
        float ox = (1.0f/tilesX) * (blockId % tilesX), oy = (1.0f/tilesY) * ((blockId / tilesX) + 1);
        oy = 1 - oy;
        uv.Add(new Vector2(ox, oy));
        uv.Add(new Vector2(1.0f/tilesX + ox, oy));
        uv.Add(new Vector2(ox, 1.0f / tilesY + oy));
        uv.Add(new Vector2(1.0f/tilesX + ox, 1.0f / tilesY + oy));    
    }
}
