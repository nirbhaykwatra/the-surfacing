using System;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class BubbleGun : MonoBehaviour
{
    [SerializeField] private GameObject _bubble;
    
    [Header("Spawn Settings")]
    [field: SerializeField] public float SpawnDistance { get; set; }
    [field: SerializeField] public float OffsetDistance { get; set; }
    
    [field: SerializeField] public float Cooldown { get; set; } = 1f;
    [field: SerializeField] public int MaxBubbleInstances { get; set; } = 5;
    
    [Tooltip("If false, bubbles cannot be created until all player-created bubbles have popped.")]
    [field: SerializeField] public bool ConsecutiveBubbleSpawning { get; set; }
    
    [Header("Attributes")]
    [field: SerializeField] public float Lifespan { get; set; }
    [field: SerializeField] public float Buoyancy { get; set; }
    [field: SerializeField] public float BubbleScaleMultiplier { get; set; }

    [Header("Audio")]
    [field: SerializeField] public EventReference _gunShoot;
    
    [HideInInspector] public float CooldownTimer { get => _shootTimer; }
    [HideInInspector] public int BubbleCount = 0;
    
    [HideInInspector] public List<Bubble> _bubbles;
    public BubbleSettings _settings;
    
    private bool _canShoot;
    private float _shootTimer = 0;
    
    private CharacterMovement3D _character;
    
    private void Awake()
    {
        _bubbles = new List<Bubble>();
        _character = GetComponent<CharacterMovement3D>();
        
        if (Lifespan == 0) Lifespan = _settings.Lifespan;
        if (BubbleScaleMultiplier == 0) BubbleScaleMultiplier = _settings.BubbleScaleMultiplier;
        if (Buoyancy == 0) Buoyancy = _settings.Buoyancy;
        
        _canShoot = true;
        _shootTimer = 0;
        BubbleCount = MaxBubbleInstances;
    }

    private void Update()
    {
        if (ConsecutiveBubbleSpawning)
        {
            if (_bubbles.Count < MaxBubbleInstances)
            {
                _canShoot = true;
            }
            else
            {
                _canShoot = false;
            }
        }
        else
        {
            if (BubbleCount <= MaxBubbleInstances && BubbleCount > 0)
            {
                _canShoot = true;
            }
            else
            {
                _canShoot = false;
                _shootTimer += Time.deltaTime;

                if (_shootTimer >= Cooldown)
                {
                    ResetBubbleCooldown();
                }
            }
        }
    }
    
    public void TryCreateBubble()
    {
        // This condition has been applied purely so that UI can use BubbleCount directly.
        // Without this condition, UI will show BubbleCount in negative if the user keeps pressing the
        // Create Bubble button past zero.
        if (BubbleCount > 0)
        {
            BubbleCount -= 1;
        }
        if (!_canShoot) return;
        SpawnBubble();
    }

    private void ResetBubbleCooldown()
    {
        _canShoot = true;
        _shootTimer = 0;
        BubbleCount = MaxBubbleInstances;
    }

    private void SpawnBubble()
    {
        Vector3 spawnPosition = (transform.forward * SpawnDistance) + (transform.position + Vector3.up);
        Vector3 destination = (transform.forward * (SpawnDistance + OffsetDistance)) + (transform.position + Vector3.up);

        AudioManager.instance.PlayOneShot(_gunShoot, transform.position);
        
        GameObject bubble = Instantiate(_bubble, spawnPosition, Quaternion.identity);

        if (bubble.TryGetComponent(out Bubble bubbleComponent))
        {
            bubbleComponent.BubbleScaleMultiplier = BubbleScaleMultiplier;
            bubbleComponent.Lifespan = Lifespan;
            bubbleComponent.Buoyancy = Buoyancy;
            _bubbles.Add(bubbleComponent);
            
            bubbleComponent.PushBubble(destination, Time.deltaTime * _settings.TimeToOffset);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Vector3 spawnPosition = (transform.forward * SpawnDistance) + (transform.position + Vector3.up);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(spawnPosition, (transform.forward * (SpawnDistance + OffsetDistance)) + (transform.position + Vector3.up));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPosition,0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.forward * (SpawnDistance + OffsetDistance)) + (transform.position + Vector3.up), 0.3f);
    }
}
