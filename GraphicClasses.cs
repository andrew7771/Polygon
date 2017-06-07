using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SharpGL
{
    public partial class Figure : GL
    {
        public int[][] Faces;
        public double size;
        protected string drawOption;
        protected List<Point> points;
        protected IDictionary<int, int[]> linkedVerticles;
        
        public Figure(double _size, List<Point> _points, string _drawOption)
        {
            this.size = _size;
            this.points = _points;
            this.drawOption = _drawOption;
        }

        public void setFaces(int[][] faces)
        {
            this.Faces = faces;
        }

        public string getOption()
        {
            return drawOption;
        }

        public void setLinkesVerticles(IDictionary<int,int[]> lV)
        {
            this.linkedVerticles = lV;
        }

        //public void DrawFace(int count, int number)
        //{
        //    glColor3d(1.0, 1.0, 1.0);
        //    glPushMatrix();
        //    glBegin(GL_POLYGON);
        //        for (int i =0; i< count; i++)
        //        {
        //            glVertex3d(points[number].X, points[number].Y, points[number].Z);
        //        }
        //    glEnd();
        //    glPopMatrix();
        //}

        public bool DrawFigure()
        {
            glColor3d(1.0, 1.0, 1.0);
            try
            {
                for (int i = 0; i < Faces.Length; i++)
                {
                    glPushMatrix();
                    glBegin(GL_POLYGON);
                    for (int j = 0; j < Faces[i].Length; j++)
                    {
                        glVertex3d(points[Faces[i][j]-1].X, points[Faces[i][j]-1].Y, points[Faces[i][j]-1].Z);
                    }
                    glEnd();
                    glPopMatrix();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Исключение!");
                this.Enabled = false;
                return false;
            }

            //glPushMatrix();
            //glBegin(GL_POLYGON);
            //glVertex3d(points[0].X, points[0].Y, points[0].Z);
            //glVertex3d(points[1].X, points[1].Y, points[1].Z);
            //glVertex3d(points[2].X, points[2].Y, points[2].Z);
            //glVertex3d(points[3].X, points[3].Y, points[3].Z);
            //glEnd();
            //glPopMatrix();

            //glPushMatrix();
            //glBegin(GL_POLYGON);
            //glVertex3d(points[4].X, points[4].Y, points[4].Z);
            //glVertex3d(points[5].X, points[5].Y, points[5].Z);
            //glVertex3d(points[6].X, points[6].Y, points[6].Z);
            //glVertex3d(points[7].X, points[7].Y, points[7].Z);
            //glEnd();
            //glPopMatrix();

            //glPushMatrix();
            //glBegin(GL_POLYGON);
            //glVertex3d(points[0].X, points[0].Y, points[0].Z);
            //glVertex3d(points[1].X, points[1].Y, points[1].Z);
            //glVertex3d(points[6].X, points[6].Y, points[6].Z);
            //glVertex3d(points[7].X, points[7].Y, points[7].Z);
            //glEnd();
            //glPopMatrix();

            //glPushMatrix();
            //glBegin(GL_POLYGON);
            //glVertex3d(points[1].X, points[1].Y, points[1].Z);
            //glVertex3d(points[2].X, points[2].Y, points[2].Z);
            //glVertex3d(points[5].X, points[5].Y, points[5].Z);
            //glVertex3d(points[6].X, points[6].Y, points[6].Z);
            //glEnd();
            //glPopMatrix();

            //glPushMatrix();
            //glBegin(GL_POLYGON);
            //glVertex3d(points[0].X, points[0].Y, points[0].Z);
            //glVertex3d(points[3].X, points[3].Y, points[3].Z);
            //glVertex3d(points[4].X, points[4].Y, points[4].Z);
            //glVertex3d(points[6].X, points[6].Y, points[6].Z);
            //glEnd();
            //glPopMatrix();

            //glPushMatrix();
            //glBegin(GL_POLYGON);
            //glVertex3d(points[2].X, points[2].Y, points[2].Z);
            //glVertex3d(points[3].X, points[3].Y, points[3].Z);
            //glVertex3d(points[4].X, points[4].Y, points[4].Z);
            //glVertex3d(points[5].X, points[5].Y, points[5].Z);
            //glEnd();
            //glPopMatrix();

            return true;
        }

        public void DrawPoints()
        {
            glColor3d(1.0, 0.0, 0.0); 
            glPointSize(4);
            glEnable(GL_POINT_SMOOTH);
            glBegin(GL_POINTS);
            if (points.Count > 0)
                for (int i = 0; i < points.Count; i++)
                {
                    points[i].Draw();
                }
            
            glEnd();
            glDisable(GL_POINT_SMOOTH);
            for (int i = 0; i < points.Count; i++)
            {
                OutText((points[i].N+1).ToString(), points[i].X + 0.25, points[i].Y + 0.25, points[i].Z);
            }
            
        }

        public bool DrawLines()
        {
            int[] verticles;
            glColor3d(0.0, 1.0, 0.0);
            glLineWidth(4);
            glEnable(GL_LINE_SMOOTH);
            glBegin(GL_LINES);
            try
            {
                for (int i = 0; i < points.Count; i++)
                {
                    verticles = linkedVerticles[i + 1];
                    for (int j = 0; j < verticles.Length; j++)
                    {
                        // координаты точки, которая будет связываться
                        glVertex3d(points[i].X, points[i].Y, points[i].Z);

                        //номер точки, с которой будет связываться текущая точка
                        int indexOfLinkedVerticle = verticles[j];
                        // точка
                        Point currentPoint = points[indexOfLinkedVerticle - 1];
                        // ее координаты
                        glVertex3d(currentPoint.X,
                                   currentPoint.Y,
                                   currentPoint.Z);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Исключение!");
                this.Enabled = false;
                return false;
            }
            glEnd();
            glDisable(GL_LINE_SMOOTH);
            return true;

        }
    }



    public class Point
    {
        double x, y, z;
        int number;
        public Point(double x, double y, double z, int _number)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.number = _number;
        }

        public double Z
        {
            get { return z; }
        }

        public double Y
        {
            get { return y; }
        }

        public double X
        {
            get { return x; }
        }

        public int N
        {
            get { return number; }
        }

        public void Draw()
        {
            ControlGL.glVertex3d(x, y, z);
        }
    }

}
