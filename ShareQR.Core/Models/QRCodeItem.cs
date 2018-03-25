using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Security.Cryptography;
using QRCoder;
using Xamarin.Forms;

namespace ShareQR.Models
{
    public class QRCodeItem
    {
        public QRCodeItem(String data) : this(DependencyService.Get<IFileHelper>(), data)
        {
        }

        public QRCodeItem(IFileHelper fileHelper, String data)
        {
            var sharedDirectoryPath = fileHelper.SharedDirectoryPath;

            Data = data ?? throw new Exception(nameof(data) + " cannot be null.");
            CreateDate = DateTime.UtcNow;
            Path = System.IO.Path.Combine(sharedDirectoryPath, HashedFileName);
        }

        protected QRCodeItem()
        {         
        }

        [Key]
        public string Data { get; protected set; }
        public string Path { get; protected set; }
        public DateTime CreateDate { get; protected set; }

        [NotMapped]
        public string HashedFileName
        {
            get
            {
                return System.IO.Path.ChangeExtension(ComputeHashString(), ".jpg");
            }
        }

        public byte[] GenerateQRCodeByteArray()
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(Data, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);

            return qrCodeAsBitmapByteArr;
        }

        private string ComputeHashString()
        {
            byte[] computedHash;

            using (var algorithm = SHA256.Create())
            {
                computedHash = algorithm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(Data));
            }

            return BitConverter.ToString(computedHash);
        }
    }
}
