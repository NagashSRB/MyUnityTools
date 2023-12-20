using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteLineRenderer : MonoBehaviour
    {
        public Transform StartPoint => startPoint;
        public Transform EndingPoint => endPoint;

        [SerializeField] protected SpriteRenderer spriteRenderer;
        protected Transform startPoint;
        protected Transform endPoint;
        protected float lineWidth = 1;
        protected float lineLength;
        protected float lineAngle;

        protected void OnValidate()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected void Awake()
        {
            spriteRenderer.transform.localScale = new Vector3(spriteRenderer.transform.localScale.x, lineWidth, spriteRenderer.transform.localScale.z);
        }

        public void Initialize(Transform startingLinePoint, Transform endLinePoint)
        {
            SetUpPoints(startingLinePoint, endLinePoint);
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        public void SetUpPoints(Transform startingLinePoint, Transform endLinePoint)
        {
            startPoint = startingLinePoint;
            endPoint = endLinePoint;
        }
        
        public void DrawLine()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            Vector2 lineBeginPosition = startPoint.position;
            Vector2 lineEndPosition = endPoint.position;
            Vector2 lineCenter = (lineBeginPosition + lineEndPosition) / 2;
            
            spriteRenderer.transform.position = new Vector3(lineCenter.x, lineCenter.y, spriteRenderer.transform.position.z);
            lineLength = Vector2.Distance(lineBeginPosition, lineEndPosition);
            spriteRenderer.transform.localScale = new Vector3(lineWidth, spriteRenderer.sprite.pixelsPerUnit / spriteRenderer.sprite.rect.height * lineLength, 1f);

            Vector2 delta = lineBeginPosition - lineEndPosition;
            lineAngle = Mathf.Acos(delta.normalized.x) * Mathf.Rad2Deg;
            if (delta.y < 0)
            {
                lineAngle = 360f - lineAngle;
            }
                
            spriteRenderer.transform.localEulerAngles = new Vector3(0, 0, lineAngle + 90);
            spriteRenderer.enabled = true;
        }

        public void SetActive(bool isActive)
        {
            spriteRenderer.enabled = isActive;
        }
    }
}