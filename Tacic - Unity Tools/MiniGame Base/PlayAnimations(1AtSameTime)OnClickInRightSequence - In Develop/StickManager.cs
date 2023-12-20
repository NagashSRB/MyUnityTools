using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebelinxGames.RoomsAndExits.MiniGames;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.PlayAnimations_1AtSameTime_OnClickInRightSequence___In_Develop
{
    public class StickManager : MiniGameManager
    {
        [Header("MiniGame Settings")]
        [SerializeField] private float miniGameFinishedWaitTime = 0.6f;
        [SerializeField] private float solveNextObjectWaitTime = 0.2f;
        [Header("Attributes")] 
        [SerializeField] private List<StickButton> buttons;
        [SerializeField] private List<int> targetSequence;
        [SerializeField] private Animator animator;
        [Header("Sounds")]
        [SerializeField] private AudioSource miniGameFinishedSound;
        [SerializeField] private AudioSource buttonClickedSound;

        private bool isMiniGameSolved;
        private bool isAnimationPlaying;
        private int currentSequenceIndex;
        private readonly int animationTriggerString = Animator.StringToHash("Index");
        private List<int> enteredSequence = new List<int>();

        #region MonoBehaviour functions

        private void Start()
        {
            for (var i = 0; i < buttons.Count; i++)
            {
                StickButton button = buttons[i];
                button.OnButtonClicked += HandleButtonClicked;
                button.Index = i;
            }

            SetActiveMiniGameInput(true);
        }

        #endregion

        #region Callbacks

        private void HandleButtonClicked(int buttonIndex)
        {
            StartCoroutine(PlayAndWaitForAnimations(buttonIndex));
        }

        #endregion
        
        #region Logic

        private IEnumerator PlayAndWaitForAnimations(int buttonIndex)
        {
            if (isAnimationPlaying)
            {
                yield break;
            }
            
            AddNumberToSequence(buttonIndex);
            yield return PlayAnimationAndWait(buttonIndex);
            CheckIfCompletedAndFinishMiniGame();
            if (!isMiniGameSolved && enteredSequence.Count == targetSequence.Count)
            {
                enteredSequence.Clear();
            }
        }

        private void AddNumberToSequence(int buttonIndex)
        {
            enteredSequence.Add(buttonIndex);
        }

        #endregion
		
		#region Animations

        private IEnumerator PlayAnimationAndWait(int buttonIndex)
        {
            isAnimationPlaying = true;
            animator.SetInteger(animationTriggerString, buttonIndex);
            AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            float animationLength = currentClipInfo[0].clip.length;
            yield return new WaitForSeconds(animationLength);
            isAnimationPlaying = false;
        }
        
		#endregion
        
        #region Completing

        private void SetActiveMiniGameInput(bool isActive)
        {
            foreach (StickButton button in buttons)
            {
                button.SetClickable(isActive);
            }
        }

        private bool IsMiniGameFinished()
        {
            return enteredSequence.SequenceEqual(targetSequence);
        }

        private void CheckIfCompletedAndFinishMiniGame()
        {
            if (isMiniGameSolved)
            {
                return;
            }
            
            if (IsMiniGameFinished())
            {
                isMiniGameSolved = true;
                StartCoroutine(SolveAndFinishMiniGame(SolverType.Manual));
            }
        }

        #endregion
        
        #region MiniGameDefaultFunctions

        protected override IEnumerator OnCompletedManuallySetFinalState()
        {
            SetActiveMiniGameInput(false);
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return new WaitForSeconds(miniGameFinishedWaitTime);
        }

        protected override IEnumerator OnHintUsedSetFinalState()
        {
            SetActiveMiniGameInput(false);
            WaitForSeconds solveNextObjectDelay = new WaitForSeconds(solveNextObjectWaitTime);

            foreach (int index in targetSequence)
            {
                // Should play every animation?
            }
            
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return new WaitForSeconds(miniGameFinishedWaitTime);
        }
        #endregion
    }
}