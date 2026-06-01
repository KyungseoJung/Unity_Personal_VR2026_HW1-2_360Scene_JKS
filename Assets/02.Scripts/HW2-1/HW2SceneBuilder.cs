using UnityEngine;

public class HW2SceneBuilder : MonoBehaviour
{
    void Start()
    {
        // Root
        GameObject W = new GameObject("W");

        // Camera
        GameObject camObj = Camera.main != null ? Camera.main.gameObject : new GameObject("Camera");
        camObj.name = "Camera";
        camObj.transform.SetParent(W.transform);
        camObj.transform.localPosition = new Vector3(0, 0, 0);
        camObj.transform.localEulerAngles = new Vector3(90, 90, 0);

        if (camObj.GetComponent<Camera>() == null)
        {
            camObj.AddComponent<Camera>();
        }

        // Objects
        GameObject O1 = CreateCube("O1", W.transform, new Vector3(20, 30, 40), new Vector3(90, 0, 90), Color.yellow);
        GameObject O2 = CreateCube("O2", W.transform, new Vector3(100, 200, 0), new Vector3(0, 90, 0), Color.green);

        GameObject O11 = CreateCube("O11", O1.transform, new Vector3(5, 200, 300), new Vector3(0, 0, 90), Color.green);
        GameObject O12 = CreateCube("O12", O1.transform, new Vector3(30, 40, 0), new Vector3(90, 90, 90), Color.magenta);

        GameObject O121 = CreateCube("O121", O12.transform, new Vector3(10, 20, 30), new Vector3(90, 0, 45), Color.yellow);

        GameObject O21 = CreateCube("O21", O2.transform, new Vector3(40, 50, 0), new Vector3(90, 45, 0), Color.cyan);

        Debug.Log("HW2-1 scene tree has been created.");
        Debug.Log("O121 local point (0,10,0) -> Camera coordinate should be approximately (-100, 10, -100), depending on Euler convention.");
    }

    GameObject CreateCube(string name, Transform parent, Vector3 localPosition, Vector3 localEulerAngles, Color color)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.SetParent(parent);
        obj.transform.localPosition = localPosition;
        obj.transform.localEulerAngles = localEulerAngles;
        obj.transform.localScale = Vector3.one * 5.0f;

        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = color;

        return obj;
    }
}