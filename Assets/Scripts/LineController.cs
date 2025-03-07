using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {
    private LineRenderer _lineRenderer;
    [SerializeField] private List<Transform> _stuffToBuildLineFrom = new List<Transform>();

    [Header("Offsets for Slingshot Arms")]
    [SerializeField] private Vector3 leftArmOffset = new Vector3(-0.5f, 0.5f, 0);
    [SerializeField] private Vector3 rightArmOffset = new Vector3(0.5f, 0.5f, 0);
    void Start() {
        _lineRenderer = GetComponent<LineRenderer>();



    }

    void Update() {
        _lineRenderer.positionCount = _stuffToBuildLineFrom.Count;
        for (int i = 0; i < _stuffToBuildLineFrom.Count; i++) {
            Vector3 pos = _stuffToBuildLineFrom[i].position;
            if (i == 0) {
                pos += leftArmOffset;
            } else if (i == 1) {
                pos += rightArmOffset;
            }
            
            _lineRenderer.SetPosition(i, pos);
        }
    }

    public void AddProjectile(Transform projectileTrans) {
        _stuffToBuildLineFrom.Add(projectileTrans);
    }

    public void RemoveProjectileFromLine() {
        // pretty sure this check isnt needed btw since its called after shooting so the projectile was already instantiated with the draw
        if (_stuffToBuildLineFrom.Count > 2) {
            _stuffToBuildLineFrom.RemoveRange(2, _stuffToBuildLineFrom.Count - 2);
        }
    }
}
