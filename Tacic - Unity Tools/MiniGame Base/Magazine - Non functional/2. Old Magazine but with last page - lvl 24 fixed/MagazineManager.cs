using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.Magazine___Non_functional._2._Old_Magazine_but_with_last_page___lvl_24_fixed
{
    public class MagazineManager : MonoBehaviour
    {
        private void SetFirstPage(int normalizedTime)
        {
            currentPage = 0;

            for (int i = 0; i < magazinePages.Count; i++)
            {
                if (i == 0)
                {
                    magazinePages[i].gameObject.SetActive(true);
                    magazinePages[i].Play("ShowPage", 0, normalizedTime);
                }
                else
                {
                    magazinePages[i].gameObject.SetActive(false);
                }
            }

            forwardCenterColliderObject.SetActive(true);
            backwardCenterColliderObject.SetActive(false);
            forwardColliderObject.SetActive(false);
            backwardColliderObject.SetActive(false);
        }
        
        public void StartGame()
        {
            if (magazinePages == null)
                magazinePages = new List<Animator>();

            if (!HintsManager.Instance.hintActive)
            {
                // Pri otvaranju igre setujemo na prvu stranicu, drugu spremamo za prikaz
                SetFirstPage(1);
            }
            else
            {
                // Proveravamo koji je hint aktivan
                int activeHintIndex = -1;
                for (int i = 0; i < HintsManager.Instance.hints.Count; i++)
                {
                    if (!HintsManager.Instance.hints[i].hintActionDone)
                    {
                        activeHintIndex = i;
                        break;
                    }
                }

                // U odnosu na to koji je setujemo stranu 
                // NOTE: 
                // FURNITURE 27, 28, 29
                // ART 7, 11, 20, 21
                // Office 15, 23
                if (GlobalVariables.selectedLevelTranslationKey == "furnitureStore" && activeHintIndex != -1)
                {
                    if (activeHintIndex == 27)
                    {
                        ShowPageTargetPointer(3);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 28)
                    {
                        ShowPageTargetPointer(5);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 29)
                    {
                        ShowPageTargetPointer(6);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                }
                else if (GlobalVariables.selectedLevelTranslationKey == "artSuppliesStore" && activeHintIndex != -1)
                {
                    if (activeHintIndex == 7)
                    {
                        ShowPageTargetPointer(3);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 11)
                    {
                        ShowPageTargetPointer(2);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 20)
                    {
                        ShowPageTargetPointer(4);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 21)
                    {
                        ShowPageTargetPointer(4);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                }
                else if (GlobalVariables.selectedLevelTranslationKey == "stydyRoomOffice" && activeHintIndex != -1)
                {
                    if (activeHintIndex == 15)
                    {
                        ShowPageTargetPointer(3);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 23)
                    {
                        ShowPageTargetPointer(6);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                }
                else
                {
                    // Pri otvaranju igre setujemo na prvu stranicu, drugu spremamo za prikaz
                    currentPage = 0;

                    for (int i = 0; i < magazinePages.Count; i++)
                    {
                        if (i == 0)
                        {
                            magazinePages[i].gameObject.SetActive(true);
                            magazinePages[i].Play("ShowPage", 0, 1);
                        }
                        else
                        {
                            magazinePages[i].gameObject.SetActive(false);
                        }
                    }

                    forwardCenterColliderObject.SetActive(true);
                    backwardCenterColliderObject.SetActive(false);
                    forwardColliderObject.SetActive(false);
                    backwardColliderObject.SetActive(false);
                }

                SetMissingPagesCollidersAccordingToCurrentPage();
            }

            canTurnPage = true;
        }

        public void StopGame()
        {

        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            if (magazinePages == null)
                magazinePages = new List<Animator>();

            forwardCenterColliderObject.SetActive(true);
            backwardCenterColliderObject.SetActive(false);
            forwardColliderObject.SetActive(false);
            backwardColliderObject.SetActive(false);

            foreach (Transform t in pagesHolder)
            {
                magazinePages.Add(t.GetComponent<Animator>());
            }

            // Za ovaj nivo dodajemo na hint dugme jos jednu funkciju
            GameObject.Find("HintButton").GetComponent<Button>().onClick.AddListener(SetHintIfGameIsOpen);
        }

        public bool canTurnPage;

        public Transform pagesHolder;
        public List<Animator> magazinePages;
        public int currentPage;

        public static MagazineManager Instance;

        public GameObject forwardCenterColliderObject;
        public GameObject backwardCenterColliderObject;
        public GameObject forwardColliderObject;
        public GameObject backwardColliderObject;

        public AudioSource pageSetSound;
        public AudioSource pageTurnSound;

        public void TurnPageForward()
        {
            if (canTurnPage && currentPage < magazinePages.Count &&
                GameplayManager.Instance.currentlyUsingItem == null
                && !GameplayManager.Instance.inventoryPanel.activeInHierarchy && !HintsManager.Instance.hintActive
                && !HintsManager.Instance.hintsPanel.activeInHierarchy &&
                EventSystem.current.currentSelectedGameObject == null)
                StartCoroutine("TurnForwardCoroutine");
            else if (canTurnPage && currentPage + 1 < magazinePages.Count &&
                     GameplayManager.Instance.currentlyUsingItem != null
                     && !GameplayManager.Instance.inventoryPanel.activeInHierarchy && !HintsManager.Instance.hintActive
                     && !HintsManager.Instance.hintsPanel.activeInHierarchy &&
                     EventSystem.current.currentSelectedGameObject == null)
            {
                RaycastHit2D[] hits;

                hits = Physics2D.RaycastAll(
                    GameplayManager.Instance.zoomElementsCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                bool targethit = false;

                if (hits != null && hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.GetComponent<TargetItem>() != null)
                        {
                            targethit = true;
                            break;
                        }
                    }
                }

                if (!targethit)
                    StartCoroutine("TurnForwardCoroutine");
            }
        }

        IEnumerator TurnForwardCoroutine()
        {
            if (currentPage == magazinePages.Count - 1)
            {
                magazinePages[0].Play("ShowPage", 0, 0);
                //for (int i = 1; i <= magazinePages.Count - 1; i++)
                // for (int i = magazinePages.Count - 1; i > 0; i--)
                // {
                //     magazinePages[i].Play("HidePage", 0, 0);
                // }
                for (int i = 1; i <= magazinePages.Count - 2; i++)
                {
                    magazinePages[i].gameObject.SetActive(false);
                }
                magazinePages[magazinePages.Count - 1].Play("HidePage", 0, 0);
                canTurnPage = false;
                yield return new WaitForSeconds(0.51f);
                //SetFirstPage(0);
                canTurnPage = true;
                currentPage = 0;
                for (int i = 0; i < magazinePages.Count; i++)
                {
                    if (i == 0)
                    {
                        magazinePages[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        magazinePages[i].gameObject.SetActive(false);
                    }
                }
                forwardCenterColliderObject.SetActive(true);
                backwardCenterColliderObject.SetActive(false);
                forwardColliderObject.SetActive(false);
                backwardColliderObject.SetActive(false);
                yield break;
            }
            canTurnPage = false;

            // Setujemo order
            //magazinePages[currentPage].GetComponent<SpriteRenderer>().sortingOrder = 1;
            //magazinePages[currentPage + 1].GetComponent<SpriteRenderer>().sortingOrder = 2;

            if (pageTurnSound != null)
                SoundManager.Instance.PlaySound(pageTurnSound);

            if (!magazinePages[currentPage + 1].gameObject.activeInHierarchy)
                magazinePages[currentPage + 1].gameObject.SetActive(true);

            magazinePages[currentPage + 1].Play("ShowPage", 0, 0);

            if (currentPage + 1 == magazinePages.Count - 1)
            {
                for (int i = 0; i < magazinePages.Count - 1; i++)
                    magazinePages[i].Play("HidePage", 0, 0);
            }
            //else if (currentPage - 1 >= 0 && magazinePages[currentPage - 1].gameObject.activeInHierarchy)
            //    magazinePages[currentPage - 1].Play("HidePage", 0, 0);

            yield return new WaitForSeconds(0.51f);

            currentPage++;

            //If is last page
            if (currentPage == magazinePages.Count - 1)
            {
                forwardCenterColliderObject.SetActive(false);
                backwardCenterColliderObject.SetActive(false);
                forwardColliderObject.SetActive(true);
                backwardColliderObject.SetActive(true);
            }
            else
            {
                forwardCenterColliderObject.SetActive(false);
                backwardCenterColliderObject.SetActive(false);
                forwardColliderObject.SetActive(true);
                backwardColliderObject.SetActive(true);
            }

            SetMissingPagesCollidersAccordingToCurrentPage();

            canTurnPage = true;
        }

        public void TurnPageBackward()
        {
            if (canTurnPage && currentPage - 1 >= 0 && GameplayManager.Instance.currentlyUsingItem == null
                && !GameplayManager.Instance.inventoryPanel.activeInHierarchy && !HintsManager.Instance.hintActive
                && !HintsManager.Instance.hintsPanel.activeInHierarchy &&
                EventSystem.current.currentSelectedGameObject == null)
                StartCoroutine("TurnBackwardCoroutine");
            else if (canTurnPage && currentPage - 1 >= 0 && GameplayManager.Instance.currentlyUsingItem != null
                     && !GameplayManager.Instance.inventoryPanel.activeInHierarchy && !HintsManager.Instance.hintActive
                     && !HintsManager.Instance.hintsPanel.activeInHierarchy &&
                     EventSystem.current.currentSelectedGameObject == null)
            {
                RaycastHit2D[] hits;

                hits = Physics2D.RaycastAll(
                    GameplayManager.Instance.zoomElementsCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                bool targethit = false;

                if (hits != null && hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.GetComponent<TargetItem>() != null)
                        {
                            targethit = true;
                            break;
                        }
                    }
                }

                if (!targethit)
                    StartCoroutine("TurnBackwardCoroutine");
            }
        }

        IEnumerator TurnBackwardCoroutine()
        {
            canTurnPage = false;

            // Setujemo order
            //magazinePages[currentPage].GetComponent<SpriteRenderer>().sortingOrder = 1;
            //magazinePages[currentPage - 1].GetComponent<SpriteRenderer>().sortingOrder = 2;

            //magazinePages[currentPage - 1].gameObject.SetActive(true);
            magazinePages[currentPage].Play("HidePage", 0, 0);

            if (pageTurnSound != null)
                SoundManager.Instance.PlaySound(pageTurnSound);

            // Ako trba nulta da se prikaze sakrivam sve ostale
            if (currentPage - 1 == 0)
            {
                for (int i = 2; i < magazinePages.Count; i++)
                    magazinePages[i].gameObject.SetActive(false);
            }
            else if (currentPage == magazinePages.Count - 1)
            {
                for (int i = 0; i < magazinePages.Count - 1; i++)
                    magazinePages[i].Play("ShowPage", 0, 0);
            }
            //else if (currentPage + 1 < magazinePages.Count && magazinePages[currentPage].gameObject.activeInHierarchy)
            //    magazinePages[currentPage + 1].Play("HidePage", 0, 0);

            yield return new WaitForSeconds(0.51f);

            currentPage--;

            if (currentPage == 0)
            {
                forwardCenterColliderObject.SetActive(true);
                backwardCenterColliderObject.SetActive(false);
                forwardColliderObject.SetActive(false);
                backwardColliderObject.SetActive(false);
            }
            else
            {
                forwardCenterColliderObject.SetActive(false);
                backwardCenterColliderObject.SetActive(false);
                forwardColliderObject.SetActive(true);
                backwardColliderObject.SetActive(true);
            }

            SetMissingPagesCollidersAccordingToCurrentPage();

            if (magazinePages[currentPage + 1].gameObject.activeInHierarchy)
                magazinePages[currentPage + 1].gameObject.SetActive(false);

            canTurnPage = true;
        }

        private bool showingHint = false;

        // Specijalne funkcije za prikazivanje pointera za targete a kasnije i koriscenje itema
        public void ShowPageTargetPointer(int page)
        {
            // Setujem odgovarajucu stranu
            for (int i = 0; i < magazinePages.Count; i++)
            {
                if (i < page)
                {
                    if (!magazinePages[i].gameObject.activeInHierarchy)
                    {
                        magazinePages[i].gameObject.SetActive(true);
                        magazinePages[i].Play("ShowPage", 0, 1);
                    }
                }
                else
                {
                    magazinePages[i].gameObject.SetActive(false);
                }
            }

            currentPage = page - 1;
        }

        public void SetHintIfGameIsOpen()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine("SetHintIfGameIsOpenCoroutine");
        }

        IEnumerator SetHintIfGameIsOpenCoroutine()
        {
            yield return new WaitForSeconds(0.05f);

            if (HintsManager.Instance.hintActive)
            {
                // Proveravamo koji je hint aktivan
                int activeHintIndex = -1;
                for (int i = 0; i < HintsManager.Instance.hints.Count; i++)
                {
                    if (!HintsManager.Instance.hints[i].hintActionDone)
                    {
                        activeHintIndex = i;
                        break;
                    }
                }

                // U odnosu na to koji je setujemo stranu
                if (GlobalVariables.selectedLevelTranslationKey == "furnitureStore" && activeHintIndex != -1)
                {
                    if (activeHintIndex == 27)
                    {
                        ShowPageTargetPointer(3);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 28)
                    {
                        ShowPageTargetPointer(5);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 29)
                    {
                        ShowPageTargetPointer(6);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                }
                else if (GlobalVariables.selectedLevelTranslationKey == "artSuppliesStore" && activeHintIndex != -1)
                {
                    if (activeHintIndex == 7)
                    {
                        ShowPageTargetPointer(3);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 11)
                    {
                        ShowPageTargetPointer(2);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 20)
                    {
                        ShowPageTargetPointer(4);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 21)
                    {
                        ShowPageTargetPointer(4);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                }
                else if (GlobalVariables.selectedLevelTranslationKey == "stydyRoomOffice" && activeHintIndex != -1)
                {
                    if (activeHintIndex == 15)
                    {
                        ShowPageTargetPointer(3);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                    else if (activeHintIndex == 23)
                    {
                        ShowPageTargetPointer(6);

                        forwardCenterColliderObject.SetActive(false);
                        backwardCenterColliderObject.SetActive(false);
                        forwardColliderObject.SetActive(true);
                        backwardColliderObject.SetActive(true);
                    }
                }
                else
                {
                    // Pri otvaranju igre setujemo na prvu stranicu, drugu spremamo za prikaz
                    currentPage = 0;

                    for (int i = 0; i < magazinePages.Count; i++)
                    {
                        if (i == 0)
                        {
                            magazinePages[i].gameObject.SetActive(true);
                            magazinePages[i].Play("ShowPage", 0, 1);
                        }
                        else
                        {
                            magazinePages[i].gameObject.SetActive(false);
                        }
                    }

                    forwardCenterColliderObject.SetActive(true);
                    backwardCenterColliderObject.SetActive(false);
                    forwardColliderObject.SetActive(false);
                    backwardColliderObject.SetActive(false);
                }

                SetMissingPagesCollidersAccordingToCurrentPage();
            }
        }

        public void SetMissingPagesCollidersAccordingToCurrentPage()
        {
            for (int i = 0; i < magazinePages.Count; i++)
            {
                if (i == currentPage)
                {
                    if (magazinePages[i].transform.childCount > 0 &&
                        magazinePages[i].transform.GetChild(0).GetComponent<TargetItem>() != null && magazinePages[i]
                            .transform.GetChild(0).GetComponent<TargetItem>().indexOfItemThatNeedsToBeUsed != -1)
                    {
                        magazinePages[i].transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
                    }
                }
                else
                {
                    if (magazinePages[i].transform.childCount > 0 &&
                        magazinePages[i].transform.GetChild(0).GetComponent<TargetItem>() != null)
                    {
                        magazinePages[i].transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
                    }
                }
            }
        }
    }
}