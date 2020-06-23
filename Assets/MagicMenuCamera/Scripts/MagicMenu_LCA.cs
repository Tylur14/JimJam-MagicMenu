using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// This is soley used on the prefab for getting all required objects out in the scene
/// includes the camera, marker, controller, and nav UI
/// </summary>
public class MagicMenu_LCA : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (PrefabUtility.IsPartOfAnyPrefab(this.gameObject))
        {
            PrefabUtility.UnpackPrefabInstance(this.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            var newObjs = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                newObjs[i] = transform.GetChild(i).gameObject;
            UnityEditor.Selection.objects = newObjs;
            transform.DetachChildren();

            if (FindObjectOfType<StandaloneInputModule>() == null)
            {
                var eventSystem = new GameObject();
                eventSystem.name = "Event System";
                eventSystem.AddComponent<StandaloneInputModule>();
            }
                

            DestroyImmediate(this.gameObject);
        }
            
        
#endif
    }
}
