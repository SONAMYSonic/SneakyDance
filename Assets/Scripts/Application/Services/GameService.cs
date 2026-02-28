using System;
using SneakyDance.Application.Interfaces;
using SneakyDance.Domain.Entities;
using SneakyDance.Domain.Enums;
using SneakyDance.Domain.Interfaces;
using SneakyDance.Domain.ValueObjects;

namespace SneakyDance.Application.Services
{
    public sealed class GameService : IGameService
    {
        private readonly TeacherAI _teacherAI;
        private readonly DanceGauge _gauge;
        private readonly IInputProvider _input;
        private bool _wasStudentDancing;
        private float _remainingTime;

        public GameState CurrentGameState { get; private set; }
        public int CurrentStage { get; private set; }

        public event Action<GameState> OnGameStateChanged;
        public event Action<TeacherState> OnTeacherStateChanged;
        public event Action<float> OnGaugeChanged;
        public event Action<bool> OnStudentDancingChanged;
        public event Action<float, float> OnTimerChanged;

        public GameService(IInputProvider input, IRandom random)
        {
            _input = input;
            CurrentStage = 1;

            var settings = DifficultySettings.ForStage(CurrentStage);
            _teacherAI = new TeacherAI(random, settings);
            _gauge = new DanceGauge();

            _teacherAI.OnStateChanged += HandleTeacherStateChanged;
            _gauge.OnValueChanged += HandleGaugeChanged;

            SetGameState(GameState.Title);
        }

        public void StartGame()
        {
            CurrentStage = 1;
            ResetStage();
            SetGameState(GameState.Playing);
        }

        public void AdvanceToNextStage()
        {
            CurrentStage++;
            ResetStage();
            SetGameState(GameState.Playing);
        }

        public void Update(float deltaTime)
        {
            if (CurrentGameState != GameState.Playing)
                return;

            _teacherAI.Update(deltaTime);

            var currentSettings = DifficultySettings.ForStage(CurrentStage);
            _remainingTime -= deltaTime;
            OnTimerChanged?.Invoke(_remainingTime, currentSettings.TimeLimit);

            if (_remainingTime <= 0f)
            {
                _remainingTime = 0f;
                SetGameState(GameState.GameOver);
                return;
            }

            bool isDancing = _input.IsDancing;

            if (isDancing != _wasStudentDancing)
            {
                _wasStudentDancing = isDancing;
                OnStudentDancingChanged?.Invoke(isDancing);
            }

            if (isDancing)
            {
                if (_teacherAI.CurrentState == TeacherState.WatchingStudents)
                {
                    SetGameState(GameState.GameOver);
                    return;
                }

                _gauge.Increase(deltaTime, currentSettings.GaugeFillRate);
            }
            else
            {
                _gauge.Decrease(deltaTime, currentSettings.GaugeDrainRate);
            }

            if (_gauge.IsFull)
            {
                SetGameState(GameState.StageClear);
            }
        }

        private void ResetStage()
        {
            var settings = DifficultySettings.ForStage(CurrentStage);
            _teacherAI.SetDifficulty(settings);
            _teacherAI.Reset();
            _gauge.Reset();
            _wasStudentDancing = false;
            _remainingTime = settings.TimeLimit;
        }

        private void SetGameState(GameState newState)
        {
            CurrentGameState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        private void HandleTeacherStateChanged(TeacherState state)
        {
            OnTeacherStateChanged?.Invoke(state);
        }

        private void HandleGaugeChanged(float value)
        {
            OnGaugeChanged?.Invoke(value);
        }
    }
}
