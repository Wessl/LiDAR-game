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
        
        // Remove zombie from world right after...?
        Destroy(gameObject);
        
        // activate 3d sound source somewhere...?
    }

    private void CreatePointsOnMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> colors = new List<Vector3>();
        var allMeshRenderers = gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var meshRenderer in allMeshRenderers)
        {
            var go = meshRenderer.gameObject;
            Mesh bakedMesh = new Mesh();
            meshRenderer.BakeMesh(bakedMesh, true);
            Matrix4x4 localToWorld = transform.localToWorldMatrix;
            for(int i = 0; i< bakedMesh.vertices.Length; ++i){
                Vector3 world_v = localToWorld.MultiplyPoint3x4(bakedMesh.vertices[i]);    // get the real time world pos
                colors.Add(new Vector3(0.733f, 0.031f, 0.031f));
                vertices.Add(world_v);
            }
        }
        // Now get a reference to the object that actually handles drawing points
        DrawCircles drawCircles = GameObject.FindObjectOfType<DrawCircles>();
        drawCircles.UploadCircleData(vertices.ToArray(), colors.ToArray());
    }
}
