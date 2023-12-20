using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.EnablePartsBasedOnGivenCombination.v1___Custom___215
{
    public class Mask : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> parts;
        [SerializeField] private int currentPartCombinationCounter;
        [SerializeField] private int targetCombinationIndex;
        public List<Combination> PartCombinations { get; set; }
        public UnityAction OnMouseClicked { get; set; }
        public bool IsClickable { get; set; }

        private void Awake()
        {
            IsClickable = true;
        }
		
        private void OnMouseDown()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }

            if (IsClickable)
            {
                ChangeCombination();
                OnMouseClicked?.Invoke();
            }
        }

        public bool IsSolved()
        {
            return targetCombinationIndex == currentPartCombinationCounter;
        }

        public void SetFinalState()
        {
            DisableAllParts();
            TurnOnPartCombination(targetCombinationIndex);
        }
        
        private void ChangeCombination()
        {
            currentPartCombinationCounter = (currentPartCombinationCounter + 1) % PartCombinations.Count;
            DisableAllParts();
            TurnOnPartCombination(currentPartCombinationCounter);
        }

        private void DisableAllParts()
        {
            foreach (SpriteRenderer part in parts)
            {
                part.enabled = false;
            }
        }

        private void TurnOnPartCombination(int partCombinationIndex)
        {
            foreach (int partIndex in PartCombinations[partCombinationIndex].partIndexes)
            {
                parts[partIndex].enabled = true;
            }
        }
    }
}