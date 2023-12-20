using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.SwapElementsByDragging.v2___Custom___215
{
    public class MaskHint : MonoBehaviour
    {
        [SerializeField] private Vector3 offsetVector;
        [SerializeField] private SpriteRenderer hintRenderer;
        [SerializeField] private int currentPositionIndex;
        [SerializeField] private int targetPositionIndex;
        public bool IsClickable { get; set; }
        public UnityAction<MaskHint> OnTargetFound { get; set; }
        public UnityAction<MaskHint> OnClicked { get; set; }
        public bool IsDragged { get; set; }
        private int defaultSpriteOrder;
        public Vector3 StartPosition { get; set; }
        public Vector3 TargetPosition { get; set; }
        public float DraggingSpeed { get; set; }

        public int CurrentPositionIndex
        {
            get => currentPositionIndex;
            set => currentPositionIndex = value;
        }
        public int TargetPositionIndex
        {
            get => targetPositionIndex;
            set => targetPositionIndex = value;
        }

        void Awake()
        {
            StartPosition = transform.position;
            defaultSpriteOrder = hintRenderer.sortingOrder;
        }
        
        public void OnMouseDown()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }
            if (IsClickable)
            {
                IsDragged = true;
                hintRenderer.sortingOrder = defaultSpriteOrder + 1;
                OnClicked?.Invoke(this);
            }
        }

        private void OnMouseDrag()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }
            if (IsDragged)
            { 
                Vector3 mousePosition = GameplayManager.Instance.zoomElementsCamera.ScreenToWorldPoint(Input.mousePosition);
                //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 position = transform.position;
                mousePosition.z = position.z;
                transform.position = Vector3.Lerp(position,
                    mousePosition + offsetVector, DraggingSpeed * Time.deltaTime);
            }
        }

        public void OnMouseUp()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }
            
            if (IsDragged)
            {
                MaskHint hit = RaycastFunction2D(Input.mousePosition, GameplayManager.Instance.zoomElementsCamera);
                OnTargetFound?.Invoke(hit);
                hintRenderer.sortingOrder++;
            }
            IsDragged = false;
        }

        public bool IsSolved()
        {
            return currentPositionIndex == targetPositionIndex;
        }

        public void SetFinalState()
        {
            currentPositionIndex = targetPositionIndex;
            StartPosition = TargetPosition;
        }

        public IEnumerator MoveToStartingPositionCoroutine(float duration = 0.5f)
        {
            Vector3 fromPositon = transform.position;
            Vector3 toPosition = StartPosition;
            hintRenderer.sortingOrder++;
            //hintRenderer.sortingOrder = setCustomOrder ? releasedSpriteOrder : defaultSpriteOrder;
            float percentage = 0;
            float customizedPercentage = 0;
            while (percentage < 1)
            {
                transform.position = Vector3.Lerp(fromPositon, toPosition, customizedPercentage);
                percentage += Time.deltaTime / duration;
                customizedPercentage = percentage * percentage * (3f - 2f * percentage);
                yield return null;
            }
            transform.position = StartPosition;
            hintRenderer.sortingOrder = defaultSpriteOrder;
        }

        private MaskHint RaycastFunction2D(Vector3 position, Camera cam)
        {
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(cam.ScreenToWorldPoint(position), Vector2.zero);

            if (hits.Length <= 0)
            {
                return null;
            }
            
            foreach (var hit in hits)
            {
                MaskHint hitComponent = hit.transform.GetComponent<MaskHint>();
                if (hitComponent != null && hitComponent != this)
                {
                    return hitComponent;
                }
            }
            return null;
        }
    }
}