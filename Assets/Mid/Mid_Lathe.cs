using System;
using System.Collections.Generic;
using UnityEngine;

// 회전체 메시: 단면 윤곽선(x = 반지름, y = 높이)을 Y축으로 sides번 돌려 만듦
// 윤곽선의 이웃한 두 점과 원 위의 이웃한 두 각도가 사각형 하나가 됨
public static class Mid_Lathe
{
    public static void Build(Mesh mesh, IList<Vector2> profile, int sides, float startAngleDeg,
                             int subMeshCount, Func<int, int, int> subMeshOf)
    {
        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();
        var tris = new List<int>[subMeshCount];
        for (int s = 0; s < subMeshCount; s++) tris[s] = new List<int>();

        float totalLength = 0f;
        for (int k = 0; k < profile.Count - 1; k++)
            totalLength += Vector2.Distance(profile[k], profile[k + 1]);

        float start = startAngleDeg * Mathf.Deg2Rad;
        float v0 = 0f;
        for (int k = 0; k < profile.Count - 1; k++)
        {
            Vector2 bottom = profile[k];
            Vector2 top = profile[k + 1];
            float v1 = v0 + Vector2.Distance(bottom, top) / totalLength;

            for (int i = 0; i < sides; i++)
            {
                float a0 = start + 2f * Mathf.PI / sides * i;
                float a1 = start + 2f * Mathf.PI / sides * (i + 1);

                // 면마다 정점을 따로 두어 모서리가 각지게 보이게 함
                int first = vertices.Count;
                vertices.Add(OnCircle(bottom, a0));
                vertices.Add(OnCircle(top, a0));
                vertices.Add(OnCircle(top, a1));
                vertices.Add(OnCircle(bottom, a1));

                float u0 = (float)i / sides, u1 = (float)(i + 1) / sides;
                uvs.Add(new Vector2(u0, v0));
                uvs.Add(new Vector2(u0, v1));
                uvs.Add(new Vector2(u1, v1));
                uvs.Add(new Vector2(u1, v0));

                // 반지름이 0인 쪽은 두 점이 겹쳐 삼각형 하나가 사라지므로 뺌
                var list = tris[Mathf.Clamp(subMeshOf(k, i), 0, subMeshCount - 1)];
                if (top.x > 0f) list.AddRange(new[] { first, first + 1, first + 2 });
                if (bottom.x > 0f) list.AddRange(new[] { first, first + 2, first + 3 });
            }
            v0 = v1;
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.subMeshCount = subMeshCount;
        for (int s = 0; s < subMeshCount; s++) mesh.SetTriangles(tris[s], s);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // 매끄러운 회전체: 정점을 이웃 면과 공유하고, normal은 윤곽선 양옆 선분의 수직 방향 평균으로 구함
    public static void BuildSmooth(Mesh mesh, IList<Vector2> profile, int sides)
    {
        int ring = sides + 1;   // 이음새에 정점을 하나 더 둠
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var tris = new List<int>();

        for (int k = 0; k < profile.Count; k++)
        {
            Vector2 n = Vector2.zero;
            if (k > 0) n += Perp(profile[k] - profile[k - 1]);
            if (k < profile.Count - 1) n += Perp(profile[k + 1] - profile[k]);
            n.Normalize();

            float v = (float)k / (profile.Count - 1);
            for (int i = 0; i < ring; i++)
            {
                float a = 2f * Mathf.PI / sides * i;
                vertices.Add(OnCircle(profile[k], a));
                normals.Add(new Vector3(n.x * Mathf.Cos(a), n.y, n.x * Mathf.Sin(a)));
                uvs.Add(new Vector2((float)i / sides, v));
            }
        }

        for (int k = 0; k < profile.Count - 1; k++)
        {
            for (int i = 0; i < sides; i++)
            {
                int b0 = k * ring + i, b1 = b0 + 1;
                int t0 = b0 + ring, t1 = t0 + 1;
                if (profile[k + 1].x > 0f) tris.AddRange(new[] { b0, t0, t1 });
                if (profile[k].x > 0f) tris.AddRange(new[] { b0, t1, b1 });
            }
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateBounds();
    }

    static Vector2 Perp(Vector2 d)
    {
        return new Vector2(d.y, -d.x).normalized;
    }

    static Vector3 OnCircle(Vector2 p, float angle)
    {
        return new Vector3(p.x * Mathf.Cos(angle), p.y, p.x * Mathf.Sin(angle));
    }
}
