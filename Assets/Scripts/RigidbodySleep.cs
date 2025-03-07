using UnityEngine;

public class RigidbodySleep : MonoBehaviour {

    void Start() {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        // causing the wall's Rigidbody to initially assume it shouldn't be moving, so the castles start out stable
        rigidbody?.Sleep();
    }

    void Update() {
        
    }
}
