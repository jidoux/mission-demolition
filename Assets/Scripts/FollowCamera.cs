using UnityEngine;

public class FollowCamera : MonoBehaviour {
    [Header("Set in Inspector")]
    // basically the logic means this causes the camera to move about 5% of the way from its current location
    // to the point of interest, every iteration of FixedUpdate. This fixes any jarring sudden movements.
    public float easing = 0.05f;
    // static point of interest, its the thing we're trying to follow/focus on (the projectile in this case)
    // since its static, the same value for this is shared by all instances of FollowCamera class, and
    // it can be accessed anywhere via FollowCamera.pointOfInterest, so you can easily determine which projectile to follow
    static public GameObject pointOfInterest;
    public Vector2 minXY = Vector2.zero;
    [Header("Set Dynamically")]
    public float cameraZPosition;

    void Awake() {
        cameraZPosition = this.transform.position.z;
    }
    void Start() {
        
    }

    // its physics so we use fixed update simple as
    void FixedUpdate() {
        Vector3 destination;
        // its null when there are no projectiles which should be followed, intuitively
        if (pointOfInterest == null) {
            // if theres no point of interest just return to [0, 0, 0]
            // TODO what would happen in the case where its slowly returning to 0, 0, 0 and
            // we fire again? Logically it would just continue going right? It should, this logic
            // is simple enough to where thats predictable right?
            // TODO also this is unrelated to this line exactly but why are some walls floating? Smh.
            destination = Vector3.zero;
        }
        else {
            destination = pointOfInterest.transform.position;
            if (pointOfInterest.tag == "Projectile") {
                // if the projectile is not moving (sleeping)
                if (pointOfInterest.GetComponent<Rigidbody>().IsSleeping()) {
                    pointOfInterest = null;
                    return; // no need to do anything else in this case I guess
                }
            }
        }
        // limiting the x and y to min values. This prevents the camera from
        // moving in negative x or y directions basically; we don't want to move backwards or down into the depths.
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        // interpolate from the current camera position towards destination
        // its called linear interpolation, if easing is 0, Lerp returns the first point,
        // if easing is 1, Lerp returns second point, and if easing is 0.5, Lerp returns the midpoint of the 2.
        destination = Vector3.Lerp(transform.position, destination, easing);
        // forcing destination.z to be camera z position to keep the camera far away
        destination.z = cameraZPosition;
        // setting the camera to the destination
        transform.position = destination;
        // this keeps the ground in view no matter what, since the y will never be less than 0 we can just do this to always show
        // the ground which is -10 position.
        Camera.main.orthographicSize = destination.y + 10;
    }

    void Update() {
        
    }
}
