using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using Autofac;
using QRCoder;
using ShareQR.Helpers;

namespace ShareQR.Models
{
    public class QRCodeItem
    {
        protected QRCodeItem()
        {
        }

        public QRCodeItem(String data)
        {
            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                var fileHelper = AppContainer.Container.Resolve<IFileHelper>();

                Initialize(fileHelper, data);
            }
        }

        public QRCodeItem(IFileHelper fileHelper, String data)
        {
            Initialize(fileHelper, data);
        }

        private void Initialize(IFileHelper fileHelper, string data)
        {
            var sharedDirectoryPath = fileHelper.SharedDirectoryPath;

            Data = data ?? throw new Exception(nameof(data) + " cannot be null.");
            Path = System.IO.Path.Combine(sharedDirectoryPath, HashedFileName);
            CreateDate = DateTime.UtcNow;
        }

        [Key]
		public string Data { get; protected set; }

        public string Path { get; protected set; }

        public DateTime CreateDate { get; protected set; }

        [NotMapped]
        public string HashedFileName
        {
            get { return System.IO.Path.ChangeExtension(ComputeHashString(), ".jpg"); }
        }

        public byte[] GenerateQRCodeByteArray()
        {
            byte[] qrCodeAsBitmapByteArr;

            using (QRCodeGenerator qrCodeGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(Data, QRCodeGenerator.ECCLevel.Q))
            using (BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData))
            {
                qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);
            }

            return qrCodeAsBitmapByteArr;
        }

        private string ComputeHashString()
        {
            byte[] computedHash;

            using (var algorithm = SHA256.Create())
            {
                computedHash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(Data));
            }

            return BitConverter.ToString(computedHash);
        }
    }
}