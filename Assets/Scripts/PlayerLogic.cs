using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    private float interactionCheckDelay = 0.2f;

    [SerializeField] private LayerMask interactionLayerMask;
    [SerializeField] private float interactionRange;
    [SerializeField] private GameObject interactionTooltip;
    private Interactible inRangeInteractible;
    void Start()
    {
        StartCoroutine(CheckForInteractibles());
    }

    IEnumerator CheckForInteractibles() {
        while (true)
        {
            Collider[] hitInteractionColliders = Physics.OverlapSphere(transform.position, interactionRange, interactionLayerMask);
            if (hitInteractionColliders.Length != 0)
            {
                // We got something - great! Now do a raycast to confirm if it's "visible". 
                RaycastHit hit;
                foreach (var hitCollider in hitInteractionColliders)
                {
                    var hitTransform = hitCollider.transform;
                    Vector3 direction = hitTransform.position - transform.position;
                    Physics.Raycast(transform.position, direction, out hit);
                    if (hit.collider == hitCollider)
                    {
                        // If the collider we hit with the raycast is the same as with the overlapsphere, there is nothing in line of sight
                        interactionTooltip.SetActive(true);
                        inRangeInteractible = hitCollider.GetComponent<Interactible>();
                    }
                }
            }
            else
            {
                interactionTooltip.SetActive(false);
                inRangeInteractible = null;
            }
            yield return new WaitForSeconds(interactionCheckDelay);
        }    
    }

    private void Update()
    {
        if (inRangeInteractible != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inRangeInteractible.Interact();
                inRangeInteractible = null; // discard reference
                interactionTooltip.SetActive(false);
            }
        }
    }
}
