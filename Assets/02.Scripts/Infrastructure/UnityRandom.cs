using SneakyDance.Domain.Interfaces;

namespace SneakyDance.Infrastructure
{
    public sealed class UnityRandom : IRandom
    {
        public float Range(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}
