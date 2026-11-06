using UnityEngine;

// 팔각 석등: 단면 윤곽선을 Y축으로 sides번 돌려 메시를 만듦 (Mid_Lathe)
// 화사석(불집) 띠에서는 한 칸 걸러 한 칸을 화창(불빛이 새는 창)으로 보고 서브메시 1에 넣음
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Mid_StoneLantern : MonoBehaviour
{
    [SerializeField, Range(4, 32)] int sides = 8;

    // x = 반지름, y = 높이 (아래에서 위로)
    [SerializeField]
    Vector2[] profile =
    {
        new Vector2(0.00f, 0.00f),
        new Vector2(0.45f, 0.00f),   // 지대석
        new Vector2(0.45f, 0.12f),
        new Vector2(0.34f, 0.12f),
        new Vector2(0.24f, 0.26f),   // 하대석
        new Vector2(0.11f, 0.26f),
        new Vector2(0.11f, 0.86f),   // 간주석(기둥)
        new Vector2(0.22f, 0.86f),
        new Vector2(0.32f, 0.98f),   // 상대석
        new Vector2(0.23f, 0.98f),
        new Vector2(0.23f, 1.32f),   // 화사석
        new Vector2(0.46f, 1.32f),
        new Vector2(0.48f, 1.37f),
        new Vector2(0.15f, 1.50f),   // 옥개석(지붕)
        new Vector2(0.09f, 1.52f),
        new Vector2(0.09f, 1.56f),
        new Vector2(0.11f, 1.62f),   // 보주
        new Vector2(0.00f, 1.72f),
    };

    // 이 띠가 화사석
    [SerializeField] int glowSegment = 9;
    [SerializeField] int windowEvery = 2;

    Mesh mesh;

    void OnEnable()
    {
        mesh = new Mesh { name = "Mid_StoneLantern" };
        GetComponent<MeshFilter>().sharedMesh = mesh;
        Build();
    }

    void OnValidate()
    {
        if (mesh != null) Build();
    }

    void Build()
    {
        Mid_Lathe.Build(mesh, profile, sides, 180f / sides, 2,
            (k, i) => (k == glowSegment && i % windowEvery == 0) ? 1 : 0);
    }
}
