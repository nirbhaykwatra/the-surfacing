using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bubble : MonoBehaviour
{
    [field: SerializeField] private float Lifespan { get; set; }
    [field: SerializeField] private float RiseSpeed { get; set; } = 9.81f;
    [Tooltip("How fast a bubble attaches to a liftable object")]
    [field: SerializeField] private float AttachmentSpeed { get; set; } = 0.8f;
    [Tooltip("How far a bubble is when it attaches to a liftable object")]
    [field: SerializeField] private float AttachmentOffset { get; set; } = 0.05f;
    [field: SerializeField] public bool Rise { get; set; } = false;
    
    private Rigidbody _rb;
    private BoxCollider _collider;
    private bool _hasPopped = false;
    private float _lifeSpanTimer;
    private Vector3 _riseVelocity;

    private void OnEnable()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        if (_collider == null) _collider = GetComponent<BoxCollider>();
        Rise = false;
        _hasPopped = false;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        //_collider.isTrigger = true;
        _riseVelocity = new Vector3(0, RiseSpeed, 0);
    }

    private void FixedUpdate()
    {
        if (_rb == null) return;
        if (!_hasPopped && Rise) _rb.MovePosition(transform.position + Vector3.up * (Time.deltaTime * RiseSpeed));
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
            Destroy(gameObject);
        }
    }
    
    public void PushBubble(Vector3 destination, float time)
    {
        // TODO: Currently, the bubbles' velocity increases the farther the player moves from world center.
        //  The force added to the bubble should be unaffected by any variable but the shooting force applied.
        StartCoroutine(MoveBubble(destination, time));
    }

    private IEnumerator MoveBubble(Vector3 destination, float time)
    {
        while (Vector3.Distance(transform.position, destination) > AttachmentOffset)
        {
            transform.position = Vector3.Lerp(transform.position, destination, time);
            yield return null;
        }
        //_rb.isKinematic = false;
        Rise = true;
        yield break;
    }
}
