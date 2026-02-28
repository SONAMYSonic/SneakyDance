using System;
using SneakyDance.Domain.Enums;

namespace SneakyDance.Application.Interfaces
{
    public interface IGameService
    {
        GameState CurrentGameState { get; }
        int CurrentStage { get; }

        void StartGame();
        void Update(float deltaTime);
        void AdvanceToNextStage();

        event Action<GameState> OnGameStateChanged;
        event Action<TeacherState> OnTeacherStateChanged;
        event Action<float> OnGaugeChanged;
        event Action<bool> OnStudentDancingChanged;
        event Action<float, float> OnTimerChanged;  // (remainingTime, timeLimit)
    }
}
