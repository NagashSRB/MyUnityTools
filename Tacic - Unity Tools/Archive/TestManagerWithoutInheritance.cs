using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Archive
{
    public class XDoer : MonoBehaviour
    {
        #region MiniGameAttributes
        
        [Header("MiniGame Management Attributes")] 
        [SerializeField] private float miniGameFinishedDelay = 1f;
        [SerializeField] private MiniGame miniGame;
		
        private WaitForSeconds miniGameDelay;

        #endregion
        
        [Header("Attributes")]
        [SerializeField] private List<Transform> holders;
        [Header("Sounds")]
        [SerializeField] private AudioSource miniGameFinishedSound;

        private bool isMiniGameCompleted;
        private const int Constant = 0;

        #region MonoBehaviour functions

        private void Awake()
        {
            // Inicijalizacija private listi
            miniGameDelay = new WaitForSeconds(miniGameFinishedDelay);
            isMiniGameCompleted = false;
        }
        
        private void Start()
        {
            // foreach (var VARIABLE in COLLECTION)
            // {
            //     field.XHappened += OnXHappened;
            // }
        }

        #endregion

        #region Callbacks

        // private void OnX()
        // {
        //     if (!isMiniGameCompleted)
        //     {
        //         StartCoroutine(TryToDoSomethingAndDoSomethingElse);
        //     }
        // }

        #endregion
        
        #region Logic
        
        
        
        #endregion
		
		#region Animations
        
        // private IENumerator XAnimation(){}
        
		#endregion
        
        #region Completing

        // private void SetActiveMiniGame(bool isActive)
        // {
        //     // Dodaj sta treba sve da se enable/disable
        //     isMiniGameActive = isActive;
        // }

        private bool IsMiniGameFinished()
        {
            if (isMiniGameCompleted)
            {
                return false;
            }
            
            // Logika za proveru za kraj
            
            return false;
            
            return true;
        }

        private IEnumerator MiniGameFinishedAnimations()
        {
            // Animacije i ostalo vezano za kraj miniigre
            yield return StartCoroutine(FinishMiniGameWithDelay());
        }
        
        private IEnumerator SolveMiniGame()
        {
            isMiniGameCompleted = true;
			// Disable i handling za kraj 
            // Logika za solve
            yield return null;
            StartCoroutine(FinishMiniGameWithDelay());
        }
        
        private void CheckIfCompletedAndFinishMiniGame()
        {
            if (IsMiniGameFinished())
            {
                isMiniGameCompleted = true;
                StartCoroutine(MiniGameFinishedAnimations());
            }
        }
        
        private IEnumerator FinishMiniGameWithDelay()
        {
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return miniGameDelay;
            miniGame.MiniGameFinished();
        }
        
        #endregion


        #region MiniGameDefaultFunctions

        public void StartGame()
        {
            Input.multiTouchEnabled = false;
        }

        public void StopGame()
        {
            Input.multiTouchEnabled = true;
        }

        public void GameFinished()
        {
        }

        public void FinishMiniGame()
        {
            StartCoroutine(SolveMiniGame());
        }

        #endregion
    }
}