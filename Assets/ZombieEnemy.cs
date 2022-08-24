using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEnemy : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource; 
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
        _audioSource.Play();    // Zombie argharhh sound
        LiDARShooter.OnThresholdReached -= Act;     // unsubscribe ourselves since we olnly want this occur once

        CreatePointsOnMesh();
    }

    private void CreatePointsOnMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> colors = new List<Vector3>();
        var allMeshRenderers = gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var meshRenderer in allMeshRenderers)
        {
            var go = meshRenderer.gameObject;
            Mesh zombieMesh = meshRenderer.sharedMesh;
            foreach (var vertex in zombieMesh.vertices)
            {
                colors.Add(new Vector3(0.733f, 0.031f, 0.031f));
                vertices.Add(transform.TransformPoint(vertex));
            }
        }
        // Now get a reference to the object that actually handles drawing points
        DrawCircles drawCircles = GameObject.FindObjectOfType<DrawCircles>();
        drawCircles.UploadCircleData(vertices.ToArray(), colors.ToArray());
    }
}
