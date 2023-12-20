using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.DraggingPassword___Missing_solve__1_._2._CustomDraggingPassword___Mayas_Incas___InProgress
{
    public class PuzzleDragManager : MonoBehaviour
    {
        #region MiniGameAttributes
        
        [Header("MiniGame Management Attributes")] 
        [SerializeField] private float miniGameFinishedDelay = 0.6f;
        [SerializeField] private MiniGame miniGame;
		
        private WaitForSeconds miniGameDelay;

        #endregion
        
        [Header("Attributes")]
        [SerializeField] private List<PuzzleDragColumn> columns;
        [SerializeField] private List<int> password;
        [SerializeField] private int firstIndexTileValue;
        [Header("Sounds")]
        [SerializeField] private AudioSource miniGameFinishedSound;

        private bool isMiniGameCompleted;
        private List<int> currentPassword;
        private const int DifferentValuesCount = 10;

        #region MonoBehaviour functions

        private void Awake()
        {
            // Inicijalizacija private listi
            miniGameDelay = new WaitForSeconds(miniGameFinishedDelay);
            currentPassword = new List<int>();
            for (int i = 0; i < columns.Count; i++)
            {
                currentPassword.Add(0);
                //starting values
            }
        }
        
        private void Start()
        {
            for (int i = 0; i < columns.Count; i++)
            {
                PuzzleDragColumn column = columns[i];
                column.OnMouseReleased += HandleMouseReleased;
                column.ColumnIndex = i;
            }

            SetActiveMiniGame(true);
        }

        #endregion

        #region Callbacks

        private void HandleMouseReleased(int value, int columnIndex)
        {
            currentPassword[columnIndex] = value;
            CheckIfCompletedAndFinishMiniGame();
            Debug.Log($"Password: {currentPassword}");
        }

        #endregion
        
        #region Logic
        
        
        
        #endregion
		
		#region Animations
        
        // private IENumerator XAnimation(){}
        
		#endregion
        
        #region Completing

        private void SetActiveMiniGame(bool isActive)
        {
            foreach (PuzzleDragColumn column in columns)
            {
                column.IsEnabled = isActive;
            }
            isMiniGameCompleted = !isActive;
        }

        private bool IsMiniGameFinished()
        {
            if (isMiniGameCompleted)
            {
                return false;
            }

            for (int i = 0; i < currentPassword.Count; i++)
            {
                if (currentPassword[i] != password[i])
                {
                    return false;
                }
            }
            return true;
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