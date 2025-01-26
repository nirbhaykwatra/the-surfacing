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

    [Header("Audio")]
    [field: SerializeField] public EventReference _gunShoot;
    
    public List<Bubble> _bubbles;
    
    private void Awake()
    {
        _bubbles = new List<Bubble>();
    }
    
    private void Update()
    {
        Debug.Log($"Bubble instances: {_bubbles.Count}");
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
        if (_bubbles.Count >= MaxBubbleInstances) return;
        Vector3 spawnPosition = (transform.forward * BubbleSpawnDistance) + (transform.position + Vector3.up);
        Vector3 destination = (transform.forward * (BubbleSpawnDistance + BubbleTravelDistance)) + (transform.position + Vector3.up);

        AudioManager.instance.PlayOneShot(_gunShoot, transform.position);
        
        GameObject bubble = Instantiate(_bubble, spawnPosition, Quaternion.identity);

        if (bubble.TryGetComponent(out Bubble bubbleComponent))
        {
            _bubbles.Add(bubbleComponent);
            bubbleComponent.PushBubble(destination, Time.deltaTime * BubbleTravelTime);
        }
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
