using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebelinxGames.RoomsAndExits.MiniGames;

namespace Tacic.Tacic___Unity_Tools.Scripts.Templates
{
    public class XDoerManager : MiniGameManager
    {
        [Header("MiniGame Settings")] 
        [SerializeField] private float miniGameFinishedDelay = 0.6f;
        [SerializeField] private float timeBetweenButtonClickWhenSolving;
        // Or solveNextObjectWaitTime
        [Header("Attributes")]
        [SerializeField] private List<Transform> positionHolders;
        [SerializeField] private List<TestClickableObject> clickableObjects;
        [SerializeField] private List<int> solutionIndexes;
        [Header("Sounds")]
        [SerializeField] private AudioSource miniGameFinishedSound;

        private bool isMiniGameSolved;
        private const int constant = 0;

        private void Awake()
        {
            foreach (TestClickableObject clickableObject in clickableObjects)
            {
                clickableObject.OnMousePressed += HandleMousePressed;
            }
        }

        #region Callbacks

        private void HandleMousePressed(TestClickableObject clickableObject)
        {
            //StartCoroutine(CoroutineForAnimatedMiniGames(clickableObject));
            CheckIfCompletedAndFinishMiniGame();
        }

        #endregion
        
        #region Logic

        private IEnumerator CoroutineForAnimatedMiniGames(TestClickableObject clickableObject)
        {
            yield return null;
            
        }

        #endregion
		
		#region Animations

        private IEnumerator XAnimation(TestClickableObject clickableObject)
        {
            // // Null check, not neccecary except in certain circumstances
            // if (!clickableObject)
            // {
            //     yield break;
            // }
            
            clickableObject.SetClickable(false);
            yield return StartCoroutine(clickableObject.ClickAnimation());
            clickableObject.SetClickable(true);
        }
        
		#endregion
        
        #region Completing

        
        private void SetActiveMiniGameInput(bool isActive)
        {
            foreach (TestClickableObject clickableObject in clickableObjects)
            {
                clickableObject.SetClickable(isActive);
            }
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
            yield return new WaitForSeconds(miniGameFinishedDelay);
        }

        protected override IEnumerator OnHintUsedSetFinalState()
        {
            SetActiveMiniGameInput(false);
            WaitForSeconds solveButtonClickedDelay = new WaitForSeconds(timeBetweenButtonClickWhenSolving);
            // Solving part
            List<Coroutine> solveCoroutines = new List<Coroutine>();
            foreach (TestClickableObject clikableObject in clickableObjects)
            {
                    solveCoroutines.Add(StartCoroutine(clikableObject.ClickAnimation()));
                    yield return solveButtonClickedDelay;
            }
                
            foreach (Coroutine coroutine in solveCoroutines)
            {
                    yield return coroutine;
            }
            
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return new WaitForSeconds(miniGameFinishedDelay);
        }

        #endregion
    }
    // Regioni - vise fja ili parametara koje hoces da sakrijes
    // Public var, public properties / Serialized fields / private var, public var / MonoBehaviour / public f(), private f()
    // One space after closed block }
}