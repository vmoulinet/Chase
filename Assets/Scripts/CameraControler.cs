using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public float lookSpeed = 2f;
    public float moveSpeed = 10f;
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 100f;

    private Vector3 dragOrigin;

    void Update()
    {
        // Rotation de la caméra (clic droit)
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0f);
        }

        // Zoom (molette)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 direction = transform.forward * scroll * zoomSpeed;
        Vector3 newPos = transform.position + direction;

        float distance = Vector3.Distance(Vector3.zero, newPos); // Distance depuis le centre
        if (distance >= minZoom && distance <= maxZoom)
            transform.position = newPos;

        // Déplacement (clic molette ou WASD)
        if (Input.GetMouseButtonDown(2))
            dragOrigin = Input.mousePosition;

        if (Input.GetMouseButton(2))
        {
            Vector3 diff = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-diff.x, -diff.y, 0) * moveSpeed;

            transform.Translate(move, Space.Self);
            dragOrigin = Input.mousePosition;
        }

        // Déplacement clavier
        float h = Input.GetAxis("Horizontal"); // A / D
        float v = Input.GetAxis("Vertical");   // W / S
        Vector3 keyboardMove = (transform.right * h + transform.forward * v) * moveSpeed * Time.deltaTime;
        transform.position += keyboardMove;
    }
}
