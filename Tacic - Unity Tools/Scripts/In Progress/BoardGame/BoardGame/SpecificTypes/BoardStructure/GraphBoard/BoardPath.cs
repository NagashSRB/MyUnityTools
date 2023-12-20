using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard
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
