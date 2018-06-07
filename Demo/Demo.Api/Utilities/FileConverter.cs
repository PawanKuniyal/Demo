using System;
using System.Drawing;
using System.IO;

namespace Helpa.Api.Utilities
{
    public static class FileConverter
    {
        public static Image ConvertByteArrayToFile(string byteArray)
        {
            using (MemoryStream mStream = new MemoryStream(Convert.FromBase64String(byteArray)))
            {
                return Image.FromStream(mStream);
            }
        }

        public static string UploadFileToFileFolder(string base64String, string targetFolder)
        {            
            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64String);
                string fileExtension = GetFileExtension(base64String);
                string fileName = "profile-image-" + Guid.NewGuid() + "." + fileExtension;
                string filePath = string.Empty;
                string file = string.Empty;
                filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/") + targetFolder;
                file = filePath + "\\" + fileName;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);                    
                }

                if (!File.Exists(file))
                {
                    FileStream stream = new FileStream(file, FileMode.Create);
                    stream.Write(fileBytes, 0, fileBytes.Length);
                    stream.Close();
                }

                return file;
            }
            catch(Exception ex)
            {
                throw new Exception("Base 64 format is wrong. " + ex.Message);
            }
        }

        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }
    }
}