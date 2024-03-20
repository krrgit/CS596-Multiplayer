using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float lerpSpeed = 1;
    
    // Update is called once per frame
    void Update()
    {
        if (!player) return;
        Vector3 newPos = player.position;
        // newPos.x = transform.position.x;
        // newPos.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, newPos, lerpSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        player = target;
    }
}
