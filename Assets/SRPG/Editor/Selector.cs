using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gregbob
{
    
    public abstract class Selector
    {
        public enum SelectionMode { Rectangle, Additive }
        public abstract Selection[] Selected { get; }
        public abstract void Select();
        public abstract void OnSceneView();
        public abstract void Deselect();
        public abstract void DisplaySelection();
        public abstract void SetSelectionMode(SelectionMode mode);


    }

}

