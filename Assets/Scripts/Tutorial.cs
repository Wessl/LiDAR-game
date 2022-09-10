using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject leftMouseHelper;
    public GameObject midMouseHelper;
    public GameObject w;
    public GameObject a;
    public GameObject s;
    public GameObject d;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            leftMouseHelper.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            midMouseHelper.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            w.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            a.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            s.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            d.SetActive(false);
        }
    }
}
