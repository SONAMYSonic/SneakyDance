using System;
using UnityEngine;
using UnityEngine.UI;
using SneakyDance.Domain.Enums;

namespace SneakyDance.Presentation.Views
{
    public sealed class UIView : MonoBehaviour
    {
        private GameObject _titlePanel;
        private GameObject _gameOverPanel;
        private GameObject _clearPanel;

        private Button _startButton;
        private Button _retryButton;
        private Button _nextStageButton;

        private Text _stageLabel;
        private Text _statusLabel;
        private Text _timerLabel;

        public event Action OnStartClicked;
        public event Action OnRetryClicked;
        public event Action OnNextStageClicked;

        private void Awake()
        {
            BuildUI();
            _startButton.onClick.AddListener(() => OnStartClicked?.Invoke());
            _retryButton.onClick.AddListener(() => OnRetryClicked?.Invoke());
            _nextStageButton.onClick.AddListener(() => OnNextStageClicked?.Invoke());
        }

        public void UpdateGameState(GameState state, int stage)
        {
            _titlePanel.SetActive(state == GameState.Title);
            _gameOverPanel.SetActive(state == GameState.GameOver);
            _clearPanel.SetActive(state == GameState.StageClear);

            _stageLabel.gameObject.SetActive(state == GameState.Playing);
            _statusLabel.gameObject.SetActive(state == GameState.Playing);
            _timerLabel.gameObject.SetActive(state == GameState.Playing);

            if (state == GameState.Playing)
            {
                _stageLabel.text = $"Stage {stage}";
            }
        }

        public void UpdateTimer(float remaining, float total)
        {
            int seconds = Mathf.CeilToInt(Mathf.Max(0f, remaining));
            _timerLabel.text = $"{seconds}s";

            if (remaining <= 5f)
                _timerLabel.color = new Color(0.9f, 0.2f, 0.2f);
            else if (remaining <= 10f)
                _timerLabel.color = new Color(0.9f, 0.8f, 0.1f);
            else
                _timerLabel.color = Color.white;
        }

        public void UpdateTeacherStatus(TeacherState state)
        {
            switch (state)
            {
                case TeacherState.WritingOnBoard:
                    _statusLabel.text = "Butler is busy...  Go!";
                    _statusLabel.color = new Color(0.2f, 0.8f, 0.2f);
                    break;
                case TeacherState.Warning:
                    _statusLabel.text = "Butler is suspicious...!";
                    _statusLabel.color = new Color(0.9f, 0.8f, 0.1f);
                    break;
                case TeacherState.WatchingStudents:
                    _statusLabel.text = "Butler is WATCHING!";
                    _statusLabel.color = new Color(0.9f, 0.2f, 0.2f);
                    break;
            }
        }

        private void BuildUI()
        {
            // Stage Label (top-left)
            _stageLabel = CreateLabel("StageLabel", "Stage 1", 28,
                new Vector2(0, 1), new Vector2(0, 1),
                new Vector2(160, -30), new Vector2(300, 50));
            _stageLabel.alignment = TextAnchor.UpperLeft;

            // Status Label (top-center)
            _statusLabel = CreateLabel("StatusLabel", "SAFE - Dance!", 40,
                new Vector2(0.5f, 1), new Vector2(0.5f, 1),
                new Vector2(0, -30), new Vector2(500, 60));
            _statusLabel.alignment = TextAnchor.UpperCenter;

            // Timer Label (top-right)
            _timerLabel = CreateLabel("TimerLabel", "30s", 32,
                new Vector2(1, 1), new Vector2(1, 1),
                new Vector2(-100, -30), new Vector2(200, 50));
            _timerLabel.alignment = TextAnchor.UpperRight;

            // Hide gameplay labels initially
            _stageLabel.gameObject.SetActive(false);
            _statusLabel.gameObject.SetActive(false);
            _timerLabel.gameObject.SetActive(false);

            // Title Panel
            _titlePanel = CreatePanel("TitlePanel");
            CreateLabel("TitleText", "Sneaky Cat!", 60,
                new Vector2(0.5f, 0.6f), new Vector2(0.5f, 0.6f),
                Vector2.zero, new Vector2(800, 80),
                _titlePanel.transform);
            CreateLabel("SubTitleText", "Make mischief while the butler isn't looking!", 24,
                new Vector2(0.5f, 0.45f), new Vector2(0.5f, 0.45f),
                Vector2.zero, new Vector2(600, 50),
                _titlePanel.transform);
            _startButton = CreateButton("StartButton", "START",
                new Vector2(0.5f, 0.3f), new Vector2(0.5f, 0.3f),
                Vector2.zero, new Vector2(300, 70),
                _titlePanel.transform,
                new Color(0.2f, 0.7f, 0.3f));

            // GameOver Panel
            _gameOverPanel = CreatePanel("GameOverPanel");
            CreateLabel("GameOverText", "CAUGHT!", 60,
                new Vector2(0.5f, 0.6f), new Vector2(0.5f, 0.6f),
                Vector2.zero, new Vector2(800, 80),
                _gameOverPanel.transform,
                new Color(1f, 0.3f, 0.3f));
            CreateLabel("CaughtText", "The butler caught you red-pawed!", 26,
                new Vector2(0.5f, 0.45f), new Vector2(0.5f, 0.45f),
                Vector2.zero, new Vector2(600, 50),
                _gameOverPanel.transform);
            _retryButton = CreateButton("RetryButton", "RETRY",
                new Vector2(0.5f, 0.3f), new Vector2(0.5f, 0.3f),
                Vector2.zero, new Vector2(300, 70),
                _gameOverPanel.transform,
                new Color(0.8f, 0.3f, 0.3f));
            _gameOverPanel.SetActive(false);

            // Clear Panel
            _clearPanel = CreatePanel("ClearPanel");
            CreateLabel("ClearText", "Mischief Complete!", 54,
                new Vector2(0.5f, 0.6f), new Vector2(0.5f, 0.6f),
                Vector2.zero, new Vector2(800, 80),
                _clearPanel.transform,
                new Color(0.3f, 0.9f, 1f));
            _nextStageButton = CreateButton("NextButton", "NEXT STAGE",
                new Vector2(0.5f, 0.3f), new Vector2(0.5f, 0.3f),
                Vector2.zero, new Vector2(400, 70),
                _clearPanel.transform,
                new Color(0.2f, 0.6f, 0.9f));
            _clearPanel.SetActive(false);
        }

        private GameObject CreatePanel(string name)
        {
            var panel = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            panel.transform.SetParent(transform, false);

            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            var image = panel.GetComponent<Image>();
            image.color = new Color(0, 0, 0, 0.75f);

            return panel;
        }

        private Text CreateLabel(string name, string text, int fontSize,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size,
            Transform parent = null, Color? color = null)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            go.transform.SetParent(parent ?? transform, false);

            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;

            var label = go.GetComponent<Text>();
            label.text = text;
            label.fontSize = fontSize;
            label.color = color ?? Color.white;
            label.alignment = TextAnchor.MiddleCenter;
            label.fontStyle = FontStyle.Bold;
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (label.font == null)
                label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            if (label.font == null)
                label.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
            label.horizontalOverflow = HorizontalWrapMode.Overflow;

            return label;
        }

        private Button CreateButton(string name, string label,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size,
            Transform parent, Color bgColor)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);

            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;

            var image = go.GetComponent<Image>();
            image.color = bgColor;

            var buttonLabel = CreateLabel(name + "Label", label, 30,
                Vector2.zero, Vector2.one,
                Vector2.zero, Vector2.zero,
                go.transform);
            var labelRect = buttonLabel.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.sizeDelta = Vector2.zero;

            return go.GetComponent<Button>();
        }
    }
}
