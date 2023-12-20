using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebelinxGames.RoomsAndExits.MiniGames.ReusableMiniGames;

namespace WebelinxGames.RoomsAndExits.Level310
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BoardElement : ColliderEventTrigger
    {
        public Vector2Int index;
        public BoardField currentField;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private readonly Vector2Int invalidIndexState = -Vector2Int.one;

        protected override void OnValidate()
        {
            base.OnValidate();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlaceOnField(BoardField boardField)
        {
            currentField = boardField;
            index = boardField.index;
        }
        
        public void RemoveFromField()
        {
            currentField = null;
            index = invalidIndexState;
        }

        public void SetEnabled(bool isEnabled)
        {
            spriteRenderer.enabled = isEnabled;
            if (isEnabled)
            {
                EnableCollider();
                return;
            }
            
            DisableCollider();
        }
    }
}