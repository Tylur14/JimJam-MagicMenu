using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagicMenuFlag : MonoBehaviour
{
    [Header("View Settings")]
    [SerializeField] TargetValues.LookTargetValues lookTarget;
    [Range(0.0f, 10.0f)] public float lookSpeed = 2f;  // How quickly the camera rotates to look in this flag's set facing direction
    [Range(0.0f, 3.0f)] public float moveDelay = 0.0f; // Simple delay from moving from flag to flag
    public Vector3 customLookDirection;

    #region Public System Refs
    [HideInInspector] public GameObject parentCam;
    [HideInInspector] public Vector3 lookAt;
    [HideInInspector] public bool trackingTarget;
    #endregion

    [SerializeField] bool useCamPreview;
    [HideInInspector] [SerializeField] List<MagicMenu_RenderFader> objs = new List<MagicMenu_RenderFader>();
    public GameObject textPrefab;

    private void Awake()
    {
        if (GetComponentInChildren<Camera>() != null)
            Destroy(GetComponentInChildren<Camera>().gameObject);
        GetLookDirection();
        CheckObjects();
    }
    #region Flag Set Status
    public void InitFlag()
    {
        if (objs != null)
            for (int i = 0; i < objs.Count; i++)
                objs[i].InitObj();
    }

    public void SleepFlag()
    {
        if (objs != null)
            for (int i = 0; i < objs.Count; i++)
                objs[i].Sleep();
    }
    #endregion
    #region Child Objects
    public void SpawnObject()
    {
        if (textPrefab)
        {
            var obj = Instantiate(textPrefab,this.transform);
            obj.transform.position = transform.position;
            AddObject(obj.GetComponent<MagicMenu_RenderFader>());
#if UNITY_EDITOR
            UnityEditor.Selection.activeObject = obj;
#endif
        }
        else Debug.LogError("Missing text object prefab!");
    }

    void AddObject(MagicMenu_RenderFader incObj)
    {
        objs.Add(incObj);
        CheckObjects();
    }

    void CheckObjects()
    {
        List<MagicMenu_RenderFader> l = new List<MagicMenu_RenderFader>();
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i] != null)
                l.Add(objs[i]);
        }
        objs.Clear();
        objs = l;
    }
    #endregion

    void GetLookDirection()
    {
        switch (lookTarget)
        {
            case TargetValues.LookTargetValues.forward:
                lookAt = Vector3.zero;
                break;
            case TargetValues.LookTargetValues.behind:
                lookAt = new Vector3(0, 180, 0);
                break;
            case TargetValues.LookTargetValues.left:
                lookAt = new Vector3(0, -90, 0);
                break;
            case TargetValues.LookTargetValues.right:
                lookAt = new Vector3(0, 90, 0);
                break;
            case TargetValues.LookTargetValues.custom:
                lookAt = customLookDirection;
                break;
        }
    }

    public void SetLook(TargetValues.LookTargetValues incValue)
    {
        lookTarget = incValue;
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (!useCamPreview)
        {
            GetLookDirection();
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(lookAt), Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, Camera.main.fieldOfView, 20f,
                Camera.main.nearClipPlane, Camera.main.aspect);
            if (GetComponentInChildren<Camera>() != null)
                DestroyImmediate(GetComponentInChildren<Camera>().gameObject);
        }
        else if (useCamPreview)
        {
            GetLookDirection();
            if (GetComponentInChildren<Camera>() != null)
                GetComponentInChildren<Camera>().transform.rotation = Quaternion.Euler(lookAt);
            else
            {
                GameObject newCam = new GameObject();
                newCam.AddComponent<Camera>();
                newCam.transform.parent = this.transform;
                newCam.transform.position = this.transform.position;
                newCam.transform.rotation = Quaternion.Euler(lookAt);
                newCam.name = "Preview Cam";
                UnityEditor.Selection.activeObject = newCam;
            }

        }
#endif
    }
}
