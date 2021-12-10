using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Lb6Paint
{
    public partial class Form1 : Form
    {
        // угол поворота
        private int angle = 45;
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        bool showTexture = true; // Выводим сферу с текстурой
        bool useMaterial = true; // Используем материал и источники света
        bool showNormals = true; // Показываем нормали
        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Gl.glClearColor(255, 255, 255, 1);
            // установка порта вывода в соответствии с размерами элемента anT
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);
            // настройка проекции
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, (float)AnT.Width / (float)AnT.Height, 0.1, 200);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            // настройка параметров OpenGL для визуализации
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            //
            Draw();
        //
        string path = "C:\\Users\\aken\\Desktop\\cosmos";

        }

        private void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glColor3f(0, 1, 1);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, -6);
            Gl.glRotated(angle, 1, 1, 0);
            // рисуем сферу с помощью библиотеки FreeGLUT
            Glut.glutWireSphere(0.7, 50, 50);
            Glut.glutWireCylinder(1,2,50,1);
            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }
        private void AnT_KeyDown(object sender, KeyEventArgs e)
        {
            //Поворот Право-Х Лево-Z
            if (e.KeyCode == Keys.X)
            {
                angle += 5;
            }
            if (e.KeyCode == Keys.Z)
            {
                angle -= 5;
            }
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            Draw();
        }
    }
}
