using System;
using SneakyDance.Domain.Enums;
using SneakyDance.Domain.Interfaces;
using SneakyDance.Domain.ValueObjects;

namespace SneakyDance.Domain.Entities
{
    public sealed class TeacherAI
    {
        private readonly IRandom _random;
        private DifficultySettings _settings;
        private float _stateTimer;
        private float _currentStateDuration;

        public TeacherState CurrentState { get; private set; }
        public event Action<TeacherState> OnStateChanged;

        public TeacherAI(IRandom random, DifficultySettings settings)
        {
            _random = random;
            _settings = settings;
        }

        public void SetDifficulty(DifficultySettings settings)
        {
            _settings = settings;
        }

        public void Reset()
        {
            TransitionTo(TeacherState.WritingOnBoard);
        }

        public void Update(float deltaTime)
        {
            _stateTimer += deltaTime;

            if (_stateTimer >= _currentStateDuration)
            {
                AdvanceState();
            }
        }

        private void AdvanceState()
        {
            switch (CurrentState)
            {
                case TeacherState.WritingOnBoard:
                    TransitionTo(TeacherState.Warning);
                    break;
                case TeacherState.Warning:
                    TransitionTo(TeacherState.WatchingStudents);
                    break;
                case TeacherState.WatchingStudents:
                    TransitionTo(TeacherState.WritingOnBoard);
                    break;
            }
        }

        private void TransitionTo(TeacherState newState)
        {
            CurrentState = newState;
            _stateTimer = 0f;
            _currentStateDuration = CalculateDuration(newState);
            OnStateChanged?.Invoke(newState);
        }

        private float CalculateDuration(TeacherState state)
        {
            switch (state)
            {
                case TeacherState.WritingOnBoard:
                    return _random.Range(_settings.SafeTimeMin, _settings.SafeTimeMax);
                case TeacherState.Warning:
                    return _settings.WarningDuration;
                case TeacherState.WatchingStudents:
                    return _random.Range(_settings.DangerTimeMin, _settings.DangerTimeMax);
                default:
                    return 1f;
            }
        }
    }
}
