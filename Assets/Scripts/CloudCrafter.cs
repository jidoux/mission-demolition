using UnityEngine;

public class CloudCrafter : MonoBehaviour {
    [Header("Set in Inspector")]
    public int numCloudsToMake = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMultiplier = 0.5f;
    private GameObject[] allCloudInstances;

    void Awake() {
        allCloudInstances = new GameObject[numCloudsToMake];
        GameObject anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;
        for (int i = 0; i < numCloudsToMake; i++) {
            cloud = Instantiate<GameObject>(cloudPrefab);
            // positioning the clouds
            Vector3 cloudPosition = Vector3.zero;
            cloudPosition.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cloudPosition.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            float scale = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scale);
            // this puts smaller clouds (smaller scale) closer to the ground
            cloudPosition.y = Mathf.Lerp(cloudPosMin.y, cloudPosition.y, scale);
            // and this puts smaller clouds further away
            cloudPosition.z = 100 - 90 * scale;

            // applying these transforms to the cloud
            cloud.transform.position = cloudPosition;
            cloud.transform.localScale = Vector3.one * scaleVal;

            // now making the cloud a child of the anchor and putting it in the all clouds list
            cloud.transform.SetParent(anchor.transform);
            allCloudInstances[i] = cloud;
        }
    }

    void Start() {
        
    }

    void Update() {
        foreach (GameObject cloud in allCloudInstances) {
            // getting cloud scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cloudPosition = cloud.transform.position;
            // this makes larger clouds move faster
            cloudPosition.x -= scaleVal * Time.deltaTime * cloudSpeedMultiplier;
            // this affects clouds which have moved too far left
            if (cloudPosition.x <= cloudPosMin.x) {
                cloudPosition.x = cloudPosMax.x;
            }
            // applying the new transform to cloud
            cloud.transform.position = cloudPosition;
        }
    }
}
