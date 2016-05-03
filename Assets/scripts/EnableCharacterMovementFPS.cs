/*
File: EnableCharacterMovementFPS.cs
How to use:
    1) Attach script to game object with CharacterController component.
Description:
    This is a general-purpose movement manager that provides FPS-style controls.
    Note that movement keys are defined under Unity's input settings.
*/
using UnityEngine;
using System.Collections;

// Has to be enclosed in directives to allow building.
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Toolbox
{
    public class EnableCharacterMovementFPS : MonoBehaviour
    {

        public Vector2 speedMultiplier = new Vector2(1, 1);

        CharacterController cc;

        void Start()
        {
            cc = GetComponent<CharacterController>();

            // Throws an error if character controller is not found.
            // Has to be enclosed in directives to allow building.
            #if UNITY_EDITOR
            if (!cc)
            {
                EditorUtility.DisplayDialog("ERROR: EnableCharacterMovementFPS.cs", "Character Controller component is missing for " + gameObject, "OK");
                EditorApplication.isPlaying = false;
            }
            #endif
        }


        void Update()
        {
            // Rotates character controller with game object
            cc.transform.rotation = GetComponent<Transform>().rotation;

            // Defines movement delta
            Vector3 movementDelta = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Scales movement delta by speed multiplier
            Vector3 _speedMultiplier = new Vector3(speedMultiplier.x, 0, speedMultiplier.y);
            movementDelta = Vector3.Scale(movementDelta, _speedMultiplier);

            // Moves character along character rotation, with a hardcoded factor
            cc.Move(cc.transform.rotation * movementDelta * 0.1f);
        }
    }

}
