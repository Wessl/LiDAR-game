using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    enum Interaction
    {
        Move,
        Destroy,
        Rotate
        // More here in the future
    }
    [SerializeField] Interaction interactionType;       // Set this in inspector of each game object 
    [SerializeField] private Vector3 interactionMovement;
    [SerializeField] private Vector3 interactionRotation;
    [SerializeField] private float rotationSpeed;
    private float rotationTime;
    private bool isBusy;

    private void Start()
    {
        rotationTime = 1 / rotationSpeed;
        isBusy = false;
    }


    public void Interact()
    {
        // Player should do occasional checks to see if they are in range of interactible object
        // If they are they are shown an iteraction button - (not just a button, think outside the box - maybe it causes all the lidar rays to concentrate on something, or maybe thats stupid and we are doing something completely different)
        if (isBusy) return;
        switch (interactionType)
        {
            case Interaction.Move:
                transform.Translate(interactionMovement);
                break;
            case Interaction.Rotate:
                StartCoroutine(RotateOverTime());
                break;
            default:
                // nothing
                break;
            
        }
    }

    private IEnumerator RotateOverTime()
    {
        isBusy = true;
        float timePassed = 0;
        while (timePassed < rotationTime)
        {
            transform.Rotate(interactionRotation * Time.deltaTime * rotationSpeed);
            timePassed += Time.deltaTime;
            yield return null;
        }

        isBusy = false;

    }
}
