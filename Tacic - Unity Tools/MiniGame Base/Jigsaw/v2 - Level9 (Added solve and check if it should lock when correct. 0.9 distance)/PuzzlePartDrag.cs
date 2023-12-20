using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.Jigsaw.v2___Level9__Added_solve_and_check_if_it_should_lock_when_correct._0._9_distance_
{

    public class PuzzlePartDrag : MonoBehaviour
    {
        public float draggingSpeed;

        public bool isHoldingItem;

        public Vector3 startPosition;

        public Vector3 offsetVector;

        public bool scaleWhenGrabbed;
        public Vector3 scaleVector;
        public Vector3 originalScale;

        public bool returnToStartPosition;

        public bool returningToStartPosition;

        public bool isSolved;

        void Awake()
        {
            originalScale = transform.localScale;
            isHoldingItem = false;
            startPosition = transform.localPosition;
            returningToStartPosition = false;
        }

        public void OnMouseDown()
        {
            if (!GameplayManager.Instance.popupOpened)
            {
                if (PuzzleGameManager.Instance.gameStarted)
                {
                    isSolved = false;
                    isHoldingItem = true;

                    if (scaleWhenGrabbed)
                        transform.localScale = scaleVector;

                    // Setujemo order na 4 dok nosimo - FIXME menjao brojke za order
                    GetComponent<SpriteRenderer>().sortingOrder = 7;
                }
            }
        }

        public void OnMouseUp()
        {
            if (PuzzleGameManager.Instance.gameStarted && isHoldingItem)
            {
                isHoldingItem = false;

                // Setujemo order na 2 kad spustimo
                GetComponent<SpriteRenderer>().sortingOrder = 6;

                if (scaleWhenGrabbed)
                    transform.localScale = originalScale;

                if (returnToStartPosition)
                    returningToStartPosition = true;

                GetComponent<JigsawPuzzlePart>().CheckAndSetPositionIfNeeded();
            }
        }

        public void OnApplicationPause(bool paused)
        {
            if (GetComponent<JigsawPuzzlePart>() != null)
            {
                isHoldingItem = false;

                if (scaleWhenGrabbed)
                    transform.localScale = originalScale;

                if (returnToStartPosition)
                    returningToStartPosition = true;
            }
        }

        public float maxTopValue = 2.4f;

        void Update()
        {
            if (isHoldingItem)
            {
                Vector3 screenPoint =
                    GameplayManager.Instance.zoomElementsCamera.ScreenToWorldPoint((Vector3) Input.mousePosition);
                screenPoint.z = 0;

                Debug.Log(screenPoint.y + offsetVector.y);

                if (screenPoint.y > maxTopValue)
                    screenPoint.y = maxTopValue;

                transform.position = Vector3.Lerp(transform.position, screenPoint + offsetVector,
                    draggingSpeed * Time.deltaTime);
            }

            if (!isHoldingItem && returnToStartPosition && returningToStartPosition)
            {
                Vector3 screenPoint = startPosition;
                screenPoint.z = 0;

                transform.localPosition = Vector3.Lerp(transform.localPosition, screenPoint,
                    draggingSpeed * Time.deltaTime / 1.5f);

                if (Vector3.Distance(transform.localPosition, screenPoint) < 0.1f)
                {
                    transform.localPosition = screenPoint;
                    returningToStartPosition = false;
                }
            }
        }
    }
}
