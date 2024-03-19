using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceDirection : MonoBehaviour
{
    [SerializeField] private Transform model;
    [SerializeField] private Transform dummyModel;

    private Vector3 prevPos;
    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
    }

    public void Init(Transform _model, Transform _dummyModel)
    {
        model = _model;
        dummyModel = _dummyModel;
    }

    // Update is called once per frame
    void Update()
    {
        if (!model || !dummyModel) return;
        if (prevPos == transform.position) return;
        
        
        Vector3 forward = transform.position - prevPos;
        forward.Normalize();
        dummyModel.forward = forward;
        model.localRotation = dummyModel.localRotation;
        prevPos = transform.position;
        print("Update Player Rotation");
    }
}
