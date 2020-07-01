using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Core controller for the Magic Menu system
/// </summary>
public class MagicMenuController : MonoBehaviour
{
    [Header("Controller Status")]
    public MagicMenuCamera cam;                 // core camera that is used to view each interest point
    [SerializeField] List<MagicMenuFlag> flags = new List<MagicMenuFlag>(); // interest points for the camera to target
    [SerializeField] int flagIndex;             // current index for target flag
    
    [Header("Settings")]
    [SerializeField] bool usingCustomLastIndex; // if true, use customLastIndex as loop / stop index
    [SerializeField] int customLastIndex = -1;  // set to value you want to be your last flag, then spawn your special flags after that one
    public bool doesLoop;                       // if true, loop back around when shifting flagIndex
    [SerializeField] TargetValues.TargetChangeRequirment targetRequirement;

    private void Start()
    {
        // Init controller & camera
        cam.SetCamFlag(flags[0]);
        CheckFlags();
    }

    #region Flag Functions
    
    public void AddFlag(MagicMenuFlag incFlag)
    {
        flags.Add(incFlag); // Add the flag to the list
        CheckFlags();       // Update flags list
    }
    void CheckFlags()
    {
        List<MagicMenuFlag> l = new List<MagicMenuFlag>(); // Create buffer list
        for (int i = 0; i < flags.Count; i++)              // go through buffer list
        {
            if (flags[i] != null)    // if original flag is still valid
                l.Add(flags[i]);     // add it to the new list
        }

        flags.Clear();    // Clear original list
        flags = l;        // set list to new buffer list
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
    
    #endregion

    #region Helper Checks
    
    // Passer function when you need to check cam status BEFORE changing target flag 
    public void TryChange(int dir)
    {
        if(CheckIfRequirementsMet())
            ChangeTargetFlag(dir);
    }
    
    // Simple checker used to see if the incoming value is a valid flag, used currently for the navigator demo
    public bool CheckIfValidChangeDir(int dir)
    {
        int i = flagIndex; // new variable with flagIndex value
        i += dir;          // increment by dir, typically either 1 or -1
        
        if (i < flags.Count && i >= 0) // if it's a valid value within range
            return true;               // then return true
        else return false;             // otherwise it's not valid and return false
    }

    // Go through the camera checks and see what its status is
    public bool CheckIfRequirementsMet()
    {
        // Hnnn, check please!
        switch (targetRequirement)
        {
            // No checks required!
            case TargetValues.TargetChangeRequirment.none:
                return true;
            
            // Check for rotation and movement
            case TargetValues.TargetChangeRequirment.completeRotationAndMovement:
                if (cam.moveComplete && cam.rotationComplete)
                    return true;
                else return false;
            
            // Check for just movement
            case TargetValues.TargetChangeRequirment.completeMovement:
                if (cam.moveComplete)
                    return true;
                else return false;
            
            // Check for just rotation
            case TargetValues.TargetChangeRequirment.completeRotation:
                if (cam.rotationComplete)
                    return true;
                else return false;
        }
        return false;
    }
    
    #endregion

    // In editor helper function
    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        // This part is because there are some negative effects trying to use this system while the controller is a prefab...
        if(PrefabUtility.IsPartOfAnyPrefab(this.gameObject)) // if this object is part of a prefab
            PrefabUtility.UnpackPrefabInstance(this.gameObject,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction); // then unpack it!
        
        // If this controller doesn't have a reference to the Magic Menu Camera, we need to get one automatically!
        if (cam == null && FindObjectOfType<MagicMenuCamera>() != null) // if reference to camera is null
            cam = FindObjectOfType<MagicMenuCamera>();                  // then find the camera and set it to cam
        
        CheckFlags(); // Simple check so the user can delete flags without having to manually adjust anything in this controller
#endif
    }
}

// Tack on enum to help with look at types for flags
public static class TargetValues
{
    public enum LookTargetValues { forward, right, behind, left, custom }
    public enum TargetChangeRequirment { none, completeMovement, completeRotation, completeRotationAndMovement }
}
