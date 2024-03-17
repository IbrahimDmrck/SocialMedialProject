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
        public static string AuthorizationDenied="Yetkisiz Kullanıcı";
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
        public static string ArticleWithDetailListed="Paylaşımlar ve yorumları listelendi";

        public static string UserImagesListed = "Kullanıcının resimleri listelendi";
        public static string UsersImagesListed = "Tum Kullanıcı resimleri listelendi";
        public static string UserImageListed = "Kullanıcı resmi listelendi";
        public static string UserImageAdded = "Kullanıcı resmi eklendi";
        public static string UserImageDeleted = "Kullanıcı resmi silindi";
        public static string UserImageUpdated = "Kullanıcı resmi guncellendi";
        public static string ErrorUpdatingImage = "Resim guncellenirken hata olustu";
        public static string ErrorDeletingImage = "Resim silinirken hata olustu";
        public static string UserImageLimitExceeded = "Bu kullanıcıya daha fazla resim eklenemez";
        public static string UserImageIdNotExist = "Kullanıcı resmi mevcut degil";

        public static string GetDefaultImage = "Kullanıcının bir resmi olmadigi icin varsayilan resim getirildi";
        public static string NoPictureOfTheUser = "Kullanıcının hic resmi yok";
        public static string TrueComment="Geçmiş bildirimler";
        public static string FalseComment = "Yeni bildirimler";

        public static string SendVerifyCode="E-postanıza şifre güncelleme işlemi için doğrulama kodu gönderilmiştir";
        public static string VerifyCodeDeleted="Doğrulama Kodu Silindi";
        public static string CodeNotFound="Herhangi bir kod bulunamadı, lütfen tekrar deneyin";
        public static string VerificationSuccessfull= "Doğrulama başarılı, yönlendiriyorsunuz";

        public static string ClaimAdded="Yetki Oluşturuldu";
        public static string ClaimDeleted="Yetki Silindi";
        public static string ClaimsListed="Yetkiler Listelendi";
        public static string ClaimListed="Yetki Getirildi";
        public static string ClaimUpdated="Yetki Güncellendi";
    }
}
    