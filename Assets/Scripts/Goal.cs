using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour {
    static public bool goalMet = false;

    void Start() {
        
    }

    void Update() {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Projectile")) {
            Goal.goalMet = true;
            Material mat = GetComponent<Renderer>().material;
            Color color = mat.color;
            color.a = 1;
            mat.color = color;
        }
    }
}
