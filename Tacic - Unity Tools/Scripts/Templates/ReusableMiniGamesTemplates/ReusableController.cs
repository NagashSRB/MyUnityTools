using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace WebelinxGames.RoomsAndExits.Level9999
{
    public class ReusableController : MonoBehaviour
    {
        public UnityAction onCheckIfFinished;

        [Header("References")] [SerializeField]
        private List<ReusableElement> elements;

        [Header("Sounds")] [SerializeField] private AudioSource clickSound;

        private void OnValidate()
        {
            
        }

        [ContextMenu("Do something")]
        public void GenerateOrReferenceSomething()
        {
            
        }

        private void Awake()
        {
            SetUpElementEvents();
            Initialize();
        }

        protected virtual void Initialize()
        {

        }

        private void SetUpElementEvents()
        {
            foreach (ReusableElement element in elements)
            {
                element.onMouseDown.AddListener(() => HandleMouseDown(element));
            }
        }

        protected void HandleMouseDown(ReusableElement element)
        {
            
        }

        public virtual void DisableAllColliders() => elements.ForEach(element => element.DisableCollider());
        
        public virtual void EnableAllColliders() => elements.ForEach(element => element.EnableCollider());

        public virtual bool IsSolved() => true;

        public virtual IEnumerator Solve(float solveNextElementDelay, float solveMovementDuration)
        {
            yield return null;
        }

        public virtual void SetFinalState()
        {
            
        }
    }
}


