using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.Magazine___Non_functional._3._Old_Magazine___Improved
{
    public class MagazinePage : MonoBehaviour
    {
        public bool forward;

        public void TurnPage()
        {
            if (forward)
                MagazineManager.Instance.TurnPageForward();
            else
                MagazineManager.Instance.TurnPageBackward();
        }

        public void OnMouseDown()
        {
            if (GameplayManager.Instance.AreAllUIElementsClosed())
            {
                TurnPage();
            }
        }
    }
}