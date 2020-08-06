using System.Windows;
using Microsoft.Win32;

namespace Client.Services.DialogService
{
    public class SelectImageDialogService : IDialogService
    {  
        public string FilePath { get; set; }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public bool OpenFileDialog()
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "Select a png image",
                Filter = "Image |*.png",
                Multiselect = false
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }
    }
}