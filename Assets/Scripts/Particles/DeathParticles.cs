using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DeathParticles : MonoBehaviour
{
    public void Init(Color emissionColor, float size)
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        Renderer renderer = particleSystem.GetComponent<Renderer>();
        renderer.material.SetColor("_EmissionColor", emissionColor * 2);

        MainModule mainModule = particleSystem.main;
        mainModule.startSize = size;
    }
}