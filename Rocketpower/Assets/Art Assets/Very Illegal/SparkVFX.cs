using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class SparkVFX : MonoBehaviour
{

    private Transform playerToFollow;
    private VisualEffect fx;
    private Color color;

    private void Start()
    {
        fx = gameObject.GetComponent<VisualEffect>();
    }

    private void Update()
    {
        fx.SetVector3("CenterPoint", transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void SetPlayerToFollow(Transform newTarget, Color c)
    {
        playerToFollow = newTarget;
        color = c;
        StartCoroutine(MoveToNewPlayer());
        fx.enabled = true;
    }

    private IEnumerator MoveToNewPlayer()
    {
        Vector3 startPos = transform.position;
        transform.parent = null;
        fx.SetVector4("PlayerColor", color);
        Debug.Log(color);
        for (int i = 0; i < 15; i++)
        {
            transform.position = Vector3.Lerp(startPos, playerToFollow.position + new Vector3(0, 1.1f, 0), i * .066666f);
            yield return null;
        }
        transform.parent = playerToFollow;
        transform.position = playerToFollow.position + new Vector3(0, 1.1f, 0);
        yield return null;
    }

}
