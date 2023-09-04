using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    
    public Transform startOrientation = null;
    public Transform endOrientation = null;

    MeshRenderer meshRenderer = null;

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void OnLeverPullStart()
    {
        meshRenderer.material.SetColor ("_BaseColor", Color.red);
    }

    public void OnLeverPullStop()
    {

        meshRenderer.material.SetColor ("_BaseColor", Color.white);
    }

    public void UpdateLever(float percent)
    {
        transform.rotation = Quaternion.Slerp (startOrientation.rotation, endOrientation.rotation, percent);

    }
}
