using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCubeSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        selectMode,
        cameraMode,
        cubeSpawn,
        modeCube
    }

    public GameObject cubePrefab; // ссылка на префаб куба
    public CubeHolder _cubeContainer;
    public SpawnState currentState = SpawnState.cubeSpawn;
    private CubeInScene _selectedCubeInScene;

    [SerializeField] private Transform _cursorPivot;

    void Update()
    {
        switch (currentState)
        {
            case SpawnState.cubeSpawn:
                {
                    UpdateSpawnCube();
                }
                break;
            case SpawnState.selectMode:
                {
                    UpdateSelectCube();
                }
                break;
    
            default:

            break;
        }
        //UpdateSpawnCube();
    }
    void UpdateSpawnCube()
    {
        RaycastHit hitInfo;
        Vector3 spawnPosition = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
        {
            // Определяем грань, на которую попал рейкаст
            Vector3 normal = hitInfo.normal;
            Vector3 point = hitInfo.point;
            // Вычисляем позицию для создания нового куба с учетом грани, на которую попал рейкаст
            spawnPosition = point + (normal * 0.5f);

            spawnPosition = new Vector3(
                Mathf.Round(spawnPosition.x),
                Mathf.Round(spawnPosition.y),
                Mathf.Round(spawnPosition.z));
        }
        else
        {
            return;
        }

        if (Input.GetMouseButtonDown(0)) // обрабатываем только нажатие левой кнопки мыши
        {
            // округляем позицию до ближайшей единицы
            if (!_cubeContainer.PositionIsFree(spawnPosition)) return;

            // Создаем новый куб с учетом грани, на которую попал рейкаст
            _cubeContainer.AddCube(Instantiate(cubePrefab, spawnPosition, rotation));
        }
        else
        {
            _cursorPivot.position = spawnPosition;

        }
    }

    void UpdateSelectCube()
    {
        if (Input.GetMouseButtonDown(0)) // обрабатываем только нажатие левой кнопки мыши
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
            {
                CubeInScene cis = hitInfo.collider.gameObject.GetComponent<CubeInScene>();
                if (cis == null)
                {
                    Debug.Log("miss CubeInScene == null");
                    return;
                }
                if(cis.TrySelect())
                {
                    currentState = SpawnState.modeCube;
                    _selectedCubeInScene = cis;
                    _cursorPivot.position = cis.transform.position;
                }
            }
        }
        else
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
            {
                CubeInScene cis = hitInfo.collider.gameObject.GetComponent<CubeInScene>();
                if (cis == null)
                {
                    //Debug.Log("miss CubeInScene == null");
                    _cursorPivot.position = hitInfo.point + 0.5f * Vector3.up;                    
                    return;
                }
                _cursorPivot.position = cis.transform.position;            
            }
        }
    }

    void UpdateModeCube()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
           
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
           
        }
    }
}
