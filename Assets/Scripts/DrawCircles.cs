using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawCircles : MonoBehaviour
{
    // Does this really need to be an object? Ah well
    public Shader shader;
    private Material _material;
    private int _bufIndex;

    private bool _canStartRendering;
    private ComputeBuffer _posBuffer;
    private ComputeBuffer _colorBuffer;
    private Vector4[] _color;
    private List<Vector3> _circlePositions = new List<Vector3>();
    private Vector3[] _prevCirclePos;
    private int computeBufferCount = 1048576; // 2^20

    private void Awake()
    {
        _material = new Material(shader);
        _canStartRendering = false;
        _prevCirclePos = Array.Empty<Vector3>();
        int stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3));
        _posBuffer = new ComputeBuffer (computeBufferCount, stride, ComputeBufferType.Append);
        _colorBuffer = new ComputeBuffer(computeBufferCount, stride, ComputeBufferType.Append);
    }

    public void UploadCircleDataSlow(Vector3 circlePos)
    {
        // _posBuffer?.Release();

        // This is not particularly efficient, we are setting buffer data for every single circle
        _bufIndex++;
        
        Vector3[] singleArr = new Vector3[] {circlePos};
        _posBuffer.SetData (singleArr, 0, _bufIndex % computeBufferCount, 1);
        
        _color = new Vector4[1];
        _color[0] = new Vector4(0.5f, 0.4f, 0.9f, 1f);  // TODO: Make this depend on the thing you hit
        _material.SetBuffer ("posbuffer", _posBuffer);
        
        _canStartRendering = true;
        
    }
    
    public void UploadCircleData(Vector3[] circlePositions, Vector3[] colors)
    {
        var amount = circlePositions.Length;
        _bufIndex += amount;

        _posBuffer.SetData (circlePositions, 0, _bufIndex % computeBufferCount, amount);
        _colorBuffer.SetData(colors, 0, _bufIndex % computeBufferCount, amount);
        _material.SetBuffer ("posbuffer", _posBuffer);
        _canStartRendering = true;
        
    }
    
    void OnRenderObject()
    {
        if (_canStartRendering)
        {
            _material.SetPass(0);
            _material.SetBuffer ("posbuffer", _posBuffer);
            _material.SetBuffer("colorbuffer", _colorBuffer);
            Graphics.DrawProceduralNow(MeshTopology.Points, _posBuffer.count, 1);
        }
    }
    void OnDestroy()
    {
        _posBuffer.Release();   // we cant have dirty data laying around (this can crash your pc if you dont have it)
        _colorBuffer.Release();
    }
}
