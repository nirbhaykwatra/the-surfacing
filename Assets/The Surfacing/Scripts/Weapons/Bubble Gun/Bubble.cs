using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bubble : MonoBehaviour
{
    [field: SerializeField] private float Lifespan { get; set; }
    
    [field: SerializeField] private float RiseSpeed { get; set; } = 9.81f;
    [field: SerializeField] private bool Rise { get; set; } = true;
    
    private Rigidbody _rb;
    private bool _hasPopped = false;
    private float _lifeSpanTimer;
    private Vector3 _riseVelocity;

    private void OnEnable()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        _hasPopped = false;
        _rb.useGravity = false;
        _riseVelocity = new Vector3(0, RiseSpeed, 0);
    }

    private void FixedUpdate()
    {
        if (_rb == null) return;
        if (!_hasPopped && Rise) _rb.AddForce(_riseVelocity, ForceMode.Force);
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
            Destroy(gameObject);
        }
    }

    public void PushBubble(Vector3 direction, float force)
    {
        // TODO: Currently, the bubbles' velocity increases the farther the player moves from world center.
        //  The force added to the bubble should be unaffected by any variable but the shooting force applied.
        _rb.AddRelativeForce(new Vector3(direction.x, 0f, direction.z) * force, ForceMode.Impulse);
    }
}
