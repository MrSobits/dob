﻿using SMEV3Library.Helpers;
using System.Security.Cryptography.X509Certificates;

namespace SMEV3Library.Entities
{
    public class SMEVOptions
    {
        /// <summary>
        /// Адрес wcf точки
        /// </summary>
        public string Endpoint { get; set; } = "";

        /// <summary>
        /// Флаг работы в тестовом режиме
        /// </summary>
        public bool TestMode { get; set; } = false;

        /// <summary>
        /// Отпечаток сертификата
        /// </summary>
        public string Thumbprint
        {
            get
            {
                return CryptoProHelper.Thumbprint;
            }
            set
            {
                CryptoProHelper.Thumbprint = value;
            }
        }

        /// <summary>
        /// Отпечаток сертификата
        /// </summary>
        public string HeaderThumbprint
        {
            get
            {
                return CryptoProHelper.HeaderThumbprint;
            }
            set
            {
                CryptoProHelper.HeaderThumbprint = value;
            }
        }

        /// <summary>
        /// Отпечаток сертификата
        /// </summary>
        public string ThumbprintPers
        {
            get
            {
                return CryptoProHelper.ThumbprintPers;
            }
            set
            {
                CryptoProHelper.ThumbprintPers = value;
            }
        }

        /// <summary>
        /// Хранилище сертификата
        /// </summary>
        public string Storage
        {
            get
            {
                return CryptoProHelper.Storage;
            }
            set
            {
                CryptoProHelper.Storage = value;
            }
        }

        /// <summary>
        /// Место хранения сертификата
        /// </summary>
        public StoreLocation StoreLocation
        {
            get
            {
                return CryptoProHelper.StoreLocation;
            }
            set
            {
                CryptoProHelper.StoreLocation = value;
            }
        }
    }
}
