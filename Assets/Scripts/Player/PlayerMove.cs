using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] private float maxVelocity;
    [SerializeField] private float grFriction = 0.25f;
    [SerializeField] float grAccel = 0.01f;
    [SerializeField] float moveLerpSpeed = 0.5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] Camera thisCamera;
    [SerializeField] private Transform model;
    [SerializeField] private Transform dummyModel;
    [SerializeField] private Animator animator;
    
    private float speed = 0;
    private Vector3 moveDirLerped;

    public Vector3 FaceDirection
    {
        get { return moveDirLerped.normalized; }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            thisCamera.enabled = false;
            thisCamera.GetComponent<AudioListener>().enabled = false;
            
            // gameObject.AddComponent<PlayerFaceDirection>().Init(model, dummyModel);
            return;
        }
    }

    private void Awake()
    {
        transform.position = PlayerSpawnPointManager.Instance.GetSpawnPoint();
        thisCamera.enabled = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        int inputX = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        int inputY = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);

        // Stop Moving
        if (Mathf.Abs(inputX) + Mathf.Abs(inputY) == 0)
        {
            speed = Mathf.Clamp(speed - (grFriction * Time.deltaTime), 0, maxVelocity);
            rb.velocity = speed * rb.velocity.normalized;
            
            RequestMoveChangeServerRpc(false);
        }
        else
        {
            // Move
            speed = Mathf.Clamp(speed + (grAccel * Time.deltaTime), 0, maxVelocity);

            Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
            moveDirLerped = Vector3.Lerp(moveDirLerped, moveDir, moveLerpSpeed * Time.deltaTime);
            
            rb.velocity = speed * moveDirLerped;

            RequestMoveChangeServerRpc(true);
        }

        if (model)
        {
            dummyModel.forward = moveDirLerped.magnitude > 0 ? moveDirLerped : dummyModel.forward;
            RequestRotateServerRpc(dummyModel.localRotation);
        }
    }
    
    [ServerRpc]
    void RequestMoveChangeServerRpc(bool state)
    {
        MoveChangeClientRpc(state);
    }

    [ClientRpc]
    void MoveChangeClientRpc(bool state)
    {
        animator.SetBool("IsMoving", state);
    }

    [ServerRpc]
    void RequestRotateServerRpc(Quaternion rotation)
    {
        RotateClientRpc(rotation);
    }

    [ClientRpc]
    void RotateClientRpc(Quaternion rotation)
    {
        Rotate(rotation);
    }

    void Rotate(Quaternion rotation)
    {
        model.localRotation = rotation;
    }

}
