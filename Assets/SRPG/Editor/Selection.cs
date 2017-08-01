using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gregbob
{
    public class Selection
    {
        public GridBlock Block;
        public Face face;

        public Selection(GridBlock block, Vector3 normal)
        {
            Block = block;
            face = new Face(block.GetComponent<MeshFilter>().sharedMesh, normal);
        }

        public Vector3[] WorldCoordinatesOfFace()
        {
            Vector3[] coords = new Vector3[4];
            for (int i = 0; i < coords.Length; i++)
            {
                coords[i] = Block.transform.TransformPoint(face.Vertices[i]);
                coords[i] += face.Normal * .001f; // stop zfighting
            }
            return coords;
        }

        public override bool Equals(object obj)
        {
            Selection s = (Selection)obj;
            return s.Block.Equals(Block) && face.Equals(s.face);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}


