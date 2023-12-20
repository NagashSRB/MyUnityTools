using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard
{
    public class TrailController : MonoBehaviour
    {
        //private Dictionary<BoardElement, List<BoardPath>> trailPaths;
        private Dictionary<BoardElement, Trail> trails;
        public UnityAction OnTrailCollision;

        //TODO: create prefab. Sprite, order in layer and component TrailPart? you have SpriteRenderer of a trail, and path it is on. Use it as data container. 
        //TODO: visited and current for example. currentpath and current position. going from x to y
        //TODO: Trail part: instantiate. initialized with prefab location. Deleting also handled here.
        //TODO: Trail: delete all parts, check if collided or other funtions. usage of drawing functions
        //TODO: Trail controller: just wrap all trails and enable easy access from other controllers. UpdateTrail for example or CheckIfColidedTrailWithOtherTrails
        
        
        //TODO: Handle paths - convert to points for drawing trail. Add one by one.. check collisions. Handle path changed? or only on current trail? current
        
        //TODO: Nekada za generator: mozda da zavisi samo od transforma. ne znam da li je potrebno field i path. path i mozda jer moze trag da zavisi od 
        //terena. Onda i field je teren? bas gde je tacka da promeni trag gradijent npr. 
        
        public void Initialize(List<DraggableElement> boardElements, List<Trail> elementTrails)
        {
            trails = new Dictionary<BoardElement, Trail>();
            for (int i = 0; i < boardElements.Count; i++)
            {
                Trail trail = elementTrails[i];
                trail.Initialize();
                trail.OnCheckIfPointAlreadyVisited += HandleCheckIfPointAlreadyVisited;
                trails.Add(boardElements[i], trail);
            }
        }
        
        public void DrawTrail(DraggableElement element, BoardPath currentPath)
        {
            Trail trail = trails[element];

            if (trail.IsTrailGoingBackwards(currentPath))
            {
                trail.RemoveLastTrailPart();
                trail.DrawTrail();
                //todo .removePart treba da obrise trenutni i na sceni i u listama, i da na trenutni namesti prethodni, onda norm.
            }
            else if (trail.IsTrailCurrentlyInPath(currentPath))
            {
                trail.DrawTrail();
            }
            else if (trail.IsTrailConnectedToPath(currentPath) || trail.IsPathAlternativeToCurrentPart(currentPath))
            {
                
                //todo .addPart treba da trenutni stavi u prethodni, namesti novi trenutni, stari iscrta ceo, i krene normalno crtanje
                trail.AddTrailPart(currentPath, element);
                trail.DrawTrail();
            }
        }

        public bool IsPointAlreadyVisited(Transform pointToCheck)
        {


            return false;
        }

        public void ResetTrails()
        {
            foreach (Trail trail in trails.Values)
            {
                trail.ResetTrail();
            }
        }

        private void HandleCheckIfPointAlreadyVisited(Trail currentTrail, Transform pointToCheck)
        {
            foreach (Trail trail in trails.Values)
            {
                if (trail.visitedPoints.Contains(pointToCheck))
                {
                    OnTrailCollision?.Invoke();

                    return;
                }
            }
            
            currentTrail.visitedPoints.Add(pointToCheck);
        }
        
    }
}

