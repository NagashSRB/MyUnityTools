using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.PlayAnimations_1AtSameTime_OnClickInRightSequence___In_Develop
{
    public class StickButton : MonoBehaviour
    {
        public UnityAction<int> OnButtonClicked { get; set; }
        
        public int Index
        {
            get => index;
            set => index = value;
        }
        
        private int index;
        private bool isClickable;

        private void Awake()
        {
            
        }

        private void OnMouseDown()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }

            if (isClickable)
            {
                OnButtonClicked?.Invoke(index);
            }
        }

        public void SetClickable(bool isObjectClickable)
        {
            isClickable = isObjectClickable;
        }
        
        public IEnumerator SolveCoroutine()
        {
            yield return null;
        }
    }
}