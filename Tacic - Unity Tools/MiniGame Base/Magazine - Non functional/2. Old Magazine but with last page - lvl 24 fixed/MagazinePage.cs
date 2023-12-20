using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.Magazine___Non_functional._2._Old_Magazine_but_with_last_page___lvl_24_fixed
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
            TurnPage();
        }
    }
}