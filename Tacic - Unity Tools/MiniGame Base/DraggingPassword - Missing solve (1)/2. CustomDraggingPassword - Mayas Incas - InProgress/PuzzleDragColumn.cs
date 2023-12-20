using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.DraggingPassword___Missing_solve__1_._2._CustomDraggingPassword___Mayas_Incas___InProgress
{
    public class PuzzleDragColumn : MonoBehaviour
    {
        public UnityAction<int, int> OnMouseReleased { get; set; }
        public bool IsEnabled { get; set; }
        public int ColumnIndex;

        public List<SpriteRenderer> plates;

        //bool first plate is top plate? is it always same distance or no? upgrade za ovaj ftr.
        public List<int> values;

        //or numbers?
        public int visibleFirstIndex;
        public int startingIndex;

        //public List<SpriteRenderer> numbers;

        private Vector3 visiblePlatePosition;
        private Vector3 topPlatePosition;
        private float distanceBetweenPlates;

        private Vector3 mousePreviousPosition;
        // private float topPlateMaxYPosition;
        // private float bottomPlateMinYPosition;

        private int topPlateIndex;
        private int bottomPlateIndex;
        private int currentPlateIndex;

        private bool shouldSnap = true;

        // Top plate has index = 0

        private void Awake()
        {
            //IsEnabled = true;

            startingIndex = visibleFirstIndex;

            topPlateIndex = 0;
            bottomPlateIndex = plates.Count - 1;
            currentPlateIndex = visibleFirstIndex;
            visiblePlatePosition = plates[visibleFirstIndex].transform.localPosition;
            topPlatePosition = plates[0].transform.localPosition;
            distanceBetweenPlates = plates[0].transform.localPosition.y - plates[1].transform.localPosition.y;
            //topPlateMaxYPosition = plates[0].transform.localPosition.y + distanceBetweenPlates;
            //bottomPlateMinYPosition = plates[plates.Count - 1].transform.localPosition.y - distanceBetweenPlates;

        }

        private void OnMouseDown()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }
            
            // Testing move to position coroutine. Manager should call these functions.
            // Random random = new Random();
            // int rnd = random.Next(0, plates.Count);
            // Debug.Log($"Random index: {rnd}");
            // float offset = GetMovementOffsetFromTargetIndex(rnd);
            // StartCoroutine(MoveCoroutine(2, offset));

            mousePreviousPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnMouseDrag()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }

            if (IsEnabled)
            {
                Vector3 mouseCurrentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float mouseYOffset = mouseCurrentPosition.y - mousePreviousPosition.y;
                mousePreviousPosition = mouseCurrentPosition;
                if (mouseYOffset == 0)
                {
                    return;
                }

                TryToMovePlates(mouseYOffset);
            }
        }

        private void OnMouseUp()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }

            if (IsEnabled)
            {
                if (shouldSnap)
                {
                    SnapPlatesToPosition();
                }

                OnMouseReleased?.Invoke(values[currentPlateIndex], ColumnIndex);
            }
        }

        public float GetMovementOffsetFromTargetIndex(int targetIndex)
        {
            //amountToIncrease
            int downAmountToMove;
            int upAmountToMove;
            int amountToMove;
            if (targetIndex > currentPlateIndex)
            {
                downAmountToMove = targetIndex - currentPlateIndex;
                upAmountToMove = currentPlateIndex + plates.Count - targetIndex;
            }
            else
            {
                downAmountToMove = plates.Count - currentPlateIndex + targetIndex;
                upAmountToMove = targetIndex - currentPlateIndex;
            }

            amountToMove = upAmountToMove > downAmountToMove ? upAmountToMove : -downAmountToMove;
            return amountToMove * distanceBetweenPlates;
        }

        public IEnumerator MoveCoroutine(float duration, float offset)
        {
            IsEnabled = false;
            float step = 0;
            while (step < 1)
            {
                float stepInThisFrame = Time.deltaTime / duration;
                step += stepInThisFrame;
                TryToMovePlates(stepInThisFrame * offset);
                yield return null;
            }

            SnapPlatesToPosition();
            IsEnabled = true;
        }

        private void SnapPlatesToPosition()
        {
            Vector3 currentPlatePosition = plates[currentPlateIndex].transform.localPosition;
            float offset = visiblePlatePosition.y - currentPlatePosition.y;
            MoveAllPlates(offset);
        }

        private void TryToMovePlates(float offset)
        {
            MoveAllPlates(offset);
            Vector3 plateTeleportDirection = GetDirectionPlateShouldTeleport();
            if (plateTeleportDirection == Vector3.zero)
            {
                return;
            }

            TeleportPlate(plateTeleportDirection);
        }

        private IEnumerator MoveObjectsToPosition(List<Transform> objectsToMove, Vector2 startPosition,
            Vector2 endPosition, float seconds = 1)
        {
            float step = 0;
            List<Vector2> startingPositions = new List<Vector2>();
            foreach (var objectToMove in objectsToMove)
            {
                startingPositions.Add(objectToMove.transform.localPosition);
            }

            while (step < 1)
            {
                for (var i = 0; i < objectsToMove.Count; i++)
                {
                    Transform objectToMove = objectsToMove[i];
                    objectToMove.localPosition = Vector2.Lerp(
                        startingPositions[i], endPosition, step);
                }


                step += Time.deltaTime / seconds;
                yield return null;
            }
        }

        private void MoveAllPlates(float offset)
        {
            foreach (SpriteRenderer plate in plates)
            {
                plate.transform.localPosition = plate.transform.localPosition + offset * Vector3.up;
            }
        }

        private Vector3 GetDirectionPlateShouldTeleport()
        {
            if (plates[currentPlateIndex].transform.localPosition.y >
                visiblePlatePosition.y + distanceBetweenPlates / 2)
            {
                return Vector3.down;
            }

            if (plates[currentPlateIndex].transform.localPosition.y <
                visiblePlatePosition.y - distanceBetweenPlates / 2)
            {
                return Vector3.up;
            }

            return Vector3.zero;
        }

        private void TeleportPlate(Vector3 direction)
        {
            // Teleport after current bottom position
            // Changing Top plate now to bottom
            // Should move current by 1 on recalculate?
            if (direction == Vector3.down)
            {
                plates[topPlateIndex].transform.localPosition = plates[bottomPlateIndex].transform.localPosition
                                                                + direction * distanceBetweenPlates;
                IncreaseAllIndexes();
                return;
            }

            if (direction == Vector3.up)
            {
                plates[bottomPlateIndex].transform.localPosition = plates[topPlateIndex].transform.localPosition
                                                                   + direction * distanceBetweenPlates;
                DecreaseAllIndexes();
            }
        }

        private void IncreaseAllIndexes()
        {
            bottomPlateIndex = MoveIndexByAmount(topPlateIndex, 1);
            topPlateIndex = MoveIndexByAmount(bottomPlateIndex, 1);
            currentPlateIndex = MoveIndexByAmount(currentPlateIndex, 1);
        }

        private void DecreaseAllIndexes()
        {
            topPlateIndex = MoveIndexByAmount(topPlateIndex, -1);
            bottomPlateIndex = MoveIndexByAmount(bottomPlateIndex, -1);
            currentPlateIndex = MoveIndexByAmount(currentPlateIndex, -1);
        }

        private int MoveIndexByAmount(int index, int amount)
        {
            return (index + plates.Count + amount) % plates.Count;

        }
    }
}