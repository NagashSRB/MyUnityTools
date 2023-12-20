using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard
{
    public class BoardField : PositionHolder
    {
        public Vector2Int index;
        private List<BoardElement> currentElements = new List<BoardElement>();
        private bool shouldAllowMultipleElements;
        public List<BoardField> adjacentFields;
        
        private List<BoardElement> startingElements;

        public void Initialize(List<BoardElement> boardElements = null, bool shouldAllowMultipleElementsOnField = false)
        {
            currentElements = boardElements;
            shouldAllowMultipleElements = shouldAllowMultipleElementsOnField;
            if (currentElements != null)
            {
                startingElements = new List<BoardElement>(currentElements);
                PlaceElement(currentElements[0]);
            }
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

        public bool AddAdjacentField(BoardField adjacentField)
        {
            if (!adjacentFields.Contains(adjacentField))
            {
                adjacentFields.Add(adjacentField);
                return true;
            }

            return false;
        }

        public void ResetFieldState()
        {
            foreach (BoardElement currentElement in currentElements)
            {
                RemoveElement(currentElement);
            }

            currentElements = startingElements;

            foreach (BoardElement element in currentElements)
            {
                PlaceElement(element);
            }
        }
    }
}
