// <copyright file="IQRCodeGeneratorSerivice.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Utilities
{
    public interface IQRCodeGeneratorSerivice
    {
        public byte[] GenerateQrCodeImageArr(string content, int pixelsPerModule = 20);
    }
}
