using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    [SerializeField] private BossMove bossMove;
    // Start is called before the first frame update
    void Start()
    {
        bossMove.StartMove();
    }
}
