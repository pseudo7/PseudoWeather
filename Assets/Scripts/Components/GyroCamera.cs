using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GyroCamera : MonoBehaviour
{
    float yRotation, xRotation;
    Vector3 lastPos, delta;

    private void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(0))
                lastPos = Input.mousePosition;
            else if (Input.GetMouseButton(0))
            {
                delta = Input.mousePosition - lastPos;

                transform.Rotate(new Vector3(-delta.y, delta.x));
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

                lastPos = Input.mousePosition;
            }
        }
        else
        {
            xRotation += -Input.gyro.rotationRateUnbiased.x;
            yRotation += -Input.gyro.rotationRateUnbiased.y;
            transform.eulerAngles = new Vector3(xRotation, yRotation, 0);
        }
    }
}