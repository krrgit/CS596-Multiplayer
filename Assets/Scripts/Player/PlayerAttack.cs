using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private PlayerMove playerMove;
    [Header("Sword")]
    [SerializeField] private bool canSword;
    [SerializeField] private float swordAttackTimer = 1.5f;
    [SerializeField] private GameObject swordHitbox;
    [Header("Bow")]
    [SerializeField] private int arrowCount;
    [SerializeField] private int minBowTargets = 1;
    [SerializeField] private int maxBowTargets = 3;
    [SerializeField] private float maxBowChargeTime = 2; // Time needed to max charge the bow 
    [SerializeField] private float arrowCooldown = 0.5f; // Time before able to attack again
    [SerializeField] private float arrowSpeed = 7;
    [SerializeField] private float arrowShortStartup = 0.1f; // Time before arrow shoot with no charge
    [SerializeField] private GameObject arrowPrefab;
    [Header("Bomb")]
    [SerializeField] private int bombCount;

    [SerializeField] private float bombLifetime = 3;
    [SerializeField] private Transform itemParent;
    [SerializeField] private GameObject bombPrefab;
    

    private bool attackActive;
    
    public override void OnNetworkSpawn()
    {
        // if (!IsOwner) enabled = false;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (swordHitbox) swordHitbox.SetActive(false);

        playerMove = GetComponent<PlayerMove>();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (!IsOwner) return;
        if (attackActive) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!swordHitbox) return;
            RequestSwingServerRpc();
            // StartCoroutine(ISwingSword());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(IChargeBow());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(IHoldBomb());
        }
    }

    IEnumerator IChargeBow()
    {
        attackActive = true;
        float chargeTimer = 0;

        // Charge Bow
        while (Input.GetKey(KeyCode.Q))
        {
            chargeTimer = Mathf.Clamp(chargeTimer + Time.deltaTime, 0, maxBowChargeTime);
            yield return new WaitForEndOfFrame();
        }

        if (chargeTimer < 0.5f)
        {
            yield return new WaitForSeconds(arrowShortStartup);
        }

        // Spawn Arrow
        Vector3 spawnPos = transform.position + playerMove.FaceDirection;
        spawnPos.y = 0.5f;
        var go = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);
        go.transform.forward = playerMove.FaceDirection;
        var arrow = go.AddComponent<Arrow>();
        arrow.Init(minBowTargets +
                             (Mathf.FloorToInt((chargeTimer / maxBowChargeTime) * (maxBowTargets - minBowTargets))), arrowSpeed);

        // Cooldown before able to attack
        yield return new WaitForSeconds(arrowCooldown);
        attackActive = false;
    }

    IEnumerator ISwingSword()
    {
        attackActive = true;
        swordHitbox.SetActive(true);
        yield return new WaitForSeconds(swordAttackTimer);
        attackActive = false;
        swordHitbox.SetActive(false);
    }

    IEnumerator IHoldBomb()
    {
        attackActive = true;
        
        // Spawn Bomb
        var go = Instantiate(bombPrefab, itemParent.position, quaternion.identity);
        go.transform.parent = itemParent;
        yield return new WaitForSeconds(0.2f);

        float timer = bombLifetime;
        
        // Wait for repress to throw
        while (!Input.GetKey(KeyCode.R))
        {
            if (itemParent.childCount <= 0) break;
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }  
        
        print("no more bomb");

        if (itemParent.childCount <= 0)
        {
            attackActive = false;
            yield return null;
        }

        // Throw
        if (go)
        {
            go.transform.parent = null;
            var bomb = go.GetComponent<Bomb>();
            bomb.ThrowBomb(playerMove.FaceDirection);
        }
        
        attackActive = false;
    }

    // Sword Swing
    [ServerRpc]
    void RequestSwingServerRpc()
    {
        SwingClientRpc();
    }
    [ClientRpc]
    void SwingClientRpc()
    {
        StartCoroutine(ISwingSword());
    }
    
    // Arrow
    
    // Bomb
}
