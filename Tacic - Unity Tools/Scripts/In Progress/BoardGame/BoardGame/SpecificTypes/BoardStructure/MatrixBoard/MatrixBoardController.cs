using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.MatrixBoard
{
    public class MatrixBoardController : MonoBehaviour
    {
        //TODO: indexers if needed for [] and [][]
        //Mozda ne treba monobehaviour. vise kao data klasa sa utilitijem

        protected List<BoardElement> elements;
        protected List<BoardField> fields;
        protected int width;
        protected int height;
        
        protected BoardField[,] board;

        public void Initialize(List<BoardElement> boardElements, List<BoardField> boardFields, 
            int boardWidth, int boardHeight)
        {
            elements = boardElements;
            fields = boardFields;
            width = boardWidth;
            height = boardHeight;

            board = new BoardField[height, width];
            SetInitialState();
        }

        private void SetInitialState()
        {
            // polja smatraj da su indeksirana. i elementi - u kontrolleru pre ovog (Input scenski za sad bez editora)
            // pozicionirana lepo preko editorskih
            
            //TODO: Mozda proslediti kao parametre elements i fields

            foreach (BoardField field in fields)
            {
                board[field.index.x, field.index.y] = field;
            }

            foreach (BoardElement element in elements)
            {
                PlaceElementOnField(board[element.index.x, element.index.y], element);
            }
        }

        //TODO: is index valid check. sta ako prosledis nevalidan index? to je ovde bitno da se proveri
        public bool PlaceElementOnField(BoardField field, BoardElement element)
        {
            if (field == null || element == null)
            {
                return false;
            }
            
            return field.PlaceElement(element);
        }

        public bool RemoveElementFromBoard(BoardElement element)
        {
            if (element == null || !IsIndexValid(element.index))
            {
                return false;
            }
            
            return GetFieldFromIndex(element.index).RemoveElement(element);
        }

        public BoardField GetFieldElementIsPlacedOn(BoardElement element)
        {
            if (element == null || !IsIndexValid(element.index))
            {
                return null;
            }

            return board[element.index.x, element.index.y];
        }

        public BoardElement GetElementFromField(BoardField field)
        {
            if (field == null)
            {
                return null;
            }

            return field.GetElement();
        }
        
        //Get element i get elemnets from field
        
        public BoardElement GetElementFromIndex(Vector2Int elementIndex)
        {
            if (!IsIndexValid(elementIndex))
            {
                return null;
            }
            
            return board[elementIndex.x, elementIndex.y].GetElement();
        }
        
        // Rad sa indeksima samo u ovoj klasi. U kontroleru ne. mozda
        
        public BoardField GetFieldFromIndex(Vector2Int fieldIndex)
        {
            if (!IsIndexValid(fieldIndex))
            {
                return null;
            }
            
            return board[fieldIndex.x, fieldIndex.y];
        }
        
        public BoardElement GetElementNeighbour(BoardElement element, Vector2Int delta)
        {
            if (element == null)
            {
                return null;
            }
            
            Vector2Int neighbourIndex = element.index + delta;
            return GetElementFromIndex(neighbourIndex);
        }
        
        // Sva kompleksnija logika za matricu ovde da se doda, nasledi klasa i samo rokaju fje. koriste se
        // usput i vec ove napravljene (koje imaju i checkove za null i sve)
        
        protected bool IsIndexValid(Vector2Int index) => index.x >= 0 && index.x < height && index.y >= 0 && index.y < width;
        
        protected Vector2Int GetVectorIndexFromIntegerIndex(int index) => new Vector2Int(index / width, index % width);
        
        protected int GetIntegerIndexFromVectorIndex(Vector2Int index) => index.x * width + index.y;
    }
}
