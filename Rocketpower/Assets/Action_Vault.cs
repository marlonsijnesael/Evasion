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

        print("hit");

        if (actionKey == KeyCode.None || Input.GetKeyDown(actionKey))
        {
            _o.overrideForce = true;
            if (!lerping)
            {
                lerping = true;
                Coroutine coroutine = StartCoroutine(LerpMove(_o.transform, lerpSpeed, _o));
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

    private IEnumerator LerpMove(Transform position, float time, TestPhysx _o)
    {
        _o.GetComponent<PlayerManager>().SetAnimation(animationName, true);
        time = time / points.Length;
        foreach (Vector3 v in points)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                if (hasTarget)
                {
                    var rot = Vector3.Angle(target.up, target.right);
                    position.transform.rotation = target.rotation;
                    ///position.rotation = Quaternion.Slerp(position.rotation, target.rotation, (elapsedTime / time));
                }
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
}
