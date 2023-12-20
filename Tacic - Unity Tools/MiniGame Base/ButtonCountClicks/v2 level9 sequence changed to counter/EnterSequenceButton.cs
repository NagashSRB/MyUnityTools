using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.ButtonCountClicks.v2_level9_sequence_changed_to_counter
{
    public class EnterSequenceButton : MonoBehaviour
    {
        public int value;

        public bool disableSpriteRendererOnMouseDown;
        public bool enableSpriteRendererOnMouseDown;

        public bool playAnimationOnMouseDown;

        public void OnMouseDown()
        {
            if (!GameplayManager.Instance.popupOpened)
            {
                EnterSequenceGameManager.Instance.AddValueToSequence(value);

                if (disableSpriteRendererOnMouseDown)
                    GetComponent<SpriteRenderer>().enabled = false;

                if (enableSpriteRendererOnMouseDown)
                    GetComponent<SpriteRenderer>().enabled = true;

                if (playAnimationOnMouseDown)
                    transform.GetChild(0).GetComponent<Animator>().Play("Show", 0, 0);
            }
        }

        public void OnMouseUp()
        {
            if (!GameplayManager.Instance.inventoryPanel.activeInHierarchy &&
                !MenuManager.Instance.settingsPopup.activeInHierarchy)
            {
                if (disableSpriteRendererOnMouseDown)
                    GetComponent<SpriteRenderer>().enabled = true;

                if (enableSpriteRendererOnMouseDown)
                    GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}