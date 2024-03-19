using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float minDist = 3;
    [SerializeField] float maxDist = 5.83f;
    [SerializeField] private Vector3 offset = new Vector3(0,3,-5);
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float lerpSpeed = 1;
    [SerializeField] private Vector3 idealPosition;

    private void Start()
    {
        idealPosition = transform.localPosition;
    }

    void Update()
    {
        GetIdealPosition();
        AdjustCameraDist();
    }

    void GetIdealPosition()
    {
        RaycastHit hit;
        Vector3 rayDir = transform.position - transform.parent.position;
        bool castHit = Physics.SphereCast(transform.parent.position, 0.2f, rayDir, out hit, maxDist+1f, layerMask);
        if (castHit)
        {
            print("hit: " + hit.transform.name);
            idealPosition = Mathf.Max(hit.distance, minDist) * offset.normalized;
        }
        else
        {
            idealPosition = maxDist * offset.normalized;
        }
    }

    void AdjustCameraDist()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, idealPosition, Time.deltaTime * lerpSpeed);
    }
    


}
