using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gregbob {

    public class Face
    {
        public Vector3 Normal;
        public Vector3[] Vertices;
        public int[] Indices;
        public Mesh mesh;

        public Face(Mesh mesh, Vector3 normal)
        {
            if (mesh == null)
            {
                throw new System.Exception("No mesh found on grid block.");
            }

            this.mesh = mesh;
            Normal = normal;
            Vertices = new Vector3[4];
            Indices = new int[4];
            FindVertices();

        }

        private void FindVertices()
        {
            
            Vector3[] verts = mesh.vertices;
            Vector3[] normals = mesh.normals;
            int count = 0;
            for (int i = 0; i < normals.Length; i++)
            {
                if (normals[i] == Normal && count < 4)
                {
                    Vertices[count] = verts[i];
                    Indices[count] = i;
                    count++;
                }
            }

            if (count != 4)
            {
                throw new System.Exception("Can not find quad on mesh.");
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Face f = (Face)obj;
            for (int i = 0; i < 4; i++)
            {
                if (f.Vertices[i] != Vertices[i] || f.Indices[i] != Indices[i])
                {
                    return false;
                }
            }
            return true;

        }
    }


}
