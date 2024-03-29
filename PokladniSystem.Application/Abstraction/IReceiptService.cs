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
        byte[] GeneratePDF(int rowCount);
        Task<string> SavePDF(byte[] pdfBytes, string fileName, string folderName);
    }
}
