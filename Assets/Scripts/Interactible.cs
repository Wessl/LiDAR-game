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
    [SerializeField] private Quaternion interactionRotation;

    
    public void Interact()
    {
        // Player should do occasional checks to see if they are in range of interactible object
        // If they are they are shown an iteraction button - (not just a button, think outside the box - maybe it causes all the lidar rays to concentrate on something, or maybe thats stupid and we are doing something completely different)
        switch (interactionType)
        {
            case Interaction.Move:
                transform.Translate(interactionMovement);
                break;
            default:
                // nothing
                break;
            
        }
    }
}
