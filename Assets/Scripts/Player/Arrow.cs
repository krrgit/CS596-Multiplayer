using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int collisionLeft = 1;
    [SerializeField] private float speed = 3;
    [SerializeField] private float lifetime = 3;
    [SerializeField] private DealEnemyDamage dealEnemyDamage;


    private void OnEnable()
    {
        if (dealEnemyDamage) dealEnemyDamage.hitDelegate += CollisionTick;
    }
    
    private void OnDisable()
    {
        if (dealEnemyDamage) dealEnemyDamage.hitDelegate -= CollisionTick;
    }

    public void Init(int _limit, float _speed)
    {
        collisionLeft = _limit;
        speed = _speed;
        StartCoroutine(LifeTimeTimer());
    }

    IEnumerator LifeTimeTimer()
    {
        yield return new WaitForSeconds(lifetime);
        if (gameObject) Destroy(gameObject);
    }

    void CollisionTick()
    {
        collisionLeft--;

        if (collisionLeft <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }


}
