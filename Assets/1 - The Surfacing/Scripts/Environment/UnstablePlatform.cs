using System;
using UnityEngine;

public class UnstablePlatform : MonoBehaviour
{
    public float BreakDelay = 0.5f;
    public float RespawnTime = 5f;

    private bool _break = false;
    private bool _broken = false;
    private float _timer;
    
    private MeshRenderer _meshRenderer;
    private Collider _collider;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (_break)
        {
            _meshRenderer.material.color = Color.red;
            _timer += Time.deltaTime;
            if (_timer >= BreakDelay)
            {
                _meshRenderer.enabled = false;
                _collider.enabled = false;
                _broken = true;
                _timer = 0;
                _break = false;
            }
        }

        if (_broken)
        {
            _timer += Time.deltaTime;
            if (_timer >= RespawnTime)
            {
                Respawn();
                _timer = 0;
                _broken = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out CharacterMovement3D character))
        {
            _break = true;
        }
    }

    private void Respawn()
    {
        _meshRenderer.material.color = Color.white;
        _meshRenderer.enabled = true;
        _collider.enabled = true;
    }
}