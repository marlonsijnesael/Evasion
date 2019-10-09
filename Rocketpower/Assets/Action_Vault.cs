using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class Action_Vault : PhysicsAction
{
    [Header("Curve settings: ")]
    public Transform[] waypoints;
    public bool debug = true;
    public float lerpSpeed = 0.1f;
    public KeyCode actionKey;

    [Range(1, 3)]
    public int smoothing = 1;

    private bool lerping;
    [SerializeField] private LineRenderer lr;
    private Vector3[] points = new Vector3[10];
    bool running = false;
    private void Start()
    {
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
                StartCoroutine(LerpMove(waypoints, _o.transform, lerpSpeed, _o));
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
        print("points: " + points.Length.ToString());
        lr.positionCount = points.Length;
        lr.SetPositions(points);
        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;
        lr.startWidth = 0.5f;
        lr.endWidth = 0.5f;

    }

    private IEnumerator LerpMove(Transform[] waypoints, Transform position, float time, TestPhysx _o)
    {
        for (int index = 0; index < points.Length; index++)
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                position.position = Vector3.Lerp(position.position, points[index], (elapsedTime / time));
                elapsedTime += Time.deltaTime;
              
                yield return new WaitForEndOfFrame();
            }
            position.position = points[index];
        }
        _o.overrideForce = false;
        lerping = false;
       
    }


}
