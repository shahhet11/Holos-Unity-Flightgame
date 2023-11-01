using UnityEngine;

public class WeaponAimLookAt : MonoBehaviour
{
    void Update()
    {
        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a world position
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        // Make the object look at the mouse position
        transform.LookAt(mousePosition, Vector3.up);
    }
}