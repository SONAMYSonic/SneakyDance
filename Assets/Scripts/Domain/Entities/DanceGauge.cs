using System;
using SneakyDance.Domain.ValueObjects;

namespace SneakyDance.Domain.Entities
{
    public sealed class DanceGauge
    {
        public GaugeValue CurrentValue { get; private set; }
        public bool IsFull => CurrentValue.IsFull;

        public event Action<float> OnValueChanged;

        public DanceGauge()
        {
            CurrentValue = GaugeValue.Zero;
        }

        public void Increase(float deltaTime, float rate)
        {
            var previous = CurrentValue;
            CurrentValue = CurrentValue.Add(deltaTime * rate);

            if (!previous.Equals(CurrentValue))
                OnValueChanged?.Invoke(CurrentValue.Value);
        }

        public void Decrease(float deltaTime, float rate)
        {
            var previous = CurrentValue;
            CurrentValue = CurrentValue.Subtract(deltaTime * rate);

            if (!previous.Equals(CurrentValue))
                OnValueChanged?.Invoke(CurrentValue.Value);
        }

        public void Reset()
        {
            CurrentValue = GaugeValue.Zero;
            OnValueChanged?.Invoke(CurrentValue.Value);
        }
    }
}
