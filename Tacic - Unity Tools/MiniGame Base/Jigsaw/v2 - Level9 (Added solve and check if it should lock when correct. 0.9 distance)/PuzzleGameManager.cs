using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.Jigsaw.v2___Level9__Added_solve_and_check_if_it_should_lock_when_correct._0._9_distance_
{
    public class PuzzleGameManager : MonoBehaviour
    {
        public static PuzzleGameManager Instance;

        public bool gameStarted;

        public Transform partsHolder;

        public List<Transform> targetPartHolders;

        public Collider2D enableColliderAtMiniGameFinish;

        public AudioSource puzzlePartSetSound;

        public Animator animatedObjectAtTheEndOfGame;

        public float precisionDistance;

        public bool shouldLockPuzzleIfCorrect;

        public void Awake()
        {
            gameStarted = true;

            multiTouch = Input.multiTouchEnabled;

            if (precisionDistance == 0)
                precisionDistance = 0.9f;

            if (Instance == null || Instance != this)
                Instance = this;
        }

        public bool CheckIfGameIsFinished()
        {
            foreach (Transform part in partsHolder)
            {
                if (part.GetComponent<JigsawPuzzlePart>() != null && !part.GetComponent<PuzzlePartDrag>().isSolved)
                    return false;
            }

            return true;
        }

        public void GameFinished()
        {
            gameStarted = false;
            GameplayManager.Instance.canInteract = false;
            float enableInteractionAfterSeconds = 0f;

            // Ovde za svaki slucaj ako se koristi neki item prestajemo sa koriscenjem istog
            if (GameplayManager.Instance.currentlyUsingItem != null)
                GameplayManager.Instance.StopUsingSelectedItem();

            // FIXME - uh ovde moram da proverim da li ce lepo da radi za level 6 - beauty salon
            if (GetComponent<MiniGame>().miniGameItem.closeMiniGameOnFinish)
                GameplayManager.Instance.CloseCurrentMiniGame();

            if (enableColliderAtMiniGameFinish != null)
                enableColliderAtMiniGameFinish.enabled = true;

            if (animatedObjectAtTheEndOfGame != null)
            {
                // Ako je kojim slucajem iskljucen ukljucujemo ga
                if (animatedObjectAtTheEndOfGame.enabled == false)
                    animatedObjectAtTheEndOfGame.enabled = true;
                enableInteractionAfterSeconds =
                    animatedObjectAtTheEndOfGame.runtimeAnimatorController.animationClips[0].length;
                animatedObjectAtTheEndOfGame.Play("Open", 0, 0);
            }

            StartCoroutine(EnableInteractAfterTime(enableInteractionAfterSeconds));
            GetComponent<MiniGame>().MiniGameFinished();
        }

        private IEnumerator EnableInteractAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            GameplayManager.Instance.canInteract = true;
        }

        public void SetFinalState()
        {
            // Solve that just sets final position
            foreach (Transform part in partsHolder)
            {
                if (part.GetComponent<JigsawPuzzlePart>() != null)
                    part.GetComponent<JigsawPuzzlePart>().SetFinalPosition();
            }
        }

        private IEnumerator SolveMiniGame()
        {
            // Solve that sets final position and animates movement of parts
            GameplayManager.Instance.canInteract = false;
            gameStarted = false;
            Coroutine lastPartMovement = null;
            foreach (Transform part in partsHolder)
            {

                if (part.GetComponent<JigsawPuzzlePart>() != null && part.position != part.GetComponent<JigsawPuzzlePart>().targetObject.position)
                {
                    lastPartMovement =
                        StartCoroutine(part.GetComponent<JigsawPuzzlePart>().SetFinalPositionAnimation());
                    yield return new WaitForSeconds(0.1f);
                }
            }

            yield return lastPartMovement;
            GameFinished();
        }

        public void FinishMiniGame()
        {
            // SetFinalState();
            // GameFinished();
            StartCoroutine(SolveMiniGame());
        }

        private bool multiTouch;

        public void StartGame()
        {
            if (Instance == null || Instance != this)
                Instance = this;

            Input.multiTouchEnabled = false;

            gameStarted = true;
        }

        public void StopGame()
        {
            // Ako je igra pocela namestamo da se prekida interakcija sa bilo kojim od delova
            if (multiTouch)
                Input.multiTouchEnabled = true;

            gameStarted = false;
        }
    }
}