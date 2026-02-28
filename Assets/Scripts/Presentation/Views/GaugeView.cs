using UnityEngine;
using UnityEngine.UI;

namespace SneakyDance.Presentation.Views
{
    public sealed class GaugeView : MonoBehaviour
    {
        private Image _fillImage;
        private Image _backgroundImage;

        private static readonly Color FillColor = new Color(1f, 0.75f, 0.2f);  // Warm gold

        private void Awake()
        {
            BuildGaugeBar();
        }

        public void SetVisible(bool visible)
        {
            if (_backgroundImage != null)
                _backgroundImage.gameObject.SetActive(visible);
        }

        public void UpdateGauge(float normalizedValue)
        {
            if (_fillImage == null)
                return;

            _fillImage.fillAmount = normalizedValue;
        }

        private void BuildGaugeBar()
        {
            // Background
            var bgGo = new GameObject("GaugeBG", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bgGo.transform.SetParent(transform, false);

            var bgRect = bgGo.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0.15f, 0);
            bgRect.anchorMax = new Vector2(0.85f, 0);
            bgRect.pivot = new Vector2(0.5f, 0);
            bgRect.anchoredPosition = new Vector2(0, 20);
            bgRect.sizeDelta = new Vector2(0, 35);

            _backgroundImage = bgGo.GetComponent<Image>();
            _backgroundImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            // Fill
            var fillGo = new GameObject("GaugeFill", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            fillGo.transform.SetParent(bgGo.transform, false);

            var fillRect = fillGo.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.sizeDelta = Vector2.zero;
            fillRect.anchoredPosition = Vector2.zero;

            _fillImage = fillGo.GetComponent<Image>();
            _fillImage.type = Image.Type.Filled;
            _fillImage.fillMethod = Image.FillMethod.Horizontal;
            _fillImage.fillOrigin = 0;
            _fillImage.fillAmount = 0f;
            _fillImage.color = FillColor;

            // Hide initially
            _backgroundImage.gameObject.SetActive(false);
        }
    }
}
