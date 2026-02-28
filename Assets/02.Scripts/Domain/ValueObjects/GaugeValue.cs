using System;

namespace SneakyDance.Domain.ValueObjects
{
    public readonly struct GaugeValue : IEquatable<GaugeValue>
    {
        public float Value { get; }
        public bool IsFull => Value >= 1f;
        public bool IsEmpty => Value <= 0f;

        public static GaugeValue Zero => new GaugeValue(0f);
        public static GaugeValue Full => new GaugeValue(1f);

        public GaugeValue(float value)
        {
            Value = Math.Clamp(value, 0f, 1f);
        }

        public GaugeValue Add(float amount)
        {
            return new GaugeValue(Value + amount);
        }

        public GaugeValue Subtract(float amount)
        {
            return new GaugeValue(Value - amount);
        }

        public bool Equals(GaugeValue other)
        {
            return Math.Abs(Value - other.Value) < float.Epsilon;
        }

        public override bool Equals(object obj)
        {
            return obj is GaugeValue other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value:P0}";
        }
    }
}
