
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class GalaxyGenerator : MonoBehaviour
{
    public int starCount = 5000;
    public int armCount = 4;
    public float galaxyRadius = 8f;
    public float rotationSpeed = 3f;

    private ParticleSystem ps;
    private ParticleSystem.Particle[] stars;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        var main = ps.main;
        main.playOnAwake = false;
        main.loop = false;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.maxParticles = starCount;
        main.startLifetime = 1000f;
        main.startSpeed = 0f;

        stars = new ParticleSystem.Particle[starCount];

        for (int i = 0; i < starCount; i++)
        {
            float distance = Random.value;
            distance = Mathf.Pow(distance, 0.65f)
                       * galaxyRadius;

            float armAngle =
                Mathf.PI * 2f * (i % armCount) / armCount;

            float twist = distance * 0.75f;
            float scatter = (1f - distance / galaxyRadius)
                            * 0.45f;

            float angle = armAngle + twist
                          + Random.Range(-scatter, scatter);

            Vector3 position = new Vector3(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance * 0.48f,
                Random.Range(-0.15f, 0.15f)
            );

            float core = 1f - distance / galaxyRadius;
            Color color;

            if (distance < 1.2f)
            {
                color = Color.Lerp(
                    new Color(1f, 0.45f, 0.08f),
                    new Color(1f, 0.95f, 0.75f),
                    Random.value
                );
            }
            else
            {
                color = Color.Lerp(
                    new Color(0.12f, 0.25f, 1f),
                    new Color(0.4f, 0.85f, 1f),
                    Random.value
                );

                if (Random.value < 0.12f)
                    color = new Color(0.8f, 0.35f, 1f);
            }

            stars[i].position = position;
            stars[i].startColor = color;
            stars[i].startSize = Random.Range(0.015f, 0.06f)
                                 + core * 0.025f;
            stars[i].startLifetime = 1000f;
            stars[i].remainingLifetime = 1000f;
        }

        ps.SetParticles(stars, stars.Length);
    }

    void Update()
    {
        transform.Rotate(
            0f, 0f, rotationSpeed * Time.deltaTime
        );
    }
}
