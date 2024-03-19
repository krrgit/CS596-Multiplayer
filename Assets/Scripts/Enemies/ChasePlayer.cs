using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] float moveLerpSpeed = 0.5f;
    [SerializeField] private float randRadiusOffset = 0.25f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform player; // TODO: change to nearest player every now and then
    
    private Vector3 moveDirLerped;

    private Vector3 randomVector; 
    
    // Start is called before the first frame update
    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        // player = GameObject.Find("PlayerObj").transform;
        randomVector = Vector3.ClampMagnitude(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), randRadiusOffset);
        
        
        // Start off Moving
        if (player)
        {
            Vector3 direction = player.position - transform.position + randomVector;
            direction.y = 0;
            direction = direction.normalized;
            moveDirLerped = direction;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) return;
        Vector3 direction = player.position - transform.position + randomVector;
        direction.y = 0;
        direction = direction.normalized;
        
        moveDirLerped = Vector3.Lerp(moveDirLerped, direction, moveLerpSpeed * Time.deltaTime);
        rb.velocity = moveSpeed * moveDirLerped;
    }

    public void SetTarget(Transform target)
    {
        player = target;
    }
}
