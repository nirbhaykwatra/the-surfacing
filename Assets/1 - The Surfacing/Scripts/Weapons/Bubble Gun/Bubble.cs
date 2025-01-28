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
    
    public Rigidbody BubbleRb;
    public BoxCollider _collider;
    private bool _hasPopped;
    private bool _isAttached;
    private bool _hasTraveled;
    private float _lifeSpanTimer;
    private BubbleGun _bubbleGun;
    
    public UnityEvent OnDestroyed;

    private void OnEnable()
    {
        BubbleRb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _bubbleGun = FindAnyObjectByType<BubbleGun>();
        Rise = false;
        InCurrent = false;
        _hasPopped = false;
        _isAttached = false;
        _hasTraveled = false;
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (!InCurrent)
        {
            if (!_hasPopped && Rise) transform.position += Vector3.up * (Time.fixedDeltaTime * RiseSpeed);
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
            return;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Liftable"))
        {
            _collider.isTrigger = true;
        }
        else
        {
            _hasPopped = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Liftable"))
        {
            if (other.gameObject.TryGetComponent(out Liftable liftable))
            {
                //if (liftable._bubble != null) return;
                StopCoroutine(MoveBubble(liftable.gameObject.transform.position, 0.1f));
                liftable._bubble = this;
                liftable.LiftableRb.isKinematic = true;
                liftable.LiftableRb.useGravity = false;
                transform.localScale = liftable._boxCollider.size * 2;
                liftable.transform.position = transform.position;
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            return;
        }
        else
        {
            _hasPopped = true;
        }
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
            transform.position = Vector3.Lerp(transform.position, destination, time);
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
