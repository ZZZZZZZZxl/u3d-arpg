using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeState
{
    NOTWORK, // 没有工作
    WORKING, // 工作中
    DONE, // 工作结束
} 

public class GameTimer
{
    // 1. 计时时长，多久后开始
    // 2. 计时后的任务
    // 3. 当前状态
    // 4. 是否暂停计时器

    private float _leftTime;
    private Action _action;
    private TimeState _timeState;
    private bool _isStopTimer;

    public GameTimer() // 创建一个计时器 并待命
    {
        ResetTimer();
    }

    public void StartTimer(float time, Action action) // 开始计时器及任务
    {
        _leftTime = time; // 倒计时时间
        _action = action;
        _timeState = TimeState.WORKING; // 倒计时中
        _isStopTimer = false; // 停止倒计时
    }

    public void UpdateTimer()
    {
        if (_isStopTimer) return;

        _leftTime -= Time.deltaTime;
        if (_leftTime < 0f) // 倒计时结束 开始任务
        {
            _action?.Invoke(); // 开始任务
            _timeState = TimeState.DONE; // 倒计时结束
            _isStopTimer = true;
        }
    }

    public TimeState GetTimerState() => _timeState;

    public void ResetTimer()
    {
        _leftTime = 0f;
        _action = null;
        _timeState = TimeState.NOTWORK;
        _isStopTimer = true;
    }
}
