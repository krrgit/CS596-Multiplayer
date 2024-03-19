using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossMove : NetworkBehaviour
{
    [SerializeField] private bool allowMove = false;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float randomness = 0.1f;
    [SerializeField] private Vector3 direction;

    public void StartMove()
    {
        allowMove = true;
    }

    void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        transform.forward = direction;
    }

    private void Update()
    {
        if (!allowMove) return;
        if (!IsOwner) return;
         transform.position += moveSpeed * Time.deltaTime * direction;
    }

    public void IncreaseSpeed(int increase)
    {
        moveSpeed += increase;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("BossWall")) return;
        
        print("Collide with Wall");
        Vector3 forward = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
        forward.y = 0;
        // forward.x = (forward.x < 0.1f) ? forward.z * Mathf.Sign(forward.x) : forward.x;
        // forward.z = (forward.z < 0.1f) ? forward.x * Mathf.Sign(forward.z) : forward.z;
        forward += new Vector3(Random.Range(0, 1f) * Mathf.Sign(forward.x), 0, Random.Range(0, 1f) * Mathf.Sign(forward.z)) * randomness;
        
        forward.Normalize();
        direction = forward;
        transform.forward = forward;
    }
}
