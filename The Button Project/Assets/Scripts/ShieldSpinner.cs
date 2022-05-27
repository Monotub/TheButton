using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShieldSpinner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject[] _shields;

    [Header("Shield Customization")]
    [SerializeField] float _spinFactor = 150;
    [SerializeField] float _startDistance = 6f;
    [SerializeField] float _minShieldDistance = 3f;
    [SerializeField] float _maxShieldDistance = 10f;

    float _shieldDistance;
    float _shieldDistanceModifier = 0.1f;
    Vector3 _spinVector;


    private void OnEnable()
    {
        Button.onLeftButtonHeld += onLeftButtonHeld;
        Button.onRightButtonHeld += onRightButtonHeld;

        _shieldDistance = _startDistance;
        _spinVector = new Vector3(0, _spinFactor, 0);
    }

    private void OnDisable()
    {
        Button.onLeftButtonHeld -= onLeftButtonHeld;
        Button.onRightButtonHeld -= onRightButtonHeld;
    }

    void Update()
    {
        SetShieldDistance(_shieldDistance);
        
        transform.Rotate(_spinVector * Time.deltaTime);
    }

    void onLeftButtonHeld()
    {
        if(_shieldDistance <= _maxShieldDistance)
            _shieldDistance += _shieldDistanceModifier;
    }

    void onRightButtonHeld()
    {
        if (_shieldDistance >= _minShieldDistance)
            _shieldDistance -= _shieldDistanceModifier;
    }

    public void ReverseSpin()
    {
        _spinVector = -_spinVector;
    }

    public void SetShieldDistance(float distance)
    {
        foreach(var shield in _shields)
        {
            Vector3 shieldPos = shield.transform.localPosition;

            switch (shield.name)
            {
                case "North Shield":
                    shieldPos.z = distance;
                    break;
                case "South Shield":
                    shieldPos.z = -distance;
                    break;
                case "East Shield":
                    shieldPos.x = distance;
                    break;
                case "West Shield":
                    shieldPos.x = -distance;
                    break;
            }

            shield.transform.localPosition = shieldPos;
        }
    }
}
