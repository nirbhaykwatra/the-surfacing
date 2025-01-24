using UnityEngine;

public class WaterCurrentVolume : MonoBehaviour
{
    private BoxCollider _collider;
    
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
        _collider.size = transform.localScale;
    }
    
    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _collider.size);
    }
}
