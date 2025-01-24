using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Bubble : MonoBehaviour
{
    [field: SerializeField] private float Lifespan { get; set; }
    [field: SerializeField] private float RiseSpeed { get; set; } = 9.81f;
    [Tooltip("How fast a bubble attaches to a liftable object")]
    [field: SerializeField] private float AttachmentSpeed { get; set; } = 0.8f;
    [Tooltip("How far a bubble is when it attaches to a liftable object")]
    [field: SerializeField] private float AttachmentOffset { get; set; } = 0.05f;
    [field: SerializeField] public bool Rise { get; set; }
    
    private Rigidbody _rb;
    private BoxCollider _collider;
    private bool _hasPopped;
    private float _lifeSpanTimer;
    
    public UnityEvent OnDestroyed;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        Rise = false;
        _hasPopped = false;
    }

    private void FixedUpdate()
    {
        if (!_hasPopped && Rise) transform.position = (transform.position + Vector3.up * (Time.fixedDeltaTime * RiseSpeed));
    }

    private void Update()
    {
        if (!_hasPopped)
        {
            _lifeSpanTimer += Time.deltaTime;
            if (_lifeSpanTimer >= Lifespan)
            {
                _hasPopped = true;
            }
        }
        else
        {
            _lifeSpanTimer = 0;
            OnDestroyed?.Invoke();
            Destroy(gameObject);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            return;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Liftable"))
        {
            
        }
        else
        {
            _hasPopped = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }

    public void PushBubble(Vector3 destination, float time)
    {
        StartCoroutine(MoveBubble(destination, time));
    }

    private IEnumerator MoveBubble(Vector3 destination, float time)
    {
        while (Vector3.Distance(transform.position, destination) > AttachmentOffset)
        {
            transform.position = Vector3.Lerp(transform.position, destination, time);
            yield return null;
        }
        Rise = true;
        yield break;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _collider.size);
    }
}
