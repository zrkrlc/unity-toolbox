/*
File: EnableCameraMouseLook.cs
How to use:
    1) Attach script to camera.
    2) If camera has parent, set camera position to parent's origin.
Description:
    This is a general-purpose, first-person mouselook script shamelessly 
        cloned from Francis R. Griffiths' Simple Smooth Mouselook script.
    Note that this script optionally locks the cursor to the viewport and 
        hides it.
    This script also binds yaw to the camera's parent, if it has one, to
        enable FPS-style movement.
*/

using UnityEngine;
using System.Collections;

namespace Toolbox
{
    public class EnableCameraMouseLook : MonoBehaviour
    {

        public Vector2 mouseSensitivity = new Vector2(1, 1);
        public Vector2 mouseSmoothing = new Vector2(2, 2);
        public bool isCursorHidden = true;
        Vector2 clampDegrees = new Vector2(360, 180);

        GameObject objectParent;
        Quaternion targetDirection;
        Quaternion targetDirectionCharacter;

        Vector2 _mouseAbsolute;
        Vector2 _mouseLerped;

        void Start()
        {

            // Initialises camera's parent
            objectParent = GetComponent<Transform>().parent.gameObject;

            // Gets initial camera direction
            targetDirection = GetComponent<Transform>().localRotation;

            // Gets initial character direction if possible
            if (objectParent)
            {
                targetDirectionCharacter = objectParent.transform.localRotation;
            }

        }

        void Update()
        {
            // Locks and hides the cursor
            if (isCursorHidden)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // Sets desired target values
            Quaternion targetOrientation = targetDirection;
            Quaternion targetOrientationCharacter = targetDirectionCharacter;

            // Gets mouse input
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            // Scales mouse input by smoothing and sensitivity
            mouseDelta = Vector2.Scale(mouseDelta, Vector2.Scale(mouseSensitivity, mouseSmoothing));

            // Lerps mouse input
            _mouseLerped.x = Mathf.Lerp(_mouseLerped.x, mouseDelta.x, 1.0f / mouseSmoothing.x);
            _mouseLerped.y = Mathf.Lerp(_mouseLerped.y, mouseDelta.y, 1.0f / mouseSmoothing.y);

            // Defines movement from zero point by lerped input
            _mouseAbsolute += _mouseLerped;

            // Clamps the rotations
            if (clampDegrees.x < 360)
            {
                _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampDegrees.x * 0.5f, clampDegrees.x * 0.5f);
            }
            if (clampDegrees.y < 360)
            {
                _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampDegrees.y * 0.5f, clampDegrees.y * 0.5f);
            }

            // Applies the x rotation
            transform.localRotation =
                Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);

            // Applies the y rotation, with modifications for the parent object
            if (objectParent)
            {
                objectParent.transform.localRotation =
                    Quaternion.AngleAxis(_mouseAbsolute.x, objectParent.transform.up);
                objectParent.transform.localRotation *= targetOrientationCharacter;
            }
            else
            {
                transform.localRotation *=
                    Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            }

        }
    }
}

