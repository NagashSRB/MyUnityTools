using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.Archive
{
    public class Button : MonoBehaviour
    {
        // [SerializeField] private SpriteRenderer buttonRenderer;
        // public int CurrentPosition { get; set; }
        // public int TargetPosition { get; set; }
        public UnityAction XHappened { get; set; }
        public bool IsClickable { get; set; }

        private const float ScaleSizeConst = 1.16f;

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
                XHappened.Invoke();
            }
        }

        public void SetSomething(bool toSelect)
        {
        }
    }
}
