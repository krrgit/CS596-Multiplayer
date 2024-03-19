using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShapeShift : MonoBehaviour
{
    [SerializeField] private GameObject[] shapes;

    [SerializeField] private int currentShape = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchShape(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchShape(1);
        }
    }

    void SwitchShape(int shape)
    {
        shapes[currentShape].SetActive(false);
        shapes[shape].SetActive(true);
        currentShape = shape;
    }
}
