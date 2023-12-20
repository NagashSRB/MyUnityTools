using System;
using System.Collections.Generic;
using UnityEngine;

namespace WebelinxGames.RoomsAndExits.Level310
{
    [Serializable]
    public class BoardPath
    {
        public BoardField startField;
        public BoardField endField;

        public bool DoesFieldExistInPath(BoardField field)
        {
            if (field == null)
            {
                return false;
            }
            
            return field == startField || field == endField;
        }

        public Vector3 GetPathVector()
        {
            return endField.transform.position - startField.transform.position;
        }
        
    }
}
