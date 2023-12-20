using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.DraggingPassword___Missing_solve__1_._1._CustomDraggingPassword___Mayas_Incas___Old_FInished
{
    public class CabinetPasswordMGManager : MonoBehaviour
    {
        #region MiniGameAttributes
        
        [Header("MiniGame Management Attributes")] 
        [SerializeField] private float miniGameFinishedDelay = 1f;
        [SerializeField] private MiniGame miniGame;
		
        private WaitForSeconds miniGameDelay;

        #endregion

        [Header("Attributes")] 
        [SerializeField] private List<PasswordColumnController> columns;
        [SerializeField] private List<Sprite> numberSprites;
        [SerializeField] private List<int> password;

        [Header("Sounds")]
        [SerializeField] private AudioSource miniGameFinishedSound;

        private bool isMiniGameCompleted;

        #region MonoBehaviour functions

        private void Awake()
        {
            // Inicijalizacija private listi
            miniGameDelay = new WaitForSeconds(miniGameFinishedDelay);
            isMiniGameCompleted = false;
        }
        
        private void Start()
        {
            foreach (PasswordColumnController column in columns)
            {
                column.OnColumnClicked += HandleOnColumnClicked;
                column.OnAnimationFinished += HandleOnAnimationFinished;
            }
        }

        #endregion

        #region Callbacks

        private void HandleOnColumnClicked(PasswordColumnController column)
        {
            column.PlayAnimation();
        }
        private void HandleOnAnimationFinished(PasswordColumnController column)
        {
            column.IncreaseNumber();
            column.UpdateSprites(numberSprites);
            CheckIfCompletedAndFinishMiniGame();
        }

        #endregion
        
        #region Logic
        
        
        
        #endregion

        #region Completing

        private void SetActiveMiniGame(bool isActive)
        {
            isMiniGameCompleted = !isActive;
            foreach (PasswordColumnController column in columns)
            {
                column.IsEnabled = isActive;
            }
            
        }

        private bool IsMiniGameFinished()
        {
            if (isMiniGameCompleted)
            {
                return false;
            }

            for (int i = 0; i < columns.Count; i++)
            {
                if (!columns[i].IsNumberCorrect(password[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private IEnumerator SolveMiniGame()
        {
            SetActiveMiniGame(false);
            for (int i = 0; i < columns.Count; i++)
            {
                columns[i].Solve(password[i], numberSprites);
                yield return null;
            }
            StartCoroutine(FinishMiniGameWithDelay());
        }
        
        private void CheckIfCompletedAndFinishMiniGame()
        {
            if (IsMiniGameFinished())
            {
                isMiniGameCompleted = true;
                StartCoroutine(FinishMiniGameWithDelay());
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