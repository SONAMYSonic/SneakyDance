using UnityEngine;
using SneakyDance.Domain.Enums;

namespace SneakyDance.Presentation.Views
{
    public sealed class TeacherView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private static readonly Color WritingColor = new Color(0.25f, 0.2f, 0.35f);  // Butler relaxed (dark purple)
        private static readonly Color WarningColor = new Color(0.5f, 0.35f, 0.2f);  // Butler suspicious (brown)
        private static readonly Color WatchingColor = new Color(0.8f, 0.15f, 0.15f); // Butler angry (red)

        private Vector3 _originalScale;

        private void Awake()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            _originalScale = transform.localScale;
        }

        public void UpdateState(TeacherState state)
        {
            switch (state)
            {
                case TeacherState.WritingOnBoard:
                    _spriteRenderer.color = WritingColor;
                    transform.localScale = _originalScale;
                    break;
                case TeacherState.Warning:
                    _spriteRenderer.color = WarningColor;
                    transform.localScale = _originalScale * 1.05f;
                    break;
                case TeacherState.WatchingStudents:
                    _spriteRenderer.color = WatchingColor;
                    transform.localScale = _originalScale * 1.1f;
                    break;
            }
        }
    }
}
