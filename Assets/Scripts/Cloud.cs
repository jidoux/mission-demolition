using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject cloudSphere;
    public int numSpheresMin = 6;
    public int numSpheresMax = 10; // it says this is 1 more than the actual max
    // this is max distance (+|-) that a CloudSphere can be from the center of the cloud in each dimension
    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);
    // the range of cloud scales in each dimension, these will be wider moreso than tall for example
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f;
    // storing all spheres to keep track of them for deletion purposes ig (is this the optimal way to do it?)
    // I guess it would be more optimal than some findall by a lot, right?
    private List<GameObject> spheres;

    void Start() {
        spheres = new List<GameObject>();
        int numSpheresToAttachToCloud = Random.Range(numSpheresMin, numSpheresMax);
        for (int i = 0; i < numSpheresToAttachToCloud; i++) {
            GameObject sphereGameObject = Instantiate<GameObject>(cloudSphere);
            spheres.Add(sphereGameObject);
            Transform sphereTransform = sphereGameObject.transform;
            sphereTransform.SetParent(this.transform);

            // randomly assigned offset to set to the sphere
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            sphereTransform.localPosition = offset;

            // random scale also
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            // this adjusts y scale by x distance from the center of the cloud
            // it makes clouds taper on their left and right sides
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);
            sphereTransform.localScale = scale;
        }

    }

    void Update() {
        // purely for testing so its commented out.
        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     Restart();
        // }
    }

    // was used for testing but it may be used in other places so its not commented out for now.
    void Restart() {
        foreach (GameObject sphereGameObject in spheres) {
            Destroy(sphereGameObject);
        }
        Start();
    }
}
