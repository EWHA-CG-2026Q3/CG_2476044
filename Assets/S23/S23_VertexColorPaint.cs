using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S23_VertexColorPaint : MonoBehaviour
{
    public Color bottomColor = new Color(0.95f, 0.45f, 0.25f);
    public Color topColor = new Color(0.3f, 0.6f, 0.95f);

    Mesh colorStream;

    void OnEnable()
    {
        Mesh source = GetComponent<MeshFilter>().sharedMesh;
        if (source == null) return;

        Vector3[] vertices = source.vertices;
        Bounds bounds = source.bounds;
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            float h = Mathf.InverseLerp(bounds.min.y, bounds.max.y, vertices[i].y);
            colors[i] = Color.Lerp(bottomColor, topColor, h);
        }

        // 원본 메시는 그대로 두고 정점 색만 추가 스트림으로 얹음
        colorStream = new Mesh { name = "VertexColorStream", hideFlags = HideFlags.HideAndDontSave };
        colorStream.vertices = vertices;
        colorStream.colors = colors;
        GetComponent<MeshRenderer>().additionalVertexStreams = colorStream;
    }

    void OnDisable()
    {
        GetComponent<MeshRenderer>().additionalVertexStreams = null;
        if (colorStream != null) DestroyImmediate(colorStream);
    }
}
