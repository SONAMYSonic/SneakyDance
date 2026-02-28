using UnityEngine;
using SneakyDance.Domain.Interfaces;

namespace SneakyDance.Infrastructure.Input
{
    public sealed class UnityInputProvider : IInputProvider
    {
        public bool IsDancing => UnityEngine.Input.GetKey(KeyCode.Space);
    }
}
