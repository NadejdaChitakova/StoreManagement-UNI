using System.Drawing;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class FileService : IFileService
    {
        public FileService()
        {
        }
        public async void WriteOnFile(string text)
        {
            string fileName = DateTime.Now.Day.ToString();
            string fileType = ".txt";
            string path = "C:\\Users\\nadej\\OneDrive\\Работен плот\\VSFiles\\" + fileName + fileType;

            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                {

                }
            };
            await File.WriteAllTextAsync(path, text);
        }
    }
}
