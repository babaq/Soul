﻿//--------------------------------------------------------------------------------
// This file is part of the Soul - Neural Network Simulation System.
//
// Copyright © 2010 Alex-Joyce. All rights reserved.
//
// For information about this application and licensing, go to http://soul.codeplex.com.
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
    public static class Ellipsoid
    {
        public static MeshGeometry3D Mesh { get; set; }


        static Ellipsoid()
        {
            Mesh = GetEllipsoid(1, 1, 1, 2);
            Mesh.Freeze();
        }

        public static MeshGeometry3D GetEllipsoid(double rx, double ry, double rz, int divisions)
        {
            divisions = Math.Max(0, divisions);
            var e = Octahedron(rx, ry, rz);
            for (int i = 0; i < divisions; i++)
            {
                Divide(rx, ry, rz, e);
            }
            return e;
        }


        public static MeshGeometry3D Octahedron(double rx, double ry, double rz)
        {
            var mesh = new MeshGeometry3D();

            AddTriangle(mesh,
                new Point3D(0, 0, rz), new Point3D(rx, 0, 0), new Point3D(0, ry, 0));
            AddTriangle(mesh,
                new Point3D(rx, 0, 0), new Point3D(0, 0, -rz), new Point3D(0, ry, 0));
            AddTriangle(mesh,
                new Point3D(0, 0, -rz), new Point3D(-rx, 0, 0), new Point3D(0, ry, 0));
            AddTriangle(mesh,
                new Point3D(-rx, 0, 0), new Point3D(0, 0, rz), new Point3D(0, ry, 0));
            AddTriangle(mesh,
                new Point3D(rx, 0, 0), new Point3D(0, 0, rz), new Point3D(0, -ry, 0));
            AddTriangle(mesh,
                new Point3D(0, 0, -rz), new Point3D(rx, 0, 0), new Point3D(0, -ry, 0));
            AddTriangle(mesh,
                new Point3D(-rx, 0, 0), new Point3D(0, 0, -rz), new Point3D(0, -ry, 0));
            AddTriangle(mesh,
                new Point3D(0, 0, rz), new Point3D(-rx, 0, 0), new Point3D(0, -ry, 0));

            return mesh;
        }

        static void AddTriangle(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3)
        {
            var index = mesh.Positions.Count;
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.Positions.Add(p3);

            mesh.Normals.Add(new Vector3D(p1.X, p1.Y, p1.Z));
            mesh.Normals.Add(new Vector3D(p2.X, p2.Y, p2.Z));
            mesh.Normals.Add(new Vector3D(p3.X, p3.Y, p3.Z));

            mesh.TriangleIndices.Add(index++);
            mesh.TriangleIndices.Add(index++);
            mesh.TriangleIndices.Add(index);
        }

        static void Divide(double rx, double ry, double rz, MeshGeometry3D mesh)
        {
            var indexCount = mesh.TriangleIndices.Count;
            for (int indexOffset = 0; indexOffset < indexCount; indexOffset += 3)
            {
                DivideTriangle(rx, ry, rz, mesh, indexOffset);
            }
        }

        static void DivideTriangle(double rx, double ry, double rz, MeshGeometry3D mesh, int indexOffset)
        {
            var nextIndex = mesh.Positions.Count;
            int i1 = mesh.TriangleIndices[indexOffset];
            int i2 = mesh.TriangleIndices[indexOffset + 1];
            int i3 = mesh.TriangleIndices[indexOffset + 2];

            Point3D p1 = mesh.Positions[i1];
            Point3D p2 = mesh.Positions[i2];
            Point3D p3 = mesh.Positions[i3];
            Point3D p4 = GetInterpolatePoint(rx, ry, rz, p1, p2);
            Point3D p5 = GetInterpolatePoint(rx, ry, rz, p2, p3);
            Point3D p6 = GetInterpolatePoint(rx, ry, rz, p3, p1);

            mesh.Positions.Add(p4);
            mesh.Positions.Add(p5);
            mesh.Positions.Add(p6);

            int i4 = nextIndex++;
            int i5 = nextIndex++;
            int i6 = nextIndex;

            mesh.Normals.Add(new Vector3D(p4.X, p4.Y, p4.Z));
            mesh.Normals.Add(new Vector3D(p5.X, p5.Y, p5.Z));
            mesh.Normals.Add(new Vector3D(p6.X, p6.Y, p6.Z));

            mesh.TriangleIndices[indexOffset] = i4;
            mesh.TriangleIndices[indexOffset + 1] = i5;
            mesh.TriangleIndices[indexOffset + 2] = i6;

            mesh.TriangleIndices.Add(i1);
            mesh.TriangleIndices.Add(i4);
            mesh.TriangleIndices.Add(i6);

            mesh.TriangleIndices.Add(i4);
            mesh.TriangleIndices.Add(i2);
            mesh.TriangleIndices.Add(i5);

            mesh.TriangleIndices.Add(i6);
            mesh.TriangleIndices.Add(i5);
            mesh.TriangleIndices.Add(i3);
        }

        static Point3D GetInterpolatePoint(double rx, double ry, double rz, Point3D p1, Point3D p2)
        {
            if (rx == ry && ry == rz)
            {
                var midpointvector = new Vector3D((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                midpointvector.Normalize();
                midpointvector *= rx;
                return new Point3D(midpointvector.X, midpointvector.Y, midpointvector.Z);
            }
            else
            {
                var midpointvector = new Vector3D((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                var ax = Vector3D.AngleBetween(midpointvector, new Vector3D(1, 0, 0)) * Math.PI / 180;
                var ay = Vector3D.AngleBetween(midpointvector, new Vector3D(0, 1, 0)) * Math.PI / 180;
                var az = Vector3D.AngleBetween(midpointvector, new Vector3D(0, 0, 1)) * Math.PI / 180;
                return new Point3D(rx * Math.Cos(ax), ry * Math.Cos(ay), rz * Math.Cos(az));
            }
        }

    }
}
