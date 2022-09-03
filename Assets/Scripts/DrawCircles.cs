using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DrawCircles : MonoBehaviour
{
    // Does this really need to be an object? Ah well
    public Shader shader;
    private Material _material;
    private int _bufIndex;

    private bool _canStartRendering;
    private ComputeBuffer _posBuffer;
    private ComputeBuffer _colorBuffer;
    private int computeBufferCount = 1048576; // 2^20. 3*4*1048576 = 12MB which is... nothing. still, buffers are seemingly routed through l2 cache which is smaller than 12MB, sometimes.. (actually idk, would love to find out)§

    private void Awake()
    {
        _material = new Material(shader);
        _canStartRendering = false;
        int stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3));    // .. its 12 bytes. 3 floats. 
        _posBuffer = new ComputeBuffer (computeBufferCount, stride, ComputeBufferType.Default);
        _colorBuffer = new ComputeBuffer(computeBufferCount, stride, ComputeBufferType.Default);
    }

    public void ResetBuffers()
    {
        int stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3)); 
        _posBuffer.Release();
        _colorBuffer.Release();
        _posBuffer = new ComputeBuffer (computeBufferCount, stride, ComputeBufferType.Default);
        _colorBuffer = new ComputeBuffer(computeBufferCount, stride, ComputeBufferType.Default);
        _bufIndex = 0;
    }

    public void UploadCircleData(Vector3[] circlePositions, Vector3[] colors)
    {
        var amount = circlePositions.Length;
        _bufIndex += amount;
        _posBuffer.SetData (circlePositions, 0, _bufIndex % computeBufferCount, amount);
        _colorBuffer.SetData(colors, 0, _bufIndex % computeBufferCount, amount);
        // _material.SetBuffer ("posbuffer", _posBuffer);
        // _material.SetBuffer("colorbuffer", _colorBuffer);
        _canStartRendering = true;
    }
    
    public void JigglePoints()
    { // very expensive method most likely - maybe possible to speed up?
        // It would be really cool if this happened in tune with some music
        var posData = new Vector3[_bufIndex];
        var colorData = new Vector3[_bufIndex];
        _posBuffer.GetData(posData);
        // _colorBuffer.GetData(colorData);
        Vector3 affection = new Vector3(0, Random.Range(-0.05f,0.01f),0);
        for (int i = 0; i < _bufIndex; i++)
        {
            // Find out some fun way of actually doing this. Is it more fun to have them fall down, or vibrate, or something else?
            posData[i] += affection;
        }
        _posBuffer.SetData(posData, 0, 0, _bufIndex);
    }
    
    void OnRenderObject()
    {
        if (_canStartRendering)
        {
            RenderPointsNow();
        }
    }

    public void RenderPointsNow()
    {
        _material.SetPass(0);
        _material.SetBuffer ("posbuffer", _posBuffer);
        _material.SetBuffer("colorbuffer", _colorBuffer);
        Graphics.DrawProceduralNow(MeshTopology.Points, _posBuffer.count, 1);
    }
    void OnDestroy()
    {
        _posBuffer.Release();   // we cant have dirty data laying around (this can crash your pc if you dont have it)
        _colorBuffer.Release();
    }
}
