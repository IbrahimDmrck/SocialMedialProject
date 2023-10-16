using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string UserAdded = "Kullanici eklendi";
        public static string UserDeleted = "Kullanici silindi";
        public static string UserUpdated = "Kullanici guncellendi";
        public static string UsersListed = "Kullanicilar listelendi";
        public static string UserListed = "Kullanici listelendi";
        public static string UserNotExist = "Kullanici mevcut degil";
        public static string UserEmailExist = "E-mail zaten kayitli";
        public static string UserEmailNotAvailable = "Kullanici e-maili gecersiz";

        public static string AuthorizationDenied = "Bu islemi yapmak icin yetkiniz yok";
        public static string UserRegistered = "Kullanici kayit basarili";
        public static string UserNotFound = "Kullanici bulunamadi";
        public static string PasswordError = "Sifre hatali";
        public static string SuccessfulLogin = "Giris basarili";
        public static string UserAlreadyExists = "Kullanici zaten sisteme kayitli";
        public static string AccessTokenCreated = "Token basariyla olusturuldu";
        public static string PasswordChanged = "Sifre basariyla degistirildi";
    }
}
