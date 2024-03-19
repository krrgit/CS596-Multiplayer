using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float lifetime = 3;
    [SerializeField] private float arcHeight = 1;
    [SerializeField] private float throwForce = 5;
    [SerializeField] private Rigidbody rb;

    public void ThrowBomb(Vector3 forward)
    {
        transform.forward = forward + (arcHeight * Vector3.up);
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }

    private void Start()
    {
        if (rb) rb = GetComponent<Rigidbody>();
        StartCoroutine(BombTick());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        Explode();
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator BombTick()
    {
        yield return new WaitForSeconds(lifetime);
        if (gameObject) Explode();
    }
}
