using System.Collections;
using System.Collections.Generic;
using Tacic.Tacic___Unity_Tools.MiniGame_Base.PlayAnimations_1AtSameTime_OnClickInRightSequence___In_Develop;
using UnityEngine;
using WebelinxGames.RoomsAndExits.MiniGames;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.PlayMultipleAnimationsAtOnce.v1_Custom_started
{
    public class StickManager : MiniGameManager
    {
        [Header("MiniGame Settings")]
        [SerializeField] private float miniGameFinishedWaitTime = 0.6f;
        [SerializeField] private float solveNextObjectWaitTime = 0.2f;
        [Header("Attributes")] 
        [SerializeField] private List<StickButton> buttons;
        [SerializeField] private Animator animator;
        [SerializeField] private int animationsAllowedAtOnce = 1;
        [Header("Sounds")]
        [SerializeField] private AudioSource miniGameFinishedSound;
        [SerializeField] private AudioSource buttonClickedSound;

        private bool isMiniGameSolved;
        private bool isAnimationPlaying;
        private int animationsCurrentlyPlaying;
        private AnimationClip[] animationClips;
        private readonly int animationTriggerString = Animator.StringToHash("Index");

        #region MonoBehaviour functions

        private void Awake()
        {
            animationClips = animator.runtimeAnimatorController.animationClips;
        }

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
            if (animationsCurrentlyPlaying <= animationsAllowedAtOnce)
            {
                yield return PlayAnimationAndWait(buttonIndex);
            }
            
            CheckIfCompletedAndFinishMiniGame();
        }

        #endregion
		
		#region Animations

        private IEnumerator PlayAnimationAndWait(int buttonIndex)
        {
            animationsCurrentlyPlaying++;
            animator.SetInteger(animationTriggerString, buttonIndex);
            AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            float animationLength = currentClipInfo[0].clip.length;
            yield return new WaitForSeconds(animationLength);
            animationsCurrentlyPlaying--;
        }
        
		#endregion
        
        #region Completing

        private void SetActiveMiniGameInput(bool isActive)
        {
            // foreach (var VARIABLE in COLLECTION)
            // {
            //     VARIABLE.IsClickable = isActive;
            // }
        }

        private bool IsMiniGameFinished()
        {
            return false;
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
            
            // Finished animations
            // foreach (var VARIABLE in COLLECTION)
            // {
            //     // Do something
            //     yield return solveNextObjectDelay;
            // }
            
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return new WaitForSeconds(miniGameFinishedWaitTime);
        }
        
        // Razmak 1 izmedju fja, razmak jedan posle closed blocka }

        #endregion
    }
}