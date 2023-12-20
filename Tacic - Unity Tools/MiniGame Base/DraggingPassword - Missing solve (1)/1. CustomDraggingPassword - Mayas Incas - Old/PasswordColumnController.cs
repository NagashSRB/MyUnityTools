using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.DraggingPassword___Missing_solve__1_._1._CustomDraggingPassword___Mayas_Incas___Old_FInished
{
    public class PasswordColumnController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer previousNumberSprite;
        [SerializeField] private SpriteRenderer currentNumberSprite;
        [SerializeField] private SpriteRenderer nextNumberSprite;
        private int previousNumber;
        private int currentNumber;
        private int nextNumber;
        public UnityAction<PasswordColumnController> OnColumnClicked { get; set; }
        public UnityAction<PasswordColumnController> OnAnimationFinished { get; set; }
        public bool IsEnabled { get; set; }

        private void Awake()
        {
            IsEnabled = true;
        }
		
        private void OnMouseDown()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }

            if (IsEnabled)
            {
                OnColumnClicked.Invoke(this);
            }
        }

        public void IncreaseNumber()
        {
            previousNumber = currentNumber;
            currentNumber = (currentNumber + 1) % 10;
            nextNumber = (currentNumber + 1) % 10;
        }

        public void UpdateSprites(List<Sprite> numberSprites)
        {
            previousNumberSprite.sprite = numberSprites[previousNumber];
            currentNumberSprite.sprite = numberSprites[currentNumber];
            nextNumberSprite.sprite = numberSprites[nextNumber];
        }

        public void PlayAnimation()
        {
            animator.Play("RotateColumn");
        }

        public void AnimationFinished()
        {
            OnAnimationFinished.Invoke(this);
        }

        public bool IsNumberCorrect(int targetNumber)
        {
            return currentNumber == targetNumber;
        }

        public void SetNumber(int number)
        {
            //prev = currnum - 1 + 10
            previousNumber = (currentNumber + 9) % 10;
            currentNumber = number;
            nextNumber = (currentNumber + 1) % 10;
        }

        public void Solve(int solution, List<Sprite> numberSprites)
        {
            SetNumber(solution);
            UpdateSprites(numberSprites);
        }
    }
}