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
using System.Windows;
using System.Windows.Media.Media3D;

namespace Soul
{
    public static class SuperQuadrics
    {
        public static Dictionary<SQType, MeshGeometry3D> Meshes { get; set; }


        static SuperQuadrics()
        {
            Meshes = new Dictionary<SQType, MeshGeometry3D>();
        }


        public static MeshGeometry3D GetSuperQuadric(double rx, double ry, double rz, double ex, double ey, double ez, int longitudedivision, int latitudedivision)
        {
            var mesh = new MeshGeometry3D();
            latitudedivision = Math.Max(2, latitudedivision);
            longitudedivision = Math.Max(3, longitudedivision);

            for (int lat = 0; lat <= latitudedivision; lat++)
            {
                double phi = Math.PI/2 - lat*(Math.PI/latitudedivision);
                //double y = ry*Math.Sin(phi);
                double y = ry *Math.Pow( Math.Sin(phi),ey);
                //double xz = -ry*Math.Cos(phi);

                for (int lon = 0; lon <= longitudedivision; lon++)
                {
                    //double theta = lon*(2*Math.PI/longitudedivision);
                    double theta = lon * (2 * Math.PI / longitudedivision)-Math.PI;
                    //double x = xz*Math.Sin(theta);
                    //double z = xz*Math.Cos(theta);
                    //double x = rx *Math.Cos(phi)* Math.Cos(theta);
                    //double z = rz * Math.Cos(phi)*Math.Sin(theta);
                    double x = rx *Math.Pow( Math.Cos(phi),ex) *Math.Pow( Math.Cos(theta),ex);
                    double z = rz *Math.Pow( Math.Cos(phi),ez) *Math.Pow( Math.Sin(theta),ez);

                    mesh.Positions.Add(new Point3D(x, y, z));
                    mesh.Normals.Add(new Vector3D(x, y, z));
                    mesh.TextureCoordinates.Add(new Point((double) lon/longitudedivision, (double) lat/latitudedivision));
                }
            }

            for (int lat = 0; lat < latitudedivision; lat++)
            {
                for (int lon = 0; lon < longitudedivision; lon++)
                {
                    if (lat != 0)
                    {
                        mesh.TriangleIndices.Add((lat + 0) * (longitudedivision + 1) + lon);
                        mesh.TriangleIndices.Add((lat + 1) * (longitudedivision + 1) + lon);
                        mesh.TriangleIndices.Add((lat + 0) * (longitudedivision + 1) + lon + 1);
                    }

                    if (lat != latitudedivision - 1)
                    {
                        mesh.TriangleIndices.Add((lat + 0) * (longitudedivision + 1) + lon + 1);
                        mesh.TriangleIndices.Add((lat + 1) * (longitudedivision + 1) + lon);
                        mesh.TriangleIndices.Add((lat + 1) * (longitudedivision + 1) + lon + 1);
                    }
                }
            }

            return mesh;
        }

        public static MeshGeometry3D Ellipsoid(double rx, double ry, double rz, int longitudedivision, int latitudedivision)
        {
            return GetSuperQuadric(rx,  ry,  rz, 1.0, 1.0, 1.0,  longitudedivision,  latitudedivision);
        }

    }

    public enum SQType
    {
        Ellipsoid,
        Sphere,
        EllipticCylinder,
        CircularCylinder
    }

}
