using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private void Awake() {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void StartTrail() {
        trailRenderer.emitting = true;
    }

    public void StopTrail() {
        trailRenderer.emitting = false;
    }
    
}
