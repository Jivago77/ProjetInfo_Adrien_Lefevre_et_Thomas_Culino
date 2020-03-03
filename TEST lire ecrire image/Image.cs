using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Projet_info
{
    class MonImage
    {
        private int hauteurImage;
        private int tailleImage;
        private int largeurImage;
        private string typeImage;
        private Pixel[,] matImage;
        private int bit;
        private byte[] tabFichier;
        private byte[] nouveauFichier;
        private Random rnd = new Random();

        public MonImage(string fichier)
        {

            tabFichier = File.ReadAllBytes(fichier);
            if (tabFichier[0] == 66 && tabFichier[1] == 77) { typeImage = ".bmp"; }
            else { typeImage = "autre"; }

            byte[] tabBytes = { tabFichier[2], tabFichier[3], tabFichier[4], tabFichier[5] };

            tailleImage = Convertir_Endian_To_Int(tabBytes);
            tabBytes = new byte[4] { tabFichier[18], tabFichier[19], tabFichier[20], tabFichier[21] };
            largeurImage = Convertir_Endian_To_Int(tabBytes);
            tabBytes = new byte[4] { tabFichier[22], tabFichier[23], tabFichier[24], tabFichier[25] };
            hauteurImage = Convertir_Endian_To_Int(tabBytes);
            tabBytes = new byte[2] { tabFichier[28], tabFichier[29] };
            bit = Convertir_Endian_To_Int(tabBytes);
            matImage = new Pixel[hauteurImage, largeurImage]; //création d'une matrice de pixels comportant une hauteur et largeur (notre image)
            int k = 54;


            for (int i = 0; i < matImage.GetLength(0); i++)
            {
                for (int j = 0; j < matImage.GetLength(1); j++)
                {
                    int[] tabOctets = { tabFichier[k], tabFichier[k + 1], tabFichier[k + 2] }; //on recopie les octets du fichier dans un tableau
                    matImage[i, j] = new Pixel(tabOctets); //on crée des pixels à partir de ces tableaux, on les places dans une matrice

                    k = k + 3;
                }
            }
        }

        public MonImage(int largeurImage, int hauteurImage)
        {
            matImage = new Pixel[hauteurImage, largeurImage];
            this.largeurImage = largeurImage;
            this.hauteurImage = hauteurImage;
            tabFichier = new byte[largeurImage * hauteurImage * 3 + 54];
        }


        public Pixel[,] MatImage
        {
            get { return (matImage); }
            set { matImage = value; }
        }

        public int Taille
        {
            get { return (tailleImage); }
        }

        public int Largeur
        {
            get { return largeurImage; }
        }

        public int Hauteur
        {
            get { return hauteurImage; }
        }

        public byte[] Fichier
        {
            get { return tabFichier; }
        }


        public void AfficherMatricePixels(Pixel[,] matricePixels)
        {
            //Les octets sont affichés dans l'ordre de l'image
            for (int i = 0; i < matricePixels.GetLength(0); i++) //on parcourt toutes les lignes de l'matrice
            {
                for (int j = 0; j < matricePixels.GetLength(1); j++) //on parcourt toutes les colonnes de l'matrice
                {
                    for (int o = 0; o < 3; o++) //on parcourt toutes les colonnes de l'matrice
                    {
                        Console.Write(matricePixels[i, j].Pixels[o] + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void AfficherMatriceInt(int[,] matrice)
        {
            //Les octets sont affichés dans l'ordre de l'image
            for (int i = 0; i < matrice.GetLength(0); i++) //on parcourt toutes les lignes de l'matrice
            {
                for (int j = 0; j < matrice.GetLength(1); j++) //on parcourt toutes les colonnes de l'matrice
                {
                    Console.Write(matrice[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void AfficherMatriceDouble(double[,] matrice)
        {
            //Les octets sont affichés dans l'ordre de l'image
            for (int i = 0; i < matrice.GetLength(0); i++) //on parcourt toutes les lignes de l'matrice
            {
                for (int j = 0; j < matrice.GetLength(1); j++) //on parcourt toutes les colonnes de l'matrice
                {
                    Console.Write(matrice[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void AfficherImageAlEndroit(MonImage image)
        {
            //Les octets sont affichés dans l'ordre de l'image
            for (int i = image.Hauteur - 1; i >= 0; i--) //on parcourt toutes les lignes de l'image
            {
                for (int j = 0; j < image.Largeur; j++) //on parcourt toutes les colonnes de l'image
                {
                    for (int o = image.matImage[i, j].Pixels.Length - 1; o >= 0; o--)
                    {
                        Console.Write(image.matImage[i, j].Pixels[o] + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void AfficherImageOriginal(Pixel[,] matPixels)
        {
            for (int i = 0; i < matPixels.GetLength(0); i++)
            {
                for (int j = 0; j < matPixels.GetLength(1); j++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        Console.Write(matPixels[i, j].Pixels[c] + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public byte[] Header(int hauteurImage, int largeurImage, byte[] unFichier)
        {
            //Format de fichier 
            unFichier[0] = 66;
            unFichier[1] = 77;
            //=================
            //Taille du fichier en octets
            for (int i = 0; i < 4; i++)
            {
                unFichier[2 + i] = Convertir_Int_To_Endian(largeurImage * hauteurImage * 3 + 54)[i];
            }
            //==========================
            //Offset du fichier
            unFichier[10] = 54;
            //==========================
            //Taille de la zone Bitmap info
            unFichier[14] = 40;
            //=========================
            //Largeur de l'image
            for (int i = 18; i < 22; i++)
            {
                unFichier[i] = Convertir_Int_To_Endian(largeurImage)[i - 18];
            }
            //unFichier0[19] = 2;
            //=========================
            //Hauteur de l'image
            for (int i = 22; i < 26; i++)
            {
                unFichier[i] = Convertir_Int_To_Endian(hauteurImage)[i - 22];
            }
            //unFichier0[23] = 2;
            //=========================
            //Nombre de plans
            unFichier[26] = 1;
            //=========================
            //Bits par pixel
            unFichier[28] = 24;
            //=========================
            //Taille de l'image en octets
            for (int i = 34; i < 38; i++)
            {
                unFichier[i] = Convertir_Int_To_Endian(largeurImage * hauteurImage * 3)[i - 34];
            }
            //unFichier0[36] = 12;
            //=========================
            return unFichier;
        }


        public void From_Image_To_File()
        {
            int compteur = Convert.ToInt32(Math.IEEERemainder(matImage.GetLength(1), 4));
            if (compteur < 0)
            {
                compteur += 4;
            }
            byte[] monFichierFinal = new byte[matImage.GetLength(0) * (matImage.GetLength(1) + compteur) * 3 + 54];
            monFichierFinal = Header(matImage.GetLength(0), matImage.GetLength(1), monFichierFinal);
            int k = 54;
            int[] tab = { 0, 0, 0 };
            Pixel monPixel = new Pixel(tab);
            for (int i = 0; i < matImage.GetLength(0); i++)
            {
                for (int j = 0; j < matImage.GetLength(1); j++)
                {
                    if (matImage[i, j] == null)
                    {
                        matImage[i, j] = monPixel;
                    }
                    for (int o = 0; o < matImage[i, j].Pixels.Length; o++)
                    {
                        monFichierFinal[k] = Convert.ToByte(matImage[i, j].Pixels[o]);
                        k++;
                    }
                }
                for (int l = 0; l < compteur; l++)
                {
                    monFichierFinal[k + l] = 0;
                }
                k += compteur;
            }
            File.WriteAllBytes("./Images/Sortie.bmp", monFichierFinal);
        }

        public void From_Image_To_File2()
        {
            byte[] fichier = new byte[matImage.GetLength(0) * matImage.GetLength(1) * 3 + 54];
            int k = 54;
            for (int i = 0; i < matImage.GetLength(0); i++)
            {
                for (int j = 0; j < matImage.GetLength(1); j++)
                {
                    for (int o = 0; o < matImage[i, j].Pixels.Length; o++)
                    {
                        fichier[k] = Convert.ToByte(matImage[i, j].Pixels[o]);
                        k++;
                    }
                }
            }
            File.WriteAllBytes("./Images/Sortie.bmp", fichier);
        }


        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int result = 0;

            for (int i = 0; i < tab.Length; i++)
            {
                result += tab[i] * Convert.ToInt32(Math.Pow(256, i));
            }
            return result;
        }

        public byte[] Convertir_Int_To_Endian(int valeur)
        {
            byte[] tab = new byte[4];
            int somme = 0;
            for (int i = tab.Length - 1; i >= 0; i--)
            {
                tab[i] = Convert.ToByte((valeur - somme) / Convert.ToInt32(Math.Pow(256, i)));
                somme += tab[i] * Convert.ToInt32(Math.Pow(256, i));
            }
            return tab;
        }


        public void GenererNuance()
        {
            /*
            int a = 0;
            int b = 120;
            int c = 255;
            for (int i = 0; i < matImage.GetLength(0); i++)
            {
                for (int j = 0; j < matImage.GetLength(1); j++)
                {
                    int[] tabIndicesCouleurs = { a, b, c };
                    matImage[i, j] = new Pixel(tabIndicesCouleurs);
                    a += 1;
                    b += 1;
                    c -= 1;
                    if (a > 255) { a = 0; }
                    if (b > 255) { b = 120; }
                    if (c < 0) { c = 255; }
                }
            }*/
            int i = 0;
            int colonne = matImage.GetLength(1) - 1;
            int j = colonne;

            for (int compteur = 0; compteur < matImage.Length; compteur++)
            {
                bool aCroiss = true;
                bool cCroiss = false;
                bool bCroiss = false;

                int a = 0;
                int b = 0;
                int c = 0;

                while (i < matImage.GetLength(0) && j < matImage.GetLength(1))
                {
                    int[] tabIndicesCouleurs = { a, b, c };
                    matImage[i, j] = new Pixel(tabIndicesCouleurs);
                    matImage[j, i] = new Pixel(tabIndicesCouleurs);

                    if (aCroiss)
                    {
                        a += 1;
                        if (a > 254) { aCroiss = false; a = 255; bCroiss = true; }
                    }
                    /*else
                    {
                        a-=1; ;
                        if (a < 1) { aCroiss = true; a = 0; }
                    }*/
                    if (bCroiss)
                    {
                        b += 1;
                        if (b > 254) { bCroiss = false; b = 255; cCroiss = true; }
                    }
                    /*else
                    {
                        b-=1;
                        if (b < 1) { bCroiss = true; b = 0; }
                    }*/
                    if (cCroiss)
                    {
                        c += 1;
                        if (c > 254) { cCroiss = false; c = 255; }
                    }
                    /*else
                    {
                        c-=1;
                        if (c < 1) { cCroiss = true; c = 0; }
                    }*/

                    i++;
                    j++;
                }

                if (colonne > 0) { colonne--; }
                j = colonne;
                i = 0;
            }

        }
        public Pixel[,] NoirEtBlanc()
        {
            Pixel[,] matImageNoirEtBlanc = new Pixel[hauteurImage, largeurImage];
            for (int i = 0; i < matImage.GetLength(0); i++)
            {
                for (int j = 0; j < matImage.GetLength(1); j++)
                {
                    int niveauDeGris = (matImage[i, j].Pixels[0] + matImage[i, j].Pixels[1] + matImage[i, j].Pixels[2]) / 3; //on fait la moyenne de rouge vert et bleu
                    int[] pixelGris = new int[3];
                    pixelGris[0] = niveauDeGris;
                    pixelGris[1] = niveauDeGris;
                    pixelGris[2] = niveauDeGris;
                    Pixel pixelgrispix = new Pixel(pixelGris);
                    matImageNoirEtBlanc[i, j] = pixelgrispix;
                }
            }
            return matImageNoirEtBlanc;
        }
        public Pixel[,] Negatif()
        {
            Pixel[,] imageNegatif = new Pixel[hauteurImage, largeurImage];
            for (int i = 0; i < matImage.GetLength(0); i++)
            {
                for (int j = 0; j < matImage.GetLength(1); j++)
                {
                    int pixelRouge = 255 - matImage[i, j].Pixels[0];
                    int pixelVert = 255 - matImage[i, j].Pixels[1];
                    int pixelBleu = 255 - matImage[i, j].Pixels[2];
                    int[] newPixel = { pixelRouge, pixelVert, pixelBleu };
                    imageNegatif[i, j] = new Pixel(newPixel);
                }
            }
            return imageNegatif;
        }

        public Pixel[,] Filtrage(double[,] kernel)
        {
            Pixel[,] matImageFinale = new Pixel[hauteurImage, largeurImage];
            int[] tabNull = { 0, 0, 0 };
            for (int i = 0; i < matImageFinale.GetLength(0); i++)
            {
                for (int j = 0; j < matImageFinale.GetLength(1); j++)
                {
                    matImageFinale[i, j] = new Pixel(tabNull);
                }
            }

            int[,] mat9Pixels = new int[kernel.GetLength(0), kernel.GetLength(1)];

            //AfficherImageOriginal(matImage);
            //AfficherMatriceDouble(kernel);
            for (int indexLigneMatImage = 0; indexLigneMatImage < matImage.GetLength(0) - (kernel.GetLength(0) - 1); indexLigneMatImage++)
            {
                for (int indexColonneMatImage = 0; indexColonneMatImage < matImage.GetLength(1) - (kernel.GetLength(1) - 1); indexColonneMatImage++)
                {
                    double somme; //somme de tous les octets de la mat9Pixels
                    double sommeKernel;
                    double sommeKernelPositif; //somme de tous les éléments du kernel 
                    double sommeKernelNegatif;

                    for (int indexColonnePixel = 0; indexColonnePixel < 3; indexColonnePixel++)
                    {
                        somme = 0;
                        sommeKernel = 0;
                        sommeKernelPositif = 0;
                        sommeKernelNegatif = 0;

                        for (int indexLigneMat9Pixels = 0; indexLigneMat9Pixels < kernel.GetLength(0); indexLigneMat9Pixels++)
                        {
                            for (int compteur = 0; compteur < kernel.GetLength(1); compteur++)
                            {
                                mat9Pixels[indexLigneMat9Pixels, compteur] = matImage[indexLigneMatImage + indexLigneMat9Pixels, indexColonneMatImage + compteur].Pixels[indexColonnePixel];
                                somme += mat9Pixels[indexLigneMat9Pixels, compteur] * kernel[indexLigneMat9Pixels, compteur];
                                //sommeKernel += (kernel[indexLigneMat9Pixels, compteur]);

                                if (kernel[indexLigneMat9Pixels, compteur] > 0)
                                {
                                    sommeKernelPositif += kernel[indexLigneMat9Pixels, compteur];
                                }
                                else
                                {
                                    sommeKernelNegatif += kernel[indexLigneMat9Pixels, compteur];
                                }
                            }
                        }


                        if (sommeKernelPositif > Math.Abs(sommeKernelNegatif)) { sommeKernel = sommeKernelPositif; }
                        else { sommeKernel = Math.Abs(sommeKernelNegatif); }
                        if (sommeKernel == 0) { sommeKernel = 1; }
                        int nouvelOctet = Convert.ToInt32(Math.Abs(somme / sommeKernel));

                        matImageFinale[indexLigneMatImage + 1, indexColonneMatImage + 1].Pixels[indexColonnePixel] = nouvelOctet;

                        //AfficherMatriceInt(mat9Pixels);
                        //AfficherMatricePixels(matImageFinale);
                    }
                }
            }

            for (int indexLigne = 0; indexLigne < matImageFinale.GetLength(0); indexLigne += matImageFinale.GetLength(0) - 1)
            {
                for (int indexColonne = 0; indexColonne < matImageFinale.GetLength(1); indexColonne++)
                {
                    if (indexLigne == 0)
                    {
                        matImageFinale[0, indexColonne].Pixels[0] = matImageFinale[1, indexColonne].Pixels[0];
                        matImageFinale[0, indexColonne].Pixels[1] = matImageFinale[1, indexColonne].Pixels[1];
                        matImageFinale[0, indexColonne].Pixels[2] = matImageFinale[1, indexColonne].Pixels[2];
                    }
                    else if (indexLigne == matImageFinale.GetLength(0) - 1)
                    {
                        matImageFinale[matImageFinale.GetLength(0) - 1, indexColonne].Pixels[0] = matImageFinale[matImageFinale.GetLength(0) - 2, indexColonne].Pixels[0];
                        matImageFinale[matImageFinale.GetLength(0) - 1, indexColonne].Pixels[1] = matImageFinale[matImageFinale.GetLength(0) - 2, indexColonne].Pixels[1];
                        matImageFinale[matImageFinale.GetLength(0) - 1, indexColonne].Pixels[2] = matImageFinale[matImageFinale.GetLength(0) - 2, indexColonne].Pixels[2];
                    }
                }
            }

            for (int indexColonne = 0; indexColonne < matImageFinale.GetLength(1); indexColonne += matImageFinale.GetLength(1) - 1)
            {
                for (int indexLigne = 0; indexLigne < matImageFinale.GetLength(0); indexLigne++)
                {
                    if (indexColonne == 0)
                    {
                        matImageFinale[indexLigne, 0].Pixels[0] = matImageFinale[indexLigne, 1].Pixels[0];
                        matImageFinale[indexLigne, 0].Pixels[1] = matImageFinale[indexLigne, 1].Pixels[1];
                        matImageFinale[indexLigne, 0].Pixels[2] = matImageFinale[indexLigne, 1].Pixels[2];
                    }
                    else if (indexColonne == matImageFinale.GetLength(1) - 1)
                    {
                        matImageFinale[indexLigne, matImageFinale.GetLength(1) - 1].Pixels[2] = matImageFinale[indexLigne, matImageFinale.GetLength(1) - 2].Pixels[2];
                        matImageFinale[indexLigne, matImageFinale.GetLength(1) - 1].Pixels[1] = matImageFinale[indexLigne, matImageFinale.GetLength(1) - 2].Pixels[1];
                        matImageFinale[indexLigne, matImageFinale.GetLength(1) - 1].Pixels[0] = matImageFinale[indexLigne, matImageFinale.GetLength(1) - 2].Pixels[0];
                    }
                }

            }
            //AfficherMatricePixels(matImageFinale);
            return matImageFinale;

        }

        public void Redimensionner(int choixRedimensionner)
        {
            Pixel[,] matNouvelleImage = null;
            if (choixRedimensionner == 'a')
            {
                matNouvelleImage = new Pixel[hauteurImage * 2, largeurImage * 2];
                for (int i = 0; i < matImage.GetLength(0); i++)
                {
                    for (int j = 0; j < matImage.GetLength(1); j++)
                    {
                        matNouvelleImage[1 + i * 2, j * 2] = matNouvelleImage[i * 2, 1 + j * 2] = matNouvelleImage[1 + i * 2, 1 + j * 2] = matNouvelleImage[i * 2, j * 2] = matImage[i, j];
                    }
                }
            }

            if (choixRedimensionner == 'r')
            {
                matNouvelleImage = new Pixel[hauteurImage / 2, largeurImage / 2];
                for (int i = 0; i < matNouvelleImage.GetLength(0); i++)
                {
                    for (int j = 0; j < matNouvelleImage.GetLength(1); j++)
                    {
                        matNouvelleImage[i, j] = matImage[i * 2, j * 2];
                    }
                }
            }

            matImage = matNouvelleImage;
        }


        public void Miroir(char charEntre)
        {
            Pixel[,] matImageMiroir = new Pixel[hauteurImage, largeurImage];

            if (charEntre == 'h')
            {
                for (int i = 0; i < matImage.GetLength(0); i++)
                {
                    for (int j = 0; j < matImage.GetLength(1); j++)
                    {
                        matImageMiroir[i, j] = matImage[matImage.GetLength(0) - i - 1, j];
                    }
                }
            }
            if (charEntre == 'v')  // miroir vertical
            {
                for (int i = 0; i < matImage.GetLength(0); i++)
                {
                    for (int j = 0; j < matImage.GetLength(1); j++)
                    {
                        matImageMiroir[i, j] = matImage[i, matImage.GetLength(1) - j - 1];
                    }
                }
            }
            matImage = matImageMiroir;
        }


        public void Flip(double degre)
        {
            double a = 0;
            double b = 0;
            if (((degre >= 0) && (degre < 90)) || ((degre > 270) && (degre <= 360)))
            {
                a = Largeur * Math.Cos(degre * (Math.PI / 180)) + Hauteur * Math.Sin(degre * (Math.PI / 180));
                b = Largeur * Math.Sin(degre * (Math.PI / 180)) + Hauteur * Math.Cos(degre * (Math.PI / 180));
            }
            else if ((degre == 90) || (degre == 270))
            {
                a = Largeur;
                b = Hauteur;
            }
            else
            {
                a = Hauteur * Math.Cos((degre - 90) * (Math.PI / 180)) + Largeur * Math.Sin((degre - 90) * (Math.PI / 180));
                b = Hauteur * Math.Sin((degre - 90) * (Math.PI / 180)) + Largeur * Math.Cos((degre - 90) * (Math.PI / 180));
            }
            Pixel[,] newMatric = new Pixel[Convert.ToInt32(Math.Truncate(a)), Convert.ToInt32(Math.Truncate(b))];
            double x = 0;
            double y = 0;
            double x0 = newMatric.GetLength(0) / 2;
            double y0 = newMatric.GetLength(1) / 2;
            for (int i = 0; i < matImage.GetLength(0); i++)
            {
                for (int j = 0; j < matImage.GetLength(1); j++)
                {
                    x = (Math.Cos(Math.PI * (degre / 180)) * (i - x0)) - (Math.Sin(Math.PI * (degre / 180)) * (j - y0)) + x0;
                    y = (Math.Sin(Math.PI * (degre / 180)) * (i - x0)) + (Math.Cos(Math.PI * (degre / 180)) * (j - y0)) + y0;
                    if ((x >= 0) && (x < newMatric.GetLength(0)) && (y >= 0) && (y < newMatric.GetLength(1)))
                    {
                        newMatric[Convert.ToInt32(Math.Truncate(x)), Convert.ToInt32(Math.Truncate(y))] = matImage[i, j];
                    }
                }
            }
            matImage = newMatric;
        }


        public void Mandelbrot()
        {
            int[] tab0 = new int[3];
            Pixel couleur = new Pixel(tab0);
            int iteration = 0;
            int[] tab = { 0, 0, 0 };
            Pixel x = new Pixel(tab);
            Complexes z = new Complexes(0, 0);
            for (int i = 0; i < Hauteur ; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {
                    double a = (double)(i - (Largeur / 2)) / (double)(Largeur / 4);
                    double b = (double)(j - (Hauteur / 2)) / (double)(Hauteur / 4);
                    Complexes c = new Complexes(a, b);
                    z.r = 0;
                    z.i = 0;
                    iteration = 0;
                    do
                    {
                        iteration++;
                        z.Square();


                        z.Add(c);
                        if (z.Module() > 2.0) break;
                    } while (iteration < 1000);

                    if (iteration == 1000)
                    {
                        matImage[i, j] = x;
                    }
                    else
                    {
                        //tab0[1] = iteration * 255 / 1000;
                        tab0 = EscapeTimeCouleur(iteration);

                        Pixel y = new Pixel(tab0);

                        matImage[i, j] = y;
                    }
                }
            }
        }

        public int[] EscapeTimeCouleur(int n)
        {
            double x = Math.Log(n + 1);

            double b = (1 / (10 * Math.Sqrt(0.1)));


            b = b * (1 / Math.Log(2));

            int BW = Convert.ToInt32(255 - (255 * ((1 + Math.Cos(2 * b * x)) / 2)));
            int[] tab = { 0, BW, BW };
            return tab;

        }

        public void Julia(double u, double n)
        {
            int[] tab0 = new int[3];
            Pixel couleur = new Pixel(tab0);
            int iteration = 0;
            int[] tab = { 0, 0, 0 };
            Pixel x = new Pixel(tab);
            Complexes c = new Complexes(u, n);
            for (int i = 0; i < Largeur; i++)
            {
                for (int j = 0; j < Hauteur; j++)
                {
                    double a = (double)(i - (Largeur / 2)) / (double)(Largeur / 4);
                    double b = (double)(j - (Hauteur / 2)) / (double)(Hauteur / 4);
                    Complexes z = new Complexes(a, b);
                    iteration = 0;
                    do
                    {
                        iteration++;
                        z.Square();
                        //z.Square();
                        //z.Square();
                        z.Add(c);
                        if (z.Module() > 2.0) break;
                    } while (iteration < 50);

                    if (iteration == 50)
                    {
                        matImage[i, j] = x;
                    }
                    else
                    {
                        //tab0[1] = iteration * 255 / 1000;
                        tab0 = EscapeTimeCouleur(iteration);

                        Pixel y = new Pixel(tab0);

                        matImage[i, j] = y;
                    }
                }
            }
        }

        /*public void TestFractale()
        {

            int nbrIteration;
            int nbrIterationMax = 50;
            double x1 = -2.1;
            double x2 = 0.6;
            double y1 = -1.2;
            double y2 = 1.2;
            int zoom = 1000;

            int[] tab0 = { 0, 0, 0 };
            Complexes z = new Complexes(0, 0);
            Complexes c = new Complexes(0, 0);
            Pixel x = new Pixel(tab0);
            int[] tab1 = { 255, 255, 255 };

            matImage = new Pixel[(int)(y2 - y1) * zoom, (int)(x2 - x1) * zoom];

            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {
                    c.r = (double)j / zoom + x1;
                    c.i = (double)i / zoom + y1;
                    z.r = 0;
                    z.i = 0;
                    nbrIteration = 0;

                    do
                    {
                        z.Calcul(c);
                        nbrIteration++;
                        if (z.Module() >= 2.0) break;
                    }
                    while (nbrIteration < nbrIterationMax);

                    if (nbrIteration == nbrIterationMax)
                    {
                        //int[] tab0 = EscapeTimeCouleur(nbrIteration);


                        matImage[i, j] = x;
                    }
                    else
                    {

                        //int[] tab1 = { 0, 0, nbrIteration * 255 / nbrIterationMax };
                        matImage[i, j] = new Pixel(tab1);
                    }
                }
            }
        }
        */
        public void MatHistogrammeLuminosite()
        {
            int[] tabHistogramme = new int[256];

            for (int i = 0; i < matImage.GetLength(0); i++)
            {
                for (int j = 0; j < matImage.GetLength(1); j++)
                {
                    for (int o = 0; o < 3; o++)
                    {
                        tabHistogramme[matImage[i, j].Pixels[o]]++;
                        //pour chaque octet on incrémente l'indice qui lui est attribué dans le tableau tabHistogramme (on compte la répartition des octets)
                    }
                }
            }

            int indiceMax = 0;
            for (int i = 0; i < tabHistogramme.Length; i++)
            {
                if (tabHistogramme[i] > indiceMax) indiceMax = tabHistogramme[i];
                //l'histogramme prendra comme hauteur la plus grande des valeurs du tableau
            }
            int coeffDecalage = 1;
            if (indiceMax / 256 >= 1) { coeffDecalage = Convert.ToInt32((indiceMax / 256) * 1.5); }

            Pixel[,] matHistogramme = new Pixel[indiceMax, 256 * coeffDecalage]; //nous multiplions la taille en largeur de l'histogramme par trois pour une meilleur visibilité
            int indiceColonneMatHistogramme = 0;
            //nous devons séparer l'indice de colonne de la matrice et celui de la colonne du tableau car ces deux indices sont différents 
            //en raison du décalage de 3 que nous appliquons
            int[] tabPixelBlanc = { 255, 255, 255 };
            Pixel pixelBlanc = new Pixel(tabPixelBlanc);
            int[] tabPixelNoir = { 0, 0, 0 };
            Pixel pixelNoir = new Pixel(tabPixelNoir);

            for (int indiceTab = 0; indiceTab < 256; indiceTab++)
            {
                for (int indiceLigneMatHistogramme = 0; indiceLigneMatHistogramme < matHistogramme.GetLength(0); indiceLigneMatHistogramme++)
                {
                    if (indiceLigneMatHistogramme < tabHistogramme[indiceTab])
                    {
                        for (int numPixel = 0; numPixel < coeffDecalage; numPixel++)
                        {
                            matHistogramme[indiceLigneMatHistogramme, indiceColonneMatHistogramme + numPixel] = pixelBlanc;
                            //si l'indice j est inférieur à la hauteur de la colonne de l'octet numéro i, onplace un pixel blanc dans l'histogramme

                        }
                    }
                    else
                    {
                        for (int numPixel = 0; numPixel < coeffDecalage; numPixel++)
                        {
                            matHistogramme[indiceLigneMatHistogramme, indiceColonneMatHistogramme + numPixel] = pixelNoir;
                        }
                       
                    }
                }
                indiceColonneMatHistogramme += coeffDecalage;

            }
            matImage = matHistogramme;
        }

        public void MatHistogrammeCouleurs()
        {
            int[] tabHistogrammeRouge = new int[256];
            int[] tabHistogrammeVert = new int[256];
            int[] tabHistogrammeBleu = new int[256];

            for (int i = 0; i < matImage.GetLength(0); i++)
            {
                for (int j = 0; j < matImage.GetLength(1); j++)
                {
                    tabHistogrammeRouge[matImage[i, j].Pixels[0]]++;
                    tabHistogrammeVert[matImage[i, j].Pixels[1]]++;
                    tabHistogrammeBleu[matImage[i, j].Pixels[2]]++;
                }
            }

            int indiceMax = 0;
            for (int i = 0; i < 256; i++)
            {
                if (tabHistogrammeRouge[i] > indiceMax) indiceMax = tabHistogrammeRouge[i];
                if (tabHistogrammeVert[i] > indiceMax) indiceMax = tabHistogrammeVert[i];
                if (tabHistogrammeBleu[i] > indiceMax) indiceMax = tabHistogrammeBleu[i];
                //l'histogramme prendra comme hauteur la plus grande des valeurs des tableaux
            }
            int coeffDecalage = 1;
            if (indiceMax * 1.5 / 256 >= 1) { coeffDecalage = Convert.ToInt32((indiceMax / 256)*1.5); }

            Pixel[,] matHistogramme = new Pixel[indiceMax, 256 * coeffDecalage]; //nous multiplions la taille en largeur de l'histogramme par trois pour une meilleur visibilité
            int indiceColonneMatHistogramme = 0;
            //nous devons séparer l'indice de colonne de la matrice et celui de la colonne du tableau car ces deux indices sont différents 
            //en raison du décalage que nous appliquons

            int[] tabPixelNoir = { 0,0,0 };

            for (int i = 0; i < matHistogramme.GetLength(0); i++)
            {
                for (int j = 0; j < matHistogramme.GetLength(1); j++)
                {
                    matHistogramme[i, j] = new Pixel(tabPixelNoir);
                }
            }

            for (int indiceTab = 0; indiceTab < 256; indiceTab++)
            {
                for (int indiceLigneMatHistogramme = 0; indiceLigneMatHistogramme < matHistogramme.GetLength(0); indiceLigneMatHistogramme++)
                {
                    if (indiceLigneMatHistogramme < tabHistogrammeRouge[indiceTab])
                    {
                        for (int numPixel = 0; numPixel < coeffDecalage; numPixel++)
                        {
                            matHistogramme[indiceLigneMatHistogramme, indiceColonneMatHistogramme + numPixel].Pixels[0] = 255;
                            //si l'indice indiceLigneMatHistogramme est inférieur à la hauteur de la colonne de l'octet numéro i, on change l'octet "rouge" dans l'histogramme

                        }
                    }
                    if (indiceLigneMatHistogramme < tabHistogrammeVert[indiceTab])
                    {
                        for (int numPixel = 0; numPixel < coeffDecalage; numPixel++)
                        {
                            matHistogramme[indiceLigneMatHistogramme, indiceColonneMatHistogramme + numPixel].Pixels[1] = 255;
                            //si l'indice indiceLigneMatHistogramme est inférieur à la hauteur de la colonne de l'octet numéro i, on change l'octet "vert" dans l'histogramme

                        }
                    }
                    if (indiceLigneMatHistogramme < tabHistogrammeBleu[indiceTab])
                    {
                        for (int numPixel = 0; numPixel < coeffDecalage; numPixel++)
                        {
                            matHistogramme[indiceLigneMatHistogramme, indiceColonneMatHistogramme + numPixel].Pixels[2] = 255;
                            //si l'indice indiceLigneMatHistogramme est inférieur à la hauteur de la colonne de l'octet numéro i, on change l'octet "bleu" dans l'histogramme

                        }
                    }
                }
                indiceColonneMatHistogramme += coeffDecalage;

            }
            matImage = matHistogramme;
        }

        public void FusionImages(Pixel[,] matImage1, Pixel[,] matImage2)
        {
            //if (matImage1.GetLength(0) == matImage2.GetLength(0) && matImage1.GetLength(1) == matImage2.GetLength(1))
            {
                for (int i = 0; i < matImage1.GetLength(0); i++)
                {
                    for (int j = 0; j < matImage1.GetLength(1); j++)
                    {
                        for (int o = 0; o < 3; o++)
                        {
                            matImage1[i, j].Pixels[o] = (matImage1[i, j].Pixels[o] + matImage2[i, j].Pixels[o]) / 2;
                        }
                    }
                }
            }
            matImage = matImage1;
        }
    }
}
