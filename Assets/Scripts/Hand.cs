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
public Animator handAnimator = null;
int m_gripAmountParam = 0;

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
        gripAction.Enable();
        Hide();
    }

    void UpdateAnimations()
    {
        float gripAmount = gripAction.ReadValue<float>();
        handAnimator.SetFloat(m_gripAmountParam, gripAmount);
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
