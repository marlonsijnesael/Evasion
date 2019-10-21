using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class Action_Vault : PhysicsAction
{
    [Header("Curve settings: ")]
    public Transform[] waypoints;
    public bool debug = true;
    public float lerpSpeed = 5f;
    public KeyCode actionKey;
    public string animationName = "";
    public Color curveColor = Color.red;
    public bool hasTarget = false;
    public Transform target;
    [Range(1, 3)]
    public int smoothing = 1;

    private bool lerping;
    [SerializeField] private LineRenderer lr;
    private Vector3[] points = new Vector3[10];
    private Vector3[] pointsReverse = new Vector3[10];

    bool running = false;
    public bool allowOffset = true;

    private void Start()
    {
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        MakeCurve();
    }

    private void Update()
    {
        if (debug && !running)
        {
            MakeCurve();
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }
    }

    public override void Act(TestPhysx _o)
    {
        if (actionKey == KeyCode.None || Input.GetKeyDown(actionKey))
        {
            _o.overrideForce = true;
            if (!lerping)
            {
                lerping = true;
                if (allowOffset)
                {
                    Coroutine coroutine = StartCoroutine(LerpMoveWithOffset(_o.transform, lerpSpeed, _o));
                }
                else
                {
                    Coroutine coroutine = StartCoroutine(LerpMoveWithoutOffset(_o.transform, lerpSpeed, _o));
                }
            }
        }
    }

    private void MakeCurve()
    {
        points = new Vector3[waypoints.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = waypoints[i].position;

        }
        points = CurveEditor.MakeSmoothCurve(points, smoothing);
        lr.positionCount = points.Length;
        lr.SetPositions(points);
        lr.startColor = curveColor;
        lr.endColor = curveColor;
    }

    private float Offset(Vector3 v1, Vector3 v2, float dir)
    {
        v1.y = 0;
        v2.y = 0;
        return Mathf.Abs(Vector3.Distance(v1, v2)) * dir;
    }


    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        Debug.Log("dir: " + dir);
        if (dir > 0f)
        {
            return -1f;
        }
        else if (dir < 0f)
        {
            return 1f;
        }
        return 0f;
    }

    private IEnumerator LerpMoveWithoutOffset(Transform position, float time, TestPhysx _o)
    {
        _o.GetComponent<PlayerManager>().SetAnimation(animationName, true);
        time = time / points.Length;

        foreach (Vector3 v in points)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                position.position = Vector3.Lerp(position.position, v, (elapsedTime / time));
                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            position.position = v;
        }
        _o.GetComponent<PlayerManager>().SetAnimation(animationName, false);
        _o.isGrounded = true;
        _o.overrideForce = false;
        lerping = false;
    }


    private IEnumerator LerpMoveWithOffset(Transform position, float time, TestPhysx _o)
    {
        time = time / points.Length;
        _o.GetComponent<PlayerManager>().SetAnimation(animationName, true);

        Vector3 heading = points[points.Length - 1] - position.position;
        float offsetSide = AngleDir(position.forward, heading, waypoints[0].up);
        float offset = Offset(position.position, points[0], offsetSide);

        foreach (Vector3 v in points)
        {
            float elapsedTime = 0;
            Vector3 v2 = v;
            v2.x += offset;
            v2.z += offset;
            while (elapsedTime < time)
            {
                position.position = Vector3.Lerp(position.position, v2, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            position.position = v2;
        }
        _o.GetComponent<PlayerManager>().SetAnimation(animationName, false);
        _o.isGrounded = true;
        _o.overrideForce = false;
        lerping = false;
    }
}
