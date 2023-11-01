using UnityEngine;

public class RotateOnClick : MonoBehaviour
{
    private bool isRotating = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Left mouse button clicked, start rotating.
            isRotating = true;
        }

        if (isRotating)
        {
            // Rotate the object by 45 degrees around the Z-axis.
            transform.Rotate(Vector3.forward * 45f * Time.deltaTime * 4f);

            if (Input.GetMouseButtonUp(0))
            {
                // Left mouse button released, stop rotating.
                isRotating = false;
            }
        }
    }
}
