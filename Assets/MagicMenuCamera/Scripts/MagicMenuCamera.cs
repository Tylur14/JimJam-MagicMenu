using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMenuCamera : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] float moveSpeed;                   // Camera move speed, HIGHER value is slower with smooth damp!
    [SerializeField] float moveThreshold = 0.05f;       // The distance allowance between the camera position and target flag position
    [SerializeField] float rotationThreshold = 0.05f;   // The angle allowance between the camera rotation and target flag rotation
    [SerializeField] private bool experimental_startOnFlag = false;

    [Header("Target Information")]
    [SerializeField] MagicMenuFlag targetFlag; // Current target
    

    [SerializeField] Vector3 currentTargetPos;   // Where the camera is currently trying to get to
    Vector3 vel = Vector3.zero; // Smooth damp required default value

    /*[HideInInspector]*/ public bool moveComplete;     // Used to determine if the camera position is within the threshold of the target flag position
    /*[HideInInspector]*/ public bool rotationComplete; // Used to determine if the camera rotation is within the threshold of the target flag rotation

    private void Start()
    {
        if(experimental_startOnFlag)
            StartCoroutine(SetInitPos());
        IEnumerator SetInitPos()
        {
            yield return new WaitForSecondsRealtime(0.05f);
            transform.position = targetFlag.transform.position;    
        }
        
    }

    private void Update()
    {
        CamMove();
        CamLook();
    }

    void CamMove()
    {
        // This was figured out thanks to - https://forum.unity.com/threads/checking-if-rotation-is-complete.515058/ ! Thanks McDev02
        if ((transform.position - currentTargetPos).magnitude <= moveThreshold)
            moveComplete = true;

        transform.position = Vector3.SmoothDamp(transform.position, currentTargetPos, ref vel, moveSpeed * Time.deltaTime);
    }

    void CamLook()
    {
        if (targetFlag != null)
        {
            // This was figured out thanks to - https://forum.unity.com/threads/checking-if-rotation-is-complete.515058/ ! Thanks McDev02
            if ((Quaternion.Angle(transform.rotation, Quaternion.Euler(targetFlag.lookAt)) <= rotationThreshold))
                rotationComplete = true;

            var rot = transform.rotation;
            rot = Quaternion.Lerp(rot, Quaternion.Euler(targetFlag.lookAt), targetFlag.lookSpeed * Time.deltaTime);
            transform.rotation = rot;
        }
    }
    public void SetCamFlag(MagicMenuFlag newFlag)
    {
        if (newFlag != targetFlag)
        {
            moveComplete = false;
            rotationComplete = false;

            targetFlag = newFlag;
            if (targetFlag.parentCam != this.gameObject)
                targetFlag.parentCam = this.gameObject;

            targetFlag.InitFlag();
            currentTargetPos = targetFlag.gameObject.transform.position;
        }
            
    }
}
