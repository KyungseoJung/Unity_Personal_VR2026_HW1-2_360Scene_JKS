using UnityEngine;

public class SimpleFirstPersonController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float lookSpeed = 2f;
    public float maxLookX = 25f;
    public float minLookX = -25f;

    private float rotX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 마우스 왼쪽 버튼을 누르고 있을 때만 회전
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            rotX -= mouseY;
            rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

            transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
            transform.parent.Rotate(Vector3.up * mouseX);
        }
        // Keyboard move
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.parent.forward * moveZ + transform.parent.right * moveX;
        transform.parent.position += move * moveSpeed * Time.deltaTime;

        // ESC to unlock cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // sphere 범위 벗어나지 않게 하기
        // sphere 밖으로 못 나가게 제한
        float maxDistance = 45f;   // sphere scale이 100이면 대충 45 정도부터 시작
        Vector3 center = Vector3.zero;

        Vector3 offset = transform.parent.position - center;
        if (offset.magnitude > maxDistance)
        {
            transform.parent.position = center + offset.normalized * maxDistance;
        }
    }
}