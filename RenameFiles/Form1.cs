using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RenameFiles
{
    public partial class Form1 : Form
    {
        public static int count = 0;
        private static Regex r = new Regex(":");

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Text = "Renaming Files...";
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    foreach (string file in files)
                        ProcessFile(file, fbd.SelectedPath);

                    this.button1.Text = "All Done! Click to select another folder.";
                    return;
                }
                else
                {
                    this.button1.Text = "Select Folder";
                    return;
                }
            }

        }

        static void ProcessFile(string file, string path)
        {
            //DateTime testing = File.GetCreationTime(file);
            //var date = testing.ToShortDateString();

            string extension = Path.GetExtension(file);
            string newfilename = "no-date-taken-";

            try
            {
                DateTime dateTaken = GetDateTakenFromImage(file);
                newfilename = dateTaken.ToString("MM-dd-yy") + $"-{count}{extension}";
            }
            // No date taken found
            catch (Exception e)
            {
                newfilename += count + extension;
            }
            finally
            {
                try
                {
                    File.Move(file, path + $"\\{newfilename}");
                }
                // file name already exists, try again with new count
                catch (Exception e)
                {
                    count++;
                    ProcessFile(file, path);
                }
                finally
                {
                    count++;
                }
            }



        }


        //retrieves the datetime WITHOUT loading the whole image
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }


    }
}
