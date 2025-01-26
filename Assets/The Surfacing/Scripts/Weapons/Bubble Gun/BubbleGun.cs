using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGun : MonoBehaviour
{
    [SerializeField] private GameObject _bubble;
    
    [field: SerializeField] public float BubbleSpawnDistance { get; set; }
    [field: SerializeField] public float BubbleTravelDistance { get; set; }
    [field: SerializeField] public float BubbleTravelTime { get; set; }
    [field: SerializeField] public int MaxBubbleInstances { get; set; } = 5;
    
    [Tooltip("If false, bubbles cannot be created until all player-created bubbles have popped.")]
    [field: SerializeField] public bool ConsecutiveBubbleSpawning { get; set; }
    
    [field: SerializeField] private float BubbleSpawnInterval { get; set; } = 1f;
    
    
    [Header("Bubble Spawn Settings")]
    [field: SerializeField] private float BubbleLifespan { get; set; } = 3;

    [Header("Bubble Spawn Settings")]
    [field: SerializeField] private float BubbleRiseSpeed { get; set; } = 0.7f;
    
    [Header("Bubble Spawn Settings")]
    [field: SerializeField] private float BubbleScaleMultiplier { get; set; } = 1f;

    [Header("Audio")]
    [field: SerializeField] public EventReference _gunShoot;
    
    public List<Bubble> _bubbles;
    private bool _canShoot;
    private float _shootTimer;
    
    private void Awake()
    {
        _bubbles = new List<Bubble>();
        _canShoot = true;
        _shootTimer = 0;
    }
    
    private void Update()
    {
        if (_bubbles.Count - 1 >= MaxBubbleInstances)
        {
            if (_bubbles[0] != null)
            {
                Destroy(_bubbles[0].gameObject);
                _bubbles.RemoveAt(0);
            }
        }
    }

    public void CreateBubble()
    {
        if (ConsecutiveBubbleSpawning)
        {
            if (_bubbles.Count >= MaxBubbleInstances) return;
        }
        else
        {
            if (_bubbles.Count >= MaxBubbleInstances) StartCoroutine(WaitForBubbles());
        }
        if (!_canShoot) return;
        
        Vector3 spawnPosition = (transform.forward * BubbleSpawnDistance) + (transform.position + Vector3.up);
        Vector3 destination = (transform.forward * (BubbleSpawnDistance + BubbleTravelDistance)) + (transform.position + Vector3.up);

        AudioManager.instance.PlayOneShot(_gunShoot, transform.position);
        
        GameObject bubble = Instantiate(_bubble, spawnPosition, Quaternion.identity);

        if (bubble.TryGetComponent(out Bubble bubbleComponent))
        {
            bubbleComponent.gameObject.transform.localScale *= BubbleScaleMultiplier;
            bubbleComponent.Lifespan = BubbleLifespan;
            bubbleComponent.RiseSpeed = BubbleRiseSpeed;
            _bubbles.Add(bubbleComponent);
            bubbleComponent.PushBubble(destination, BubbleTravelTime);
        }
    }

    private IEnumerator WaitForBubbles()
    {
        _canShoot = false;
        yield return new WaitForSeconds(BubbleSpawnInterval);
        _canShoot = true;
    }
    
    private void OnDrawGizmosSelected()
    {
        Vector3 spawnPosition = (transform.forward * BubbleSpawnDistance) + (transform.position + Vector3.up);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPosition,0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.forward * (BubbleSpawnDistance + BubbleTravelDistance)) + (transform.position + Vector3.up), 0.3f);
    }
}
