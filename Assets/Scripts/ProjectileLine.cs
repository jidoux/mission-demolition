using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileLine : MonoBehaviour {
    static public ProjectileLine SingletonInstance;
    [Header("Set in Inspector")]
    public float minDistance = 0.1f;
    private LineRenderer lineRenderer;
    private GameObject _pointOfInterest;
    private List<Vector3> points;

    void Awake() {
        SingletonInstance = this;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        points = new List<Vector3>(); // initializing the points list
    }

    void Start() {
        
    }

    void Update() {
        
    }

    void FixedUpdate() {
        if (pointOfInterest == null) {
            if (FollowCamera.pointOfInterest?.tag == "Projectile") {
                pointOfInterest = FollowCamera.pointOfInterest;
            }
            else {
                return;
            }
            // TODO ensure this logic works its the above logic for the return i think the return is necessary right?
            // pointOfInterest = (FollowCamera.pointOfInterest?.tag == "Projectile") ? FollowCamera.pointOfInterest : null;
            // if (FollowCamera.pointOfInterest != null) {
            //     if (FollowCamera.pointOfInterest.tag == "Projectile") {
            //         pointOfInterest = FollowCamera.pointOfInterest;
            //     }
            //     else {
            //         return;
            //     }
            // }
        }
        AddPoint();
        if (FollowCamera.pointOfInterest == null) {
            // once followCamera point of interest is null, the local one should also be null
            pointOfInterest = null;
        }
    }

    // pointOfInterest property
    public GameObject pointOfInterest {
        get {
            return _pointOfInterest;
        }
        set {
            _pointOfInterest = value;
            if (_pointOfInterest != null) {
                // when point of interest is set to something new, it resets everything
                lineRenderer.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    // can be used to clear the line directly
    public void Clear() {
        _pointOfInterest = null;
        lineRenderer.enabled = false;
        points = new List<Vector3>();
    }

    // this adds a point to the line, intuitively
    public void AddPoint() {
        Vector3 point = _pointOfInterest.transform.position;
        if (points.Count > 0 && (point - lastPoint).magnitude < minDistance) {
            // if the point isnt far enough from the last point we just return
            return;
        }

        // this is the launch point
        if (points.Count == 0) {
            Vector3 launchPositionDifference = point - Slingshot.LAUNCH_POS;
            points.Add(point + launchPositionDifference);
            points.Add(point);
            lineRenderer.positionCount = 2;
            // setting the first 2 points
            lineRenderer.SetPosition(0, points[0]);
            lineRenderer.SetPosition(1, points[1]);
            lineRenderer.enabled = true; // enabling it here I guess since we fired
        }
        else {
            // this is just normal point adding behavior
            points.Add(point);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, lastPoint);
            lineRenderer.enabled = true;

        }
    }

    public Vector3 lastPoint {
        get {
            // TODO ensure this works like the below code
            return (points == null) ? Vector3.zero : points[points.Count - 1];
            // if (points == null) {
            //     return Vector3.zero;
            // }
            // return points[points.Count - 1];
        }
    }

}
