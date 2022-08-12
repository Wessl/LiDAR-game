using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LiDARShooter : MonoBehaviour
{
    private Camera mainCam;

    public float coneAngle;
    public float fireRate;
    public int superScanSqrtNum = 200;
    public KeyCode lidarActivationKey;
    public KeyCode superScanKey = KeyCode.Y;
    public DrawCircles drawCirclesObj;

    private List<RaycastHit> hits = new List<RaycastHit>();
    
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(superScanKey))
        {
            StartCoroutine(SuperScan());
        }
        else if (Input.GetKey(lidarActivationKey))
        {
            LiDAR();
        }
    }

    IEnumerator SuperScan()
    {
        // lmao how the fuck did I figure all this out
        float aspect = mainCam.aspect;
        float magic = 1.75f;
        float fov = mainCam.fieldOfView;
        var mainCamGO = mainCam.gameObject;
        var facingDir = mainCamGO.transform.forward;
        var upDir =  mainCamGO.transform.up;
        var cameraPos = mainCam.transform.position;
        var cameraRay = (cameraPos + facingDir);
        
        var q = Vector3.Cross(facingDir.normalized, upDir.normalized);
        var r = Mathf.Tan(Mathf.Deg2Rad * (fov));
        Vector3[] pointsOnPlane = new Vector3[superScanSqrtNum];
        for (int i = 0; i < superScanSqrtNum; i++)
        {
            var meta = i / (float)superScanSqrtNum * Mathf.PI ;
            for (int j = 0; j < superScanSqrtNum; j++)
            {
                var theta = j / (float)superScanSqrtNum * Mathf.PI + Math.PI/2.0f;
                var v =  (Mathf.Cos(meta) * upDir/(magic) + Mathf.Sin((float)theta) * q/(magic/aspect));    // instead of magic numbers use randoms that are half of cell size and use screen ratio for other numbers
                pointsOnPlane[j] = v;
            }
            Tuple<Vector3[],string[]> pointsHit = CheckRayIntersections(cameraPos, cameraRay-cameraPos, pointsOnPlane);
            drawCirclesObj.UploadCircleData(pointsHit.Item1, TagsToColors(pointsHit.Item2));     // It makes more sense to split these into two
            yield return null;
        }
    }

    void LiDAR()
    {
        // Store variables relating to camera view dir and pos
        var facingDir = mainCam.gameObject.transform.forward;
        var cameraPos = mainCam.transform.position;
        var cameraRay = (cameraPos + facingDir);
        // Calculate perpendicular angles to view direction to generate circle on which points can be created
        var p = GetPerpendicular(facingDir);
        var q = Vector3.Cross(facingDir.normalized, p);
        int i_fireRate = (int)Mathf.Ceil(fireRate * Time.deltaTime);
        Vector3[] pointsOnDisc = new Vector3[i_fireRate];
        for (int i = 0; i < i_fireRate; i++)
        {
            pointsOnDisc[i] = GenRandPointDisc(p,q);
        }
        
        // DrawDebug(cameraRay, p, q, pointsOnDisc);

        Tuple<Vector3[],string[]> pointsHit = CheckRayIntersections(cameraPos, cameraRay-cameraPos, pointsOnDisc);
        drawCirclesObj.UploadCircleData(pointsHit.Item1, TagsToColors(pointsHit.Item2));     // It makes more sense to split these into two
    }

    private Vector3[] TagsToColors(string[] tags)
    {
        Vector3[] colors = new Vector3[tags.Length];
        int i = 0;
        foreach (var tag in tags)
        {
            if (tag == "Interactible")
            {
                colors[i++] = new Vector3(0,1,0);
            }
            else
            {
                colors[i++] = new Vector3(1, 1, 1);
            }
        }

        return colors;
    }

    
    private Tuple<Vector3[],String[]> CheckRayIntersections(Vector3 cameraPos, Vector3 cameraRay, Vector3[] points)
    {
        Vector3[] pointsHit = new Vector3[points.Length];
        string[] tagsOfPoints = new string[points.Length];
        int i = 0;
        foreach (var point in points)
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraPos, (cameraRay + point), out hit))
            {
                tagsOfPoints[i] = hit.collider.tag;
                pointsHit[i++] = hit.point;
            }    
        }
        return new Tuple<Vector3[], string[]>(pointsHit, tagsOfPoints);
    }
    
    private Vector3 GenRandPointDisc(Vector3 p, Vector3 q)
    {
        // Generate random point in the PQ plane disc - actually makes sense if u think about it, a pretty simple alg
        var rmax = Mathf.Tan(Mathf.Deg2Rad * coneAngle);
        var theta = Random.Range(0f, 2 * Mathf.PI);
        var r = rmax * Mathf.Sqrt(Random.Range(0f, 1f));
        var v = r * (p * Mathf.Cos(theta) + q * Mathf.Sin(theta));
        return v;
    }

    private void DrawDebug(Vector3 cameraRay, Vector3 perpendicular, Vector3 q, Vector3[] pointOnDisc)
    {
        Debug.DrawLine(mainCam.transform.position, cameraRay, Color.green, 20);
        Debug.DrawLine(cameraRay, perpendicular + cameraRay, Color.red, 20);
        Debug.DrawLine(cameraRay, q + cameraRay, Color.yellow, 20);
        foreach (var point in pointOnDisc)
        {
            // Debug.DrawLine(mainCam.transform.position, point + cameraRay, Color.yellow);
        }
        
    }

    private Vector3 GetPerpendicular(Vector3 cameraRay)
    {
        /// https://stackoverflow.com/questions/39404576/cone-from-direction-vector
        cameraRay.Normalize();
        float max = float.NegativeInfinity;
        float min = float.PositiveInfinity;
        int axisMin = 1, axisMax = 1;
        for (int i = 0; i < 3; i++)
        {
            if (Mathf.Abs(cameraRay[i]) > max)
            {
                axisMax = i;
                max = cameraRay[i];
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (Mathf.Abs(cameraRay[i]) < min)
            {
                axisMin = i;
                min = cameraRay[i];
            }
        }
        // construct perpendicular
        Vector3 perp = new Vector3();
        var midIndex = 2 * (axisMax + axisMin) % 3;
        perp[axisMax] = cameraRay[midIndex];
        perp[midIndex] = -max;
        return perp.normalized;
    }
}
