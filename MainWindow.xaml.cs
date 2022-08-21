using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace InDevLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string gamePath { get; set; }
        string pathInDev = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\AppData\Roaming\IndevLauncher\";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainWindow()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();

            gamename.Content = "Select a game";
            IMG_INDEV.Visibility = Visibility.Visible;

            btn_to_path_exe.Visibility = Visibility.Hidden;
            btn_to_path_exe.IsEnabled = false;

            HideAddGameForm();

            ReadGameAtStart();


        }

        public void ReadGameAtStart()
        {
            try
            {
                string[] lines = File.ReadAllLines($@"{pathInDev}GameData.txt");
                foreach (string s in lines)
                {
                    if (s.StartsWith("- "))
                    {
                        string[] gamedata_path = s.Split("|");
                        combo_b_game.Items.Add(gamedata_path[0]);
                    }
                }
            }
            catch
            {
                Directory.CreateDirectory(pathInDev);
                File.WriteAllText($@"{pathInDev}GameData.txt", $"");
                MessageBox.Show("Création du fichier GameData...");
            }
        }


        private void Wiki_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://localhost/wiki/wiki.html")
            {
                UseShellExecute = true
            });
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(gamePath);
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show($"Le Chemin vers l'executable n'est pas correct :\n - Vous pouvez le Modifier dans le dossier :\n\nC:/Users/{Environment.UserName}/AppData/Roaming/IndevLauncher/GameData.txt\n\n - Puis redermarer le launcher en attente de Mise a jour sur le sujet...\nOuvrir l'explorer a cette endroit ?", "ExePath invalide", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string unevar = $@"{pathInDev}";
                        Process.Start("explorer.exe", string.Format("/select,\"{0}\"", unevar));
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            }

        }


        public void HideAddGameForm()
        {
            NomDuJeu.Visibility = Visibility.Hidden;
            PathToExe.Visibility = Visibility.Hidden;
            GAME_NAME_FIELD.Visibility = Visibility.Hidden;
            PATH_TO_EXE_FIELD.Visibility = Visibility.Hidden;
            //btn_to_path_exe.Visibility = Visibility.Hidden;
            Btn_save.Visibility = Visibility.Hidden;

        }

        public void ShowAddGameForm()
        {
            NomDuJeu.Visibility = Visibility.Visible;
            PathToExe.Visibility = Visibility.Visible;
            GAME_NAME_FIELD.Visibility = Visibility.Visible;
            PATH_TO_EXE_FIELD.Visibility = Visibility.Visible;
            //btn_to_path_exe.Visibility = Visibility.Visible;
            Btn_save.Visibility = Visibility.Visible;

        }

        private void Btn_Add_Game_Click(object sender, RoutedEventArgs e)
        {
            GAME_NAME_FIELD.Clear();
            PATH_TO_EXE_FIELD.Clear();
            ShowAddGameForm();
        }


        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            string GameName = GAME_NAME_FIELD.Text;
            string GamePath = PATH_TO_EXE_FIELD.Text;

            HideAddGameForm();

            string gameData = $@"{pathInDev}GameData.txt";
            File.AppendAllText(gameData, $"- {GameName} -|{GamePath}\n=--------=\n");

            combo_b_game.Items.Add($"- {GameName} -");

        }

        public ColorConvertedBitmapExtension stringToImage(string inputString)
        {
            byte[] imageBytes = Encoding.Unicode.GetBytes(inputString);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return new ColorConvertedBitmapExtension(ms);
            }
        }

        private void combo_b_game_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object selectedItem = combo_b_game.SelectedItem;

            string[] lines = File.ReadAllLines($@"{pathInDev}GameData.txt"); //ya une erreur quand on supprime un element dans GameData.txt (soit refresh la page chaque seconde / soit fermer le launcher et le redemarer)
            foreach (string s in lines)
            {
                if (s.StartsWith(Convert.ToChar(selectedItem)))
                {
                    string[] gamedata_path = s.Split("|");
                    path_to_exe.Content = "Exe Path : " + gamedata_path[1];
                    gamename.Content = gamedata_path[0];
                    gamePath = gamedata_path[1];


                }
            }
        }

        private void btn_to_path_exe_Click(object sender, RoutedEventArgs e)
        {
            //enregistrement de l'image
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.jpg;*.jpeg:*.png";
            dialog.InitialDirectory = $@"{pathInDev}";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == true)
            {

                //gameLogo.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }

        private void Btn_Edit_Game_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
