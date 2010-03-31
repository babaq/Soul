//--------------------------------------------------------------------------------
// This file is part of The Soul, A Neural Network Simulation System.
//
// Copyright © 2010 LBE Group. All rights reserved.
//
// For information about this application and licensing, go to http://soul.codeplex.com
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace Soul
{
    public static class Cylinder
    {
        public static MeshGeometry3D Mesh { get; set; }


        static Cylinder()
        {
            Mesh = GetCylinder(0.2, 1, 0.2, 8);
            Mesh.Freeze();
        }


        public static MeshGeometry3D GetCylinder(double r,double l,double endr,int divisions)
        {
            var mesh = new MeshGeometry3D();

            var rei = 2 * divisions;
            var lei = rei + 1;

            for (int i = 0; i < divisions; i++)
            {
                var theta = 2 * Math.PI * i / divisions;

                var z = r * Math.Cos(theta);
                var y = r * Math.Sin(theta);

                mesh.Positions.Add(new Point3D(0, y, z));
                mesh.Positions.Add(new Point3D(l, y, z));

                mesh.Normals.Add(new Vector3D(0, y, z));
                mesh.Normals.Add(new Vector3D(0, y, z));

                int i1 = 2 * i;
                int i2 = i1 + 1;
                int i3 = 2 * ((i + 1) % divisions);
                int i4 = i3 + 1;

                mesh.TriangleIndices.Add(i1);
                mesh.TriangleIndices.Add(i2);
                mesh.TriangleIndices.Add(i3);

                mesh.TriangleIndices.Add(i3);
                mesh.TriangleIndices.Add(i2);
                mesh.TriangleIndices.Add(i4);

                mesh.TriangleIndices.Add(i2);
                mesh.TriangleIndices.Add(rei);
                mesh.TriangleIndices.Add(i4);

                mesh.TriangleIndices.Add(i3);
                mesh.TriangleIndices.Add(lei);
                mesh.TriangleIndices.Add(i1);
            }

            mesh.Positions.Add(new Point3D(l+endr, 0, 0));
            mesh.Normals.Add(new Vector3D(l+endr, 0, 0));

            mesh.Positions.Add(new Point3D(-endr, 0, 0));
            mesh.Normals.Add(new Vector3D(-1, 0, 0));

            return mesh;
        }

    }

}
