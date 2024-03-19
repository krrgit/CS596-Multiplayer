using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private TextMeshProUGUI heartsText;

    private void OnEnable()
    {
        PlayerHealth.playerTakeDamageDelegate += UpdateUI;
    }
    private void OnDisable()
    {
        PlayerHealth.playerTakeDamageDelegate -= UpdateUI;
    }

    private void UpdateUI(int health)
    {
        heartsText.text = health.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
