using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts
{
    public class SolveMiniGameInEditorTacic : MonoBehaviour
    {
        private GameObject miniGame;

        private void Awake()
        {
            CoinsManager.Instance.coins = 15000;
        }

        void Start()
        {
            miniGame = gameObject.transform.GetChild(0).gameObject;
        
        }
    #if UNITY_EDITOR
        
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space) && miniGame.activeSelf)
            {
                gameObject.SendMessage("FinishMiniGame");
            }

        }
    #endif
    }
}