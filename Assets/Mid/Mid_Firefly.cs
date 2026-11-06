using UnityEngine;

// 반딧불: center 주위를 원 궤도로 돌면서 위아래로 출렁임
// 위치는 이동, Y축 회전, 이동 행렬을 곱한 결과의 4열에서 꺼냄
[ExecuteAlways]
public class Mid_Firefly : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float radius = 0.8f;
    [SerializeField] float height = 1.2f;
    [SerializeField] float orbitSpeed = 40f;
    [SerializeField] float bobAmount = 0.15f;
    [SerializeField] float bobSpeed = 2.3f;
    [SerializeField] float phase = 0f;          // 반딧불마다 다르게

    void Update()
    {
        if (center == null) return;

        float t = Application.isPlaying ? Time.time : 0f;
        float angle = phase + orbitSpeed * t;
        float r = radius * (1f + 0.15f * Mathf.Sin(t * 0.7f + phase));
        float bob = bobAmount * Mathf.Sin(t * bobSpeed + phase * Mathf.Deg2Rad);

        Matrix4x4 m = Matrix4x4.Translate(center.position)
                    * Matrix4x4.Rotate(Quaternion.Euler(0f, angle, 0f))
                    * Matrix4x4.Translate(new Vector3(r, height + bob, 0f));

        transform.position = m.GetColumn(3);
    }
}
