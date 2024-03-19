using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private Collider[] segments;
    [SerializeField] private int hittableSegment;
    [SerializeField] private Material mat_hittable;

    [SerializeField] private int speedIncrease = 1;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BossMove bossMove;

    private void OnEnable()
    {

        enemyHealth.enemyTakeDamageDelegate += LastSegmentHit;
    }

    private void OnDisable()
    {
        enemyHealth.enemyTakeDamageDelegate -= LastSegmentHit;
    }

    // Start is called before the first frame update
    void Start()
    {
        hittableSegment = segments.Length - 1;
        // for (int i = 0; i < segments.Length; i++)
        // {
        //     segments[i].enabled = i == hittableSegment;
        // }
    }

    IEnumerator SetLastSegmentToHittable()
    {
        segments[hittableSegment].GetComponent<MeshRenderer>().material = mat_hittable;
        yield return new WaitForSeconds(0.75f);
        segments[hittableSegment].enabled = true;
        segments[hittableSegment].gameObject.tag = "Enemy";
    }

    public void LastSegmentHit()
    {
        // Destroy last segment
        segments[hittableSegment].gameObject.SetActive(false);
        // Check if more segments exist
        // If yes, set it. If no, then end boss fight.
        if (hittableSegment > 0)
        {
            hittableSegment--;
            StartCoroutine(SetLastSegmentToHittable());
            bossMove.IncreaseSpeed(speedIncrease);
        }
        else
        {
            // End Game
            print("Boss Defeated");
        }
    }
    
}