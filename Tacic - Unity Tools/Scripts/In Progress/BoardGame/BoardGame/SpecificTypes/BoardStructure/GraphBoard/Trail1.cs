using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard
{
    public class Trail : MonoBehaviour
    {
        [SerializeField] private List<TrailPart> previousTrailParts;
        [SerializeField] private GameObject trailPartPrefab;
        [SerializeField] private Transform trailParent;
        [SerializeField] private Color trailColor;
        private TrailPart currentTrailPart;
        public List<Transform> visitedPoints;
        public UnityAction<Trail, Transform> OnCheckIfPointAlreadyVisited;

        //????????
        //private Transform startingPoint;
        //protected List<Transform> nodes;

        //TODO: Transform position pass to parts. they dont have value. They mby dont need to be monobehaviours.
        
        public void Initialize(List<Transform> trailNodes = null)
        {
            previousTrailParts ??= new List<TrailPart>();
            visitedPoints ??= new List<Transform>();
            //TODO: starting fields or paths
            // nodes = trailNodes ?? new List<Transform>{transform};
            //startingPoint = transform;
        }

        public void AddTrailPart(BoardPath currentPath, DraggableElement element, Transform startPoint = null)
        {

            //Transform startPoint = transform;
            //TODO: current ili closest, null potencijalno
            if (startPoint == null)
            {
                startPoint = element.closestField.transform;
            }
            
            GameObject trailPartPrefabRoot = Instantiate(trailPartPrefab, trailParent);
            TrailPart trailPart = trailPartPrefabRoot.GetComponent<TrailPart>();
            trailPart.SetTrailColor(trailColor);
            //if null or force it
            
            //add if doesnt exist for now only.

            if (currentTrailPart != null)
            {
                // Adding new trail part in addition to previous trail part.
                if (currentTrailPart.IsPartConnectedToPath(currentPath))
                {
                    startPoint = currentTrailPart.partEndPoint;
                }
                else
                {
                    // When changing path, remove previous trail part because you are taking alternative route
                    if (currentTrailPart.DoesPartStartFromSameField(currentPath))
                    {
                        startPoint = currentTrailPart.partStartPoint;
                        RemoveLastTrailPart();
                    }
                }

                currentTrailPart.DrawWholeTrailPart();
                previousTrailParts.Add(currentTrailPart);
            }
            
            currentTrailPart = trailPart;
            currentTrailPart.Initialize(startPoint, currentPath, trailPartPrefabRoot);
            
            OnCheckIfPointAlreadyVisited?.Invoke(this, startPoint);
        }

        public void RemoveLastTrailPart()
        {
            if (currentTrailPart != null)
            {
                if (visitedPoints.Contains(currentTrailPart.partStartPoint))
                {
                    visitedPoints.Remove(currentTrailPart.partStartPoint);
                }
                
                currentTrailPart.DestroyTrailPart();
            }

            if (previousTrailParts.Count > 0)
            {
                
                currentTrailPart = previousTrailParts[previousTrailParts.Count - 1];
                previousTrailParts.RemoveAt(previousTrailParts.Count - 1);
            }
            else
            {
                currentTrailPart = null;
            }
        }

        public void DrawTrail()
        {
            currentTrailPart.DrawTrailPart(transform);
        }

        public bool IsTrailGoingBackwards(BoardPath currentPath)
        {
            if (previousTrailParts == null || previousTrailParts.Count == 0)
            {
                return false;
            }

            return previousTrailParts[previousTrailParts.Count - 1].IsTrailPartOnPath(currentPath);
        }

        public bool IsTrailCurrentlyInPath(BoardPath currentPath) => currentTrailPart != null && currentTrailPart.IsTrailPartOnPath(currentPath);

        public bool IsTrailConnectedToPath(BoardPath currentPath) =>
            currentTrailPart == null || currentTrailPart.IsPartConnectedToPath(currentPath);
        
        public bool IsPathAlternativeToCurrentPart(BoardPath currentPath) => currentTrailPart.DoesPartStartFromSameField(currentPath);

        public void SetActive(bool isActive)
        {
            foreach (TrailPart trailPart in previousTrailParts)
            {
                trailPart.SetActive(isActive);
            }

            if (currentTrailPart != null)
            {
                currentTrailPart.SetActive(isActive);
            }
        }

        public void ResetTrail()
        {
            visitedPoints = new List<Transform>();
            while (previousTrailParts.Count > 0)
            {
                RemoveLastTrailPart();
            }
            
            currentTrailPart.DestroyTrailPart();
            currentTrailPart = null;
        }
    }
}