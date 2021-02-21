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
            wall.Move();
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
