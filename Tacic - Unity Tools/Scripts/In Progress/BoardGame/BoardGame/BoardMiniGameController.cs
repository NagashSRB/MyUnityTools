using System;
using System.Collections;
using System.Collections.Generic;
using Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.MatrixBoard;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame
{
    //TODO: IBoardController? Controller in general? Ili abstract class I: initialize, is solved, disableenable
    public abstract class BoardMiniGameController : Controller
    {
        [Header("References")] 
        [SerializeField] private MatrixBoardController boardController;
        [SerializeField] private List<BoardElement> elements;
        [SerializeField] private List<BoardField> fields;
        [SerializeField] private int boardWidth;
        [SerializeField] private int boardHeight;
        //[Header("Settings")]
        //[Header("Sounds")]

        private void Awake()
        {
            boardController.Initialize(elements, fields, boardWidth, boardHeight);
        }

        public override void EnableAllColliders() => elements.ForEach(element => element.EnableCollider());

        public override void DisableAllColliders() => elements.ForEach(element => element.DisableCollider());
    }

    public interface IClickable
    {
        public void HandleMouseDown();
        public void HandleMouseUp();
    }

    public interface IDraggable
    {
        public void HandleMouseDown();
        public void HandleMouseDrag();
        public void HandleMouseUp();
    }
}
