using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WebelinxGames.RoomsAndExits.Level310
{
    public class DraggableElement : BoardElement
    {
        public UnityAction<DraggableElement, BoardPath> OnPathChanged { get; set; }
        public UnityAction<DraggableElement> OnElementMoved { get; set; }

        [Tooltip("Snap to closest field on mouse release")]
        //public bool shouldSnapOnMouseRelease;
        private float snapDistance;
        private float switchingDirectionOffset;
        private float fieldAcceptanceOffset;
        private Vector3 previousMousePosition;
        private BoardField targetField;
        
        public BoardPath currentPath;
        private List<BoardPath> startFieldAdjacentPaths;
        private List<BoardPath> endFieldAdjacentPaths;
        public BoardField closestField;
        public BoardField startedFromField;

        public BoardPath startingPath;
        private BoardField startingField;

        private bool isClicked;
        
        public void ResetToInitialState()
        {
            currentPath = startingPath;
            //currentField = startingField; trebalo bi da resetuje field, proveriti
            closestField = startingField;
            transform.position = startingField.transform.position;
            //Debug.Log($"Reset element: {gameObject.name}");
            OnPathChanged?.Invoke(this, currentPath);
        }

        public void Initialize(float switchDirectionOffset, float elementSnapDistance, float fieldOffsetDistance, BoardField targetField)
        {
            switchingDirectionOffset = switchDirectionOffset;
            snapDistance = elementSnapDistance;
            fieldAcceptanceOffset = fieldOffsetDistance;
            this.targetField = targetField;

            startingField = currentField;
        }

        public override void OnMouseDown()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }

            previousMousePosition = GameplayManager.Instance.zoomElementsCamera.ScreenToWorldPoint(Input.mousePosition);
            isClicked = true;
            UpdateClosestField();
            
            base.OnMouseDown();
        }

        public override void OnMouseDrag()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }
            
            //TODO: Closest holder logic if not snapping. parameters like direction? moved toward which holder? events also for dragging
            //TODO: limit dist per frame?
            //todo: snap logika kad prelaze holdere da se ne gomila sitni offset

            if (isClicked)
            {
                Vector3 mousePosition = GameplayManager.Instance.zoomElementsCamera.ScreenToWorldPoint(Input.mousePosition);
                TryToMove(mousePosition);

                base.OnMouseDrag();
            }
        }

        private void TryToMove(Vector3 mousePosition)
        {
            // Code to measure...

            Vector3 mouseMovementVector = mousePosition - previousMousePosition;
            if (mouseMovementVector == Vector3.zero)
            {
                return;
            }
            
            mouseMovementVector = Vector3.ClampMagnitude(mouseMovementVector, 0.4f);
            
            //New position is transform.position + mouseMovementVector
            
            Vector3 closestProjectedPoint = GetClosestPointOnPaths(mouseMovementVector, transform.position + mouseMovementVector);
            if (!isClicked)
            {
                return;
            }
            //Debug.Log($"Drag continues");
            transform.position = GetClampedPosition(closestProjectedPoint);
            previousMousePosition = mousePosition;
            UpdateClosestField();
            OnElementMoved?.Invoke(this);
        }

        /// <summary>
        /// Check which point is closest to any of adjacent paths
        /// </summary>
        /// <returns>Closest point</returns>
        private Vector3 GetClosestPointOnPaths(Vector3 mouseMovementVector, Vector3 newPosition)
        {
            Vector3 closestProjectedPoint = GetProjectionPointOnPath(currentPath, mouseMovementVector);

            // If element not close enough to another field
            if (Vector3.Distance(transform.position, closestField.transform.position) >= switchingDirectionOffset)
            {
                return closestProjectedPoint;
            }

            BoardPath closestPath = currentPath;
            float minDistanceFromPath = GetPointDistanceFromVector(newPosition, currentPath.startField.transform.position, 
                currentPath.endField.transform.position);

            // Find closest point and closest path from adjacent(neighbour) paths
            List<BoardPath> adjacentPaths = closestField == currentPath.startField ? startFieldAdjacentPaths : endFieldAdjacentPaths;
            foreach (BoardPath adjacentPath in adjacentPaths)
            {
                Vector3 projectedPointOnPath = GetProjectionPointOnPath(adjacentPath, mouseMovementVector);
                float distanceFromPath = GetPointDistanceFromVector(newPosition, adjacentPath.startField.transform.position, 
                    adjacentPath.endField.transform.position);
                        
                if (distanceFromPath < minDistanceFromPath && Math.Abs(distanceFromPath - minDistanceFromPath) > 0.001f)
                {
                    minDistanceFromPath = distanceFromPath;
                    closestProjectedPoint = projectedPointOnPath;
                    closestPath = adjacentPath;
                }
            }
            
            // Current path has changed
            if (currentPath != closestPath)
            {
                OnPathChanged?.Invoke(this, closestPath);
            }

            return closestProjectedPoint;
        }

        private void UpdateClosestField()
        {
            float distanceToStartField =
                Vector3.Distance(currentPath.startField.transform.position, transform.position);
            float distanceToEndField =
                Vector3.Distance(currentPath.endField.transform.position, transform.position);

            closestField = distanceToStartField < distanceToEndField ? currentPath.startField : currentPath.endField;
        }

        /// <summary>
        /// Restricts movement of an element to the path (path vector) that is is currently being dragged on
        /// </summary>
        /// <param name="projectedPoint">Point you are trying to restrict</param>
        /// <returns></returns>
        private Vector3 GetClampedPosition(Vector3 projectedPoint)
        {
            float t = 0;

            if (Vector3.Dot(projectedPoint - currentPath.startField.transform.position, currentPath.GetPathVector()) >= 0)
            {
                t = Mathf.Clamp01((projectedPoint - currentPath.startField.transform.position).magnitude / currentPath.GetPathVector().magnitude);
            }

            return currentPath.startField.transform.position + t * currentPath.GetPathVector();
        }
        
        /// <summary>
        /// Returns closest distance between point and line segment (vector)
        /// </summary>
        /// <param name="point">Point that we calculate distance for</param>
        /// <param name="startVectorPosition">Coordinates of starting vector point</param>
        /// <param name="endVectorPosition">Coordinates of ending vector point</param>
        /// <returns></returns>
        public float GetPointDistanceFromVector(Vector3 point, Vector3 startVectorPosition, Vector3 endVectorPosition)
        {
            // For simplicity, we will name startPoint = A, endPoint = B, point = P
            Vector3 AB = endVectorPosition - startVectorPosition;
            Vector3 AP = point - startVectorPosition;
            Vector3 BP = point - endVectorPosition;

            float projectionLength = Vector3.Dot(AP, AB) / AB.magnitude;
            float t = projectionLength / AB.magnitude;
            if (t < 0)
            {
                return AP.magnitude;
            }

            if (t > 1)
            {
                return BP.magnitude;
            }

            Vector3 projectedPoint = startVectorPosition + projectionLength * AB.normalized;
            return Vector3.Distance(point, projectedPoint);
        }
        
        
        public override void OnMouseUp()
        {
            if (!GameplayManager.Instance.AreAllUIElementsClosed())
            {
                return;
            }

            // if (shouldSnapOnMouseRelease)
            // {
            //     SnapToClosestField();
            // }

            //Debug.Log($"MouseUpEntry: {gameObject.name}");
            if (isClicked)
            {
                //Debug.Log($"MouseUp: {gameObject.name}");
                isClicked = false;
                SetCurrentField();
                SnapToClosestFieldIfCloseEnough();

                base.OnMouseUp();
            }

        }

        public void StopDragging()
        {
            //isClicked = false;
            //Debug.Log($"StopDragging: {gameObject.name}");
            OnMouseUp();
        }
        
        private Vector3 GetProjectionPointOnPath(BoardPath path, Vector3 offsetedPosition)
        {
            float projectedVectorLength = Vector3.Dot(path.GetPathVector(), offsetedPosition) / path.GetPathVector().magnitude;
            Vector3 projectedVector = projectedVectorLength * path.GetPathVector().normalized;
            Vector3 projectedPoint = transform.position + projectedVector;
            return projectedPoint;
        }

        //TODO: Combine into one function what u can.
        public BoardPath GetCurrentPath() => currentPath;

        public void SetCurrentPath(BoardPath path, List<BoardPath> startFieldAdjacentPaths, List<BoardPath> endFieldAdjacentPaths)
        {
            currentPath = path;
            this.startFieldAdjacentPaths = startFieldAdjacentPaths;
            this.endFieldAdjacentPaths = endFieldAdjacentPaths;
        }

        public void SetCurrentField()
        {
            if (Vector3.Distance(transform.position, closestField.transform.position) <= fieldAcceptanceOffset)
            {
                currentField = closestField;
            }
        }

        public void SnapToClosestFieldIfCloseEnough()
        {
            if (Vector3.Distance(transform.position, closestField.transform.position) < snapDistance)
            {
                transform.position = closestField.transform.position;
            }
        }

        public bool IsCorrect()
        {
            return currentField == targetField;
        }
    }
}