using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.Magazine___Non_functional._1._Old_Magazine___lvl_8
{
    public class MagazinePage : MonoBehaviour
    {
        public bool forward;

        public void TurnPage()
        {
            if (forward)
                global::MagazineManager.Instance.TurnPageForward();
            else
                global::MagazineManager.Instance.TurnPageBackward();
        }

        public void OnMouseDown()
        {
            TurnPage();
        }
    }
}
