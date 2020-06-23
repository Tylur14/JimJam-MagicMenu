using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMenu_CameraMarker : MonoBehaviour
{
    [Header("View Settings")]
    [SerializeField] TargetValues.LookTargetValues lookTarget;
    [SerializeField] Vector3 customLookDirection;   // When look target is set to custom this is the initial passthrough value sent to the flag ...
                                                    // |-> ... it can be changed on the flag at any time after spawning

    [Header("Spawn Settings")]
    [SerializeField] GameObject flagPrefab;         // The flag prefab, generally there isn't any reason to change this prefab unless you make another flag prefab
    [SerializeField] GameObject spawnPosition;      // Child transform on the camera marker that will be the world position the flags spawns in at
    [SerializeField] GameObject flagObjSpawnPrefab; // The flag object prefab, this can be often swapped out with whatever you want to spawn frequently ON the flag
    [SerializeField] string flagName;               // When a flag is spawned, it can be named with this variable
    Vector3 lookAt;

    private void Awake()
    {
        Destroy(this.gameObject); // This only gets called at runtime so shouldn't affect anything while editing, but it's to make sure it's not ...
                                  // |-> in the scene during normal play. You can also manually delete it before making builds
    }

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
        transform.rotation = Quaternion.Euler(lookAt);
    }

    public void SpawnFlag()
    {
        if (flagPrefab)
        {
            var obj = Instantiate(flagPrefab);

            obj.transform.position = spawnPosition.transform.position;
            obj.GetComponent<MagicMenuFlag>().SetLook(lookTarget);
            obj.GetComponent<MagicMenuFlag>().textPrefab = flagObjSpawnPrefab;
            if (lookTarget == TargetValues.LookTargetValues.custom)
                obj.GetComponent<MagicMenuFlag>().customLookDirection = this.customLookDirection;
                    
            if(flagName != string.Empty && flagName != "")
                obj.name = "Flag - " + flagName;
            else obj.name = "Flag";
            
            FindObjectOfType<MagicMenuController>().AddFlag(obj.GetComponent<MagicMenuFlag>());
        }
        else Debug.LogError("Missing flag prefab!");
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        GetLookDirection();
#endif
    }
}
