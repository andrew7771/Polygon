using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;

namespace SharpGL
{
    public partial class MainForm : Form
    {
        Figure cl;
        
        IDictionary<int, int[]>  linkedVerticles= new Dictionary<int, int[]>();
        List<Point> points = new List<Point>();
        int[][] Faces;

        public MainForm()
        {
            InitializeComponent();
            controlGL1.Invalidate();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            controlGL1.Invalidate();
            double size = 1.1;
            try
            {
                if (comboBox1.Text == "") throw new Exception("Выберите количество вершин!");
                int numberOfVerticles = int.Parse(comboBox1.Text);
                points = GetVerticlesList(numberOfVerticles);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Исключение");
                return;
            }

            string selOption = SelectedOption(checkBoxPoints, checkBoxLines, checkBoxFillTheFigure);
            Faces = getFaces();

            cl = new Figure(size, points, selOption);

            controlGL1.F = cl;
            controlGL1.F.setFaces(Faces);
            controlGL1.F.setLinkesVerticles(linkedVerticles);
            controlGL1.Invalidate();
        }

        private int[] GetLinkedVerticles(TextBox tb)
        {
            IDictionary<int, int[]> _linkedVerticles = new Dictionary<int, int[]>();
            string[] temp = tb.Text.Split(new char[] { ' ', ',' });
            int[] verticlesForLinking = new int[temp.Length];
            
            for (int i = 0; i < temp.Length; i++)
            {
                verticlesForLinking[i] = int.Parse(temp[i]);
            }
            return verticlesForLinking;
        }


        private int[][] getFaces()
        {
            int length = textBoxFaces.Lines.Count();
            int[][] faces = new int[length][];

            string[] temp = textBoxFaces.Text.Split(new char[] { ';', '.' });
            for (int i = 0; i < temp.Length-1; i++)
            {
                if (temp[i].Contains("\r\n"))
                {
                    temp[i] = temp[i].Remove(0, 2);
                }

                string[] tempF = temp[i].Split(new char[] { ',' });

                faces[i] = new int[tempF.Length];
                for (int j = 0; j < tempF.Length; j++)
                {
                    faces[i][j] = int.Parse(tempF[j]);
                }
            }
            return faces;
        }

        private string SelectedOption(CheckBox chB, CheckBox chB2, CheckBox chB3)
        {
            if (chB.Checked)
            {
                if (chB.Checked && chB2.Checked && chB3.Checked)
                {
                    return "all";
                }
                else if (chB.Checked && chB2.Checked)
                {
                    return "points_lines";                    
                }
                else if (chB.Checked && chB3.Checked)
                {
                    return "points_figure";
                }
                else return "points";
            }

            if (chB2.Checked)
            {
                if (chB2.Checked && chB3.Checked) return "lines_figure";

                else return "lines";
            }
            else if (chB3.Checked)
            {
                return "figure";
            }

            else return "all";
        }

        private List<Point> GetVerticlesList(int numberOfVerticles)
        {
            List<Point> _points = new List<Point>();
            switch (numberOfVerticles)
            {
                case 1:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text),
                                              1));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        break;
                    }
                case 2:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text) , 1));

                        _points.Add(new Point(double.Parse(textBoxV2x.Text),
                                              double.Parse(textBoxV2y.Text),
                                              double.Parse(textBoxV2z.Text), 2));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        linkedVerticles.Add(2, GetLinkedVerticles(textBox2V));
                        break;
                    }
                case 3:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text), 1));

                        _points.Add(new Point(double.Parse(textBoxV2x.Text),
                                              double.Parse(textBoxV2y.Text),
                                              double.Parse(textBoxV2z.Text), 2));

                        _points.Add(new Point(double.Parse(textBoxV3x.Text),
                                              double.Parse(textBoxV3y.Text),
                                              double.Parse(textBoxV3z.Text), 3));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        linkedVerticles.Add(2, GetLinkedVerticles(textBox2V));
                        linkedVerticles.Add(3, GetLinkedVerticles(textBox3V));

                        break;
                    }
                case 4:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text),1 ));

                        _points.Add(new Point(double.Parse(textBoxV2x.Text),
                                              double.Parse(textBoxV2y.Text),
                                              double.Parse(textBoxV2z.Text),2));

                        _points.Add(new Point(double.Parse(textBoxV3x.Text),
                                              double.Parse(textBoxV3y.Text),
                                              double.Parse(textBoxV3z.Text),3));

                        _points.Add(new Point(double.Parse(textBoxV4x.Text),
                                              double.Parse(textBoxV4y.Text),
                                              double.Parse(textBoxV4z.Text),4));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        linkedVerticles.Add(2, GetLinkedVerticles(textBox2V));
                        linkedVerticles.Add(3, GetLinkedVerticles(textBox3V));
                        linkedVerticles.Add(4, GetLinkedVerticles(textBox4V));

                        break;
                    }
                case 5:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text),1));

                        _points.Add(new Point(double.Parse(textBoxV2x.Text),
                                              double.Parse(textBoxV2y.Text),
                                              double.Parse(textBoxV2z.Text),2));

                        _points.Add(new Point(double.Parse(textBoxV3x.Text),
                                              double.Parse(textBoxV3y.Text),
                                              double.Parse(textBoxV3z.Text),3));

                        _points.Add(new Point(double.Parse(textBoxV4x.Text),
                                              double.Parse(textBoxV4y.Text),
                                              double.Parse(textBoxV4z.Text),4));

                        _points.Add(new Point(double.Parse(textBoxV5x.Text),
                                              double.Parse(textBoxV5y.Text),
                                              double.Parse(textBoxV5z.Text),5));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        linkedVerticles.Add(2, GetLinkedVerticles(textBox2V));
                        linkedVerticles.Add(3, GetLinkedVerticles(textBox3V));
                        linkedVerticles.Add(4, GetLinkedVerticles(textBox4V));
                        linkedVerticles.Add(5, GetLinkedVerticles(textBox5V));
                        break;
                    }
                case 6:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text),1));

                        _points.Add(new Point(double.Parse(textBoxV2x.Text),
                                              double.Parse(textBoxV2y.Text),
                                              double.Parse(textBoxV2z.Text),2));

                        _points.Add(new Point(double.Parse(textBoxV3x.Text),
                                              double.Parse(textBoxV3y.Text),
                                              double.Parse(textBoxV3z.Text),3));

                        _points.Add(new Point(double.Parse(textBoxV4x.Text),
                                              double.Parse(textBoxV4y.Text),
                                              double.Parse(textBoxV4z.Text),4));

                        _points.Add(new Point(double.Parse(textBoxV5x.Text),
                                              double.Parse(textBoxV5y.Text),
                                              double.Parse(textBoxV5z.Text),5));

                        _points.Add(new Point(double.Parse(textBoxV6x.Text),
                                              double.Parse(textBoxV6y.Text),
                                              double.Parse(textBoxV6z.Text),6));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        linkedVerticles.Add(2, GetLinkedVerticles(textBox2V));
                        linkedVerticles.Add(3, GetLinkedVerticles(textBox3V));
                        linkedVerticles.Add(4, GetLinkedVerticles(textBox4V));
                        linkedVerticles.Add(5, GetLinkedVerticles(textBox5V));
                        linkedVerticles.Add(6, GetLinkedVerticles(textBox6V));
                        break;
                    }
                case 7:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text),1));

                        _points.Add(new Point(double.Parse(textBoxV2x.Text),
                                              double.Parse(textBoxV2y.Text),
                                              double.Parse(textBoxV2z.Text),2));

                        _points.Add(new Point(double.Parse(textBoxV3x.Text),
                                              double.Parse(textBoxV3y.Text),
                                              double.Parse(textBoxV3z.Text),3));

                        _points.Add(new Point(double.Parse(textBoxV4x.Text),
                                              double.Parse(textBoxV4y.Text),
                                              double.Parse(textBoxV4z.Text),4));

                        _points.Add(new Point(double.Parse(textBoxV5x.Text),
                                              double.Parse(textBoxV5y.Text),
                                              double.Parse(textBoxV5z.Text),5));

                        _points.Add(new Point(double.Parse(textBoxV6x.Text),
                                              double.Parse(textBoxV6y.Text),
                                              double.Parse(textBoxV6z.Text),6));

                        _points.Add(new Point(double.Parse(textBoxV7x.Text),
                                              double.Parse(textBoxV7y.Text),
                                              double.Parse(textBoxV7z.Text),7));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        linkedVerticles.Add(2, GetLinkedVerticles(textBox2V));
                        linkedVerticles.Add(3, GetLinkedVerticles(textBox3V));
                        linkedVerticles.Add(4, GetLinkedVerticles(textBox4V));
                        linkedVerticles.Add(5, GetLinkedVerticles(textBox5V));
                        linkedVerticles.Add(6, GetLinkedVerticles(textBox6V));
                        linkedVerticles.Add(7, GetLinkedVerticles(textBox7V));

                        break;
                    }
                case 8:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text),1));

                        _points.Add(new Point(double.Parse(textBoxV2x.Text),
                                              double.Parse(textBoxV2y.Text),
                                              double.Parse(textBoxV2z.Text),2));

                        _points.Add(new Point(double.Parse(textBoxV3x.Text),
                                              double.Parse(textBoxV3y.Text),
                                              double.Parse(textBoxV3z.Text),3));

                        _points.Add(new Point(double.Parse(textBoxV4x.Text),
                                              double.Parse(textBoxV4y.Text),
                                              double.Parse(textBoxV4z.Text),4));

                        _points.Add(new Point(double.Parse(textBoxV5x.Text),
                                              double.Parse(textBoxV5y.Text),
                                              double.Parse(textBoxV5z.Text),5));

                        _points.Add(new Point(double.Parse(textBoxV6x.Text),
                                              double.Parse(textBoxV6y.Text),
                                              double.Parse(textBoxV6z.Text),6));

                        _points.Add(new Point(double.Parse(textBoxV7x.Text),
                                              double.Parse(textBoxV7y.Text),
                                              double.Parse(textBoxV7z.Text),7));

                        _points.Add(new Point(double.Parse(textBoxV8x.Text),
                                              double.Parse(textBoxV8y.Text),
                                              double.Parse(textBoxV8z.Text),8));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        linkedVerticles.Add(2, GetLinkedVerticles(textBox2V));
                        linkedVerticles.Add(3, GetLinkedVerticles(textBox3V));
                        linkedVerticles.Add(4, GetLinkedVerticles(textBox4V));
                        linkedVerticles.Add(5, GetLinkedVerticles(textBox5V));
                        linkedVerticles.Add(6, GetLinkedVerticles(textBox6V));
                        linkedVerticles.Add(7, GetLinkedVerticles(textBox7V));
                        linkedVerticles.Add(8, GetLinkedVerticles(textBox8V));

                        break;
                    }
                case 9:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text),1));

                        _points.Add(new Point(double.Parse(textBoxV2x.Text),
                                              double.Parse(textBoxV2y.Text),
                                              double.Parse(textBoxV2z.Text),2));

                        _points.Add(new Point(double.Parse(textBoxV3x.Text),
                                              double.Parse(textBoxV3y.Text),
                                              double.Parse(textBoxV3z.Text),3));

                        _points.Add(new Point(double.Parse(textBoxV4x.Text),
                                              double.Parse(textBoxV4y.Text),
                                              double.Parse(textBoxV4z.Text),4));

                        _points.Add(new Point(double.Parse(textBoxV5x.Text),
                                              double.Parse(textBoxV5y.Text),
                                              double.Parse(textBoxV5z.Text),5));

                        _points.Add(new Point(double.Parse(textBoxV6x.Text),
                                              double.Parse(textBoxV6y.Text),
                                              double.Parse(textBoxV6z.Text),6));

                        _points.Add(new Point(double.Parse(textBoxV7x.Text),
                                              double.Parse(textBoxV7y.Text),
                                              double.Parse(textBoxV7z.Text),7));

                        _points.Add(new Point(double.Parse(textBoxV8x.Text),
                                              double.Parse(textBoxV8y.Text),
                                              double.Parse(textBoxV8z.Text),8));

                        _points.Add(new Point(double.Parse(textBoxV9x.Text),
                                              double.Parse(textBoxV9y.Text),
                                              double.Parse(textBoxV9z.Text),9));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        linkedVerticles.Add(2, GetLinkedVerticles(textBox2V));
                        linkedVerticles.Add(3, GetLinkedVerticles(textBox3V));
                        linkedVerticles.Add(4, GetLinkedVerticles(textBox4V));
                        linkedVerticles.Add(5, GetLinkedVerticles(textBox5V));
                        linkedVerticles.Add(6, GetLinkedVerticles(textBox6V));
                        linkedVerticles.Add(7, GetLinkedVerticles(textBox7V));
                        linkedVerticles.Add(8, GetLinkedVerticles(textBox8V));
                        linkedVerticles.Add(9, GetLinkedVerticles(textBox9V));

                        break;
                    }
                case 10:
                    {
                        linkedVerticles.Clear();
                        _points.Add(new Point(double.Parse(textBoxV1x.Text),
                                              double.Parse(textBoxV1y.Text),
                                              double.Parse(textBoxV1z.Text),1));

                        _points.Add(new Point(double.Parse(textBoxV2x.Text),
                                              double.Parse(textBoxV2y.Text),
                                              double.Parse(textBoxV2z.Text),2));

                        _points.Add(new Point(double.Parse(textBoxV3x.Text),
                                              double.Parse(textBoxV3y.Text),
                                              double.Parse(textBoxV3z.Text),3));

                        _points.Add(new Point(double.Parse(textBoxV4x.Text),
                                              double.Parse(textBoxV4y.Text),
                                              double.Parse(textBoxV4z.Text),4));

                        _points.Add(new Point(double.Parse(textBoxV5x.Text),
                                              double.Parse(textBoxV5y.Text),
                                              double.Parse(textBoxV5z.Text),5));

                        _points.Add(new Point(double.Parse(textBoxV6x.Text),
                                              double.Parse(textBoxV6y.Text),
                                              double.Parse(textBoxV6z.Text),6));

                        _points.Add(new Point(double.Parse(textBoxV7x.Text),
                                              double.Parse(textBoxV7y.Text),
                                              double.Parse(textBoxV7z.Text),7));

                        _points.Add(new Point(double.Parse(textBoxV8x.Text),
                                              double.Parse(textBoxV8y.Text),
                                              double.Parse(textBoxV8z.Text),8));

                        _points.Add(new Point(double.Parse(textBoxV9x.Text),
                                              double.Parse(textBoxV9y.Text),
                                              double.Parse(textBoxV9z.Text),9));

                        _points.Add(new Point(double.Parse(textBoxV10x.Text),
                                              double.Parse(textBoxV10y.Text),
                                              double.Parse(textBoxV10z.Text),10));
                        linkedVerticles.Add(1, GetLinkedVerticles(textBox1V));
                        linkedVerticles.Add(2, GetLinkedVerticles(textBox2V));
                        linkedVerticles.Add(3, GetLinkedVerticles(textBox3V));
                        linkedVerticles.Add(4, GetLinkedVerticles(textBox4V));
                        linkedVerticles.Add(5, GetLinkedVerticles(textBox5V));
                        linkedVerticles.Add(6, GetLinkedVerticles(textBox6V));
                        linkedVerticles.Add(7, GetLinkedVerticles(textBox7V));
                        linkedVerticles.Add(8, GetLinkedVerticles(textBox8V));
                        linkedVerticles.Add(9, GetLinkedVerticles(textBox9V));
                        linkedVerticles.Add(10, GetLinkedVerticles(textBox10V));

                        break;
                    }
                default:
                    break;
            }
            return _points;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string counter = comboBox1.Text;
            switch (counter)
            {
                case "1":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = false;
                        textBoxV2y.Enabled = false;
                        textBoxV2z.Enabled = false;

                        textBoxV3x.Enabled = false;
                        textBoxV3y.Enabled = false;
                        textBoxV3z.Enabled = false;

                        textBoxV4x.Enabled = false;
                        textBoxV4y.Enabled = false;
                        textBoxV4z.Enabled = false;

                        textBoxV5x.Enabled = false;
                        textBoxV5y.Enabled = false;
                        textBoxV5z.Enabled = false;

                        textBoxV6x.Enabled = false;
                        textBoxV6y.Enabled = false;
                        textBoxV6z.Enabled = false;

                        textBoxV7x.Enabled = false;
                        textBoxV7y.Enabled = false;
                        textBoxV7z.Enabled = false;

                        textBoxV8x.Enabled = false;
                        textBoxV8y.Enabled = false;
                        textBoxV8z.Enabled = false;

                        textBoxV9x.Enabled = false;
                        textBoxV9y.Enabled = false;
                        textBoxV9z.Enabled = false;
                        textBoxV10x.Enabled = false;
                        textBoxV10y.Enabled = false;
                        textBoxV10z.Enabled = false;

                        textBox1V.Enabled = false;
                        textBox2V.Enabled = false;
                        textBox3V.Enabled = false;
                        textBox4V.Enabled = false;
                        textBox5V.Enabled = false;
                        textBox6V.Enabled = false;
                        textBox7V.Enabled = false;
                        textBox8V.Enabled = false;
                        textBox9V.Enabled = false;
                        textBox10V.Enabled = false;

                        textBoxFaces.Enabled = false;
                        break;
                    }
                case "2":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = true;
                        textBoxV2y.Enabled = true;
                        textBoxV2z.Enabled = true;

                        textBoxV3x.Enabled = false;
                        textBoxV3y.Enabled = false;
                        textBoxV3z.Enabled = false;

                        textBoxV4x.Enabled = false;
                        textBoxV4y.Enabled = false;
                        textBoxV4z.Enabled = false;

                        textBoxV5x.Enabled = false;
                        textBoxV5y.Enabled = false;
                        textBoxV5z.Enabled = false;

                        textBoxV6x.Enabled = false;
                        textBoxV6y.Enabled = false;
                        textBoxV6z.Enabled = false;

                        textBoxV7x.Enabled = false;
                        textBoxV7y.Enabled = false;
                        textBoxV7z.Enabled = false;

                        textBoxV8x.Enabled = false;
                        textBoxV8y.Enabled = false;
                        textBoxV8z.Enabled = false;

                        textBoxV9x.Enabled = false;
                        textBoxV9y.Enabled = false;
                        textBoxV9z.Enabled = false;
                        textBoxV10x.Enabled = false;
                        textBoxV10y.Enabled = false;
                        textBoxV10z.Enabled = false;

                        textBox1V.Enabled = true;
                        textBox2V.Enabled = true;
                        textBox3V.Enabled = false;
                        textBox4V.Enabled = false;
                        textBox5V.Enabled = false;
                        textBox6V.Enabled = false;
                        textBox7V.Enabled = false;
                        textBox8V.Enabled = false;
                        textBox9V.Enabled = false;
                        textBox10V.Enabled = false;

                        textBoxFaces.Enabled = false;
                        break;
                    }
                case "3":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = true;
                        textBoxV2y.Enabled = true;
                        textBoxV2z.Enabled = true;

                        textBoxV3x.Enabled = true;
                        textBoxV3y.Enabled = true;
                        textBoxV3z.Enabled = true;

                        textBoxV4x.Enabled = false;
                        textBoxV4y.Enabled = false;
                        textBoxV4z.Enabled = false;

                        textBoxV5x.Enabled = false;
                        textBoxV5y.Enabled = false;
                        textBoxV5z.Enabled = false;

                        textBoxV6x.Enabled = false;
                        textBoxV6y.Enabled = false;
                        textBoxV6z.Enabled = false;

                        textBoxV7x.Enabled = false;
                        textBoxV7y.Enabled = false;
                        textBoxV7z.Enabled = false;

                        textBoxV8x.Enabled = false;
                        textBoxV8y.Enabled = false;
                        textBoxV8z.Enabled = false;

                        textBoxV9x.Enabled = false;
                        textBoxV9y.Enabled = false;
                        textBoxV9z.Enabled = false;
                        textBoxV10x.Enabled = false;
                        textBoxV10y.Enabled = false;
                        textBoxV10z.Enabled = false;

                        textBox1V.Enabled = true;
                        textBox2V.Enabled = true;
                        textBox3V.Enabled = true;
                        textBox4V.Enabled = false;
                        textBox5V.Enabled = false;
                        textBox6V.Enabled = false;
                        textBox7V.Enabled = false;
                        textBox8V.Enabled = false;
                        textBox9V.Enabled = false;
                        textBox10V.Enabled = false;
                        textBoxFaces.Enabled = true;
                        break;
                    }
                case "4":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = true;
                        textBoxV2y.Enabled = true;
                        textBoxV2z.Enabled = true;

                        textBoxV3x.Enabled = true;
                        textBoxV3y.Enabled = true;
                        textBoxV3z.Enabled = true;

                        textBoxV4x.Enabled = true;
                        textBoxV4y.Enabled = true;
                        textBoxV4z.Enabled = true;

                        textBoxV5x.Enabled = false;
                        textBoxV5y.Enabled = false;
                        textBoxV5z.Enabled = false;

                        textBoxV6x.Enabled = false;
                        textBoxV6y.Enabled = false;
                        textBoxV6z.Enabled = false;

                        textBoxV7x.Enabled = false;
                        textBoxV7y.Enabled = false;
                        textBoxV7z.Enabled = false;

                        textBoxV8x.Enabled = false;
                        textBoxV8y.Enabled = false;
                        textBoxV8z.Enabled = false;

                        textBoxV9x.Enabled = false;
                        textBoxV9y.Enabled = false;
                        textBoxV9z.Enabled = false;
                        textBoxV10x.Enabled = false;
                        textBoxV10y.Enabled = false;
                        textBoxV10z.Enabled = false;

                        textBox1V.Enabled = true;
                        textBox2V.Enabled = true;
                        textBox3V.Enabled = true;
                        textBox4V.Enabled = true;
                        textBox5V.Enabled = false;
                        textBox6V.Enabled = false;
                        textBox7V.Enabled = false;
                        textBox8V.Enabled = false;
                        textBox9V.Enabled = false;
                        textBox10V.Enabled = false;
                        textBoxFaces.Enabled = true;
                        break;
                    }
                case "5":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = true;
                        textBoxV2y.Enabled = true;
                        textBoxV2z.Enabled = true;

                        textBoxV3x.Enabled = true;
                        textBoxV3y.Enabled = true;
                        textBoxV3z.Enabled = true;

                        textBoxV4x.Enabled = true;
                        textBoxV4y.Enabled = true;
                        textBoxV4z.Enabled = true;

                        textBoxV5x.Enabled = true;
                        textBoxV5y.Enabled = true;
                        textBoxV5z.Enabled = true;

                        textBoxV6x.Enabled = false;
                        textBoxV6y.Enabled = false;
                        textBoxV6z.Enabled = false;

                        textBoxV7x.Enabled = false;
                        textBoxV7y.Enabled = false;
                        textBoxV7z.Enabled = false;

                        textBoxV8x.Enabled = false;
                        textBoxV8y.Enabled = false;
                        textBoxV8z.Enabled = false;

                        textBoxV9x.Enabled = false;
                        textBoxV9y.Enabled = false;
                        textBoxV9z.Enabled = false;
                        textBoxV10x.Enabled = false;
                        textBoxV10y.Enabled = false;
                        textBoxV10z.Enabled = false;

                        textBox1V.Enabled = true;
                        textBox2V.Enabled = true;
                        textBox3V.Enabled = true;
                        textBox4V.Enabled = true;
                        textBox5V.Enabled = true;
                        textBox6V.Enabled = false;
                        textBox7V.Enabled = false;
                        textBox8V.Enabled = false;
                        textBox9V.Enabled = false;
                        textBox10V.Enabled = false;
                        textBoxFaces.Enabled = true;
                        break;
                    }
                case "6":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = true;
                        textBoxV2y.Enabled = true;
                        textBoxV2z.Enabled = true;

                        textBoxV3x.Enabled = true;
                        textBoxV3y.Enabled = true;
                        textBoxV3z.Enabled = true;

                        textBoxV4x.Enabled = true;
                        textBoxV4y.Enabled = true;
                        textBoxV4z.Enabled = true;

                        textBoxV5x.Enabled = true;
                        textBoxV5y.Enabled = true;
                        textBoxV5z.Enabled = true;

                        textBoxV6x.Enabled = true;
                        textBoxV6y.Enabled = true;
                        textBoxV6z.Enabled = true;

                        textBoxV7x.Enabled = false;
                        textBoxV7y.Enabled = false;
                        textBoxV7z.Enabled = false;

                        textBoxV8x.Enabled = false;
                        textBoxV8y.Enabled = false;
                        textBoxV8z.Enabled = false;

                        textBoxV9x.Enabled = false;
                        textBoxV9y.Enabled = false;
                        textBoxV9z.Enabled = false;
                        textBoxV10x.Enabled = false;
                        textBoxV10y.Enabled = false;
                        textBoxV10z.Enabled = false;

                        textBox1V.Enabled = true;
                        textBox2V.Enabled = true;
                        textBox3V.Enabled = true;
                        textBox4V.Enabled = true;
                        textBox5V.Enabled = true;
                        textBox6V.Enabled = true;
                        textBox7V.Enabled = false;
                        textBox8V.Enabled = false;
                        textBox9V.Enabled = false;
                        textBox10V.Enabled = false;
                        textBoxFaces.Enabled = true;
                        break;
                    }
                case "7":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = true;
                        textBoxV2y.Enabled = true;
                        textBoxV2z.Enabled = true;

                        textBoxV3x.Enabled = true;
                        textBoxV3y.Enabled = true;
                        textBoxV3z.Enabled = true;

                        textBoxV4x.Enabled = true;
                        textBoxV4y.Enabled = true;
                        textBoxV4z.Enabled = true;

                        textBoxV5x.Enabled = true;
                        textBoxV5y.Enabled = true;
                        textBoxV5z.Enabled = true;

                        textBoxV6x.Enabled = true;
                        textBoxV6y.Enabled = true;
                        textBoxV6z.Enabled = true;

                        textBoxV7x.Enabled = true;
                        textBoxV7y.Enabled = true;
                        textBoxV7z.Enabled = true;

                        textBoxV8x.Enabled = false;
                        textBoxV8y.Enabled = false;
                        textBoxV8z.Enabled = false;

                        textBoxV9x.Enabled = false;
                        textBoxV9y.Enabled = false;
                        textBoxV9z.Enabled = false;
                        textBoxV10x.Enabled = false;
                        textBoxV10y.Enabled = false;
                        textBoxV10z.Enabled = false;

                        textBox1V.Enabled = true;
                        textBox2V.Enabled = true;
                        textBox3V.Enabled = true;
                        textBox4V.Enabled = true;
                        textBox5V.Enabled = true;
                        textBox6V.Enabled = true;
                        textBox7V.Enabled = true;
                        textBox8V.Enabled = false;
                        textBox9V.Enabled = false;
                        textBox10V.Enabled = false;
                        textBoxFaces.Enabled = true;
                        break;
                    }
                case "8":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = true;
                        textBoxV2y.Enabled = true;
                        textBoxV2z.Enabled = true;

                        textBoxV3x.Enabled = true;
                        textBoxV3y.Enabled = true;
                        textBoxV3z.Enabled = true;

                        textBoxV4x.Enabled = true;
                        textBoxV4y.Enabled = true;
                        textBoxV4z.Enabled = true;

                        textBoxV5x.Enabled = true;
                        textBoxV5y.Enabled = true;
                        textBoxV5z.Enabled = true;

                        textBoxV6x.Enabled = true;
                        textBoxV6y.Enabled = true;
                        textBoxV6z.Enabled = true;

                        textBoxV7x.Enabled = true;
                        textBoxV7y.Enabled = true;
                        textBoxV7z.Enabled = true;

                        textBoxV8x.Enabled = true;
                        textBoxV8y.Enabled = true;
                        textBoxV8z.Enabled = true;

                        textBoxV9x.Enabled = false;
                        textBoxV9y.Enabled = false;
                        textBoxV9z.Enabled = false;
                        textBoxV10x.Enabled = false;
                        textBoxV10y.Enabled = false;
                        textBoxV10z.Enabled = false;

                        textBox1V.Enabled = true;
                        textBox2V.Enabled = true;
                        textBox3V.Enabled = true;
                        textBox4V.Enabled = true;
                        textBox5V.Enabled = true;
                        textBox6V.Enabled = true;
                        textBox7V.Enabled = true;
                        textBox8V.Enabled = true;
                        textBox9V.Enabled = false;
                        textBox10V.Enabled = false;
                        textBoxFaces.Enabled = true;
                        break;
                    }
                case "9":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = true;
                        textBoxV2y.Enabled = true;
                        textBoxV2z.Enabled = true;

                        textBoxV3x.Enabled = true;
                        textBoxV3y.Enabled = true;
                        textBoxV3z.Enabled = true;

                        textBoxV4x.Enabled = true;
                        textBoxV4y.Enabled = true;
                        textBoxV4z.Enabled = true;

                        textBoxV5x.Enabled = true;
                        textBoxV5y.Enabled = true;
                        textBoxV5z.Enabled = true;

                        textBoxV6x.Enabled = true;
                        textBoxV6y.Enabled = true;
                        textBoxV6z.Enabled = true;

                        textBoxV7x.Enabled = true;
                        textBoxV7y.Enabled = true;
                        textBoxV7z.Enabled = true;

                        textBoxV8x.Enabled = true;
                        textBoxV8y.Enabled = true;
                        textBoxV8z.Enabled = true;

                        textBoxV9x.Enabled = true;
                        textBoxV9y.Enabled = true;
                        textBoxV9z.Enabled = true;

                        textBoxV10x.Enabled = false;
                        textBoxV10y.Enabled = false;
                        textBoxV10z.Enabled = false;

                        textBox1V.Enabled = true;
                        textBox2V.Enabled = true;
                        textBox3V.Enabled = true;
                        textBox4V.Enabled = true;
                        textBox5V.Enabled = true;
                        textBox6V.Enabled = true;
                        textBox7V.Enabled = true;
                        textBox8V.Enabled = true;
                        textBox9V.Enabled = true;
                        textBox10V.Enabled = false;
                        textBoxFaces.Enabled = true;
                        break;
                    }
                case "10":
                    {
                        textBoxV1x.Enabled = true;
                        textBoxV1y.Enabled = true;
                        textBoxV1z.Enabled = true;

                        textBoxV2x.Enabled = true;
                        textBoxV2y.Enabled = true;
                        textBoxV2z.Enabled = true;

                        textBoxV3x.Enabled = true;
                        textBoxV3y.Enabled = true;
                        textBoxV3z.Enabled = true;

                        textBoxV4x.Enabled = true;
                        textBoxV4y.Enabled = true;
                        textBoxV4z.Enabled = true;

                        textBoxV5x.Enabled = true;
                        textBoxV5y.Enabled = true;
                        textBoxV5z.Enabled = true;

                        textBoxV6x.Enabled = true;
                        textBoxV6y.Enabled = true;
                        textBoxV6z.Enabled = true;

                        textBoxV7x.Enabled = true;
                        textBoxV7y.Enabled = true;
                        textBoxV7z.Enabled = true;

                        textBoxV8x.Enabled = true;
                        textBoxV8y.Enabled = true;
                        textBoxV8z.Enabled = true;

                        textBoxV9x.Enabled = true;
                        textBoxV9y.Enabled = true;
                        textBoxV9z.Enabled = true;

                        textBoxV10x.Enabled = true;
                        textBoxV10y.Enabled = true;
                        textBoxV10z.Enabled = true;

                        textBox1V.Enabled = true;
                        textBox2V.Enabled = true;
                        textBox3V.Enabled = true;
                        textBox4V.Enabled = true;
                        textBox5V.Enabled = true;
                        textBox6V.Enabled = true;
                        textBox7V.Enabled = true;
                        textBox8V.Enabled = true;
                        textBox9V.Enabled = true;
                        textBox10V.Enabled = true;
                        textBoxFaces.Enabled = true;
                        break;
                    }
                default:
                    break;
            }
        }

        
    }
}
