using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame
{
    [RequireComponent(typeof(BoardMiniGameManager))]
    public abstract class Controller : MonoBehaviour
    {
        private UnityAction onGameFinished;

        public void Initialize(UnityAction handleGameFinished)
        {
            onGameFinished = handleGameFinished;
        }

        public abstract void EnableAllColliders();
        
        public abstract void DisableAllColliders();
        
        protected virtual void CheckIfGameFinished()
        {
            if (IsStateCorrect())
            {
                onGameFinished?.Invoke();
            }
        }
        
        protected abstract bool IsStateCorrect();

        public abstract IEnumerator Solve(float solveNextElementWaitTime, float solveMovementDuration);

        public abstract void SetFinalState();
    }
}