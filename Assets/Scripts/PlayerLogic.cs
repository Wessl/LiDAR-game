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
                interactionTooltip.SetActive(true);
                foreach (var hitCollider in hitInteractionColliders)
                {
                    inRangeInteractible = hitCollider.GetComponent<Interactible>();
                    Debug.Log(hitCollider.name);
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
