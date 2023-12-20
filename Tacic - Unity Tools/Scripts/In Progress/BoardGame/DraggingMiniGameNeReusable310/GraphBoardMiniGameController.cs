using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WebelinxGames.RoomsAndExits.Level310
{
    public class GraphBoardMiniGameController : Controller
    {
        [Header("References")] 
        [SerializeField] private GraphBoardController boardController;
        [SerializeField] private TrailController trailController;
        [SerializeField] private List<DraggableElement> elements;
        [SerializeField] private List<BoardField> fields;
        [SerializeField] private List<BoardPath> paths;
        [Header("Optional")] 
        [SerializeField] private List<Trail> trails;
        [SerializeField] private List<BoardField> targetFields;
        [Header("Snapping and offsets")] 
        [SerializeField] private float snapDistance = 0.2f;
        [SerializeField] private float fieldOffsetDistance = 0.2f;
        [SerializeField] private float switchingDirectionOffset = 0.2f;
        [Header("Sounds")] 
        [SerializeField] private AudioSource wrongSound;
        [SerializeField] private AudioSource clickedSound;
        
        private DraggableElement draggedElement;

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
            
            //Draggable part
            for (int i = 0; i < elements.Count; i++)
            {
                DraggableElement element = elements[i];
                element.startedFromField = element.currentField;
                element.Initialize(switchingDirectionOffset, snapDistance, fieldOffsetDistance, targetFields[i]);
                element.OnPathChanged += HandleElementPathChanged;
                element.OnElementMoved += HandleElementMoved;
                element.onMouseDown.AddListener(() => HandleMouseDown(element));
                element.onMouseUp.AddListener(() => HandleMouseUp(element));
                element.transform.position = element.currentField.transform.position;

            }
        }

        protected virtual void HandleMouseDown(DraggableElement element)
        {
            SoundManager.Instance.PlaySound(clickedSound);
            draggedElement = element;
            boardController.RemoveElementFromBoard(element);
        }

        protected virtual void HandleElementMoved(DraggableElement element)
        {
            trailController.DrawTrail(element, element.GetCurrentPath());

            foreach (DraggableElement draggableElement in elements)
            {
                if (draggableElement != element &&
                    Vector2.Distance(draggableElement.transform.position, element.transform.position) <= 0.4f)
                {
                    ResetMiniGame();
                    return;
                }
            }
        }

        protected virtual void HandleElementPathChanged(DraggableElement element, BoardPath newPath)
        {
            boardController.UpdateElementNeighbourPaths(element, newPath);
            trailController.DrawTrail(element, newPath);
        }
        
        protected virtual void HandleMouseUp(DraggableElement element)
        {
            draggedElement = null;
            boardController.PlaceElementOnField(element, element.currentField);
            trailController.DrawTrail(element, element.GetCurrentPath());
            CheckIfGameFinished();
        }

        protected void ResetMiniGame()
        {
            SoundManager.Instance.PlaySound(wrongSound);
            if (draggedElement != null)
            {
                draggedElement.StopDragging();   
            }
            
            boardController.ResetBoardState();
            trailController.ResetTrails();
            EnableAllColliders();
        }

        public override void EnableAllColliders() => elements.ForEach(element => element.EnableCollider());

        public override void DisableAllColliders() => elements.ForEach(element => element.DisableCollider());
        protected override bool IsStateCorrect() => elements.All(element => element.IsCorrect());

        public override IEnumerator Solve(float solveNextElementWaitTime, float solveMovementDuration)
        {
            yield return null;
        }

        public override void SetFinalState()
        {
        }
    }
}