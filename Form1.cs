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
        private int angle = 0;
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
            // устанавливаем ось X по умолчанию
            comboBox1.SelectedIndex = 0;
        }

        bool showTexture = true; // Выводим сферу с текстурой
        bool useMaterial = true; // Используем материал и источники света
        bool showNormals = true; // Показываем нормали

        // Смещение
        private float x_tr = 0;
        private float y_tr = 0;
        private float z_tr = 0;
        //Масштаб 
        private float y_scale = 2;
        private float x_scale = 2;
        private float z_scale = 2;
        //Цвет
        private Color Color = Color.Blue;
        // загружена ли текстура
        private bool textureIsLoad = false;
        // идентификатор текстуры
        public int imageId = 0;
        // текстурный объект
        public uint mGlTextureObject = 0;


        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Gl.glClearColor(1, 1, 1, 1);
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
            // начало визуализации (активируем таймер)
            RenderTimer.Start();
            // инициализация библиотеки openIL
            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);

            

        }

        private void showSolid()
        {
            Glut.glutSolidSphere(0.75, 16, 16); // Сфера
        }

        /// <summary>
            /// Отрисовка примитива
            /// </summary>
            private void Draw()
        {
            if (radioButton1.Checked)//wire
            {
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                Gl.glLoadIdentity();
                Gl.glColor3f(Color.R, Color.G, Color.B);
                // масштаб
                Gl.glScalef(x_scale, y_scale, z_scale);
                Gl.glPushMatrix();
                Gl.glTranslated(0, 0, -6);
                // перемещаем камеру для более хорошего обзора объекта
                Gl.glTranslated(x_tr, y_tr, z_tr);
                Gl.glRotated(angle, 1, 1, 0);
                // рисуем сферу с помощью библиотеки FreeGLUT
                Glut.glutWireSphere(0.7, 30, 30);
                Gl.glPopMatrix();
                Gl.glFlush();
                AnT.Invalidate();
            }
            else if (radioButton2.Checked)//solid
            {
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                Gl.glLoadIdentity();
                Gl.glColor3f(Color.R, Color.G, Color.B);
                // масштаб
                Gl.glScalef(x_scale, y_scale, z_scale);
                Gl.glPushMatrix();
                Gl.glTranslated(0, 0, -6);
                // перемещаем камеру для более хорошего обзора объекта
                Gl.glTranslated(x_tr, y_tr, z_tr);
                Gl.glRotated(angle, 1, 1, 0);
                // рисуем сферу с помощью библиотеки FreeGLUT
                Glut.glutSolidSphere(0.7, 30, 30);

                // Модель освещенности с одним источником цвета
                float[] light_position = { 100, 0, 0, 0 }; // Координаты источника света
                float[] lghtClr = { Color.R, Color.G, Color.B, 1 }; // Источник излучает белый цвет 
                float[] mtClr = { 1, 1, 1, 0 }; // Цвет излучения сферы на которую падает цвет

                if (radioButton4.Checked)
                {
                    Gl.glPolygonMode(Gl.GL_FRONT, Gl.GL_FILL); // Заливка полигонов
                    Gl.glShadeModel(Gl.GL_SMOOTH); // Вывод с интерполяцией цветов
                    Gl.glEnable(Gl.GL_LIGHTING); // Будем рассчитывать освещенность
                    Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light_position);
                    Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, lghtClr); // Рассеивание
                    Gl.glEnable(Gl.GL_LIGHT0); // Включаем в уравнение освещенности источник GL_LIGHT0
                    // Диффузионная компонента цвета материала
                    Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mtClr);
                    // Выводим тонированный glut-примитив
                    showSolid();
                    Gl.glPopMatrix();
                    Gl.glFlush();
                    AnT.Invalidate();
                    Gl.glDisable(Gl.GL_LIGHT0);
                }
                else if (radioButton5.Checked)
                {
                    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                    //Gl.glClearColor(0.5f, 0.5f, 0.5f, 1.0f);
                    Gl.glLoadIdentity();
                    Gl.glColor3f(Color.R, Color.G, Color.B);
                    // масштаб
                    Gl.glScalef(x_scale, y_scale, z_scale);
                    Gl.glPushMatrix();
                    Gl.glTranslated(0, 0, -6);
                    // перемещаем камеру для более хорошего обзора объекта
                    Gl.glTranslated(x_tr, y_tr, z_tr);
                    Gl.glRotated(angle, 1, 1, 0);
                    //// рисуем сферу с помощью библиотеки FreeGLUT
                    //Glut.glutSolidSphere(0.7, 30, 30);

                    float[] fogColor = new float[4] { 0.5f, 0.5f, 0.5f, 1.0f }; // Цвет тумана

                    Gl.glEnable(Gl.GL_BLEND); // Включает туман (GL_FOG)
                    Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                    Gl.glEnable(Gl.GL_ALPHA_TEST);
                    Gl.glColor3f(1, 1, 1);
                    Glut.glutSolidSphere(3, 30, 30);
                    Gl.glAlphaFunc(Gl.GL_GREATER, (float)0.3);
                    Gl.glDisable(Gl.GL_BLEND);
                    // Выводим тонированный glut-примитив

                    Gl.glPopMatrix();
                    Gl.glFlush();
                    AnT.Invalidate();
                }
                
                

                Gl.glPopMatrix();
                Gl.glFlush();
                AnT.Invalidate();
             


            }
            else if (radioButton3.Checked)//texture
            {
                if (textureIsLoad)
                {
                    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                    Gl.glClearColor(1, 1, 1, 1);
                    Gl.glLoadIdentity();
                    
                    Gl.glRotated(angle, 0, 0, 1);

                    // масштаб
                    Gl.glScalef(x_scale, y_scale, z_scale);

                    // установка белого цвета
                    Gl.glColor3f(1, 1, 1);

                    // помещаем состояние матрицы в стек матриц
                    Gl.glPushMatrix();

                    // перемещаем камеру для более хорошего обзора объекта
                    Gl.glTranslated(x_tr, y_tr, z_tr);
                    Gl.glRotated(angle, 1, 1, 0);

                    // Активизируем генерацию координат текстуры
                    Gl.glEnable(Gl.GL_TEXTURE_GEN_S);
                    Gl.glEnable(Gl.GL_TEXTURE_GEN_T);
                    // включаем режим текстурирования
                    Gl.glEnable(Gl.GL_TEXTURE_2D);
                    // включаем режим текстурирования, указывая идентификатор mGlTextureObject
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, mGlTextureObject);
                    Gl.glTexGeni(Gl.GL_S, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_SPHERE_MAP); // GL_OBJECT_LINEAR, GL_EYE_LINEAR
                    Gl.glTexGeni(Gl.GL_T, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_SPHERE_MAP);
                    showSolid();
                   // Gl.glDisable(Gl.GL_TEXTURE_GEN_S);
                    //Gl.glDisable(Gl.GL_TEXTURE_GEN_T);
                    //Gl.glDisable(Gl.GL_TEXTURE_2D);
                    
                    //Gl.glDeleteTextures(1, ref mGlTextureObject);
                    
                    Gl.glPopMatrix();
                    Gl.glFlush();
                    AnT.Invalidate();
                }
                
            }

        }



        /// <summary>
        /// Поворот и перемещение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            // W A S D отвечают за перенос
            if (e.KeyCode == Keys.D) //вправо
            {
                x_tr += 0.05f;
            }
            else if (e.KeyCode == Keys.W)//вверх
            {
                y_tr += 0.05f;
            }
            else if (e.KeyCode == Keys.S)//вниз
            {
                y_tr -= 0.05f;
            }
            else if (e.KeyCode == Keys.A)//влево
            {
                x_tr -= 0.05f;
            }
            else if (e.KeyCode == Keys.Q)//Масштаб -
            {
                if (comboBox1.SelectedIndex == 0)//x
                {
                    x_scale /= 1.05f;
                }
                else if (comboBox1.SelectedIndex == 1)//y
                {
                    y_scale /= 1.05f;
                }
                else if (comboBox1.SelectedIndex == 1)//z
                {
                    z_scale /= 1.05f;
                }
            }
            else if (e.KeyCode == Keys.E)//Масштаб +
            {
                if (comboBox1.SelectedIndex == 0)//x
                {
                    x_scale *= 1.05f;
                }
                else if (comboBox1.SelectedIndex == 1)//y
                {
                    y_scale *= 1.05f;
                }
                else if (comboBox1.SelectedIndex == 1)//z
                {
                    z_scale *= 1.05f;
                }
            }
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            Draw();
        }

        /// <summary>
        /// Выбор оси в combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnT.Focus();
        }

        /// <summary>
        /// Цвет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
                Color = color;
                pictureBox1.BackColor = color;
            }
        }

        // Создание текстуры в памяти openGL
        private static uint MakeGlTexture(int Format, IntPtr pixels, int w, int h)
        {
            // идентификатор текстурного объекта
            uint texObject;
            // генерируем текстурный объект
            Gl.glGenTextures(1, out texObject);
            // устанавливаем режим упаковки пикселей
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
            // создаем привязку к только что созданной текстуре
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);
            // устанавливаем режим фильтрации и повторения текстуры
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_3D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);

            // создаем RGB или RGBA текстуру
            switch (Format)
            {
                case Gl.GL_RGB:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, w, h, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;
                case Gl.GL_RGBA:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;
            }
            // возвращаем идентификатор текстурного объекта
            return texObject;
        }

        /// <summary>
        /// Загрузка текстуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            // открываем окно выбора файла
            DialogResult res = openFileDialog1.ShowDialog();
            // если файл выбран - и возвращен результат O
            if (res == DialogResult.OK)
            {
                // создаем изображение с идентификатором imageId
                Il.ilGenImages(1, out imageId);
                // делаем изображение текущим
                Il.ilBindImage(imageId);
                // адрес изображения полученный с помощью окна выбора файла
                string url = openFileDialog1.FileName;
                // пробуем загрузить изображение
                if (Il.ilLoadImage(url))
                {
                    // если загрузка прошла успешно 
                    // сохраняем размеры изображения
                    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                    // определяем число бит на пиксель
                    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                    switch (bitspp)
                        // в зависимости от полученного результата
                    {
                        // создаем текстуру, используя режим GL_RGB или GL_RGBA
                        case 24:
                            mGlTextureObject = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                            break;
                        case 32:
                            mGlTextureObject = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                            break;
                    }
                    // активируем флаг, сигнализирующий загрузку текстуры
                    textureIsLoad = true;
                    // очищаем память
                    Il.ilDeleteImages(1, ref imageId);
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Обновить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Gl.glClearColor(1, 1, 1, 1);
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
            // начало визуализации (активируем таймер)
            RenderTimer.Start();
            // инициализация библиотеки openIL
            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
        }
    }
}
