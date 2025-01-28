using FMODUnity;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Bubble : MonoBehaviour
{
    [field: SerializeField] public float Lifespan { get; set; } = 10f;
    [field: SerializeField] public float RiseSpeed { get; set; } = 0.7f;
    [Tooltip("How fast a bubble attaches to a liftable object")]
    [field: SerializeField] private float AttachmentSpeed { get; set; } = 0.8f;
    [Tooltip("How far a bubble is when it attaches to a liftable object")]
    [field: SerializeField] private float AttachmentOffset { get; set; } = 0.05f;
    [field: SerializeField] public bool Rise { get; set; }
    [field: SerializeField] public bool InCurrent { get; set; }

    [Header("Audio")]
    [field: SerializeField] public EventReference _bubblePop;
    
    public Rigidbody Rigidbody;
    public BoxCollider _collider;
    private bool _hasPopped;
    private bool _isAttached;
    private bool _hasTraveled;
    private bool _isKinematic;
    private bool _hasLeftCurrent;
    private float _lifeSpanTimer;
    private BubbleGun _bubbleGun;
    private Vector3 _previousPosition = new Vector3();
    private Vector3 _kinematicVelocity = new Vector3();
    
    public UnityEvent OnDestroyed;
    
    public bool HasLeftCurrent { get => _hasLeftCurrent; set => _hasLeftCurrent = value; }
    
    public Vector3 Velocity => _kinematicVelocity;

    private void OnEnable()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _bubbleGun = FindAnyObjectByType<BubbleGun>();
        Rise = false;
        InCurrent = false;
        _hasPopped = false;
        _isAttached = false;
        _hasTraveled = false;
        _isKinematic = false;
        _hasLeftCurrent = false;
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (!InCurrent)
        {
            if (_hasLeftCurrent)
            {
                Rise = false;

                if (_isKinematic)
                {
                    
                }
                else
                {
                    Rigidbody.linearVelocity = Vector3.Lerp(Rigidbody.linearVelocity, Vector3.zero, Time.fixedDeltaTime);

                    if (Rigidbody.linearVelocity.magnitude < 0.05f)
                    {
                        Rise = true;
                        _hasLeftCurrent = false;
                    }
                }
            }
            
            if (!_hasPopped && Rise)
            {
                //Debug.Log(_isKinematic);
                Vector3 nextPosition = transform.position + new Vector3(0f, Time.fixedDeltaTime * RiseSpeed, 0f);

                if (_isKinematic)
                {
                    Rigidbody.MovePosition(nextPosition);
                }
                else
                {
                    Rigidbody.Move(nextPosition, Quaternion.identity);
                }
                
                //transform.position += Vector3.up * (Time.fixedDeltaTime * RiseSpeed);
            }
        }
    }

    private void Update()
    {
        if (!_hasPopped)
        {
            if (InCurrent)
            {
                StopCoroutine(MoveBubble(transform.position, 0.1f));
            }

            if (_hasTraveled)
            {
                _lifeSpanTimer += Time.deltaTime;
                if (_lifeSpanTimer >= Lifespan)
                {
                    _hasPopped = true;
                }
            }
        }
        else
        {
            _lifeSpanTimer = 0;
            _isAttached = false;
            if (_bubbleGun != null)
            {
                if (_bubbleGun._bubbles.Contains(this))
                {
                    _bubbleGun._bubbles.Remove(this);
                }
            }
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _isKinematic = true;
            Rigidbody.isKinematic = true;
            _previousPosition = transform.position;
            return;
        }
        else
        {
            _hasPopped = true;
        }
        
    }

    private void OnCollisionStay(Collision other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            _kinematicVelocity = (transform.position - _previousPosition) / Time.fixedDeltaTime;
            //BubbleRb.AddForce(new Vector3(0f, 13f, 0f), ForceMode.Force);
            _previousPosition = transform.position;
        }
        
    }

    private void OnCollisionExit(Collision other)
    {
        _isKinematic = false;
        Rigidbody.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Liftable"))
        {
            if (other.gameObject.TryGetComponent(out Liftable liftable))
            {
                //if (liftable._bubble != null) return;

                if (Vector3.Distance(transform.position, liftable.gameObject.transform.position) > AttachmentOffset)
                {
                    //liftable.transform.position = Vector3.Lerp(liftable.transform.position, transform.position, AttachmentSpeed * Time.deltaTime);
                }
                else
                {
                    _isAttached = true;
                }
                
                if (!_hasPopped && _isAttached)
                {
                    transform.position += Vector3.up * (Time.fixedDeltaTime * RiseSpeed);
                    liftable.gameObject.transform.position = transform.position;
                }
            }
        }
    }

    public void PushBubble(Vector3 destination, float time)
    {
        StartCoroutine(MoveBubble(destination, time));
    }

    private IEnumerator MoveBubble(Vector3 destination, float time)
    {
        while (Vector3.Distance(transform.position, destination) > AttachmentOffset)
        {
            if (_isKinematic)
            {
                Rigidbody.MovePosition(Vector3.Lerp(transform.position, destination, time));
            }
            else
            {
                Rigidbody.Move(Vector3.Lerp(transform.position, destination, time), Quaternion.identity);
            }
            yield return null;
        }
        _hasTraveled = true;
        Rise = true;
        yield break;
    }

    public void BubblePop()
    {
        AudioManager.instance.PlayOneShot(_bubblePop, transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _collider.size);
    }
}
