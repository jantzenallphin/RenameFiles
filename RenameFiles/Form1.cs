using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace RenameFiles
{
    public partial class Form1 : Form
    {
        public static int count = 0;

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
            var directories = ImageMetadataReader.ReadMetadata(path);
            var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            
            if (subIfdDirectory != null)
            {
                // Try Date/Time Original first (tag 0x9003)
                if (subIfdDirectory.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime dateTimeOriginal))
                {
                    return dateTimeOriginal;
                }
                
                // Fall back to Date/Time Digitized (tag 0x9004)
                if (subIfdDirectory.TryGetDateTime(ExifDirectoryBase.TagDateTimeDigitized, out DateTime dateTimeDigitized))
                {
                    return dateTimeDigitized;
                }
                
                // Fall back to Date/Time (tag 0x0132)
                if (subIfdDirectory.TryGetDateTime(ExifDirectoryBase.TagDateTime, out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            
            // If no date found, throw exception to trigger fallback filename
            throw new Exception("No date metadata found in image");
        }


    }
}
