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
        public static string UserClaimNotFound = "Kullanıcı yetkisi bulunamadı";
        public static string UsersListed = "Kullanıcılar Listelendi";
        public static string Article_Add = "Paylaşım yapıldı";
        public static string Article_Edit = "Paylaşım Düzenlendi";
        public static string Article_Deleted = "içerik silindi";
        public static string Articles_Listed = "içerikler listelendi";
        public static string Article_Listed = "içerik listelendi";
        public static string Comment_Add = "yorum yapıldı";
        public static string Comment_Delete = "yorum silindi";
        public static string Comment_Update = "yorum güncellendi";
        public static string Comment_Listed = "yorum listelendi";
        public static string Comments_List = "Yorumlar listelendi";
        public static string Topic_Add = "konu oluşturuldu";
        public static string Topic_Delete = "Konu silindi";
        public static string Topic_Update = "konu güncellendi";
        public static string Topic_List = "Konu listelendi";
        public static string Topics_List = "Konular listelendi";
        public static string UserRegistered = "Kullanıcı kayıt edildi";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Şifre hatalı";
        public static string SuccessfulLogin = "giriş başarılı";
        public static string UserExist = "Kullanıcı mevcut";
        public static string UserNotExist = "Böyle bir kullanıcı yok";
        public static string UserEmailExist = "Bu e-posta kullanılıyor";
        public static string userEmailNotAvailable = "Geçersiz bir email girdiniz";
        public static string UserUpdated = "Kullanıcı güncellendi";
        public static string UserListed = "Kullanıcı listelendi";
        public static string userDeleted = "Kullanıcı silindi";
        public static string UserAdded = "Kullanıcı eklendi";
        public static string AuthorizationDenied = "Bu işlemi yapabilmek için yetkiniz yok";
        public static string ArticleWithDetailListed = "Paylaşımlar ve yorumları listelendi";
        public static string UserImageLimitExceeded = "";
        public static string GetDefaultImage = "varsayılan görsel getirildi";
        public static string UserImageAdded = "kullanıcı görseli eklendi";
        public static string ErrorDeletingImage = "görsel silinemedi";
        public static string UserImageDeleted = "kullanıcı görseli silindi";
        public static string NoPictureOfTheUser = "kullanıcı görseli yok";
        public static string UserImagesListed = "Kullanıcı görselleri listeledi";
        public static string UserImageListed = "Kullanıcı görseli listelendi";
        public static string ErrorUpdatingImage = "Görsel güncellenirken bir hata oluştu";
        public static string UserImageUpdated = "Kullanıcı görseli güncellendi";
        public static string UserImageIdNotExist = "Kullanıcı görseli bulunamadı";
        public static string SendVerifyCode = " E-posta adresinize doğrulama Kodu gönderildi";
        public static string VerifyCodesDeleted = "Doğrulama kodu silindi";
        public static string CodeNotFound = "KOd bulunamadı";
        public static string VerificationSuccessfull = "Kod doğrulama başarılı, şifre güncelleme sayfasına yönlendiriliyorsunuz";
        public static string PasswordChanged = "Şifre başarıyla güncellendi, ana sayfaya yönlendiriliyorsunuz";
        public static string ClaimAdd = "Yetki Eklendi";
        public static string ClaimsListed = "Yetkiler listelendi";
        public static string UserClaimAdd = "Kullanıcıya Yetki Eklendi";
        public static string UserClaimslisted = "Kullanıcı Yetkileri Listelendi";
        public static string UserClaimUpdate = "Kullanıcıya ait yetki güncellendi";
        public static string ClaimDeleted = "Yetki silindi";
        public static string UserClaimDeleted = "Kullanıcı Yetkisi Kaldırıldı";
        public static string UserImageIdExist;
        public static string ArticleNotFound;
        public static string CommentNotFound;
        public static string ClaimExist;
        public static string ClaimNotFound;
        public static string TopicNotFound;
        public static string UserClaimExist;
        internal static string FalseComment;
        internal static string Comments_Listed;
        internal static string ClaimUpdate;
        internal static string ClaimListed;
        internal static string Topics_Listed;
        internal static string Topic_Listed;
        internal static string UserClaimAdded;
        internal static string UserClaimDelete;
        internal static string UsersClaimsListed;
    }
}
    