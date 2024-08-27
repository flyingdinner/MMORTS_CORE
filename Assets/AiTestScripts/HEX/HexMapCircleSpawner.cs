using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HexMapCircleSpawner : MonoBehaviour
{
    public HexGrid grid;
    public GameObject hexgo;
    public int hexCount = 100;

    [SerializeField] private Transform _spawnParent;
    [SerializeField] private float _multiplier_y = 0.9f;
    [SerializeField] private float _multiplier_x = 2f;
    [SerializeField] private float _nearestRange = 1f;

    [field: SerializeField] public List<Hex> hexes { get; private set; }
    
    private void Start()
    {
        hexes = new List<Hex>();
        grid = new HexGrid();
        grid.Generate(hexCount);
        SpawnPoints(grid);
        InitializeCells();
    }

    private void SpawnPoints(HexGrid grid)
    {
        foreach(Hexagon h in grid.hexagons)
        {            
           Vector3 point = new Vector3((float)h.x + (float)h.y / _multiplier_x, 0, (float)h.y * _multiplier_y);
           GameObject hexGO = Instantiate(hexgo, point, Quaternion.identity);

           hexGO.transform.parent = _spawnParent;
           hexGO.transform.localPosition = point;
           hexGO.transform.localRotation = Quaternion.identity;
           hexGO.name = "< HEX : " + h.x + " : " + h.y + " >"; 
           Hex hex = hexGO.GetComponent<Hex>();
           hex.Initialized(h);
           hexes.Add(hex);
        }
    }

    private void InitializeCells()
    {        
        foreach (Hex hex in hexes)
        {
            hex.SetNearest(GetNeighbors(hex).ToList());
        }
    }

    public Hex[] GetNeighbors(Hex hex)
    {
        List<Hex> gos = new List<Hex>();
        foreach (Hex g in hexes)
        {
            if (g.transform.position == hex.transform.position) continue;
            if (Vector3.Distance(g.transform.position, hex.transform.position) < _nearestRange)
            {
                gos.Add(g);
            }
        }

        return gos.ToArray();
    }
}

[System.Serializable]
public class Hexagon
{
    public int x;
    public int y;
    public Hexagon(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

[System.Serializable]

public class HexGrid
{
    public List<Hexagon> hexagons = new List<Hexagon>();
    private Queue<Hexagon> queue = new Queue<Hexagon>();

    public void Generate(int numHexagons)
    {
        Hexagon firstHexagon = new Hexagon(0, 0);
        hexagons.Add(firstHexagon);
        queue.Enqueue(firstHexagon);

        while (hexagons.Count < numHexagons && queue.Count > 0)
        {
            Hexagon hexagon = queue.Dequeue();
            FindNeighbours(hexagon);
        }
    }

    private void FindNeighbours(Hexagon hexagon)
    {
        int[] dx = { 0, 1, 1, 0, -1, -1 };
        int[] dy = { 1, 0, -1, -1, 0, 1 };

        for (int i = 0; i < 6; i++)
        {
            int neighbourX = hexagon.x + dx[i];
            int neighbourY = hexagon.y + dy[i];

            Hexagon neighbour = hexagons.Find(h => h.x == neighbourX && h.y == neighbourY);
            if (neighbour == null)
            {
                neighbour = new Hexagon(neighbourX, neighbourY);
                hexagons.Add(neighbour);
                queue.Enqueue(neighbour);
            }
        }
    }
}