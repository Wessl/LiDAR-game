using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    private float interactionCheckDelay = 0.2f;

    [SerializeField] private LayerMask interactionLayerMask;
    [SerializeField] private float interactionRange;
    void Start()
    {
        StartCoroutine(CheckForInteractibles());
    }

    IEnumerator CheckForInteractibles() {
        while (true)
        {
            Collider[] hitInteractionColliders = Physics.OverlapSphere(transform.position, interactionRange, interactionLayerMask);
            foreach (var hitCollider in hitInteractionColliders)
            {
                hitCollider.GetComponent<Interactible>().Interact();
                Debug.Log(hitCollider.name);
            }
            yield return new WaitForSeconds(interactionCheckDelay);
        }    
    }
}
