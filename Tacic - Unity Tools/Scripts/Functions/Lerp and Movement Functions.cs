using System.Collections;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.Functions
{
    public class LerpAndMovementFunctions : MonoBehaviour
    {
        // Simple lerp with time
        private IEnumerator MoveAnimation(Transform objectToMove, Vector2 startPosition, Vector2 endPosition, float duration = 1)
        {
            float step = 0;
            while (step < 1)
            {
                objectToMove.localPosition = Vector2.Lerp(
                    startPosition, endPosition, step);

                step += Time.deltaTime / duration;
                yield return null;
            }
            objectToMove.localPosition = new Vector3(endPosition.x, endPosition.y, objectToMove.localPosition.z);
        }
        
        //Color lerp
        public IEnumerator ColorLerp(bool turnPageOn, float duration)
        {
            SpriteRenderer spriteRenderer = new SpriteRenderer();
            
            Color startColor = spriteRenderer.color;
            Color endColor = startColor;
            startColor.a = turnPageOn ? 0 : 1;
            endColor.a = 1 - startColor.a;
            float step = 0;
            while (step < 1)
            {
                spriteRenderer.color = Color.Lerp(
                    startColor, endColor, step);

                step += Time.deltaTime / duration;
                yield return null;
            }

            spriteRenderer.color = endColor;
        }
    }
}

