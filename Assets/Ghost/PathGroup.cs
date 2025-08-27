using System.Collections.Generic;
using UnityEngine;

public class PathGroup : MonoBehaviour
{
    [Tooltip("Danh sách waypoint, kéo thả theo thứ tự muốn.")]
    public List<Transform> waypoints = new List<Transform>();

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
