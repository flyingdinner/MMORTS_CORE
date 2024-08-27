using UnityEngine;

namespace RTSCamera
{
    public class CameraController : MonoBehaviour
    {
        public float panSpeed = 20f;       // Скорость перемещения камеры
        public float panBorderThickness = 10f;  // Расстояние от границ экрана, при котором камера начнет перемещаться
        public Vector2 panLimit;           // Ограничение по оси X и Z, чтобы камера не выходила за пределы карты

        public float scrollSpeed = 20f;    // Скорость зума
        public float minY = 20f;           // Минимальная высота камеры
        public float maxY = 120f;          // Максимальная высота камеры

        void Update()
        {
            Vector3 pos = transform.position;

            // Управление перемещением камеры с помощью клавиш WASD или стрелок
            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.z += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            {
                pos.z -= panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
            }

            // Ограничение перемещения камеры
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

            // Управление зумом с помощью колеса мыши
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

            // Ограничение зума
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            transform.position = pos;
        }
    }
}
