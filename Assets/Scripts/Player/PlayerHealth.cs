using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public int maxHealth = 3;
    [SerializeField] private short invincibilityDur = 1;
    [SerializeField] private bool isInvincible = false;

    [SerializeField] private NetworkVariable<int> health = new(3, writePerm: NetworkVariableWritePermission.Owner);
    
    public delegate void PlayerTakeDamage(int health);
    public static PlayerTakeDamage playerTakeDamageDelegate;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        health.Value = maxHealth;
    }

    public void RestoreMaxHealth()
    {
        if (!IsOwner) return;
        if (health.Value == maxHealth) return;
        
        health.Value = maxHealth;
        playerTakeDamageDelegate?.Invoke(health.Value);
        SoundManager.Instance.PlayClip("powerup03");
        print("Restored Max Health");
    }

    public void DealDamage(int damage)
    {
        if (!IsOwner || health.Value <= 0 || isInvincible) return;

        health.Value -= damage > health.Value ? health.Value : damage;
        playerTakeDamageDelegate?.Invoke(health.Value);
        StartCoroutine(IInvincibility());
        print("Damage Dealt. Health: " + health);
        if (health.Value <= 0)
        {
            // Destroy(gameObject);
            DespawnPlayerServerRpc();
        }
    }

    [ServerRpc]
    void DespawnPlayerServerRpc()
    {
        NetworkObject.Despawn(false);
        transform.parent.gameObject.SetActive(false);
    }

    IEnumerator IInvincibility ()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1);
        isInvincible = false;
    }
}
