using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHolder : MonoBehaviour
{
    [SerializeField] private List<CubeContainer> _cubes = new List<CubeContainer>();
    
    [System.Serializable]
    public class CubeContainer
    {
       public GameObject go;
       public Vector3Int position;
       public CubeInScene cis;

        public CubeContainer(GameObject cube)
        {
            go = cube;
            position = Vecto3ToInt(cube.transform.position);            
            cis = go.GetComponent<CubeInScene>();
            if(cis == null) cis = go.AddComponent<CubeInScene>();
            //
        }
    }
    public void AddCube(GameObject cube)
    {
        _cubes.Add(new CubeContainer (cube));
    }

    public bool PositionIsFree(Vector3 v3)
    {
        Vector3Int v3i = Vecto3ToInt(v3);

        foreach(CubeContainer cc in _cubes)
        {
            if (cc.position == v3i) return false;
        }

        return true;
    }

    public static Vector3Int Vecto3ToInt(Vector3 v3)
    {
        Vector3Int v3i = new Vector3Int(
            (int)v3.x,
            (int)v3.y,
            (int)v3.z
            );
        return v3i;
    }
}
