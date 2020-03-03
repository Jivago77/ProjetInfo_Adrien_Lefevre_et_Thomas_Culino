using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_info
{
    class Pixel
    {
        private int[] tabPixels;

        public Pixel(int[] tabPixels) //en paramètre : le tableau comportant les trois indices de couleur de chaque pixel (ou plus si l'on définit une image)

        {
            this.tabPixels = new int[3];
            for (int i = 0; i < tabPixels.Length; i++)
            {
                this.tabPixels[i] = tabPixels[i];
            }
        }

        public int[] Pixels
        {
            get { return tabPixels; }  //accès en lecture
            set { tabPixels = value; } //accès en écriture
        }
        public void OctetPixel(int index, int value)
        {
            tabPixels[index] = value; //accès en écriture
        }


        static public byte ConvolutionLocale(byte[,] partieImage, double[,] filtre)
        {
            double resultat = 0;
            if (partieImage.GetLength(0) == filtre.GetLength(0) && partieImage.GetLength(1) == filtre.GetLength(1))
            {
                //double sommeElementsFiltre = 0;
                double sommeElementsNeg = 0;
                double sommeElementsPos = 0;
                for (int i = 0; i < filtre.GetLength(0); i++)
                {
                    for (int j = 0; j < filtre.GetLength(1); j++)
                    {
                        if (filtre[i, j] < 0) sommeElementsNeg += filtre[i, j];
                        else sommeElementsPos += filtre[i, j];
                        //sommeElementsFiltre += filtre[i, j];
                    }
                }
                double f = Math.Max(-sommeElementsNeg, sommeElementsPos);

                for (int i = 0; i < partieImage.GetLength(0); i++)
                {
                    for (int j = 0; j < partieImage.GetLength(1); j++)
                    {
                        resultat += partieImage[i, j] * filtre[i, j];
                    }
                }
                //resultat /= sommeElementsFiltre;
                resultat /= f;
            }

            return Convert.ToByte(Math.Abs(resultat));
        }

        static public byte[,] ConvolutionTotale(byte[,] image, double[,] filtre, int pas)
        {
            if (pas <= 0) pas = 1;
            byte[,] imageArrivee = new byte[image.GetLength(0) / pas, image.GetLength(1) / pas];
            //for (int i = 0; i < image.GetLength(0); i += pas)
            //{
            //    for (int j = 0; j < image.GetLength(1); j += pas)
            //    {
            //        byte[,] partieImage = new byte[filtre.GetLength(0), filtre.GetLength(1)];
            //        for (int l = 0; l < partieImage.GetLength(0); l++)
            //        {
            //            for (int c = 0; c < partieImage.GetLength(1); c++)
            //            {
            //                partieImage[l, c] = image[Math.Abs((i - (partieImage.GetLength(0) / 2) + l) % image.GetLength(0)), Math.Abs((j - (partieImage.GetLength(1) / 2) + c) % image.GetLength(1))];
            //            }
            //        }
            //        imageArrivee[i, j] = ConvolutionLocale(partieImage, filtre);
            //    }
            //}

            for (int i = 0; i < image.GetLength(0); i += pas)
            {
                for (int j = 0; j < image.GetLength(1); j += pas)
                {
                    byte[,] partieImage = new byte[filtre.GetLength(0), filtre.GetLength(1)];
                    for (int l = 0; l < partieImage.GetLength(0); l++)
                    {
                        for (int c = 0; c < partieImage.GetLength(1); c++)
                        {
                            partieImage[l, c] = image[Math.Abs((i - (partieImage.GetLength(0) / 2) + l) % image.GetLength(0)), Math.Abs((j - (partieImage.GetLength(1) / 2) + c) % image.GetLength(1))];
                        }
                    }
                    imageArrivee[i / pas, j / pas] = ConvolutionLocale(partieImage, filtre);
                }
            }

            return imageArrivee;
        }

        
    }
}



