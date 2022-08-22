using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkinnedMeshRendererToMeshCollider : MonoBehaviour
{
    private void Start()
    {
        UpdateColliders();
    }

    public void UpdateColliders()
    {
        var allMeshRenderers = gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var meshRenderer in allMeshRenderers)
        {
            var go = meshRenderer.gameObject;
            var collider = go.AddComponent<MeshCollider>();
            Mesh colliderMesh = new Mesh();
            meshRenderer.BakeMesh(colliderMesh, true);
            collider.sharedMesh = null;
            collider.sharedMesh = colliderMesh;
        }
    }
}
