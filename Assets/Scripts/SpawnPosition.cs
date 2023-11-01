using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public Color gizmoColor = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}