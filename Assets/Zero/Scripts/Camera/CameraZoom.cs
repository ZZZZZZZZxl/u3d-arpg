using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField, Range(0, 10f)] private float _defaultDistance = 6f;
    [SerializeField, Range(0, 10f)] private float _maxmumDistance = 6f;
    [SerializeField, Range(0, 10f)] private float _minmumDistance = 1f;

    [SerializeField, Range(0, 10f)] private float _zoomSensitivity = 1f;
    [SerializeField, Range(0, 10f)] private float _smoothing = 4f;
    
    private CinemachineFramingTransposer _framingTransposer;
    private CinemachineInputProvider _inputProvider;
    private float _currentTargetDistance;
    // private float _currentDistance;
    private void Awake()
    {
        _framingTransposer = GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineFramingTransposer>();
        _inputProvider = GetComponent<CinemachineInputProvider>();
    }

    private void Start()
    {
        _currentTargetDistance =  _defaultDistance;
    }

    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        float zoomValue = _inputProvider.GetAxisValue(2) * _zoomSensitivity;
        _currentTargetDistance = Mathf.Clamp(_currentTargetDistance + zoomValue, 
            _minmumDistance, _maxmumDistance);

        float currentDistance = _framingTransposer.m_CameraDistance;
        if (currentDistance == _currentTargetDistance)
            return;
        
        _framingTransposer.m_CameraDistance = 
            Mathf.Lerp(currentDistance, _currentTargetDistance, _smoothing * Time.deltaTime);
    }
}