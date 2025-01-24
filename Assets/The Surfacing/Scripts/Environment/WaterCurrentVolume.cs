using System;
using System.Collections;
using UnityEngine;

public class WaterCurrentVolume : MonoBehaviour
{
    public enum WaterCurrentAxis
    {
        Right,
        Left,
        Up,
    }

    [field: SerializeField] private float WaterCurrentStrength { get; set; } = 3;
    [field: SerializeField] private float Buoyancy { get; set; } = 0.2f;
    
    private BoxCollider _collider;
    public WaterCurrentAxis _axis;
    
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }
    
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            if (other.TryGetComponent<Bubble>(out Bubble bubble))
            {
                bubble.InCurrent = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            if (other.TryGetComponent<Bubble>(out Bubble bubble))
            {
                switch (_axis)
                {
                    case WaterCurrentAxis.Right:
                        bubble.gameObject.transform.position += 
                            (Vector3.right + new Vector3(0f, Buoyancy, 0f)) * (Time.deltaTime * WaterCurrentStrength);
                        break;
                    case WaterCurrentAxis.Up:
                        bubble.gameObject.transform.position += 
                            Vector3.up * (Time.deltaTime * WaterCurrentStrength);
                        break;
                    case WaterCurrentAxis.Left:
                        bubble.gameObject.transform.position += 
                            (Vector3.left + new Vector3(0f, Buoyancy, 0f)) * (Time.deltaTime * WaterCurrentStrength);
                        break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            if (other.TryGetComponent<Bubble>(out Bubble bubble))
            {
                bubble.InCurrent = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
