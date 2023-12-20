using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebelinxGames.RoomsAndExits.MiniGames;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.SwapElementsByDragging.v2___Custom___215
{
    public class MasksHintsManager : MiniGameManager
    {
        #region MiniGameAttributes
        
        [Header("MiniGame Management Attributes")] 
        [SerializeField] private float miniGameFinishedDelay = 0.6f;
        [SerializeField] private float draggingSpeed = 3f;
        private WaitForSeconds miniGameDelay;

        #endregion
        
        [Header("Attributes")]
        [SerializeField] private List<MaskHint> maskHints;

        private MaskHint selectedHint;
        [Header("Sounds")]
        [SerializeField] private AudioSource miniGameFinishedSound;
        [SerializeField] private AudioSource objectPickedUpSound;
        [SerializeField] private AudioSource objectReleasedSound;

        private bool isMiniGameSolved;
        private const float ReturnAnimationDuration = 0.35f;

        #region MonoBehaviour functions

        private void Awake()
        {
            // private Lists init
            miniGameDelay = new WaitForSeconds(miniGameFinishedDelay);
        }
        
        private void Start()
        {
            foreach (MaskHint maskHint in maskHints)
            {
                maskHint.OnTargetFound += HandleTargetFound;
                maskHint.OnClicked += HandleOnClicked;
                maskHint.DraggingSpeed = draggingSpeed;
                MaskHint targetHint = maskHints.Find(targetHint =>
                    targetHint.CurrentPositionIndex == maskHint.TargetPositionIndex);
                maskHint.TargetPosition = targetHint.transform.position;
            }

            SetActiveMiniGameInput(true);
        }

        #endregion

        #region Callbacks

        private void HandleTargetFound(MaskHint maskHint)
        {
            StartCoroutine(SwapCoroutine(maskHint));
        }

        private void HandleOnClicked(MaskHint maskHint)
        {
            if (selectedHint != null)
            {
                StartCoroutine(maskHint.MoveToStartingPositionCoroutine(ReturnAnimationDuration));
            }
            selectedHint = maskHint;
            SoundManager.Instance.PlaySound(objectPickedUpSound);
        }

        #endregion
        
        #region Logic

        private IEnumerator SwapCoroutine(MaskHint maskHint)
        {
            if (selectedHint == null)
            {
                yield break;
            }
            SoundManager.Instance.PlaySound(objectPickedUpSound);
            if (maskHint != null)
            {
                // Swap attributes
                (selectedHint.CurrentPositionIndex, maskHint.CurrentPositionIndex) =
                    (maskHint.CurrentPositionIndex, selectedHint.CurrentPositionIndex);
                (selectedHint.StartPosition, maskHint.StartPosition) =
                    (maskHint.StartPosition, selectedHint.StartPosition);
            }

            // Return to starting position
            Coroutine coroutine1 = StartCoroutine(MoveToStartingPositionAnimation(selectedHint));
            selectedHint = null;
            if (maskHint != null)
            {
                yield return StartCoroutine(MoveToStartingPositionAnimation(maskHint));
            }
            yield return coroutine1;
            CheckIfCompletedAndFinishMiniGame();
        }
        
        #endregion
        
        #region Animations

        private IEnumerator MoveToStartingPositionAnimation(MaskHint maskHint)
        {
            maskHint.IsClickable = false;
            maskHint.IsDragged = false;
            yield return StartCoroutine(maskHint.MoveToStartingPositionCoroutine(ReturnAnimationDuration));
            maskHint.IsClickable = true;
        }
        
        #endregion

        #region Completing

        private void SetActiveMiniGameInput(bool isActive)
        {
            foreach (MaskHint maskHint in maskHints)
            {
                maskHint.IsClickable = isActive;
            }
        }

        private bool IsMiniGameFinished()
        {
            foreach (MaskHint maskHint in maskHints)
            {
                if (!maskHint.IsSolved())
                {
                    return false;
                }
            }
            return true;
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
                SetActiveMiniGameInput(false);
                StartCoroutine(SolveAndFinishMiniGame(SolverType.Manual));
            }
        }

        #endregion
        
        #region MiniGameDefaultFunctions

        protected override IEnumerator OnCompletedManuallySetFinalState()
        {
            SetActiveMiniGameInput(false);
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return miniGameDelay;
        }

        protected override IEnumerator OnHintUsedSetFinalState()
        {
            SetActiveMiniGameInput(false);
            List<Coroutine> coroutines = new List<Coroutine>();
            foreach (MaskHint maskHint in maskHints)
            {
                maskHint.SetFinalState();
                coroutines.Add(StartCoroutine(maskHint.MoveToStartingPositionCoroutine()));
            }
            foreach (Coroutine coroutine in coroutines)
            {
                yield return coroutine;
            }
            
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return miniGameDelay;
        }

        #endregion
    }
}