using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossfadeActivator : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        var go = GetComponentInChildren<Animator>().gameObject;
        go.SetActive(true);
        go.GetComponentInChildren<CanvasGroup>().alpha = 1f;
    }
}
