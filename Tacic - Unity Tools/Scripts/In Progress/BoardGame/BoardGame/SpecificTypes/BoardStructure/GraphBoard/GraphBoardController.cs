using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard
{
    public class GraphBoardController : MonoBehaviour
    {
        private Dictionary<BoardField, List<BoardPath>> paths;
        
        //TODO: Razmisli, ali verovatno je visak jer je duplikat
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
            //Indeksirana polja sa int id neki. 
            //Kako kreirati graf.
            
            //Dictionary za path da se za pocetak iz inspectora popuni.
            //Polja i veze na osnovu dictionaryja onda
            //Postavljanje elementa na polja. Direktan pristup? ili ne bese, polja
            //indeksi je bilo poljima, pa za svaki elem namestis. Closest field bi trazio npr za eleemente. al to u onvalidate
            //elem da ima field. a field medjusobno
            
            
            foreach (BoardPath path in pathList)
            {
                //moze duplo u fji. long term instruction count gain
                AddPathToDictionary(path, path.startField);
                AddPathToDictionary(path, path.endField);
                path.startField.AddAdjacentField(path.endField);
                path.endField.AddAdjacentField(path.startField);
            }

            foreach (DraggableElement element in elements)
            {
                PlaceElementOnField(element, element.currentField);
                //check if path exists?
                UpdateElementNeighbourPaths(element);
                element.startingPath = element.currentPath;
                //, paths[element.currentField][0]
            }
            
            
            //TODO: Element placing logic, initial and also later on
            // foreach (BoardField field in fields)
            // {
            //     board[field.index.x, field.index.y] = field;
            // }
            //
            // foreach (BoardElement element in elements)
            // {
            //     PlaceElementOnField(board[element.index.x, element.index.y], element);
            // }
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

        //Draw path between two fields
        public void AddNewPath(BoardField startField, BoardField endField)
        {
            
        }

        //Todo : Consider adding support for multiple graph parts (disconnected graph) and their
        //TODO: merging or
        
        
        //////////////////////////////////////////////////////////////
        
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
