using UnityEngine;

public class GetBounds : MonoBehaviour
{
    // Visual display of the bounding box for testing.
    private void OnDrawGizmos()
    {
        // Draw each child's bounds as a green box.
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        foreach (var child in GetComponentsInChildren<Collider>())
        {
            Gizmos.DrawCube(child.bounds.center, child.bounds.size);
        }

        // Draw total bounds of all the children as a white box.
        Gizmos.color = new Color(1f, 1f, 1f, 0.1f);
        var maxBounds = GetMaxBounds(gameObject);
        Gizmos.DrawCube(maxBounds.center, maxBounds.size);
        //        Debug.Log("Total height is " + maxBounds.size.y);
    }

    public static Bounds GetMaxBounds(GameObject parent)
    {
        var total = new Bounds(parent.transform.position, Vector3.zero);
        foreach (var child in parent.GetComponentsInChildren<Collider>())
        {
            total.Encapsulate(child.bounds);
        }
        return total;
    }
}