using UnityEngine;
[AddComponentMenu("DangSon/MouseMovement")]
public class MouseMovement : MonoBehaviour
{
    [Header("Mouse controller")]
    public float mouseSensivity = 500f;
    [Header("Value Clam")]
    public float topClamp=-90f;
    public float bottonClamp = 90f;
    private float xRotation;
    private float yRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X")*mouseSensivity*Time.deltaTime;
        //Debug.Log(mouseX);
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
        //Debug.Log(mouseY);
        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottonClamp);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
