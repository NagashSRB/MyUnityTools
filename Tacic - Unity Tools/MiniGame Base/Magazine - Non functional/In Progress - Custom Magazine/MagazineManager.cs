using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.Magazine___Non_functional.In_Progress___Custom_Magazine
{
    public class MagazineManager : MonoBehaviour
    {
        #region MiniGameAttributes
        
        [Header("MiniGame Management Attributes")] 
        [SerializeField] private float miniGameFinishedDelay = 1f;

        [SerializeField] private MiniGame miniGame;
        private WaitForSeconds miniGameDelay;

        #endregion
        
        [Header("Attributes")]
        [SerializeField] private List<MagazinePage> pages;
        [SerializeField] private bool isFirstPageFrontPage;
        [SerializeField] private bool isLastPageRearPage;
        [SerializeField] private bool shouldOpenFrontPageAfterRead = true;
        [SerializeField] private List<int> lockedPageIndexes;

        [Header("Sounds")] 
        [SerializeField] private AudioSource turnPagesSound;
        [SerializeField] private AudioSource miniGameFinishedSound;

        private int currentPageIndex;
        private int currentPageIndexForUnlock;
        
        private const int Constant = 0;

        #region MonoBehaviour functions

        private void Awake()
        {
            // Inicijalizacija private listi
            miniGameDelay = new WaitForSeconds(miniGameFinishedDelay);
        }
        
        private void Start()
        {

            InitializePages();
            foreach (MagazinePage page in pages)
            {
                page.ShowPage(false);
            }

            if (isFirstPageFrontPage)
            {
                pages[0].ShowPage(true);
            }
            
        }

        #endregion

        #region Initialization

        private void InitializePages()
        {
            bool isEven = false;
            for (int i = 0; i < pages.Count; i++)
            {
                MagazinePage magazinePage = pages[i];
                magazinePage.PageIndex = i;
                magazinePage.OnPageClicked += HandlePageClicked;

                if (i == 0 && isFirstPageFrontPage)
                {
                    magazinePage.PageSide = MagazinePageSide.Front;
                }
                else if (i == pages.Count - 1 && isLastPageRearPage)
                {
                    magazinePage.PageSide = MagazinePageSide.Rear;
                }
                else
                {
                    magazinePage.PageSide = isEven ? MagazinePageSide.Right : MagazinePageSide.Left;
                    isEven = !isEven;
                }
            }

            if (!isLastPageRearPage)
            {
                pages[pages.Count - 1].IsStaticPage = true;
            }

            if (!isFirstPageFrontPage)
            {
                pages[0].IsStaticPage = true;
            }

            foreach (int pageIndex in lockedPageIndexes)
            {
                pages[pageIndex].IsCurrentlyLocked = true;
            }
        }
        

        #endregion

        public void EnableLockedPage()
        {
            if (lockedPageIndexes.Count >= currentPageIndexForUnlock)
            {
                return;
            }
            pages[lockedPageIndexes[currentPageIndexForUnlock]].IsCurrentlyLocked = false;
            currentPageIndexForUnlock++;
        }

        #region Callbacks

        private void HandlePageClicked(MagazinePage magazinePage)
        {
            
        }

        #endregion
        
        #region Logic

        private MagazinePage currentPage1;
        private MagazinePage currentPage2;


        private IEnumerator TurnPageLeft()
        {
            MagazinePage nextPage1;
            MagazinePage nextPage2 = null;
            if (currentPageIndex - 1 == 0)   //sledeca je prva
            {
                currentPageIndex--;
                nextPage1 = pages[currentPageIndex];
            }
            else
            {
                nextPage1 = pages[currentPageIndex - 1];
                nextPage2 = pages[currentPageIndex - 2];
                currentPageIndex -= 2;
            }

            yield return StartCoroutine(TurnPagesCoroutine(nextPage1, nextPage2));
        }
        
        private IEnumerator TurnPageRight()
        {
            MagazinePage nextPage1;
            MagazinePage nextPage2 = null;
            if (currentPageIndex + 1 == pages.Count) //ovo je zadnja
            {
                currentPageIndex = 0;
                nextPage1 = pages[currentPageIndex];
                if (pages[currentPageIndex].PageSide != MagazinePageSide.Front)
                {
                    currentPageIndex++;
                    nextPage2 = pages[currentPageIndex]; 
                }
            }
            else if (currentPageIndex + 2 == pages.Count)   //sledeca je zadnja
            {
                currentPageIndex++;
                nextPage1 = pages[currentPageIndex];
            }
            else
            {
                nextPage1 = pages[currentPageIndex + 1];
                nextPage2 = pages[currentPageIndex + 2];
                currentPageIndex += 2;
            }

            yield return StartCoroutine(TurnPagesCoroutine(nextPage1, nextPage2));
        }

        private IEnumerator TurnPagesCoroutine(MagazinePage nextPage1, MagazinePage nextPage2)
        {
            SoundManager.Instance.PlaySound(turnPagesSound);
            List<Coroutine> coroutines = new List<Coroutine>
            {
                StartCoroutine(ShowPage(currentPage1, false)),
                StartCoroutine(ShowPage(currentPage2, false)),
                StartCoroutine(ShowPage(nextPage1,true)),
                StartCoroutine(ShowPage(nextPage2,true))
            };
            foreach (Coroutine coroutine in coroutines)
            {
                yield return coroutine;
            }
            //yield delay
            
        }

        private IEnumerator ShowPage(MagazinePage page, bool toShow)
        {
            if (page != null)
            {
                //page.IsClickable = 
                yield return StartCoroutine(page.ShowPageCoroutine(toShow));
            }
        }

        private void TurnPage(MagazinePage magazinePage)
        {
            //okretanje : trenutne stranice, fade i 
            if (magazinePage.PageSide == MagazinePageSide.Front || magazinePage.PageSide == MagazinePageSide.Right)
            {
                StartCoroutine(TurnPageRight());
            }
            //else if()
            //if left vrati 2 unazad ili 1 na front           ili blokiran(nema brige)

            // if front open next 2
            if (magazinePage.PageSide == MagazinePageSide.Rear)
            {
                if (shouldOpenFrontPageAfterRead)
                {
                    //open from start 2
                }
                else
                {
                    //open backwards 2
                }
            }
        }
        
        
        
        #endregion
		
		#region Animations
        
        // private IENumerator XAnimation(){}
        
		#endregion
        
        #region Completing

        private void SetActiveMiniGame(bool isActive)
        {
            // Dodaj sta treba sve da se enable/disable
            foreach (MagazinePage page in pages)
            {
                page.IsClickable = isActive;
            }
        }

        private bool IsMiniGameFinished()
        {

            // Logika za proveru za kraj
            
            return false;
            
            return true;
        }

        private IEnumerator MiniGameFinishedAnimations()
        {
            // Animacije i ostalo vezano za kraj miniigre
            yield return StartCoroutine(FinishMiniGameWithDelay());
        }
        
        private IEnumerator SolveMiniGame()
        {
            // Disable i handling za kraj 
            // Logika za solve
            yield return null;
            StartCoroutine(FinishMiniGameWithDelay());
        }
        
        private void CheckIfCompletedAndFinishMiniGame()
        {
            if (IsMiniGameFinished())
            {
                StartCoroutine(MiniGameFinishedAnimations());
            }
        }
        
        private IEnumerator FinishMiniGameWithDelay()
        {
            SoundManager.Instance.PlaySound(miniGameFinishedSound);
            yield return miniGameDelay;
            miniGame.MiniGameFinished();
        }
        
        #endregion


        #region MiniGameDefaultFunctions

        public void StartGame()
        {
            Input.multiTouchEnabled = false;
        }

        public void StopGame()
        {
            Input.multiTouchEnabled = true;
        }

        public void GameFinished()
        {
        }

        public void FinishMiniGame()
        {
            StartCoroutine(SolveMiniGame());
        }

        #endregion
    }
}