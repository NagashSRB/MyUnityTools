using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.Jigsaw.v2___Level9__Added_solve_and_check_if_it_should_lock_when_correct._0._9_distance_
{

    public class JigsawPuzzlePart : MonoBehaviour
    {
        public Transform targetObject;

        public string objectNameInInventory;

        public Transform TryToSnap(List<Transform> snapPositions)
        {
            float closestDistance = Single.PositiveInfinity;
            Transform positionToSnapTo = null;
            foreach (Transform snapPosition in snapPositions)
            {
                float distance = Vector2.Distance(transform.position, snapPosition.position);
                if (distance < PuzzleGameManager.Instance.precisionDistance)
                {
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        positionToSnapTo = snapPosition;
                    }
                }
            }

            if (positionToSnapTo != null)
            {
                transform.position = positionToSnapTo.position;
            }
            return positionToSnapTo;
        }

        public void CheckAndSetPositionIfNeeded()
        {
            Transform targetTransform = TryToSnap(PuzzleGameManager.Instance.targetPartHolders);
            // v1.08 - distanca promenjena sa 0.1 na 0.9

            if (targetTransform != null)
            {
                SoundManager.Instance.PlaySound(PuzzleGameManager.Instance.puzzlePartSetSound);
            }
            if (targetObject == targetTransform && Vector2.Distance(transform.position, targetObject.position) <
                PuzzleGameManager.Instance.precisionDistance)
            {
                //transform.position = targetObject.position;
                GetComponent<PuzzlePartDrag>().isSolved = true;
                
                if (PuzzleGameManager.Instance.shouldLockPuzzleIfCorrect)
                {

                    GetComponent<PuzzlePartDrag>().enabled = false;

                    GetComponent<SpriteRenderer>().sortingOrder = 5;

                    // v1.14 gasimo i collider ukoliko je deo postavljen
                    if (GetComponent<Collider2D>() != null)
                        GetComponent<Collider2D>().enabled = false;
                }

                if (PuzzleGameManager.Instance.CheckIfGameIsFinished())
                    PuzzleGameManager.Instance.GameFinished();
            }
        }

        public void SetFinalPosition()
        {
            // Prvo proveravamo da liaktivni objekat jedan od onih koji su u igri
            if (GameplayManager.Instance.currentlyUsingItem != null &&
                GameplayManager.Instance.currentlyUsingItem.name == objectNameInInventory)
                GameplayManager.Instance.StopUsingSelectedItem();

            transform.position = targetObject.position;
            GetComponent<PuzzlePartDrag>().enabled = false;
        }

        public IEnumerator SetFinalPositionAnimation(float duration = 0.5f)
        {
            if (GameplayManager.Instance.currentlyUsingItem != null &&
                GameplayManager.Instance.currentlyUsingItem.name == objectNameInInventory)
                GameplayManager.Instance.StopUsingSelectedItem();
            
            GetComponent<PuzzlePartDrag>().enabled = false;
            Vector3 startingPosition = transform.position;
            float step = 0;
            while (step < 1)
            {
                step += Time.deltaTime / duration;
                float movementFactor = step * step * (3f - 2f * step);
                transform.position = Vector3.Lerp(startingPosition, targetObject.position, movementFactor);
                yield return null;
            }
            transform.position = targetObject.position;
        }

        private void OnEnable()
        {
            // Ako objekat vec nije ukljucen proveravamo da li treba da se ukljuci
            if (GetComponent<Collider2D>().enabled == false)
            {
                if (objectNameInInventory != null && objectNameInInventory != "")
                {
                    foreach (Transform t in GameplayManager.Instance.inventoryItemsHolder)
                    {
                        if (t.name.Contains(objectNameInInventory))
                        {
                            GetComponent<Collider2D>().enabled = true;
                            GetComponent<SpriteRenderer>().enabled = true;

                            // Izbacujemo ga iz inventara
                            t.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    GetComponent<Collider2D>().enabled = true;
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }
}
