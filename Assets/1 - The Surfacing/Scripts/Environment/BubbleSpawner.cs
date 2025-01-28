using System;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    
    [field: SerializeField] private float SpawnInterval { get; set; }
    
    [field: SerializeField] private float SpawnDelay { get; set; }

    [Header("Bubble Spawn Settings")]
    [field: SerializeField] private float BubbleLifespan { get; set; } = 10;

    [Header("Bubble Spawn Settings")]
    [field: SerializeField] private float BubbleRiseSpeed { get; set; } = 0.7f;
    
    [Header("Bubble Spawn Settings")]
    [field: SerializeField] private float BubbleScaleMultiplier { get; set; } = 1f;

    private float _timer;
    private bool _canSpawn;

    private void Awake()
    {
        _timer = 0;
        _canSpawn = false;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (!_canSpawn)
        {
            if (_timer >= SpawnDelay)
            {
                _canSpawn = true;
            }
        }

        if (_timer >= SpawnInterval && _canSpawn)
        {
            GameObject bubbleObject = Instantiate(bubblePrefab, transform.position, Quaternion.identity);

            if (bubbleObject != null)
            {
                if (bubbleObject.TryGetComponent(out Bubble bubble))
                {
                    bubble.gameObject.transform.localScale *= BubbleScaleMultiplier;
                    bubble.Lifespan = BubbleLifespan;
                    bubble.RiseSpeed = BubbleRiseSpeed;
                    bubble.Rise = true;
                }
            }
            _timer = 0;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
