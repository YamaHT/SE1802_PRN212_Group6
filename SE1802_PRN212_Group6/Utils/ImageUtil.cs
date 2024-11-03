using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace SE1802_PRN212_Group6.Utils
{
    public static class ImageUtil
    {
        public static OpenFileDialog? GetImageDialog()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
            };
            return openFileDialog.ShowDialog() == true ? openFileDialog : null;
        }

        public static string? AddImage(string entity, OpenFileDialog? fileDialog)
        {
            if (fileDialog == null)
            {
                return null;
            }

            BitmapImage bitmapImage = new(new Uri(fileDialog.FileName));

            JpegBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            string rootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            string directoryPath = Path.Combine(rootPath, "Images", entity);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = $"{Guid.NewGuid()}-{Path.GetFileNameWithoutExtension(fileDialog.SafeFileName)}.jpg";

            if (fileName.Length < 255)
            {
                string path = Path.Combine(directoryPath, fileName);

                using var fileStream = new FileStream(path, FileMode.Create);
                encoder.Save(fileStream);
            }

            return fileName;
        }

        public static void DeleteImage(string entity, string? imageName)
        {
            Task.Run(() =>
            {
                string rootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                string directoryPath = Path.Combine(rootPath, "Images", entity);

                if (!string.IsNullOrEmpty(imageName))
                {
                    var filePath = Path.Combine(directoryPath, imageName);

                    System.Threading.Thread.Sleep(500);
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch (IOException)
                        {
                        }
                    }
                }
            });
        }

        public static string? UpdateImage(string entity, string? oldImageName, OpenFileDialog? fileDialog)
        {
            if (fileDialog == null)
            {
                return oldImageName;
            }

            DeleteImage(entity, oldImageName);

            var newFileName = AddImage(entity, fileDialog);

            return newFileName;
        }
    }
}
