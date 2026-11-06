using UnityEngine;

// 윤곽선만 바꿔 여러 회전체(찻주전자 몸통, 뚜껑, 찻잔, 쟁반 등)를 만드는 범용 컴포넌트
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Mid_LatheShape : MonoBehaviour
{
    [SerializeField] Vector2[] profile =
    {
        new Vector2(0f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(0f, 1f),
    };
    [SerializeField, Range(3, 64)] int sides = 24;
    [SerializeField] bool smooth = true;

    Mesh mesh;

    public void SetProfile(Vector2[] p, int s, bool isSmooth)
    {
        profile = p;
        sides = s;
        smooth = isSmooth;
        if (mesh != null) Build();
    }

    void OnEnable()
    {
        mesh = new Mesh { name = "Mid_LatheShape" };
        GetComponent<MeshFilter>().sharedMesh = mesh;
        Build();
    }

    void OnValidate()
    {
        if (mesh != null) Build();
    }

    void Build()
    {
        if (profile == null || profile.Length < 2) return;
        if (smooth) Mid_Lathe.BuildSmooth(mesh, profile, sides);
        else Mid_Lathe.Build(mesh, profile, sides, 180f / sides, 1, (k, i) => 0);
    }
}
