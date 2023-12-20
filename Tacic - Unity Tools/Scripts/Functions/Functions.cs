using System.Collections;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.Functions
{
    public class TestComponent : MonoBehaviour
    {
                
    }
        
    public class TacicFunction
    {
 
        // Raycast function
        private TestComponent RaycastFunction2D(Vector3 position)
        {
            //TestComponent targetComponent = this;
            TestComponent targetComponent = null;
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(GameplayManager.Instance.zoomElementsCamera.ScreenToWorldPoint(position), Vector2.zero);

            if (hits.Length <= 0)
            {
                return null;
            }
             
            foreach (RaycastHit2D hit in hits)
            {
                TestComponent component = hit.transform.GetComponent<TestComponent>();
                if (component != null) //  && component != this if this object is current one trying to find 
                {
                    return component;
                }
            }
            
            return null;
        }
    }
}

////////////////////////////
/// Editor
// private GameObject CreateGameObject(Transform objectParent)
// {
//     return new GameObject
//     {
//         transform =
//         {
//             position = Vector3.zero,
//             rotation = Quaternion.identity,
//             localScale = Vector3.one,
//             parent = objectParent
//         }
//     };
// }
////////////////////////////



// private IEnumerator MoveAnimation(ComputerPuzzleTile puzzle, int currPos1, int currPos2, int num)
// {
//     puzzle.DisableCollider();
//     puzzle.GetComponent<SpriteRenderer>().sortingOrder--;
//     float step = 0;
//     float seconds = 1f;
//     Quaternion puzzle1Begin = puzzle.transform.rotation * Quaternion.Euler(0, 0, 1 * Math.Sign(num));
//     Quaternion puzzle1End = puzzle.transform.rotation * Quaternion.Euler(0, 0, num * angle);
//     while (step < 1)
//     {
//         puzzle.transform.localPosition = Vector3.Slerp(
//             holders[currPos1].transform.localPosition, holders[currPos2].transform.localPosition, step);
//         
//         puzzle.transform.rotation = Quaternion.Slerp(
//             puzzle1Begin, puzzle1End, step);
//
//         step += Time.deltaTime / seconds;
//         yield return null;
//     }
//
//     puzzle.transform.position = holders[currPos2].transform.position;
//     puzzle.transform.rotation = puzzle1End;
//     puzzle.EnableCollider();
//     puzzle.GetComponent<SpriteRenderer>().sortingOrder++;
// }
        
        
// Wait for all coroutines to finish

// List<Coroutine> moveToStartingPositionCoroutines = new List<Coroutine>();
// foreach (DragAndDropSwapObject swapObject in swapObjects)
// {
//         moveToStartingPositionCoroutines.Add(StartCoroutine(swapObject.MoveToStartingPositionCoroutine()));
// }
//     
// foreach (Coroutine coroutine in moveToStartingPositionCoroutines)
// {
//         yield return coroutine;
// }




// Smooth Lerp Coroutine. Should test, not sure if it works properly
// public IEnumerator MoveToStartingPositionCoroutine(float duration = 0.5f)
// {
//         Vector3 fromPositon = transform.position;
//         Vector3 toPosition = StartPosition;
//         hintRenderer.sortingOrder++;
//         float percentage = 0;
//         float customizedPercentage = 0;
//         while (percentage < 1)
//         {
//                 transform.position = Vector3.Lerp(fromPositon, toPosition, customizedPercentage);
//                 percentage += Time.deltaTime / duration;
//                 customizedPercentage = percentage * percentage * (3f - 2f * percentage);
//                 yield return null;
//         }
//             
//         transform.position = StartPosition;
//         hintRenderer.sortingOrder = defaultSpriteOrder;
// }