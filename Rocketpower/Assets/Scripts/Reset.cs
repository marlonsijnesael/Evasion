using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    public Transform startPoint;

    private void Update()
    {
        Reset();
    }
    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
        }
    }

}
