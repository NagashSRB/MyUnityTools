using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard
{
    //TODO: IBoardController? Controller in general? Ili abstract class I: initialize, is solved, disableenable
    public class GraphBoardMiniGameController : Controller
    {
        [Header("References")] 
        [SerializeField] private GraphBoardController boardController;
        [SerializeField] private TrailController trailController;
        
        [SerializeField] private List<DraggableElement> elements;
        [SerializeField] private List<BoardField> fields;
        [SerializeField] private List<BoardPath> paths;
        //optional
        [SerializeField] private List<Trail> trails;
        [SerializeField] private List<BoardField> targetFields;
        // snap
        [SerializeField] private float snapDistance = 0.2f;
        [SerializeField] private float fieldOffsetDistance = 0.2f;
        [SerializeField] private float switchingDirectionOffset = 0.2f;
        //[Header("Settings")]
        //[Header("Sounds")]
        // TODO: Maybe this would be example, inherited values

        private void OnValidate()
        {
            if (elements.Count == 0)
            {
                elements = GetComponentsInChildren<DraggableElement>().ToList();
            }
            
            if (fields.Count == 0)
            {
                fields = GetComponentsInChildren<BoardField>().ToList();
            }

            if (trailController != null && trails.Count == 0)
            {
                trails = GetComponentsInChildren<Trail>().ToList();
            }
        }

        private void Awake()
        {
            Initialize();
        }

        protected void Initialize()
        {
            boardController.Initialize(elements, fields, paths);
            trailController.Initialize(elements, trails);
            trailController.OnTrailCollision += ResetMiniGame;
            //Za svaki draggable? u initialize board controllera npr

            //Draggable part
            for (int i = 0; i < elements.Count; i++)
            {
                DraggableElement element = elements[i];
                element.startedFromField = element.currentField;
                element.Initialize(switchingDirectionOffset, snapDistance, fieldOffsetDistance, targetFields[i]);
                //TODO: Pass in initialize method.
                element.OnPathChanged += HandleElementPathChanged;
                element.OnElementMoved += HandleElementMoved;
                element.onMouseDown.AddListener(() => HandleMouseDown(element));
                element.onMouseUp.AddListener(() => HandleMouseUp(element));
                element.transform.position = element.currentField.transform.position;

            }
        }

        protected virtual void HandleMouseDown(DraggableElement element)
        {
            boardController.RemoveElementFromBoard(element);
        }

        protected virtual void HandleElementMoved(DraggableElement element)
        {
            trailController.DrawTrail(element, element.GetCurrentPath());
        }

        protected virtual void HandleElementPathChanged(DraggableElement element, BoardPath newPath)
        {
            boardController.UpdateElementNeighbourPaths(element, newPath);
            trailController.DrawTrail(element, newPath);
        }
        
        protected virtual void HandleMouseUp(DraggableElement element)
        {
            boardController.PlaceElementOnField(element, element.currentField);
            trailController.DrawTrail(element, element.GetCurrentPath());
            CheckIfGameFinished();
        }

        protected void ResetMiniGame()
        {
            Debug.Log("Reset mini game");
            boardController.ResetBoardState();
            trailController.ResetTrails();
            EnableAllColliders();
        }
        
        //TODO: Is casting expensive?
        /// /////////

        public override void EnableAllColliders() => elements.ForEach(element => element.EnableCollider());

        public override void DisableAllColliders() => elements.ForEach(element => element.DisableCollider());
        protected override bool IsStateCorrect() => elements.All(element => element.IsCorrect());

        public override IEnumerator Solve(float solveNextElementWaitTime, float solveMovementDuration)
        {
            yield return null;
        }

        public override void SetFinalState()
        {
            throw new System.NotImplementedException();
        }
    }
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