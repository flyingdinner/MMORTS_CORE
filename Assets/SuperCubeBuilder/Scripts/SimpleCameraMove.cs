using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraMove : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform target;
    public float distanceFromTarget = 2f;
    public float scrollSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // проверяем, нажата ли правая кнопка мыши
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            yRotation += mouseX;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel"); // получаем значение скролла мыши
        distanceFromTarget -= scroll * scrollSensitivity; // изменяем расстояние от цели на основе скролла мыши


        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
