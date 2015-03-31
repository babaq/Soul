//--------------------------------------------------------------------------------
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
                double phi = lat * (Math.PI / latitudedivision) - Math.PI / 2;
                var sinphi = Math.Sin(phi);
                var cosphi = Math.Cos(phi);
                double y = ry * Math.Sign(sinphi) * Math.Pow(Math.Abs(sinphi), ey);

                for (int lon = 0; lon <= longitudedivision; lon++)
                {
                    double theta = lon * (2 * Math.PI / longitudedivision) - Math.PI;
                    var costheta = Math.Cos(theta);
                    var sintheta = Math.Sin(theta);
                    double x = rx * Math.Sign(cosphi) * Math.Pow(Math.Abs(cosphi), ex) * Math.Sign(costheta) * Math.Pow(Math.Abs(costheta), ex);
                    double z = rz * Math.Sign(cosphi) * Math.Pow(Math.Abs(cosphi), ez) * Math.Sign(sintheta) * Math.Pow(Math.Abs(sintheta), ez);

                    mesh.Positions.Add(new Point3D(x, y, z));
                    mesh.Normals.Add(new Vector3D(x, y, z));
                    mesh.TextureCoordinates.Add(new Point((double)lon / longitudedivision, (double)lat / latitudedivision));
                }
            }

            for (int lat = 0; lat < latitudedivision; lat++)
            {
                for (int lon = 0; lon < longitudedivision; lon++)
                {
                    var upleft = lat * (longitudedivision + 1) + lon;
                    var downleft = upleft + (longitudedivision + 1);
                    var upright = upleft + 1;
                    var downright = downleft + 1;

                    mesh.TriangleIndices.Add(upleft);
                    mesh.TriangleIndices.Add(downleft);
                    mesh.TriangleIndices.Add(upright);

                    mesh.TriangleIndices.Add(upright);
                    mesh.TriangleIndices.Add(downleft);
                    mesh.TriangleIndices.Add(downright);

                }
            }

            return mesh;
        }

        public static MeshGeometry3D Ellipsoid(double rx, double ry, double rz, int longitudedivision, int latitudedivision)
        {
            return GetSuperQuadric(rx, ry, rz, 1.0, 1.0, 1.0, longitudedivision, latitudedivision);
        }

        public static MeshGeometry3D Sphere(double r, int longitudedivision, int latitudedivision)
        {
            return Ellipsoid(r, r, r, longitudedivision, latitudedivision);
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
