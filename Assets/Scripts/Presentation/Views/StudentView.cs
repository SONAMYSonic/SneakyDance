using UnityEngine;

namespace SneakyDance.Presentation.Views
{
    public sealed class StudentView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Sprite _idleSprite;
        private Sprite[] _spinSprites;

        private bool _isDancing;
        private float _animTimer;
        private int _currentFrame;
        private const float FrameDuration = 0.035f; // ~28fps for smooth spin

        private void Awake()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            LoadSprites();
        }

        private void LoadSprites()
        {
            // Load idle sprite
            _idleSprite = Resources.Load<Sprite>("oiia_idle");

            // Load spin sprite sheet (sliced into multiple sprites)
            _spinSprites = Resources.LoadAll<Sprite>("oiia_spin_sheet");

            if (_idleSprite != null)
            {
                _spriteRenderer.sprite = _idleSprite;
                _spriteRenderer.color = Color.white;
            }
            else
            {
                Debug.LogWarning("StudentView: oiia_idle sprite not found in Resources!");
            }

            if (_spinSprites == null || _spinSprites.Length == 0)
            {
                Debug.LogWarning("StudentView: oiia_spin_sheet sprites not found in Resources! " +
                    "Run Tools > Slice OIIA Sprite Sheet first.");
            }
            else
            {
                Debug.Log($"StudentView: Loaded {_spinSprites.Length} spin frames");
            }
        }

        private void Update()
        {
            if (!_isDancing || _spinSprites == null || _spinSprites.Length == 0)
                return;

            _animTimer += Time.deltaTime;
            if (_animTimer >= FrameDuration)
            {
                _animTimer -= FrameDuration;
                _currentFrame = (_currentFrame + 1) % _spinSprites.Length;
                _spriteRenderer.sprite = _spinSprites[_currentFrame];
            }
        }

        public void SetDancing(bool isDancing)
        {
            _isDancing = isDancing;

            if (isDancing)
            {
                _animTimer = 0f;
                _currentFrame = 0;
                _spriteRenderer.color = Color.white;

                if (_spinSprites != null && _spinSprites.Length > 0)
                    _spriteRenderer.sprite = _spinSprites[0];
            }
            else
            {
                _spriteRenderer.color = Color.white;

                if (_idleSprite != null)
                    _spriteRenderer.sprite = _idleSprite;
            }
        }
    }
}
