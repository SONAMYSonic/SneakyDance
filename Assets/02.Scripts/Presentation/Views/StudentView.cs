using UnityEngine;

namespace SneakyDance.Presentation.Views
{
    public sealed class StudentView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AudioClip _danceMusic;

        private Sprite _idleSprite;
        private Sprite[] _spinSprites;
        private AudioSource _audioSource;

        private bool _isDancing;
        private float _animTimer;
        private int _currentFrame;
        private float _frameDuration;

        private static readonly float[] SpeedVariants = new float[]
        {
            0.07f,   // 느린 버전 (~14fps)
            0.035f,  // 보통 버전 (~28fps)
            0.018f,  // 빠른 버전 (~55fps)
        };

        private void Awake()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();

            _audioSource.loop = true;
            _audioSource.playOnAwake = false;

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
            if (_animTimer >= _frameDuration)
            {
                _animTimer -= _frameDuration;
                _currentFrame = (_currentFrame + 1) % _spinSprites.Length;
                _spriteRenderer.sprite = _spinSprites[_currentFrame];
            }
        }

        /// <summary>
        /// 음악을 처음부터 다시 시작하도록 리셋합니다 (스테이지 시작/게임오버 등).
        /// </summary>
        public void ResetMusic()
        {
            if (_audioSource != null)
            {
                _audioSource.Stop();
                _audioSource.time = 0f;
            }
        }

        public void SetDancing(bool isDancing)
        {
            _isDancing = isDancing;

            if (isDancing)
            {
                _frameDuration = SpeedVariants[Random.Range(0, SpeedVariants.Length)];
                _animTimer = 0f;
                _currentFrame = 0;
                _spriteRenderer.color = Color.white;

                if (_spinSprites != null && _spinSprites.Length > 0)
                    _spriteRenderer.sprite = _spinSprites[0];

                // 음악 재개 (일시정지 지점부터 이어서 재생)
                if (_audioSource != null && _danceMusic != null)
                {
                    if (_audioSource.clip != _danceMusic)
                        _audioSource.clip = _danceMusic;

                    _audioSource.UnPause();
                    if (!_audioSource.isPlaying)
                        _audioSource.Play();
                }
            }
            else
            {
                _spriteRenderer.color = Color.white;

                if (_idleSprite != null)
                    _spriteRenderer.sprite = _idleSprite;

                // 음악 일시정지 (위치 유지)
                if (_audioSource != null && _audioSource.isPlaying)
                    _audioSource.Pause();
            }
        }
    }
}
