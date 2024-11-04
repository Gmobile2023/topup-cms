﻿using System;

namespace TW.CardMapping.Authorization.Users.Importing.Dto
{
    public class ImportCardDto
    {
        public string CardCode { get; set; }
        public string Serial { get; set; }
        public DateTime ExpiredDate { get; set; }
        public byte Status { get; set; }
        public DateTime? UsedDate { get; set; }
        public string ExportTransCode { get; set; }
        public decimal CardValue { get; set; }
        public string BatchCode { get; set; }
        public string StockType { get; set; }

        /// <summary>
        /// Can be set when reading data from excel or when importing user
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}