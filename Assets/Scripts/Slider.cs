using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public Transform startPosition = null;
    public Transform endPosition = null;

    MeshRenderer meshRenderer = null;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnSlideStart()
    {
        Debug.Log("OnSlideStart called");
        meshRenderer.material.SetColor ("_BaseColor", Color.red);
    }

    public void OnSlideStop()
    {
        Debug.Log("OnSlideStop called");
        meshRenderer.material.SetColor ("_BaseColor", Color.white);
    }

    public void UpdateSlider(float percent)
    {
        transform.position = Vector3.Lerp (startPosition.position, endPosition.position, percent);

        Debug.Log("UpdateSlider called");

        Debug.Log($"Percent: {percent}, Position: {transform.position}");

    }

}
