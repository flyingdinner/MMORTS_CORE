using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapSpawner : MonoBehaviour
{
    public GameObject hexPrefab; // префаб для гексагонів
    public int numRows;
    public int numColumns;
    public float hexSize;
    public float gap;

    void Start()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                // створити новий гексагон з префабу
                GameObject hex = Instantiate(hexPrefab);

                // обчислити позицію гексагону
                float x = col * hexSize * 1.5f;
                float y = 0;
                float z = row * (hexSize * Mathf.Sqrt(3) + gap);
                if (col % 2 == 1) z += hexSize * Mathf.Sqrt(3) / 2 + gap / 2;
                hex.transform.position = new Vector3(x, y, z);

                // налаштувати орієнтацію гексагону
                if (row % 2 == 1) hex.transform.Rotate(0, 0, 30);
            }
        }
    }
}
