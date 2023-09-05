using System;
// using System.Reflection.Metadata;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public enum HandType
{
    Left,
    Right
};

public class Hand : MonoBehaviour
{

public HandType type = HandType.Left;
public bool isHidden {get; private set;} = false;
public InputAction trackedAction = null;

public InputAction gripAction = null;
public InputAction triggerAction = null;
public Animator handAnimator = null;
int m_gripAmountParam = 0;
int m_pointAmountParam = 0;

bool m_isCurrentlyTracked = false;

List<Renderer> m_currentRenderers = new List<Renderer>();

Collider[] m_colliders = null;

public XRBaseInteractor interactor = null;

public bool isCollisionEnabled { get; private set;} = false;

void Awake()
{
    if (interactor == null)
    {
        interactor = GetComponentInParent<XRBaseInteractor>();
    }
}


private void OnEnable()
{
    interactor.onSelectEntered.AddListener(OnGrab);
    interactor.onSelectExited.AddListener(OnRelease);
}

private void OnDisable()
{
    interactor.onSelectEntered.RemoveListener(OnGrab);
    interactor.onSelectExited.RemoveListener(OnRelease);
}

    void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();
        trackedAction.Enable();
        m_gripAmountParam = Animator.StringToHash("GripAmount");
        m_pointAmountParam = Animator.StringToHash("PointAmount");
        gripAction.Enable();
        triggerAction.Enable();
        Hide();
    }

    void UpdateAnimations()
    {
        float pointAmount = triggerAction.ReadValue<float>();
        handAnimator.SetFloat(m_pointAmountParam, pointAmount);

        float gripAmount = gripAction.ReadValue<float>();
        handAnimator.SetFloat(m_gripAmountParam, Mathf.Clamp01(gripAmount + pointAmount));  

        // Debug.Log("Grip Amount: " + gripAction.ReadValue<float>());
        // Debug.Log("Trigger Amount: " + triggerAction.ReadValue<float>());
        // Debug.Log($"Grip Amount: {gripAmount}, Point Amount: {pointAmount}, Combined: {Mathf.Clamp01(gripAmount + pointAmount)}");
    }

    void Update()
    {
        float isTracked = trackedAction.ReadValue<float>();
        if (isTracked == 1.0f && !m_isCurrentlyTracked)
        {
            m_isCurrentlyTracked = true;
            Show();
        }
        else if (isTracked == 0 && m_isCurrentlyTracked)
        {
            m_isCurrentlyTracked = false;
            Hide();
        }

        UpdateAnimations();
    }

    public void Show()
    {
        foreach(Renderer renderer in m_currentRenderers)
        {
            renderer.enabled = true;
        }
        isHidden = false;
        EnableCollisions(true);
    }

    public void Hide()
    {   
        m_currentRenderers.Clear();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            renderer.enabled = false;
            m_currentRenderers.Add(renderer);
        }
        isHidden = true;
        EnableCollisions(false);
    }

    public void EnableCollisions (bool enabled)
    {
        if (isCollisionEnabled == enabled) return;

        isCollisionEnabled = enabled;
        foreach (Collider collider in m_colliders)
        {
            collider.enabled = isCollisionEnabled;
        }
    }

    void OnGrab(XRBaseInteractable grabbleObject)
    {
        HandControl ctrl = grabbleObject.GetComponent<HandControl>();

         if (ctrl != null)
        {
            if(ctrl.hideHand)
            {
                Hide();
            }
        }
    }

    void OnRelease(XRBaseInteractable releaseObject)
    {
        HandControl ctrl = releaseObject.GetComponent<HandControl>();

         if (ctrl != null)
        {
            if (ctrl.hideHand)
            {
                Show();
            }
        }
    }
}
