using System.Runtime.CompilerServices;
using UnityEngine;

public class Slingshot : MonoBehaviour {
    static private Slingshot SingletonInstance;
    [Header("Set in Inspector")]
    public GameObject projectilePrefab;
    public float velocityMultiplier = 10f;
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPosition;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;
    static public Vector3 LAUNCH_POS {
        get {
            if (SingletonInstance == null) {
                return Vector3.zero;
            }
            return SingletonInstance.launchPosition;
        }
    }
    [SerializeField] private LineController lineController;
    [SerializeField] private MissionDemolition missionDemolition;

    void Awake() {
        SingletonInstance = this;
        // I guess this is just a way to find the launch point, idk why this way was used
        // ah thats why its used to find the launch point and easily set the launch position
        // for the projectile logic.
        Transform launchPointTransform = transform.Find("LaunchPoint");
        launchPoint = launchPointTransform.gameObject;
        launchPoint.SetActive(false); // intuitively this deactvates this GameObject
        launchPosition = launchPointTransform.position;
    }
    void Start() {
        
    }

    void Update() {
        if (!aimingMode) {
            return;
        }
        Vector3 mousePosition2D = Input.mousePosition;
        mousePosition2D.z = -Camera.main.transform.position.z;
        Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(mousePosition2D);

        Vector3 mouseDelta = mousePosition3D - launchPosition;
        // keeping the center of the projectile within this Slingshot's SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize(); // sets its length to 1 but keeps it in the same direction
            mouseDelta *= maxMagnitude; // idk for what purpose this is
        }

        // so theres a new position, we move the projectile there
        // what is this new position, I don't even know.
        Vector3 projectilePosition = launchPosition + mouseDelta;
        projectile.transform.position = projectilePosition;

        // this is executed on the frame the mouse button is released
        if (Input.GetMouseButtonUp(0)) { // its just a way to read mouse button state
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            // TODO it was velocity in the text; ensure this works correctly okay?
            projectileRigidbody.linearVelocity = -mouseDelta * velocityMultiplier;
            FollowCamera.pointOfInterest = projectile;
            projectile = null; // it opens the projectile field to be set by another GameObject
            missionDemolition.ShotFired(); // a
            ProjectileLine.SingletonInstance.pointOfInterest = projectile;
        }
    }

    // these functions work automatically on any collider or trigger
    void OnMouseEnter() {
        // TODO make this work, the halo is not visibly changing anything since SRP doesn't support Halo component.
        // I would also say try to see if you can solely disable the Halo without disabling the entire GameObject component.
        // it was not possible at the time of the text release but it appears to be possible now.
        // https://www.youtube.com/watch?v=f0T0fiWwZxM
        launchPoint.SetActive(true);
    }

    void OnMouseExit() {
        launchPoint.SetActive(false);
    }

    // only called on the frame where the user presses mouse down over the collider component of the slingshot gameobject
    void OnMouseDown() {
        // at this point the player pressed mouse down while hoving over the slingshot
        aimingMode = true;
        projectile = Instantiate(projectilePrefab);
        // starting it at the launch point
        projectile.transform.position = launchPosition;
        // just setting it to isKinematic for now, which lets it not be affected by physics (not falling)
        // but it can still affect other gameobjects I believe.
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
        lineController.AddProjectile(projectile.transform);
    }
}
