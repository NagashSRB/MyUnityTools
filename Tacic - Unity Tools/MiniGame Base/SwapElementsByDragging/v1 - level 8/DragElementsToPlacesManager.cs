using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.SwapElementsByDragging.v1___level_8
{
    public class DragElementsToPlacesManager : MonoBehaviour
    {
        public bool gameStarted;

        public bool gameFinished;

        public Transform allElementsHolder;

        public DragElementsScript selectedElement;

        public List<DragElementsScript> allElements;

        public static DragElementsToPlacesManager Instance;

        public Animator endGameObjectAnimator;

        public AudioSource swapElementsSound;

        // Dodajemo ako treba prvo da se iskoristi neki item na neki target
        // (tj. ako nesto nedostaje)
        public GameObject missingElement;

        public List<Vector3> originalStartPositions;

        // v1.01
        public bool turnOffPartsOnEndGame;

        // v1.14.1
        // If algorythm should not mix elements at the begining, 
        // default is already set to mix them
        public bool dontMixElementsAtTheBegining;

        private void Awake()
        {
            gameStarted = false;

            int indexCounter = 0;

            originalStartPositions = new List<Vector3>();

            foreach (Transform t in allElementsHolder)
            {
                t.GetComponent<DragElementsScript>().currentPositionIndex = indexCounter;
                allElements.Add(t.GetComponent<DragElementsScript>());
                originalStartPositions.Add(t.localPosition);
                indexCounter++;
            }

            Instance = this;
        }

        IEnumerator SetGameOnStart()
        {
            yield return new WaitForSeconds(0.15f);

            for (int i = 0; i < 10; i++)
            {
                int randomElement1 = Random.RandomRange(0, allElements.Count);
                int randomElement2 = Random.RandomRange(0, allElements.Count);

                if (randomElement1 != randomElement2)
                    SwapElementsInstant(allElements[randomElement1], allElements[randomElement2]);
            }
        }

        public void StartMiniGame()
        {
            if (!gameFinished && !gameStarted && (missingElement == null || missingElement.activeInHierarchy))
            {
                gameStarted = true;

                if (!dontMixElementsAtTheBegining)
                    StartCoroutine("SetGameOnStart");
            }
        }

        public void JustStartMiniGame()
        {
            if (!gameFinished && !gameStarted && (missingElement == null || missingElement.activeInHierarchy))
            {
                gameStarted = true;
            }
        }

        public void SwapElementsInstant(DragElementsScript element1, DragElementsScript element2)
        {
            int temp = element1.currentPositionIndex;
            element1.currentPositionIndex = element2.currentPositionIndex;
            element2.currentPositionIndex = temp;

            Vector3 tempStartPosition = element1.startPosition;
            element1.startPosition = element2.startPosition;
            element2.startPosition = tempStartPosition;

            // Zatim setujemo da objekti promene mesta
            Vector3 screenPoint = element1.startPosition;
            screenPoint.z = 10f;
            element1.transform.localPosition = screenPoint;

            screenPoint = element2.startPosition;
            screenPoint.z = 10f;
            element2.transform.localPosition = screenPoint;
        }

        public void SwapElements(DragElementsScript element1, DragElementsScript element2)
        {
            // Prvo zamenimo indexe i startne pozicije
            int temp = element1.currentPositionIndex;
            element1.currentPositionIndex = element2.currentPositionIndex;
            element2.currentPositionIndex = temp;

            Vector3 tempStartPosition = element1.startPosition;
            element1.startPosition = element2.startPosition;
            element2.startPosition = tempStartPosition;

            // Zatim setujemo da objekti promene mesta
            element1.returningToStartPosition = true;
            element2.returningToStartPosition = true;

            selectedElement = null;

            if (swapElementsSound != null)
                SoundManager.Instance.PlaySound(swapElementsSound);
        }

        /// <summary>
        /// Funkcija koja proverava da li svaki element na svom mestu.
        /// </summary>
        /// <returns><c>true</c>, Ako su svi elementi na svojim mestima, <c>false</c> u suprotnom.</returns>
        public bool CheckIfAllElementsAreSet()
        {
            for (int i = 0; i < allElements.Count; i++)
            {
                if (!allElements[i].originalPositionIndices.Contains(allElements[i].currentPositionIndex) || allElements[i].isHoldingItem)
                    return false;
            }

            return true;
        }

        public void MiniGameFinished()
        {
            if (turnOffPartsOnEndGame)
                allElementsHolder.gameObject.SetActive(false);

            // Gasimo collidere
            for (int i = 0; i < allElements.Count; i++)
                allElements[i].GetComponent<Collider2D>().enabled = false;

            gameStarted = false;
            gameFinished = true;
            GetComponent<MiniGame>().MiniGameFinished();

            if (endGameObjectAnimator != null)
                endGameObjectAnimator.Play("Show", 0, 0);
        }

        public void SetFinalState()
        {
            // Podesavamo da svaki objekat ide na svoje mesto i ima odgovarajuci index
            List<int> usedIndices = new List<int>();

            for (int i = 0; i < allElements.Count; i++)
            {
                for (int j = 0; j < allElements[i].originalPositionIndices.Count; j++)
                {
                    if (!usedIndices.Contains(allElements[i].originalPositionIndices[j]))
                    {
                        allElements[i].currentPositionIndex = allElements[i].originalPositionIndices[j];
                        allElements[i].startPosition = originalStartPositions[i];
                        usedIndices.Add(allElements[i].currentPositionIndex);
                        break;
                    }
                }

                allElements[i].returningToStartPosition = true;
            }
        }

        public void FinishMiniGame()
        {
            if (!gameStarted)
                StartGame();

            SetFinalState();

            MiniGameFinished();
        }

        public void StartGame()
        {
            if (Instance != this)
                Instance = this;

            StartMiniGame();
        }

        public void StopGame()
        {
        }

        public void MissingElementUsed()
        {
            missingElement.SetActive(true);
            StartMiniGame();
        }

        // Posebno za igru u levelu Shoes Store - Level 21
        public void MissingElementUsedShoeStore()
        {
            missingElement.SetActive(true);
            JustStartMiniGame();
        }
    }
    
}