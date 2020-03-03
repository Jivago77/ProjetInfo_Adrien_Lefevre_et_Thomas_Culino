using System;
using System.Windows;

namespace Projet_info
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Le programme est en cours d'exécution");

            string fichier = "./Images/coco.bmp";

            MonImage monImage = new MonImage(fichier);
            

            ConsoleKeyInfo cki;
            do
            {
                Console.Clear();
                Console.WriteLine("\nMenu :\n\n"
                                 + "Sélectionnez le numeroEntreéro pour la commande désirée \n"
                                 + "0.Mandelbrot \n"
                                 + "1.Julia \n"
                                 + "2.NoirEtBlanc \n"
                                 + "3.Redimention\n"
                                 + "4.Filtrage \n"
                                 + "5.Rotation \n"
                                 + "6.Miroir \n"
                                 + "7.Histogramme de Luminosité \n"
                                 + "8.Histogramme de Luminosité selon Couleurs\n"
                                 + "9.Afficher les octets de l'image\n");


                int numeroEntre = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (numeroEntre)
                {
                    case 0:
                        monImage = new MonImage(10000, 10000);
                        monImage.Mandelbrot();
                        break;
                    case 1:
                        Console.WriteLine("Saisir la partie réelle > ");
                        double a = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("Saisir la partie imaginaire > ");
                        double b = Convert.ToDouble(Console.ReadLine());
                        monImage.Julia(a, b);
                        Console.WriteLine();
                        
                        break;

                    case 2:
                        monImage.MatImage = monImage.NoirEtBlanc();
                        break;

                    case 3:
                        char choixRedimentionner = 'b';
                        while (choixRedimentionner != 'a' && choixRedimentionner != 'r')
                        {
                            Console.Clear();
                            Console.WriteLine("Entrer a pour un agrandissement ou r pour un rétrécissement");
                            bool charValide = char.TryParse(Console.ReadLine(), out choixRedimentionner);
                            if (!charValide) choixRedimentionner = 'b';
                        }
                        Console.Clear();
                        monImage.Redimensionner(choixRedimentionner);
                        break;

                    case 4:
                        //double[,] kernel = new double[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
                        //double[,] kernel = new double[,] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
                        //double[,] kernel = new double[,] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };
                        double[,] kernel1 = new double[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
                        double[,] kernel2 = new double[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
                        //double[,] kernel = new double[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
                        //double[,] kernel = new double[,] { { 0, 1, 0 }, { 1, 4, 1 }, { 0, 1, 0 } };
                        //double[,] kernel = new double[,] { { 1, 0, -1 }, { 0, 0, 0 }, { -1, 0, 1 } };
                        //double[,] kernel = new double[,] { { 0.111111, 0.111111, 0.111111 }, { 0.111111, 0.111111, 0.111111 }, { 0.111111, 0.111111, 0.111111 } };
                        //double[,] kernel = new double[,] { { 1/16, 2 / 16, 1 / 16 }, { 2 / 16, 4 / 16, 2 / 16 }, { 1 / 16, 2 / 16, 1 / 16 } };
                        //double[,] kernel = new double[,] { { 1, 4, 6, 4, 1 }, { 4, 16, 24, 16, 4 }, { 6, 24, 36, 24, 6 }, { 4, 16, 24, 16, 4 }, { 1, 4, 6, 4, 1 } }; for (int i = 0; i < kernel.GetLength(0); i++) { for (int j = 0; j < kernel.GetLength(1); j++) { kernel[i, j] *= 1 / 256; } }

                        MonImage monImage1 = new MonImage(monImage.Hauteur,monImage.Largeur);
                        monImage1 = monImage;
                        monImage.MatImage = monImage.Filtrage(kernel1);
                        monImage1.MatImage = monImage1.Filtrage(kernel2);
                        monImage.FusionImages(monImage.MatImage, monImage1.MatImage);
                        break;

                    case 5:
                        Console.WriteLine("Saisir degre > ");
                        double degre = Convert.ToDouble(Console.ReadLine());
                        monImage.Flip(degre);
                        break;

                    case 6:
                        char charMiroir = 'a';
                        while (charMiroir != 'h' && charMiroir != 'v')
                        {
                            Console.Clear();
                            Console.WriteLine("Entrez h ou v pour miroir horizontal ou vertical");

                            bool charValide = char.TryParse(Console.ReadLine(), out charMiroir);
                            if (!charValide) charMiroir = 'a';
                        }
                        Console.Clear();
                        monImage.Miroir(charMiroir);
                        break;

                    case 7:
                        monImage.MatHistogrammeLuminosite();
                        break;

                    case 8:
                        monImage.MatHistogrammeCouleurs();
                        
                        break;
                    case 9:
                        monImage.AfficherImageAlEndroit(monImage);
                        break;
                    case 10:
                        string fichier1 = "./Images/Pito1.bmp";

                        MonImage monImage2 = new MonImage(fichier1);
                        monImage.FusionImages(monImage.MatImage, monImage2.MatImage);
                        break;

                }

                monImage.From_Image_To_File();
                //monImage.From_Image_To_File2();
                Console.WriteLine("\nAppuyez sur Echap pour quitter ou entrez une commande");
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);
        }

    }
}




