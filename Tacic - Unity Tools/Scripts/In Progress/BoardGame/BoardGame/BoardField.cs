using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame
{
    public class BoardField : PositionHolder
    {
        public Vector2Int index;
        
        private List<BoardElement> currentElements;

        private bool shouldAllowMultipleElements;

        public void Initialize(List<BoardElement> boardElements, bool shouldAllowMultipleElementsOnField)
        {
            currentElements = boardElements;
            shouldAllowMultipleElements = shouldAllowMultipleElementsOnField;
            PlaceElement(currentElements[0]);
        }

        public bool PlaceElement(BoardElement element)
        {
            if (!IsFieldTaken())
            {
                element.PlaceOnField(this);
                currentElements.Add(element);
                return true;
            }

            return false;
        }

        public bool RemoveElement(BoardElement element)
        {
            if (currentElements.Contains(element))
            {
                element.RemoveFromField();
                currentElements.Remove(element);
                return true;
            }

            return false;
        }
        
        public bool IsFieldTaken()
        {
            if (shouldAllowMultipleElements || currentElements.Count == 0)
            {
                return false;
            }

            return true;
        }

        public BoardElement GetElement()
        {
            if (currentElements.Count == 0)
            {
                return null;
            }
            
            return currentElements[0];
        }
        
        public List<BoardElement> GetAllElements() => currentElements;
    }
}
