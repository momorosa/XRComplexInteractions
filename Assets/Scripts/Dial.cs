using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    Vector3 m_startRotating;

    MeshRenderer m_meshRenderer = null;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    public void StartTurn()
    {
        m_startRotating = transform.localEulerAngles;
        m_meshRenderer.material.SetColor ("_BaseColor", Color.red);
    }

    public void StopTurn()
    {
        m_meshRenderer.material.SetColor ( "_BaseColor", Color.white);
    }

    public void DialUpdate(float angle)
    {
        Vector3 angles = m_startRotating;
        angles.y += angle;
        transform.localEulerAngles = angles;
    }
}
