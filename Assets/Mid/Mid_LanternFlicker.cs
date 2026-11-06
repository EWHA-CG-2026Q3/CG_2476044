using UnityEngine;

// 석등 불빛이 촛불처럼 일렁이게 함: Light 세기와 화창 재질의 Emission을 같은 노이즈로 흔듦
[RequireComponent(typeof(Light))]
public class Mid_LanternFlicker : MonoBehaviour
{
    [SerializeField] Renderer lantern;
    [SerializeField] int glowMaterialIndex = 1;
    [SerializeField] float amount = 0.25f;
    [SerializeField] float speed = 6f;

    Light lamp;
    float baseIntensity;
    Color baseEmission;
    MaterialPropertyBlock block;
    float seed;

    void Start()
    {
        lamp = GetComponent<Light>();
        baseIntensity = lamp.intensity;
        seed = Random.value * 100f;
        block = new MaterialPropertyBlock();
        if (lantern != null && lantern.sharedMaterials.Length > glowMaterialIndex)
            baseEmission = lantern.sharedMaterials[glowMaterialIndex].GetColor("_EmissionColor");
    }

    void Update()
    {
        float n = Mathf.PerlinNoise(seed, Time.time * speed) - 0.5f;
        float k = 1f + n * 2f * amount;

        lamp.intensity = baseIntensity * k;

        if (lantern != null)
        {
            lantern.GetPropertyBlock(block, glowMaterialIndex);
            block.SetColor("_EmissionColor", baseEmission * k);
            lantern.SetPropertyBlock(block, glowMaterialIndex);
        }
    }
}
