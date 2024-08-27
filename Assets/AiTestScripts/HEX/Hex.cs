using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    [field: SerializeField] public List<Hex> neighborsHexs {  get; private set; }
    public int row, col;

    public void Initialized(Hexagon h)
    {
        row = h.x;
        col = h.y;
    }

    public void SetNearest(List<Hex> hexes)
    {
        neighborsHexs = hexes;
    }

}
