using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebelinxGames.RoomsAndExits.MiniGames;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.EnablePartsBasedOnGivenCombination.v1___Custom___215
{
    [Serializable]
    public struct Combination
    {
        public List<int> partIndexes;
    }
    public class MasksManager : MiniGameManager
    {
        #region MiniGameSettings
        
        [SerializeField] private float miniGameFinishedDelay = 0.6f;
        [SerializeField] private float solveNextWaitTime = 0.1f;
        private WaitForSeconds miniGameDelay;
        private WaitForSeconds solveNextDelay;

        #endregion

        [Header("Attributes")]
        [SerializeField] private List<Mask> masks;
        [SerializeField] private List<Combination> combinations;
        [Header("Sounds")]
        [SerializeField] private AudioSource miniGameFinishedSound;

        private bool isMiniGameSolved;

        #region MonoBehaviour functions

        private void Awake()
        {
            miniGameDelay = new WaitForSeconds(miniGameFinishedDelay);
        }
        
        private void Start()
        {
            foreach (Mask mask in masks)
            {
                mask.OnMouseClicked += HandleMouseClicked;
                mask.PartCombinations = combinations;
            }
            SetActiveMiniGameInput(true);
        }

        #endregion

        #region Callbacks

        private void HandleMouseClicked()
        {
            CheckIfCompletedAndFinishMiniGame();
        }

        #endregion

        #region Completing

        private void SetActiveMiniGameInput(bool isActive)
        {
            foreach (Mask mask in masks)
            {
                mask.IsClickable = isActive;
            }
        }

        private bool IsMiniGameFinished()
        {
            foreach (Mask mask in masks)
            {
                if (!mask.IsSolved())
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

            foreach (Mask mask in masks)
            {
                mask.SetFinalState();
                yield return solveNextDelay;
            }
            
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return miniGameDelay;
        }

        #endregion
    }
}