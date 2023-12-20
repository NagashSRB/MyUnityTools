using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebelinxGames.RoomsAndExits.MiniGames;

namespace WebelinxGames.RoomsAndExits.Level9999
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(ReusableController))]
    public class ReusableManager : MiniGameManager
    {
        [Header("Delays")] 
        [SerializeField] private float miniGameFinishedDelay = 0.6f;
        [SerializeField] private float solveMovementDuration = 2f;
        [SerializeField] private float solveNextElementDelay = 0.3f;
        [Header("Controllers")] 
        [SerializeField] private ReusableController controller;
        [Header("Mini Game Management Sounds")] 
        [SerializeField] private AudioSource miniGameFinishedSound;
        [Header("Solve Hint Animator")] 
        [SerializeField] private Animator solveHintAnimator;
        [SerializeField] private string solveHintAnimationName;

        private void Awake()
        {
            controller.onCheckIfFinished += CheckIfCompletedAndFinishMiniGame;
        }
        
        private void CheckIfCompletedAndFinishMiniGame()
        {
            if (controller.IsSolved())
            {
                StartCoroutine(SolveAndFinishMiniGame(SolverType.Manual));
            }
        }

        #region MiniGameDefaultFunctions

        protected override IEnumerator OnCompletedManuallySetFinalState()
        {
            controller.DisableAllColliders();
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return new WaitForSeconds(miniGameFinishedDelay);
        }

        protected override IEnumerator OnHintUsedSetFinalState()
        {
            controller.DisableAllColliders();
            yield return SolveMiniGame();
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return new WaitForSeconds(miniGameFinishedDelay);
        }

        private IEnumerator SolveMiniGame()
        {
            if (LevelProgressController.Instance.IsRestoring)
            {
                controller.SetFinalState();
                yield break;
            }

            if (solveHintAnimator != null && !string.IsNullOrWhiteSpace(solveHintAnimationName))
            {
                solveHintAnimator.Play(solveHintAnimationName, 0, 0);
            }
            else
            {
                yield return StartCoroutine(
                    controller.Solve(solveNextElementDelay, solveMovementDuration));
            }
        }

        #endregion
    }
}