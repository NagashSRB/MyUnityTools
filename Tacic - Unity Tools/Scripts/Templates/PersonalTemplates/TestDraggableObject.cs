using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.Scripts.Templates
{
    public class TestDraggableObject : MonoBehaviour
    {
        public UnityAction<TestDraggableObject> OnMousePressed { get; set; }
        public UnityAction<TestDraggableObject> OnMouseMoved { get; set; }
        public UnityAction<TestDraggableObject> OnMouseReleased { get; set; }
        // On Object moved or Something Happened if you know the context
        // OnXHappened, OnXHappening, OnXHappen - past, present, future
        
        // public int currentPosition;
        
        public int Index
        {
            get => index;
            set => index = value;
        } 
        public bool IsDragged
        {
            get => isDragged;
            set => isDragged = value;
        }
        
        // [SerializeField] private SpriteRenderer buttonRenderer;
        private int index;
        private bool isClickable;
        private bool isDragged;
        private const float ScaleSizeConst = 1.16f;

        private void OnMouseDown()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }

            if (isClickable)
            {
                isDragged = true;
                OnMousePressed?.Invoke(this);
            }
        }

        private void OnMouseDrag()
        {
            // Change logic for dragging here, if you need both click and drag for example. Take some offset 
            // from starting position and set dragged to true if is still within range. or just do movement 2nd way
            
            // Movement: 1st type: offset - not moving on start. 2nd : Pivot is same as mouse pos
            // Animation types: 1st snap to position x = y. 2nd using coroutine to move to target position (can be customized)
            // 3rd: pivot following mouse with some kind of function - Lerp from current position is cool. Maybe can customize 
            
            // position
            // Vector3 mousePosition = GameplayManager.Instance.zoomElementsCamera.ScreenToWorldPoint(Input.mousePosition);
            // Vector3 position = transform.position;
            // mousePosition.z = position.z;
            // End = transform.position = ballposition
            
            // localposition. check if it works without offset. potential problem: z
            // converting to Vector2 doesnt count in z coordinate, so doing calculations with that in mind
            // and then on the end setting the right z coordinate is cool. it works with localPos, also pos.
            // other option is to set it on start, escpecially mouse input only needs correction so thats that.
            //
            // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2 mouseMovementOffset = mousePosition - previousMousePosition;
            // Vector2 position = transform.localPosition;
            // End - transform.localPosition = new Vector3(ballPosition.x, ballPosition.y, transform.localPosition.z);

            // probably if you are trying to move object to mouse position or smth depending on mouse position,
            // then z coordinate will be important to set.
            
            
            if (IsDragged)
            {
                // MoveObject()
            }
        }

        private void OnMouseUp()
        {
            // Multi touch disabled, user shouldnt be able to pause game before executing this.
            // If menu doesnt pause, it will execute and wait for next click, which wont work while its open.
            // If game is closed/stopped, when you enter, level progress controller loads from start, and
            // since mini game isnt finished there is no problems
            
            if (IsDragged)
            {

            }
            
            IsDragged = false;
        }

        public void SetClickable(bool isObjectClickable)
        {
            isClickable = isObjectClickable;
        }
        
        
        public void SetFinalState()
        {

        }
        
        public bool IsSolved()
        {
            return false;
        }

        public IEnumerator ClickAnimation()
        {
            yield return null;
        }

        private void DoSomething()
        {
            
        }
    }
}