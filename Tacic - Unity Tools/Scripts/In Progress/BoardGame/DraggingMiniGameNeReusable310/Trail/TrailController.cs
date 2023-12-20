using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WebelinxGames.RoomsAndExits.Level310
{
    public class TrailController : MonoBehaviour
    {
        private Dictionary<BoardElement, Trail> trails;
        private Dictionary<Transform, Trail> visitedPoints;
        public UnityAction OnTrailCollision;
        
        public void Initialize(List<DraggableElement> boardElements, List<Trail> elementTrails)
        {
            trails = new Dictionary<BoardElement, Trail>();
            visitedPoints = new Dictionary<Transform, Trail>();
            for (int i = 0; i < boardElements.Count; i++)
            {
                Trail trail = elementTrails[i];
                trail.Initialize();
                trails.Add(boardElements[i], trail);
            }
        }
        
        public void DrawTrail(DraggableElement element, BoardPath currentPath)
        {
            Trail trail = trails[element];

            if (trail.IsTrailGoingBackwards(currentPath))
            {
                if (trail.currentTrailPart != null)
                {
                    if (visitedPoints.ContainsKey(trail.currentTrailPart.partStartPoint))
                    {
                        visitedPoints.Remove(trail.currentTrailPart.partStartPoint);
                    }
                }
                trail.RemoveLastTrailPart();
                trail.DrawTrail();
            }
            else if (trail.IsTrailCurrentlyInPath(currentPath))
            {
                trail.DrawTrail();
            }
            else if (trail.IsTrailConnectedToPath(currentPath))
            {
                trail.AddTrailPart(currentPath, element.closestField.transform);
                if (IsPointAlreadyVisited(trail.currentTrailPart.partStartPoint))
                {
                    OnTrailCollision?.Invoke();
                }
                else
                {
                    trail.visitedPoints.Add(trail.currentTrailPart.partStartPoint);
                    visitedPoints.Add(trail.currentTrailPart.partStartPoint, trail);
                    trail.DrawTrail();
                }
            }
            else if (trail.IsPathAlternativeToCurrentPart(currentPath))
            {
                trail.AddTrailPart(currentPath, element.closestField.transform);
                trail.DrawTrail();
            }
        }

        public void ResetTrails()
        {
            visitedPoints = new Dictionary<Transform, Trail>();
            foreach (Trail trail in trails.Values)
            {
                trail.ResetTrail();
            }
        }

        public bool IsPointAlreadyVisited(Transform pointToCheck)
        {
            return visitedPoints.ContainsKey(pointToCheck);
        }

    }
}

