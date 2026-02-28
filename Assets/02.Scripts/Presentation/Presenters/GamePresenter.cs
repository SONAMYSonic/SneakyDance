using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SneakyDance.Application.Interfaces;
using SneakyDance.Application.Services;
using SneakyDance.Domain.Enums;
using SneakyDance.Infrastructure;
using SneakyDance.Infrastructure.Input;
using SneakyDance.Presentation.Views;

namespace SneakyDance.Presentation.Presenters
{
    public sealed class GamePresenter : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] private TeacherView _teacherView;
        [SerializeField] private StudentView _studentView;
        [SerializeField] private GaugeView _gaugeView;
        [SerializeField] private UIView _uiView;

        private IGameService _gameService;

        private void Awake()
        {
            ResolveViews();
            EnsureEventSystem();

            var inputProvider = new UnityInputProvider();
            var random = new UnityRandom();

            _gameService = new GameService(inputProvider, random);

            _gameService.OnGameStateChanged += HandleGameStateChanged;
            _gameService.OnTeacherStateChanged += HandleTeacherStateChanged;
            _gameService.OnGaugeChanged += HandleGaugeChanged;
            _gameService.OnStudentDancingChanged += HandleStudentDancingChanged;
            _gameService.OnTimerChanged += HandleTimerChanged;

            _uiView.OnStartClicked += HandleStartClicked;
            _uiView.OnRetryClicked += HandleRetryClicked;
            _uiView.OnNextStageClicked += HandleNextStageClicked;
        }

        private void Update()
        {
            _gameService.Update(Time.deltaTime);
        }

        private void ResolveViews()
        {
            if (_teacherView == null)
                _teacherView = FindAnyObjectByType<TeacherView>();

            if (_studentView == null)
                _studentView = FindAnyObjectByType<StudentView>();

            var canvas = FindAnyObjectByType<Canvas>();
            if (canvas != null)
            {
                if (_gaugeView == null)
                {
                    _gaugeView = canvas.GetComponentInChildren<GaugeView>();
                    if (_gaugeView == null)
                        _gaugeView = canvas.gameObject.AddComponent<GaugeView>();
                }

                if (_uiView == null)
                {
                    _uiView = canvas.GetComponentInChildren<UIView>();
                    if (_uiView == null)
                        _uiView = canvas.gameObject.AddComponent<UIView>();
                }
            }
        }

        private void EnsureEventSystem()
        {
            if (FindAnyObjectByType<EventSystem>() == null)
            {
                var esGo = new GameObject("EventSystem",
                    typeof(EventSystem),
                    typeof(StandaloneInputModule));
            }
        }

        private void HandleGameStateChanged(GameState state)
        {
            _uiView.UpdateGameState(state, _gameService.CurrentStage);
            _gaugeView.SetVisible(state == GameState.Playing);

            if (state == GameState.Playing)
            {
                _studentView.ResetMusic();
                _studentView.SetDancing(false);
            }
            else if (state == GameState.GameOver || state == GameState.StageClear)
            {
                _studentView.SetDancing(false);
                _studentView.ResetMusic();
            }
        }

        private void HandleTeacherStateChanged(TeacherState state)
        {
            _teacherView.UpdateState(state);
            _uiView.UpdateTeacherStatus(state);
        }

        private void HandleGaugeChanged(float value)
        {
            _gaugeView.UpdateGauge(value);
        }

        private void HandleStudentDancingChanged(bool isDancing)
        {
            _studentView.SetDancing(isDancing);
        }

        private void HandleTimerChanged(float remaining, float total)
        {
            _uiView.UpdateTimer(remaining, total);
        }

        private void HandleStartClicked()
        {
            _gameService.StartGame();
        }

        private void HandleRetryClicked()
        {
            _gameService.StartGame();
        }

        private void HandleNextStageClicked()
        {
            _gameService.AdvanceToNextStage();
        }

        private void OnDestroy()
        {
            if (_gameService != null)
            {
                _gameService.OnGameStateChanged -= HandleGameStateChanged;
                _gameService.OnTeacherStateChanged -= HandleTeacherStateChanged;
                _gameService.OnGaugeChanged -= HandleGaugeChanged;
                _gameService.OnStudentDancingChanged -= HandleStudentDancingChanged;
                _gameService.OnTimerChanged -= HandleTimerChanged;
            }

            if (_uiView != null)
            {
                _uiView.OnStartClicked -= HandleStartClicked;
                _uiView.OnRetryClicked -= HandleRetryClicked;
                _uiView.OnNextStageClicked -= HandleNextStageClicked;
            }
        }
    }
}
