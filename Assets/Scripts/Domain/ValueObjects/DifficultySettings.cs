using System;

namespace SneakyDance.Domain.ValueObjects
{
    public sealed class DifficultySettings
    {
        public float SafeTimeMin { get; }
        public float SafeTimeMax { get; }
        public float WarningDuration { get; }
        public float DangerTimeMin { get; }
        public float DangerTimeMax { get; }
        public float GaugeFillRate { get; }
        public float GaugeDrainRate { get; }
        public float TimeLimit { get; }

        public DifficultySettings(
            float safeTimeMin, float safeTimeMax,
            float warningDuration,
            float dangerTimeMin, float dangerTimeMax,
            float gaugeFillRate, float gaugeDrainRate,
            float timeLimit)
        {
            SafeTimeMin = safeTimeMin;
            SafeTimeMax = safeTimeMax;
            WarningDuration = warningDuration;
            DangerTimeMin = dangerTimeMin;
            DangerTimeMax = dangerTimeMax;
            GaugeFillRate = gaugeFillRate;
            GaugeDrainRate = gaugeDrainRate;
            TimeLimit = timeLimit;
        }

        public static DifficultySettings ForStage(int stage)
        {
            int index = Math.Max(0, stage - 1);

            float safeMin = Math.Max(1.5f, 4f - index * 0.7f);
            float safeMax = Math.Max(3f, 6f - index * 0.7f);
            float warning = Math.Max(0.2f, 0.5f - index * 0.05f);
            float dangerMin = Math.Min(3f, 1f + index * 0.3f);
            float dangerMax = Math.Min(4f, 2f + index * 0.3f);
            float fillRate = Math.Max(0.1f, 0.2f - index * 0.015f);
            float drainRate = 0.05f;
            float timeLimit = Math.Max(20f, 30f - index * 3f);

            return new DifficultySettings(
                safeMin, safeMax,
                warning,
                dangerMin, dangerMax,
                fillRate, drainRate,
                timeLimit);
        }
    }
}
