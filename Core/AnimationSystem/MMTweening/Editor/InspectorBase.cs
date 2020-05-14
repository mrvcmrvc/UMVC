﻿using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class InspectorBase : Editor
{
    protected float duration;
    protected MMTweeningEaseEnum ease;
    protected AnimationCurve animCurve;
    protected bool initOnAwake;

    MMUITweener _tweenerBase;

    protected void Enable(MMUITweener tweenerBase)
    {
        _tweenerBase = tweenerBase;

        animCurve = tweenerBase.CustomAnimationCurve;
        ease = tweenerBase.Ease;
        duration = tweenerBase.Duration;
        initOnAwake = tweenerBase.InitOnAwake;
    }

    protected void DrawEaseField()
    {
        EditorGUI.BeginChangeCheck();

        ease = (MMTweeningEaseEnum)EditorGUILayout.EnumPopup("Ease", ease);

        if (EditorGUI.EndChangeCheck())
        {
            _tweenerBase.SetEase(ease);

            ease = _tweenerBase.Ease;
        }
    }

    protected void DrawAnimCurveField()
    {
        EditorGUI.BeginChangeCheck();

        animCurve = EditorGUILayout.CurveField(animCurve);

        if (EditorGUI.EndChangeCheck())
            _tweenerBase.SetAnimationCurve(animCurve);
    }

    protected void InitOnAwakeField()
    {
        EditorGUI.BeginChangeCheck();

        initOnAwake = EditorGUILayout.Toggle("Init On Awake", initOnAwake);

        if (EditorGUI.EndChangeCheck())
            _tweenerBase.SetInitOnAwake(initOnAwake);
    }

    protected void DrawDurationField()
    {
        EditorGUI.BeginChangeCheck();

        duration = EditorGUILayout.FloatField("Duration", duration);

        if (EditorGUI.EndChangeCheck())
            _tweenerBase.SetDuration(duration);
    }

    protected void DrawShakePunchAmountField()
    {
        _tweenerBase.ShakePunchAmount = EditorGUILayout.FloatField("Effect Amount", _tweenerBase.ShakePunchAmount);
        _tweenerBase.ShakePunchDirection = EditorGUILayout.Vector3Field("Effect Direction", _tweenerBase.ShakePunchDirection);
    }
}
