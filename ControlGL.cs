using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SharpGL
{
    [ToolboxItem(true)]
    public partial class ControlGL : GL
    {
        // Признак того, что мышь активна - вращение системы
        bool mouse_active = false;
        // Значения координат указателя мыши при нажатии на ЛКМ
        int x_start, y_start;
        // Углы для вращения всей системы в окне (в градусах): 
        int angle_y = -30;    // Поворот относительно оси OY
        int angle_x = 30;    // Поворот относительно оси OX
        double scale; // Масштаб
        // Область видимости в координатах OpenGL
        const double MinX = -10.0, MaxX = 10.0, 
                     MinY = -10.0, MaxY = 10.0, 
                     MinZ = -10.0, MaxZ = 10.0;
        
        public ControlGL()
        {
            InitializeComponent();
            MouseWheel += new MouseEventHandler(ControlGL_MouseWheel);
            SetScale(1);
        }

        Figure figure;
        public Figure F
        {
            get
            {
                return figure;
            }
            set
            {
                figure = value;
            }            
        }

        

        private void ControlGL_MouseWheel(object sender, MouseEventArgs e)
        {
            if (SetScale(scale + e.Delta / 2000.0))
            Invalidate();
        }

        private void ControlGL_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_active)
            {
                // Изменение углов вращения системы
                angle_x += e.Y - y_start;
                angle_y += e.X - x_start;

                x_start = e.X;
                y_start = e.Y;
                Invalidate();
            }

        }

        private void ControlGL_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Мышь активна - запоминание стартовых координат
                mouse_active = true;
                x_start = e.X;
                y_start = e.Y;
            }
        }

        private void ControlGL_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_active = false;
        }

        private void ControlGL_Paint(object sender, PaintEventArgs e)
        {
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            glLoadIdentity();
            if (figure == null)
            {
                glFinish();
                wglSwapBuffers(wglGetCurrentDC());
                return;
            }

            glOrtho(MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
            Isometric();

            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
            glEnable(GL_BLEND);
            glEnable(GL_LINE_SMOOTH);

            // Масштаб 
            glScaled(scale, scale, scale);

            // Вращение системы
            glRotated(angle_x, 1, 0, 0);    // Вокруг оси OX
            glRotated(angle_y, 0, 1, 0);    // Вокруг оси OY

            glEnable(GL_DEPTH_TEST);
            PaintAxes(0.8, 0.8, 0.8);

            // Установка источника света
            float[] lightPos = new float[] { 10, 10, 10 };
            glEnable(GL_LIGHTING);
            glEnable(GL_LIGHT0);
            glEnable(GL_COLOR_MATERIAL);
            glLightfv(GL_LIGHT0, GL_POSITION, lightPos);
            glShadeModel(GL_SMOOTH);


            string option = figure.getOption();
            switch (option)
            {
                case "points":
                    {
                        figure.DrawPoints();
                        break;
                    }
                case "lines":
                    {
                        if (!figure.DrawLines())
                        {
                            Enabled = false;
                        }
                        else Enabled = true;
                        break;
                    }
                case "figure":
                    {
                        if (!figure.DrawFigure())
                        {
                            Enabled = false;
                        }
                        else Enabled = true;
                        break;
                    }
                case "points_lines":
                    {
                        figure.DrawPoints();
                        if (!figure.DrawLines())
                        {
                            Enabled = false;
                        }
                        else Enabled = true;
                        break;
                    }
                case "points_figure":
                    {
                        figure.DrawPoints();

                        if (!figure.DrawFigure())
                        {
                            Enabled = false;
                        }
                        else Enabled = true;
                        break;
                    }
                case "lines_figure":
                    {
                        if (!figure.DrawLines() || !figure.DrawFigure())
                        {
                            Enabled = false;
                        }
                        else Enabled = true;
                        break;
                    }
                case "all":
                    {
                        figure.DrawPoints();
                        if (!figure.DrawLines() || !figure.DrawFigure())
                        {
                            Enabled = false;
                        }
                        else Enabled = true;
                        break;
                    }
                default:
                    break;
            }

            //figure.DrawFigure();
            //figure.DrawPoints();
            //figure.DrawLines();

            glDisable(GL_COLOR_MATERIAL);
            glDisable(GL_LIGHT0);
            glDisable(GL_LIGHTING);

            glDisable(GL_DEPTH_TEST);
            glDisable(GL_LINE_SMOOTH);
            glDisable(GL_BLEND);

            glFinish();
            wglSwapBuffers(wglGetCurrentDC());

        }

        // Изометрия
        private void Isometric()
        {
            if (Width > Height)
                glViewport((Width - Height) / 2, 0, Height, Height);
            else
                glViewport(0, (Height - Width) / 2, Width, Width);
        }

        // Отображение кординатных осей
        private void PaintAxes(double red, double green, double blue)
        {
            glColor3d(red, green, blue);
            glLineWidth(2.5f);
            glBegin(GL_LINES);
            {
                glVertex3d(0, 0, 0); glVertex3d(0.9 * MaxX, 0, 0);
                glVertex3d(0, 0, 0); glVertex3d(0, 0.9 * MaxY, 0);
                glVertex3d(0, 0, 0); glVertex3d(0, 0, 0.9 * MaxZ);
            }
            glEnd();
            OutText("X", 0.93 * MaxX, 0, 0);
            OutText("Y", 0, 0.93 * MaxY, 0);
            OutText("Z", 0, 0, 0.93 * MaxZ);
            
        }
              
        // Изменение масштаба
        private bool SetScale(double scale_new)
        {
            bool res = false;
            if ((scale_new > 0) && (scale_new < 10))
            {
                scale = scale_new;
                res = true;
            }
            return res;
        }

       
    }
}
