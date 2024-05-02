using Microsoft.AspNetCore.Http;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Infrastructure.Identity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Abstraction
{
    public interface IReceiptService
    {
        byte[] GenerateReceiptPDF(int rowCount);
        string GetReceiptPath(int orderId);
        Task<string> SaveReceiptPDFAsync(byte[] pdfBytes, string fileName, string folderName);
        Task<string> GenerateReceiptAsync(int orderId);
    }
}
