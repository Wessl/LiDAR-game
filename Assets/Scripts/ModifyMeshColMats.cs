using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMeshColMats : MonoBehaviour
{
    public Material blackMat;
    private void Start()
    {
        var meshRenderers = FindObjectsOfType<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.gameObject.AddComponent<MeshCollider>();
            meshRenderer.material = blackMat;
        }
    }
}
