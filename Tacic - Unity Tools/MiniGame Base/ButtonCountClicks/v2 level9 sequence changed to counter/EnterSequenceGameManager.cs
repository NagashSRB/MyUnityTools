using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.ButtonCountClicks.v2_level9_sequence_changed_to_counter
{
    public class EnterSequenceGameManager : MonoBehaviour
    {
        public bool gameStarted;

        public List<int> enteredSequence;

        public List<int> targetSequence;

        public static EnterSequenceGameManager Instance;

        public GameObject animationObjectOnGameFinished;

        public bool sequenceWithFixedLength;
        public Animator finishCheckAnimator;
        public bool canAddValueForFixedLength;
        public Transform buttonsHolder;

        public AudioSource clickSound;
        private List<int> buttonClickedCounters = new List<int>(new int[2]);
        public List<Transform> leftButtonMaskHolders;
        public List<Transform> rightButtonMaskHolders;
        public GameObject leftLight;
        public GameObject rightLight;
        public List<int> solution;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;

            if (sequenceWithFixedLength)
                canAddValueForFixedLength = true;

            gameStarted = true;
        }

        public void StartGame()
        {
            if (Instance != this)
                Instance = this;

            // Ako je fiksna duzina sekvence koju trebamo da pogodimo onda restartujemo game
            if (sequenceWithFixedLength && gameStarted)
            {
                enteredSequence.Clear();
            }
        }

        public void StopGame()
        {
        }

        public void AddValueToSequence(int val)
        {
            if (gameStarted)
            {
                if (clickSound != null)
                    SoundManager.Instance.PlaySound(clickSound);

                if (!sequenceWithFixedLength)
                {
                    buttonClickedCounters[val] = (buttonClickedCounters[val] + 1) % leftButtonMaskHolders.Count;
                    if (val == 0)
                    {
                        //left
                        leftLight.transform.position = leftButtonMaskHolders[buttonClickedCounters[val]].position;
                    }
                    else if (val == 1)
                    {
                        //right
                        rightLight.transform.position = rightButtonMaskHolders[buttonClickedCounters[val]].position;
                    }
                    // if (enteredSequence.Count < targetSequence.Count)
                    //     enteredSequence.Add(val);
                    // else
                    // {
                    //     // Shiftujemo vrednosti u levo
                    //     for (int i = 0; i < enteredSequence.Count - 1; i++)
                    //         enteredSequence[i] = enteredSequence[i + 1];
                    //
                    //     enteredSequence[enteredSequence.Count - 1] = val;
                    // }
                    //

                    // Provera da li je igra zavrsena
                    if (CheckIfEnterdSequenceIsValid())
                    {
                        GameFinished();
                    }
                }
                else if (canAddValueForFixedLength)
                {
                    if (enteredSequence.Count < targetSequence.Count)
                        enteredSequence.Add(val);

                    if (enteredSequence.Count == targetSequence.Count)
                    {
                        StartCoroutine("FixedLengthFinishedEntering");
                    }
                }
            }
        }

        public bool CheckIfEnterdSequenceIsValid()
        {
            for (int i = 0; i < buttonClickedCounters.Count; i++)
            {
                if (buttonClickedCounters[i] != solution[i])
                {
                    return false;
                }
            }

            return true;
        }

        IEnumerator FixedLengthFinishedEntering()
        {
            canAddValueForFixedLength = false;

            // Zabranjujemo i klikove na dugmice
            foreach (Transform t in buttonsHolder)
            {
                t.GetComponent<BoxCollider2D>().enabled = false;
            }

            if (CheckIfEnterdSequenceIsValid())
            {
                finishCheckAnimator.Play("Correct", 0, 0);

                yield return new WaitForSeconds(1f);

                GameFinished();
            }
            else
            {
                finishCheckAnimator.Play("Wrong", 0, 0);

                yield return new WaitForSeconds(2f);

                // Restartujemo sekvencu
                enteredSequence.Clear();

                // Omogucavamo da igrac ponovo pokusa
                canAddValueForFixedLength = true;

                // Ukljucujemo da opet moze da se klikce na dugmice
                foreach (Transform t in buttonsHolder)
                {
                    t.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }

        public void GameFinished()
        {
            gameStarted = false;

            if (animationObjectOnGameFinished != null)
            {
                animationObjectOnGameFinished.GetComponent<Animator>().Play("Open", 0, 0);
            }

            GetComponent<MiniGame>().MiniGameFinished();
        }

        public void FinishMiniGame()
        {
            GameFinished();
        }
    }
}