using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.Magazine___Non_functional.In_Progress___Custom_Magazine
{
    public enum MagazinePageSide
    {
        Front,
        Left,
        Right,
        Rear
    }

    public class MagazinePage : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer pageRenderer;
        public UnityAction<MagazinePage> OnPageClicked { get; set; }
        public bool IsClickable { get; set; }
        public bool IsCurrentlyLocked { get; set; }
        public bool IsStaticPage { get; set; }
        public int PageIndex { get; set; }
        public MagazinePageSide PageSide { get; set; }

        private void OnMouseDown()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }

            if (IsClickable && !IsCurrentlyLocked && !IsStaticPage)
            {
                OnPageClicked.Invoke(this);
            }
        }

        // public void ShowPage(bool shouldShow)
        // {
        //     gameObject.SetActive(shouldShow);
        // }
        public void ShowPage(bool shouldShow)
        {
            Color pageColor = pageRenderer.color;
            float alpha = shouldShow ? 0 : 1;
            pageRenderer.color = new Color(pageColor.r, pageColor.g, pageColor.b, alpha);
        }
        

        public IEnumerator ShowPageCoroutine(bool shouldShow, float duration = 0.3f)
        {
            Color pageColor = pageRenderer.color;
            float step = 0;
            float alpha = shouldShow ? 0 : 1;
            Color startColor = new Color(pageColor.r, pageColor.g, pageColor.b, alpha);
            Color endColor = new Color(pageColor.r, pageColor.g, pageColor.b, 1 - alpha);
            while (step < 1)
            {
                pageRenderer.color = Color.Lerp(
                    startColor, endColor, step);

                step += Time.deltaTime / duration;
                yield return null;
            }

            pageRenderer.color = endColor;
        }
    }
}