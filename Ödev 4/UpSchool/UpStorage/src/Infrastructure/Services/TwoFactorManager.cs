using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Microsoft.AspNetCore.Http.Features;
using OtpNet;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TwoFactorManager : ITwoFactorService
    {
        private const string SecretKey = "VerySecretPassword123";
        public TwoFactorGeneratedDto Generate(string email)
        {
            var key = Encoding.UTF8.GetBytes(SecretKey);
            var uri= new OtpUri(OtpType.Totp, key, email, "Upstorage");
            var qrCodeGenerator = new QRCodeGenerator();
            var qrData=qrCodeGenerator.CreateQrCode(uri.ToString(), QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qRCode=new BitmapByteQRCode(qrData);
            var twoFactorDto=new TwoFactorGeneratedDto();
            twoFactorDto.QrCodeImage=qRCode.GetGraphic(10);
            twoFactorDto.Key=Base32Encoding.ToString(key);
            return twoFactorDto;
            

        }

        public bool Validate(string userCode)
        {
            var key =Encoding.UTF8.GetBytes(SecretKey);
            var totp = new Totp(key);
            //long.TryParse(userCode, out long parsedCode);
            return totp.VerifyTotp(userCode, out _);
        }
    }
}
