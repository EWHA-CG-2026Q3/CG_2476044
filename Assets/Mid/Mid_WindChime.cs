using UnityEngine;

// 고리, 종, 물고기 추 순서로 행렬을 곱해서 흔들림을 아래로 전달
// bell, fish는 Hierarchy의 부모-자식으로 묶지 않고, 여기서 계산한 행렬로 위치와 회전을 정함
[ExecuteAlways]
public class Mid_WindChime : MonoBehaviour
{
    [SerializeField] Transform bell;
    [SerializeField] Transform fish;
    [SerializeField] LineRenderer rope;

    [SerializeField] float ropeLength = 0.15f;
    [SerializeField] float bellHeight = 0.18f;
    [SerializeField] float clapperLength = 0.12f;

    [SerializeField] float bellSwing = 10f;
    [SerializeField] float fishSwing = 25f;
    [SerializeField] float speed = 1.6f;

    void LateUpdate()
    {
        float t = Application.isPlaying ? Time.time : 0f;
        float gust = Mathf.PerlinNoise(t * 0.3f, 0f) * 1.5f;

        float bellAngle = bellSwing * gust * Mathf.Sin(t * speed);
        float fishAngleZ = fishSwing * gust * Mathf.Sin(t * speed * 1.7f + 0.8f);
        float fishAngleX = fishSwing * 0.5f * gust * Mathf.Sin(t * speed * 1.3f);

        Matrix4x4 hook = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        Matrix4x4 bellM = hook
                        * Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, bellAngle))
                        * Matrix4x4.Translate(new Vector3(0f, -ropeLength, 0f));

        Matrix4x4 fishM = bellM
                        * Matrix4x4.Translate(new Vector3(0f, -bellHeight, 0f))
                        * Matrix4x4.Rotate(Quaternion.Euler(fishAngleX, 0f, fishAngleZ))
                        * Matrix4x4.Translate(new Vector3(0f, -clapperLength, 0f));

        Apply(bell, bellM);
        Apply(fish, fishM);

        if (rope != null)
        {
            rope.positionCount = 3;
            rope.useWorldSpace = true;
            rope.SetPosition(0, hook.GetColumn(3));
            rope.SetPosition(1, bellM.GetColumn(3));
            rope.SetPosition(2, fishM.GetColumn(3));
        }
    }

    static void Apply(Transform target, Matrix4x4 m)
    {
        if (target == null) return;
        target.SetPositionAndRotation(m.GetColumn(3), m.rotation);
    }
}
