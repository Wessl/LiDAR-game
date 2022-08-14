using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is a mishmash of code from Freya Holmér and Brackeys, credit goes to them
public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    private float xRotation = 0f;
    public bool focusOnEnable = true; // whether or not to focus and lock cursor immediately on enable

    
    static bool Focused {
        get => Cursor.lockState == CursorLockMode.Locked;
        set {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = value == false;
        }
    }
    void OnEnable() {
        if( focusOnEnable ) Focused = true;
    }

    void OnDisable() => Focused = false;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Input
        if( Focused )
            UpdateInput();
        else if( Input.GetMouseButtonDown( 0 ) )
            Focused = true;
        
    }

    private void UpdateInput()
    {
        // Rotation
        Vector2 mouseDelta = mouseSensitivity * new Vector2( Input.GetAxis( "Mouse X" ), -Input.GetAxis( "Mouse Y" ) );
        Quaternion rotation = transform.rotation;
        Quaternion horiz = Quaternion.AngleAxis( mouseDelta.x, Vector3.up );
        Quaternion vert = Quaternion.AngleAxis( mouseDelta.y, Vector3.right );
        playerBody.rotation = horiz * rotation * vert;
        
        // Leave cursor lock
        if( Input.GetKeyDown( KeyCode.Escape ) )
            Focused = false;
    }

    public float MouseSensitivity
    {
        get => mouseSensitivity;           
        set => mouseSensitivity = value; 
    }

    public float verticalRotation => xRotation;
}
