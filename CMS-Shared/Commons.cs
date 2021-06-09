using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared
{
    public static class Commons
    {
        public const string Image100_50 = "http://placehold.it/100x50";
        public const string Image75_75 = "http://placehold.it/75x75";
        public const string Image100_100 = "http://placehold.it/100x100";
        public const string Image200_100 = "http://placehold.it/200x100";
        public const string Image375_205 = "http://placehold.it/375x205";
        public const string Image300_300 = "http://placehold.it/300x300";
        public const string Image600_400 = "http://placehold.it/600x400";
        public const string Image470_366 = "http://placehold.it/470x366";
        public const string Image2000_2000 = "http://placehold.it/2000x2000";
        public const string Image220_220 = "http://placehold.it/220x220";
        public const string Image170_170 = "http://placehold.it/170x170";
        public const string Image480_480 = "http://placehold.it/480x480";
        public const string Image770_395 = "http://placehold.it/770x395";
        public const string Image1920_730 = "http://placehold.it/1920x730";
        public const string Image1200_650 = "http://placehold.it/1200x650";

        public static int WidthProductMain = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageProductMain"]);
        public static int HeightProductMain = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageProductMain"]);
        public static int WidthProductSe = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageProductSe"]);
        public static int HeightProductSe = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageProductSe"]);
        public static int WidthCate = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageCate"]);
        public static int HeightCate = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageCate"]);
        public static int WidthListCate = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageListCate"]);
        public static int HeightListCate = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageListCate"]);
        public static int WidthCollection = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageCollection"]);
        public static int HeightCollection = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageCollection"]);
        public static int WidthBrand = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageBrand"]);
        public static int HeightBrand = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageBrand"]);
        public static int WidthBanner = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageBanner"]);
        public static int HeightBanner = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageBanner"]);
        public static int WidthImageNews = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageNews"]);
        public static int HeightImageNews = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageNews"]);
        public static int WidthImageAuthor = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageAuthor"]);
        public static int HeightImageAuthor = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageAuthor"]);
        public static int WidthImageSilder = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageSilder"]);
        public static int HeightImageSilder = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageSilder"]);
        public static int WidthImageStyle = Convert.ToInt16(ConfigurationManager.AppSettings["WidthImageStyle"]);
        public static int HeightImageStyle = Convert.ToInt16(ConfigurationManager.AppSettings["HeightImageStyle"]);

        public static string Phone1 = ConfigurationManager.AppSettings["Phone1"];
        public static string Phone2 = ConfigurationManager.AppSettings["Phone2"];
        public static string EmailSend = ConfigurationManager.AppSettings["EmailSend"];
        public static string PassEmailSend = ConfigurationManager.AppSettings["PassEmailSend"];
        public static string EmailReceive = ConfigurationManager.AppSettings["EmailReceive"];
        public static string AddressCompany = ConfigurationManager.AppSettings["AddressCompnay"];
        public static string CompanyTitle = ConfigurationManager.AppSettings["CompanyTitle"];
        public static string Website = ConfigurationManager.AppSettings["Website"];
        public static string HostImage = ConfigurationManager.AppSettings["HostImage"];
        public static string _PublicImages = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PublicImages"]) ? "" : ConfigurationManager.AppSettings["PublicImages"];
        
        public const string PasswordChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        #region Enum
        public enum ECateType
        {
            NuocTayTrang = 0,
            SuaRuaMat = 1,
            TayTeBaoChet = 2,
            NuocHoaHong = 3,
            KemChongNang = 4,
        }
        public enum ESizeType
        {
            XS = 0,
            S = 1,
            M = 2,
            L = 3,
            XL = 4,
        }
        public enum EStarType
        {
            Star1 = 1,
            Star2 = 2,
            Star3 = 3,
            Star4 = 4,
            Star5 = 5,
        }

        public enum EStateType
        {
            Avalable = 0,
            None = 1,
        }

        public enum ERangeType
        {
            Leatest = 0,
            Hightest = 1,
        }

        public enum EStatus
        {
            Actived = 1,
            Deleted = 9,
        }

        public enum ETypeLink
        {
            Images = 0,
            Links = 1,
            Sliders = 2,
            Videos = 3,
        }

        public enum EAnswer
        {
            Answer1 = 1,
            Answer2 = 2,
            Answer3 = 3,
            Answer4 = 4,
            Answer5 = 5,
        }

        public enum ETypeUserLogin
        {
            Normal = 1,
            Fb = 2,
            Gg = 3,
        }

        public enum ETypeUserRequest
        {
            RequestNormal = 1,
            RequestNewsLetter = 2,
            RequestInfor = 3,
            RequestBook = 4,
            RequestMakeInfor = 5,
            RequestSendFriend = 6,
        }

        public enum ETypeNews
        {
            KhuyenMai = 1,
            ThiTruong = 2,
        }
        public enum ETypePage
        {
            GioiThieu = 1,
            KinhNghiem = 2,
            KhuyenMai = 3,
        }
        public enum ETypeMenu
        {
            RuouNgoai = 1,
        }
        #endregion
    }
}
