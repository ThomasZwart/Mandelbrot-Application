﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Mandelbrot
{
    public partial class Form1 : Form
    {
        Bitmap mandelbrotBitmap = new Bitmap(800, 800);
        float scale = 0.01f;
        double centerWidth = 0, centerHeight = 0;
        int maxIterations = 255;

        public Form1()
        {
            InitializeComponent();

            mandelbrotPictureBox.MouseClick += ClickMandelbrot;
            GenerateMandelbrot();

            resetButton.Click += ClickResetButton;


            Controls.Add(mandelbrotPictureBox);             
        }

        // For every pixel in the bitmap the mandelbrot number gets calculated and a color assigned according to that number
        public void GenerateMandelbrot()
        {
            int width = mandelbrotPictureBox.Width;
            int height = mandelbrotPictureBox.Height;

            for (int i = 1; i < width; i++) {
                for (int j = 1; j < height; j++) {
                    // Shifting pixels to the center in order to get the figure in the center
                    double x = (i - width / 2) * scale - centerWidth;
                    double y = (height / 2 - j) * scale - centerHeight;
                    int mandelbrotNumber = CalculateMandelbrotNumber(x, y);
                    ColorPixel(mandelbrotNumber, i, j);
                }
            }
            mandelbrotPictureBox.Image = mandelbrotBitmap;
        }
        
        // Given a complex number c (x and y), calculate how many times you can iterate the function f(z) = z^2 + c
        // with z = 0 untill f(z) >= 2
        public int CalculateMandelbrotNumber(double x, double y)
        {
            // The real and Imaginary part of z
            double z_a = 0, z_b = 0;
            double new_z_a, new_z_b;

            int mandelbrotNumber = 0;

            while(DistanceToRoot(z_a, z_b) < 2 && mandelbrotNumber < maxIterations) {
                new_z_a = z_a * z_a - z_b * z_b + x;
                new_z_b = 2 * z_a * z_b + y;
                z_a = new_z_a;
                z_b = new_z_b;
                mandelbrotNumber++;
            }
            return mandelbrotNumber;
        }

        public double DistanceToRoot(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        public void ColorPixel(int mandelbrotNumber, int x, int y)
        {
            if (mandelbrotNumber > 255) mandelbrotNumber = 255;
            mandelbrotBitmap.SetPixel(x, y, Color.FromArgb(mandelbrotNumber / 5, mandelbrotNumber, mandelbrotNumber/2));
        }

        public void ClickMandelbrot(object o, MouseEventArgs mea)
        {
            // Screen moves to where the mouse is clicked
            centerWidth += (mandelbrotPictureBox.Width / 2 - mea.X) * scale;
            centerHeight += (mea.Y - mandelbrotPictureBox.Height / 2) * scale;

            // Zooming in and out
            if (mea.Button == MouseButtons.Left)
                scale /= 2;
            if (mea.Button == MouseButtons.Right)
                scale *= 2;

            GenerateMandelbrot();
        }

        public void ClickResetButton (object o, EventArgs ea)
        {
            centerHeight = 0;
            centerWidth = 0;
            scale = 0.01f;
            GenerateMandelbrot();
        }

        private double Clamp(double x, double min, double max)
        {
            if (x < min) {
                x = min;
            }
            else if (x > max) {
                x = max;
            }
            return x;
        }
    }
}
