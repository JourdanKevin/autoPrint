using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Printing;
using System.Text.RegularExpressions;



namespace AutoPrint
{
    public partial class MainWindow : Window
    {
        private List<String> exten = new List<String>();
        private Boolean FilePathValid = false; 
        public MainWindow()
        {
            InitializeComponent();
            PrinterSettings ps = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                PrinterChoose.Items.Add(printer);
            }
            PrinterChoose.SelectedItem = ps.PrinterName;
        }

    private void Imprimer(object sender, RoutedEventArgs e)
        {
            if (chemin.Text.ToString() != "") {
                if (FilePathValid)
                {
                    string[] fileEntries = Directory.GetFiles(chemin.Text.ToString(), "*" + extension.SelectedItem);
                    foreach (String filePath in fileEntries)
                    {
                        for (int i = 0; i < int.Parse(Copi.Text); i++)
                        {
                            try
                            {
                                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(filePath);
                                info.Arguments = "\"" + PrinterChoose.SelectedItem + "\"";
                                info.CreateNoWindow = true;
                                info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                info.UseShellExecute = true;
                                info.Verb = "PrintTo";
                                System.Diagnostics.Process.Start(info);
                                System.Threading.Thread.Sleep(5000);
                            }
                            catch { MessageBox.Show("L'extension :" + System.IO.Path.GetExtension(filePath) + " ne peut être imprimer. Installer un logiciel permettant de lire cette extension."); }
                        }
                    }
                }
                else { MessageBox.Show("Le chemin entrer n'est pas un chemin valide"); }
            }
            else { MessageBox.Show("Veuillez indiquer un chemin de dossier a l'aide du bouton avec les trois petits point"); }                  
        }

        private void PreviewNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void CopiUp(object sender, RoutedEventArgs e)
         {
             int copiNumber = int.Parse(Copi.Text);
             copiNumber++;
             Copi.Text = copiNumber.ToString();
         }
         private void CopiDown(object sender, RoutedEventArgs e)
         {
             int copiNumber = int.Parse(Copi.Text);
            if (copiNumber > 1)
            {
                copiNumber--;
                Copi.Text = copiNumber.ToString();
            }
         }
        private void ChooseFolder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                chemin.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }
        private void TextChanged_chemin(object sender, TextChangedEventArgs e)
        {
            extension.Items.Clear();
            extension.Items.Add("");
            this.FilePathValid = false;
            try {
                string[] fileEntries = Directory.GetFiles(chemin.Text.ToString());
                this.exten.Clear();
                foreach (String file in fileEntries)
                {
                    if (!exten.Contains(System.IO.Path.GetExtension(file)))
                        this.exten.Add(System.IO.Path.GetExtension(file));
                }
                foreach (String ext in this.exten)
                {
                    extension.Items.Add(ext);
                }
                this.FilePathValid = true;
            }
            catch { }          
        }
    }
}