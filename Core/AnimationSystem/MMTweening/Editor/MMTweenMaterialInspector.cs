using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(MMTweenMaterial))]
public class MMTweenMaterialInspector : InspectorBase
{
    MMTweenMaterial myTarget;

    void OnEnable()
    {
        myTarget = (MMTweenMaterial)target;

        Enable(myTarget);
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        DrawDefaultInspector();

        myTarget.LoopType = (MMTweeningLoopTypeEnum)EditorGUILayout.EnumPopup("Loop Type", myTarget.LoopType);

        DrawEaseField();

        if (ease == MMTweeningEaseEnum.Curve)
            DrawAnimCurveField();

        myTarget.Delay = EditorGUILayout.Toggle("Delay", myTarget.Delay);
        if (myTarget.Delay)
            myTarget.DelayDuration = EditorGUILayout.FloatField("Delay Duration", myTarget.DelayDuration);

        InitOnAwakeField();

        DrawDurationField();

        myTarget.IgnoreTimeScale = EditorGUILayout.Toggle("Ignore TimeScale", myTarget.IgnoreTimeScale);

        myTarget.PlayAutomatically = EditorGUILayout.Toggle("Play Automatically", myTarget.PlayAutomatically);

        if (!Application.isPlaying && (EditorGUI.EndChangeCheck() || GUI.changed))
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}
