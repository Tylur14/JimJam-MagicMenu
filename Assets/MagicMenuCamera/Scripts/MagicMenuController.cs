using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicMenuController : MonoBehaviour
{
    
    [SerializeField] MagicMenuCamera cam; // core camera that is used to view each interest point
    [SerializeField] List<MagicMenuFlag> flags = new List<MagicMenuFlag>(); // interest points for the camera to target
    [SerializeField] int flagIndex; // current index for target flag
    [SerializeField] bool usingCustomLastIndex; // if true, use customLastIndex as loop / stop index
    [SerializeField] int customLastIndex = -1;  // set to value you want to be your last flag, then spawn your special flags after that one
    [SerializeField] bool doesLoop; // if true, loop back around when shifting flagIndex
    [SerializeField] TargetValues.TargetChangeRequirment targetRequirement;

    private void Start()
    {
        cam.SetCamFlag(flags[0]);
        CheckFlags();
    }

    public void AddFlag(MagicMenuFlag incFlag)
    {
        flags.Add(incFlag);
        CheckFlags();
    }
    void CheckFlags()
    {
        List<MagicMenuFlag> l = new List<MagicMenuFlag>();
        for (int i = 0; i < flags.Count; i++)
        {
            if (flags[i] != null)
                l.Add(flags[i]);
        }
        flags.Clear();
        flags = l;
    }

    public void TryChange(int dir)
    {
        print("trying");
        switch (targetRequirement)
        {
            case TargetValues.TargetChangeRequirment.none:
                ChangeTargetFlag(dir);
                break;
            case TargetValues.TargetChangeRequirment.completeRotationAndMovement:
                if (cam.moveComplete && cam.rotationComplete)
                    ChangeTargetFlag(dir);
                break;
            case TargetValues.TargetChangeRequirment.completeMovement:
                if (cam.moveComplete)
                    ChangeTargetFlag(dir);
                break;
            case TargetValues.TargetChangeRequirment.completeRotation:
                if (cam.rotationComplete)
                    ChangeTargetFlag(dir);
                break;
        }
    }

    void ChangeTargetFlag(int dir)
    {
        int originalIndex = flagIndex;
        flagIndex += dir;
        if(!usingCustomLastIndex)
        {
            if (flagIndex < 0)
                flagIndex = doesLoop ? flags.Count - 1 : 0;

            else if (flagIndex > flags.Count - 1)
                flagIndex = doesLoop ? 0 : flags.Count - 1;
        }
        else if (usingCustomLastIndex)
        {
            if (flagIndex < 0)
                flagIndex = doesLoop ? customLastIndex : 0;

            else if (flagIndex > customLastIndex)
                flagIndex = doesLoop ? 0 : customLastIndex;
        }
        

        for(int i = 0; i < flags.Count; i++)
        {
            if (i == flagIndex)
            {
                StartCoroutine(SetWithDelay());
                IEnumerator SetWithDelay()
                {
                    yield return new WaitForSeconds(flags[originalIndex].moveDelay);
                    cam.SetCamFlag(flags[flagIndex]);
                }
            }
            else flags[i].SleepFlag();
        }
    }

    public void TargetSpecificFlag(int target)
    {
        if (flags[target] != null)
        {
            int originalIndex = flagIndex;
            flagIndex = target;
            for (int i = 0; i < flags.Count; i++)
            {
                if (i == flagIndex)
                {
                    StartCoroutine(SetWithDelay());
                    IEnumerator SetWithDelay()
                    {
                        yield return new WaitForSeconds(flags[originalIndex].moveDelay);
                        cam.SetCamFlag(flags[flagIndex]);
                    }
                }
                else flags[i].SleepFlag();
            }
        }
        else Debug.LogError("MagicMenuCam Error : Requested index does not exist!");
        
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if(PrefabUtility.IsPartOfAnyPrefab(this.gameObject))
            PrefabUtility.UnpackPrefabInstance(this.gameObject,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
        if (cam == null && FindObjectOfType<MagicMenuCamera>() != null)
            cam = FindObjectOfType<MagicMenuCamera>();
        CheckFlags();
#endif
    }
}

// Tack on enum to help with look at types for flags
public static class TargetValues
{
    public enum LookTargetValues { forward, right, behind, left, custom }
    public enum TargetChangeRequirment { none, completeMovement, completeRotation, completeRotationAndMovement }
}
