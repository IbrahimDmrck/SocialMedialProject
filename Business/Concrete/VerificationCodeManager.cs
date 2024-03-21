using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Concrete.Entityframework;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using System.Diagnostics;
using Entities.DTOs;

namespace Business.Concrete
{
    public class VerificationCodeManager : IVerificationCodeService
    {
        private readonly IVerificationCodeDal _verificationCodeDal;
        private readonly IUserDal _userDal;
        private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(3); // Doğrulama kodunun geçerlilik süresi
        public VerificationCodeManager(IVerificationCodeDal verificationCodeDal, IUserDal userDal)
        {
            _verificationCodeDal = verificationCodeDal;
            _userDal = userDal;
        }

        public IResult SendVerificationCode(VerificationCodeDto verificationCode)
        {
            string randomCode = GenerateRandomCode(12);

            var rulesResult = BusinessRules.Run(CheckIfUserIdExist(verificationCode.UserId), CheckIfEmailAvailable(verificationCode.Email), SendEmail(verificationCode.Email, randomCode));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            var verifyCode = new VerificationCode
            {
                UserId = verificationCode.UserId,
                Code = randomCode,
                CreationTime = DateTime.Now
            };

            _verificationCodeDal.Add(verifyCode);
            Task.Run(() => DeleteExpiredCodes());
            return new SuccessResult(Messages.SendVerifyCode);
        }


        public IResult CheckVerifyCode(VerificationCodeDto verificationCode)
        {
            var checkCode = _verificationCodeDal.Get(x=>x.UserId== verificationCode.UserId && x.Code== verificationCode.Code);

            if (checkCode == null)
            {
                return new ErrorResult(Messages.CodeNotFound);
            }
            return new SuccessResult(Messages.VerificationSuccessfull);
        }

        public IResult DeleteVerifyCode(int userId)
        {
            var deletedCode = _verificationCodeDal.GetAll(x => x.UserId == userId);

            if (deletedCode != null)
            {
                foreach (var item in deletedCode)
                {
                    _verificationCodeDal.Delete(item);
                }
            }
            return new SuccessResult(Messages.VerifyCodeDeleted);
        }

        private void DeleteExpiredCodes()
        {
            // Uygulama başlatıldığında bu işlemi sürekli çalıştırmak için bir mekanizma gerekebilir
            while (true)
            {
                // Tüm doğrulama kodlarını al
                var allCodes = _verificationCodeDal.GetAll();

                // Geçerlilik süresi dolmuş kodları filtrele
                var expiredCodes = allCodes.Where(code => (DateTime.Now - code.CreationTime) > _expirationTime).ToList();

                // Geçerlilik süresi dolmuş kodları sil
                foreach (var code in expiredCodes)
                {
                    _verificationCodeDal.Delete(code);
                }

                // Belirli bir süre bekleyerek işlemi tekrarla
                Thread.Sleep(TimeSpan.FromMinutes(1)); // Örneğin her dakika kontrol edebilirsiniz
            }
        }

        //Business Rules

        private IResult CheckIfUserIdExist(int userId)
        {
            var result = _userDal.GetAll(u => u.Id == userId).Any();
            if (!result)
            {
                return new ErrorResult(Messages.UserNotExist);
            }
            return new SuccessResult();
        }

        private IResult CheckIfEmailAvailable(string userEmail)
        {
            var result = BaseCheckIfEmailExist(userEmail);
            if (!result)
            {
                return new ErrorResult(Messages.UserEmailNotAvailable);
            }
            return new SuccessResult();
        }

        private bool BaseCheckIfEmailExist(string userEmail)
        {
            return _userDal.GetAll(u => u.Email == userEmail).Any();
        }

        private IResult SendEmail(string Email,string randomCode)
        {
            MimeMessage mimeMessage = new();
            MailboxAddress mailboxAddressFrom = new("Sosyal Medya Web Sitesi", "ibrahimdemircik1@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);
            MailboxAddress mailboxAddressTo = new("User", Email);
            mimeMessage.To.Add(mailboxAddressTo);

            BodyBuilder bodyBuilder = new();
            bodyBuilder.HtmlBody = $"<div><h3>Doğrulama Kodu : </h3> <div style='background-color:lightgray;" +
                                                $"width:100%;" +
                                                $"border:1px solid gray;" +
                                                $"border-radius:7px;" +
                                                $"text-align:center;" +
                                                $"font-size:1.5rem;" +
                                                $"font-weight:bolder;" +
                                                $"padding:15px;'>{randomCode}</div></div>";

            mimeMessage.Subject = "Doğrulama Kodu";
            mimeMessage.Body = bodyBuilder.ToMessageBody();



            using ( var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("ibrahimdemircik1@gmail.com", "qwltpfdiesmpgsmn");
                client.Send(mimeMessage);
                client.Disconnect(true);
            }


            return new SuccessResult(Messages.SendVerifyCode);
        }

        public static string GenerateRandomCode(int length)
        {
            const string validChars = @"[A-Za-z0-9!@#$%^&*()_+{}|[\]:;=,.?/]";
            var chars = new char[length];
            var rand = new Random();

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[rand.Next(validChars.Length)];
            }

            return new string(chars);

        }

    }
}
