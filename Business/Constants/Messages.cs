using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class Messages
    {
        public static string UserRegistered="Kayıt başarılı bir şekilde yapıldı";
        public static string UserNotFound="Girilen bilgilere ait bir kullanıcı bulunamadı";
        public static string PasswordError="Yanlış şifre girdiniz, lütfen şifrenizi kontrol edip tekrar deneyin";
        public static string SuccessfulLogin="Giriş Başarılı , Hoşgeldiniz";
        public static string UserAlreadyExists="Böyle bir kullanıcı zaten kayıtlı";
        public static string AccessTokenCreated="Token Oluşturuldu";
        public static string PasswordChanged="Şifre başarılı bir şekilde değiştirildi";
        public static string AuthorizationDenied;
        public static string UsersListed="Kullanıcılar listelendi";
        public static string UserListed="Kullanıcı listelendi";
        public static string UserNotExist="Kullanıcı bulunamadı";
        public static string UserAdded="Kullanıcı başarıyla eklendi";
        public static string UserUpdated="Kullanıcı başarıyla güncellendi";
        public static string UserDeleted="Kullanıcı silindi";
        public static string UserEmailExist="Bu maile ait kullanıcı zaten tanımlı";
        public static string UserEmailNotAvailable="Bu kullanıcı maili erişilebilir değil";

        public static string ArticleListed="Paylaşım listelendi";
        public static string ArticlesListed="Paylaşımlar listelendi";
        public static string ArticleUpdated="Paylaşım güncellendi";
        public static string ArticleDeleted="Paylaşım silindi";
        public static string ArticleAdded="Paylaşım yayımlandı";

        public static string Comment_Add="Yorum yapıldı";
        public static string Comment_Delete="Yorum silindi";
        public static string Comment_Update="Yorum güncellendi";
        public static string Comment_Listed="Yorum listelendi";
        public static string Comments_Listed="Yorumlar listelendi";

        public static string Topic_Add="Konu oluşturuldu";
        public static string Topic_Delete = "Konu silindi";
        public static string Topic_Update = "Konu güncellendi";
        public static string Topic_Listed = "Konu listelendi";
        public static string Topics_Listed = "Konular listelendi";
    }
}
