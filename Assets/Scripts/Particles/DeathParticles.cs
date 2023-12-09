using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    /**
     * Initialize the Death Particles with provided 
     * emission Color (will be multiplied with 2 for increased glow effect)
     * and the size of the particles.
     */
    public void Init(Color emissionColor, float size)
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        Renderer renderer = particleSystem.GetComponent<Renderer>();

        ShapeMonoBehaviour.SetMaterialEmission(renderer.material, emissionColor);

        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startSize = size;
    }
}