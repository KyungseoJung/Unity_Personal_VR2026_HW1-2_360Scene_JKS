using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MatrixDisplay : MonoBehaviour
{

}


#if UNITY_EDITOR
[CustomEditor(typeof(MatrixDisplay))]
public class MatrixDisplayEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        MatrixDisplay script = (MatrixDisplay)target;

        // Calculate matrix relative to parent
        Matrix4x4 localToParentMatrix = Matrix4x4.identity;
        
        if (script.transform.parent != null) 
        {
            localToParentMatrix = script.transform.parent.worldToLocalMatrix * script.transform.localToWorldMatrix;
        } 
        else 
        {
            localToParentMatrix = script.transform.localToWorldMatrix;
        }
        
        // Display in Inspector
        EditorGUILayout.LabelField("Local to Parent Matrix", EditorStyles.boldLabel);
        GUILayout.TextArea(localToParentMatrix.ToString("F3"));
    }
}
#endif