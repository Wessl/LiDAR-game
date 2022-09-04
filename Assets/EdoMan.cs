using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdoMan : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject activateAfterGO;
    [SerializeField] private float activeSec = 4f;
    private AudioSource activateAfterAS;
    private bool activated;
    private DrawCircles drawCircles;
    public GameObject backWall;
    
    private float t = 0;
    Camera cam;

    private void Start()
    {
        _audioSource.volume = 0f;
        cam = Camera.main;
        drawCircles = GameObject.FindObjectOfType<DrawCircles>();
        activated = false;
        GameObject.FindObjectOfType<LiDARShooter>().enemyHitAmountTriggerThreshold = 1000; //arbitrary but works okay. 
    }

    private void OnEnable()
    {
        LiDARShooter.OnThresholdReached += Act;
    }

    private void OnDisable()
    {
        LiDARShooter.OnThresholdReached -= Act;
    }

    void Act()
    {
        Debug.Log("somefin happen");
        _audioSource.Play();    // PLay some sound
        LiDARShooter.OnThresholdReached -= Act;     // unsubscribe ourselves since we only want this occur once
        activated = true;
        
        // See Update() for all the cool shit that happens after activation

     
        

        // activate 3d sound source somewhere...?
        // activateAfterGO.SetActive(true);
        // activateAfterAS = activateAfterGO.GetComponent<AudioSource>();

        // Disable the lidar shooter briefly
        // GameObject.FindObjectOfType<LiDARShooter>().DisableForSeconds(activeSec);
    }

    private void Update()
    {
        if (activated)
        {
            t += 1 / activeSec * Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(0, 0.8f, t);
            // Make the points jiggle around a little
            drawCircles.JigglePoints();
            // Have the back wall move down
            backWall.transform.position = Vector3.Lerp(Vector3.zero, Vector3.down*4, t);
            
            // turn off activation...
            if (t >= 0.98)
            {
                activated = false;
            }
        }
        
    }
}