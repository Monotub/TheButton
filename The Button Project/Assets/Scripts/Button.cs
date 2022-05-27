using UnityEngine;
using System;

public class Button : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ShieldSpinner shieldSpinner;
    [SerializeField] Transform buttonCore;
    [Header("Button Customization")]
    [SerializeField] float clickThreshold = 0.3f;

    float _buttonHeldTimer = 0f;
    bool isMouseOverButton = false;
    Vector3 coreRestingPosition = new Vector3(0, 1.6f, 0);
    Vector3 coreDownPosition = new Vector3(0, 1.25f, 0);

    public static event Action OnLeftButtonHeld;
    public static event Action OnRightButtonHeld;
    public static event Action OnButtonClicked;


    private void Update()
    {
        if (isMouseOverButton && !GameManager.Instance.IsPaused)
        {
            if (Input.GetMouseButton(0)) ProcessMouseClick(0);
            else if (Input.GetMouseButton(1)) ProcessMouseClick(1);

            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                ProcessMouseRelease();
        }
    }
    private void OnMouseDown()
    {
        if(_buttonHeldTimer < clickThreshold)
            OnButtonClicked?.Invoke();
    }

    private void ProcessMouseClick(int value)
    {
        buttonCore.localPosition = coreDownPosition;
        _buttonHeldTimer += Time.deltaTime;

        if (_buttonHeldTimer > clickThreshold)
        {
            if(value == 0)
                OnLeftButtonHeld?.Invoke();
            else if (value == 1)
                OnRightButtonHeld?.Invoke();
        }
    }

    private void ProcessMouseRelease()
    {
        if (_buttonHeldTimer <= clickThreshold && !GameManager.Instance.IsPaused)
            shieldSpinner.ReverseSpin();
        
        buttonCore.localPosition = coreRestingPosition;
        _buttonHeldTimer = 0f;
    }

    private void OnMouseOver()
    {
        isMouseOverButton = true;
        buttonCore.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    private void OnMouseExit()
    {
        isMouseOverButton = false;
        buttonCore.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }
}
