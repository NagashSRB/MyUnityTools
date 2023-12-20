using System.Collections;
using UnityEngine;
using WebelinxGames.RoomsAndExits.MiniGames;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame
{
    [RequireComponent(typeof(Controller))]
    public class BoardMiniGameManager : MiniGameManager
    {
        [Header("Delays")] 
        [SerializeField] private float miniGameFinishedDelay = 0.6f;
        [SerializeField] private float solveMovementDuration = 2f;
        [SerializeField] private float solveNextElementDelay = 0.3f;
        [Header("Controllers")] 
        [SerializeField] private Controller controller;
        [Header("Mini Game Management Sounds")] 
        [SerializeField] private AudioSource miniGameFinishedSound;
        [Header("Finishing game animators")] 
        [SerializeField] private Animator solveHintAnimator;
        [SerializeField] private string solveHintAnimationName;
        [SerializeField] private Animator finishAnimator;
        [SerializeField] private string finishAnimationName;

        private new void OnValidate()
        {
            base.OnValidate();
            
            if (!controller)
            {
                controller = GetComponent<Controller>();
            }
        }

        private void Awake()
        {
            controller.Initialize(() => StartCoroutine(SolveAndFinishMiniGame(SolverType.Manual)));
        }
 
        protected override IEnumerator OnCompletedManuallySetFinalState()
        {
            controller.DisableAllColliders();

            if (finishAnimator != null && !string.IsNullOrWhiteSpace(finishAnimationName))
            {
                solveHintAnimator.Play(finishAnimationName, 0, 0);
                yield return new WaitForSeconds(GetAnimationLength(finishAnimationName));
            }

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
                solveHintAnimator.gameObject.SetActive(true);
                solveHintAnimator.Play(solveHintAnimationName, 0, 0);
                yield return new WaitForSeconds(GetAnimationLength(solveHintAnimationName));
            }
            else
            {
                yield return StartCoroutine(
                    controller.Solve(solveNextElementDelay, solveMovementDuration));
            }
        }

        private float GetAnimationLength(string animationName)
        {
            AnimationClip[] clips = solveHintAnimator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Equals(animationName))
                {
                    return clip.length;
                }
            }

            return 0;
        }
    }
}