using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.Scripts.Templates
{
    public class TestClickableObject : MonoBehaviour
    {
        public UnityAction<TestClickableObject> OnMousePressed { get; set; }
        // public int currentPosition;
        
        public int Index
        {
            get => index;
            set => index = value;
        }
        
        // [SerializeField] private SpriteRenderer buttonRenderer;
        
        private int index;
        private bool isClickable;
        private const float ScaleSizeConst = 1.16f;

        private void OnMouseDown()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
			{
                return;
			}

            if (isClickable)
            {
                OnMousePressed?.Invoke(this);
            }
        }

        public void SetClickable(bool isObjectClickable)
        {
            isClickable = isObjectClickable;
        }
        
        
        public void SetFinalState()
        {

        }
        
        public bool IsSolved()
        {
            return false;
        }

        public IEnumerator ClickAnimation()
        {
            yield return null;
        }

        private void DoSomething()
        {
            
        }
    }
}
