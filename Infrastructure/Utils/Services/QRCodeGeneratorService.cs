// <copyright file="QRCodeGeneratorService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Utils.Services
{
    using System;
    using EventManagementAPI.Core.Application.Contracts.Utilities;
    using QRCoder;

    public class QRCodeGeneratorService : IQRCodeGeneratorSerivice
    {
        public byte[] GenerateQrCodeImageArr(string content, int pixelsPerModule = 20)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be null or empty.", nameof(content));
            }

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);
            using var qrCode = new PngByteQRCode(qrCodeData);

            byte[] qrCodeBytes = qrCode.GetGraphic(pixelsPerModule);
            return qrCodeBytes;
        }
    }
}
