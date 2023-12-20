using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WebelinxGames.RoomsAndExits.Level310
{
    public class GraphBoardController : MonoBehaviour
    {
        private Dictionary<BoardField, List<BoardPath>> paths;
        private List<DraggableElement> elements;
        private List<BoardField> fields;
        private List<BoardPath> pathList;
        
        public void Initialize(List<DraggableElement> boardElements, List<BoardField> boardFields, 
            List<BoardPath> boardPaths)
        {
            elements = boardElements;
            fields = boardFields;
            pathList = boardPaths;
            paths = new Dictionary<BoardField, List<BoardPath>>();
            
            SetInitialState();
        }

        private void SetInitialState()
        {
            foreach (BoardPath path in pathList)
            {
                AddPathToDictionary(path, path.startField);
                AddPathToDictionary(path, path.endField);
                path.startField.AddAdjacentField(path.endField);
                path.endField.AddAdjacentField(path.startField);
            }

            foreach (DraggableElement element in elements)
            {
                PlaceElementOnField(element, element.currentField);
                UpdateElementNeighbourPaths(element);
                element.startingPath = element.currentPath;
            }
            
            foreach (BoardField field in fields)
            {
                field.Initialize();
            }
        }

        private void AddPathToDictionary(BoardPath path, BoardField field)
        {
            if (paths.ContainsKey(field))
            {
                paths[field].Add(path);
            }
            else
            {
                paths.Add(field, new List<BoardPath>{path});
            }   
        }

        // Function for updating information for draggable element
        public void UpdateElementNeighbourPaths(DraggableElement element, BoardPath nextPath = null)
        {
            // Set default element path 
            nextPath ??= paths[element.currentField].FirstOrDefault();
            if (nextPath == null)
            {
                return;
            }

            List<BoardPath> startingFieldAdjacentPaths = paths[nextPath.startField].Where(path => path != nextPath).ToList();
            List<BoardPath> endingFieldAdjacentPaths = paths[nextPath.endField].Where(path => path != nextPath).ToList();
            element.SetCurrentPath(nextPath, startingFieldAdjacentPaths, endingFieldAdjacentPaths);
        }

        public bool PlaceElementOnField(BoardElement element, BoardField field)
        {
            if (field == null || element == null)
            {
                return false;
            }
            
            return field.PlaceElement(element);
        }

        public bool RemoveElementFromBoard(BoardElement element)
        {
            if (element == null || element.currentField == null)
            {
                return false;
            }

            return element.currentField.RemoveElement(element);
        }

        public void ResetBoardState()
        {
            foreach (BoardField field in fields)
            {
                field.ResetFieldState();
            }

            foreach (DraggableElement element in elements)
            {
                element.ResetToInitialState();
            }
        }
    }
}
