﻿using System;
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
        private readonly IFileHelper _fileHelper;

        protected QRCodeItem()
        {
        }

        public QRCodeItem(String data)
        {
            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                _fileHelper = AppContainer.Container.Resolve<IFileHelper>();
            }

            Initialize(data);
        }

        public QRCodeItem(IFileHelper fileHelper, String data)
        {
            _fileHelper = fileHelper;

            Initialize(data);
        }

        private void Initialize(string data)
        {
            var sharedDirectoryPath = _fileHelper.SharedDirectoryPath;

            if (data == null) throw new Exception(nameof(data) + " cannot be null.");

            Data = data.Length > 2083 ? data.Substring(0, 2083) : data;
            Path = data == "" ? "" : System.IO.Path.Combine(sharedDirectoryPath, HashedFileName);
            CreateDate = DateTime.UtcNow;
        }

        [Key]
        public string Data { get; protected set; }

        public string Path { get; protected set; }

        public DateTime CreateDate { get; protected set; }

        [NotMapped]
        private byte[] ByteArray { get; set; }

        [NotMapped]
        public string HashedFileName => System.IO.Path.ChangeExtension(ComputeHashString(), ".jpg");

        private bool? _isURL;

        [NotMapped]
        public bool IsURL
        {
            get
            {
                if (_isURL == null)
                    _isURL = Uri.TryCreate(Data, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                return _isURL.Value;
            }
        }

        public byte[] GenerateQRCodeByteArray()
        {
            if (ByteArray != null) return ByteArray;

            byte[] qrCodeAsBitmapByteArr;

            using (var qrCodeGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrCodeGenerator.CreateQrCode(Data, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new BitmapByteQRCode(qrCodeData))
            {
                ByteArray = qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);
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