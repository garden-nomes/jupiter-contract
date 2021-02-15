using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoxEdit))]
public class BoxEditEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        var wall = (BoxEdit) target;

        var snap = Vector3.one / 8f;

        EditorGUI.BeginChangeCheck();

        var newPositionA = Handles.FreeMoveHandle(
            wall.a,
            Quaternion.identity,
            1f / 16f,
            snap,
            Handles.DotHandleCap);

        var newPositionB = Handles.FreeMoveHandle(
            wall.b,
            Quaternion.identity,
            1f / 16f,
            snap,
            Handles.DotHandleCap);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(wall, "Reposition wall");
            wall.a = snapToPixelGrid(newPositionA);
            wall.b = snapToPixelGrid(newPositionB);
            wall.Update();
        }
    }

    private Vector3 snapToPixelGrid(Vector3 v)
    {
        float pixelsPerUnit = ((BoxEdit) target).pixelsPerUnit;
        var offset = Vector3.one / (2f * pixelsPerUnit);

        return offset + new Vector3(
            Mathf.Floor(v.x * pixelsPerUnit) / pixelsPerUnit,
            Mathf.Floor(v.y * pixelsPerUnit) / pixelsPerUnit,
            Mathf.Floor(v.z * pixelsPerUnit) / pixelsPerUnit
        );
    }
}

[ExecuteInEditMode]
public class BoxEdit : MonoBehaviour
{
    [HideInInspector] public Vector2 a;
    [HideInInspector] public Vector2 b;
    public float pixelsPerUnit = 8f;

    void Awake()
    {
        var extent = new Vector2(
            (transform.localScale.x - 1f / 8f) / 2f,
            (transform.localScale.y - 1f / 8f) / 2f
        );

        a = (Vector2) transform.position - extent;
        b = (Vector2) transform.position + extent;
    }

    public virtual void Update()
    {
        var height = Mathf.Abs(a.y - b.y) + 1 / 8f;
        var width = Mathf.Abs(a.x - b.x) + 1 / 8f;

        transform.position = new Vector3(0f, 0f, transform.position.z) + (Vector3) (a + b) / 2f;
        transform.localScale = new Vector3(width, height, 1f);
    }
}
