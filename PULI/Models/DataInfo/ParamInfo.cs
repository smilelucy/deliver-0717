using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class ParamInfo
    {
        public string PRIVACY_WEB = "https://www.privacypolicies.com/privacy/view/f7dc393400a40ce5d71990c3ce3c8d6e";
        public string PRIVACY_NEED_MESSAGE = "需要您同意將資料提供給我們作為通知使用，謝謝您!";
        public string PRIVACY_MESSAGE = "是否同意將資料提供給竹山鎮公所用於日後活動訊息與優惠活動的通知管道，竹山鎮公所將不會提供給第三方團體並遵守個人資料保護法。";
        //public string HOST = "http://192.168.50.202:8001";
        //public string HOST = "http://10.105.24.231:8001";
        //public string HOST = "http://163.22.17.249:8001/";
        //public string HOST = "https://jsj.unlla.com";
        //public string HOST = "http://192.168.123.190:8001";
        //public string HOST = "http://192.168.125.148:8001";

        //public string DEVICE_ID = DependencyService.Get<IDeviceID>().GetDeviceID();
        //public string USER_ID = "";

        public string TEXT_COLOR = "#326292";

        public string CONNECT_SERVER_ERROR_MESSAGE = "無法連線上伺服器，請確認網路或稍後再嘗試。";
        public string CONNECT_BLUETOOTH_ERROR_MESSAGE = "藍芽未開啟，請確認或稍後再嘗試。";
        public string CONNECT_PASSWORD_ERROR_MESSAGE = "帳號或密碼錯誤，請確認或稍後再嘗試。";

        public string SYSYTEM_MESSAGE = "系統訊息";
        public string INTERNET_ERROR_MESSAGE = "系統偵測不到網路，請確認網路狀況。";
        public string DIALOG_MESSAGE = "好的";
        public string DIALOG_AGREE_MESSAGE = "確定";
        public string DIALOG_DISAGREE_MESSAGE = "取消";

        public string OPEN_URL_MESSAGE = "系統將開啟外部連結，請問要前往嗎?";

        public string RESERVE_NOTOPEN_MESSAGE = "尚未開放報名!";
        public string RESERVE_LIMIT_MESSAGE = "報名額滿!";
        public string RESERVE_DATALENGTH_ERROR_MESSAGE = "有欄位長度不符標準!";
        public string RESERVE_REPEAT_MESSAGE = "個人報名次數已達上限";
        public string RESERVE_LOCK_MESSAGE = "由於您上次報名未領花，故這周無法進行報名!";
        public string RESERVE_FAILED_MESSAGE = "報名失敗，請再試一次!";
        public string RESERVE_DELETE_MESSAGE = "確定要取消報名嗎?";
        public string RESERVE_DELETE_FAILED_MESSAGE = "取消報名失敗，請再試一次!";
        public string RESERVE_DELETE_SUCCESS_MESSAGE = "取消報名成功!";
        public string RESERVE_SUSPENSION_MESSAGE = "您已被停權，無法報名，請聯絡工作人員處理。";

        public string BONUS_EXHIBIT_REPEAT_MESSAGE = "此點數獲得方式只能使用一次，所以無法再獲得點數囉!";
        public string BONUS_EXHIBIT_OVER_MESSAGE = "目前點數已達上限，所以無法再獲得點數囉!";
        public string BONUS_EXHIBIT_ERROR_MESSAGE = "獲得點數失敗，請再試一次!";

        public string BONUS_RESERVE_UNRESERVE_MESSAGE = "系統沒有您報名的紀錄，所以無法兌換唷!";
        public string LOCATION_ERROR_MESSAGE = "您目前沒有開啟定位，Ibeacon與地圖導覽服務需要獲得您的定位權限進行導覽與顯示附近的廠商與景點";
        public string BROWSER_ERROR_MESSAGE = "無法開啟瀏覽器";

        public string NOTIFICATIONS_ALERT_MESSAGE = "您有通知訊息，要馬上到訊息中心查看嗎!?";

        public string ACTIVITY_RESERVE_TYPE = "reserve";
        public string ACTIVITY_ACCUMULATION_TYPE = "accumulation";
        public string ACTIVITY_MERGE_TYPE = "merge";
        public string ACTIVITY_ANNOUNCE_TYPE = "announce";

        public string SCAN_TYPE_EXHIB = "exhibit";
        public string SCAN_TYPE_AWARD = "award";
        public string SCAN_TYPE_GET = "get";
        public string SCAN_TYPE_RESERVE = "reserve";
        public string SCAN_TYPE_VOTE = "vote";
        public string SCAN_TYPE_COMPANY = "company";
        public string SCAN_TYPE_PRODUCT = "product";
        public string SCAN_TYPE_REPLACE = "replace"; /// 代理

        public string VOTE_TYPE_EXHIB = "exhibit";
        public string VOTE_TYPE_COMPANY = "company";
        public string VOTE_TYPE_PRODUCT = "product";

        public string PULI_FARM_TYPE_NUMBER = "1";
        public string PULI_SHOP_TYPE_NUMBER = "2";
        public string YUCHIH_FARM_TYPE_NUMBER = "3";
        public string YUCHIH_SHOP_TYPE_NUMBER = "4";
        public string KUOHSING_FARM_TYPE_NUMBER = "5";
        public string KUOHSING_SHOP_TYPE_NUMBER = "6";

        public string MARQUEE_TYPE_ACTIVITY = "activity";
        public string MARQUEE_TYPE_COMPANY = "company";
        public string MARQUEE_TYPE_WEBSITE = "website";

        public string LISTVIEW_COMPANY_TYPE = "COMPANY";

        public string PNG_FARM_ICON = "PYK.icon.product_icon.png";
        public string PNG_SHOP_ICON = "PYK.icon.gift_icon.png";

        public string PNG_MAP_ART_ICON = "JSJ.icon.art.png";
        public string PNG_MAP_FOOD_ICON = "JSJ.icon.food.png";
        public string PNG_MAP_HOTEL_ICON = "JSJ.icon.hotel.png";
        public string PNG_MAP_GIFT_ICON = "JSJ.icon.gift.png";
        public string PNG_MAP_TRAVEL_ICON = "JSJ.icon.travel_icon.png";
        public string PNG_MAP_NIGHT_ICON = "JSJ.icon.nightlogo.png";
        public string PNG_MAP_HOME_ICON = "PULI.icon.home.png";

        public string MESSAGECENTER_MAP_CHANNEL = "MAP";

        public string RESERVE_INFO_NAME = "姓名";
        public string RESERVE_INFO_NAMEA = "姓";
        public string RESERVE_INFO_NAMEB = "名";
        public string RESERVE_INFO_CARD = "身分證";
        public string RESERVE_INFO_LON = "經度";
        public string RESERVE_INFO_LAT = "緯度";
        public string RESERVE_INFO_BIRTHDAY = "生日";
        public string RESERVE_INFO_TELEPHONE = "電話";
        public string RESERVE_INFO_PLACE = "住址(里別)";
        public string RESERVE_INFO_AGE = "出生年次";
        public string RESERVE_INFO_GENDER = "性別";
        public string RESERVE_INFO_ADDRESS = "完整住址";
        public string RESERVE_INFO_IDCARD = "身分證號碼";
        public string RESERVE_INFO_NOTE = "備註";

        public string VOTE_ALERT_MESSAGE = "送出後，將無法變更投票，是否繼續?";

        public string VOTE_INSERT_SUCCESS_MESSAGE = "投票成功!謝謝您投下神聖的一票!";
        public string VOTE_INSERT_FAILED_MESSAGE = "投票失敗!請在試一次!";
        public string VOTE_INSERT_OVER_MESSAGE = "個人票數已達上限囉!";
        public string VOTE_INSERT_LIMIT_MESSAGE = "總票數已達上限囉!";

        public string USER_AUTH_ADMIN = "admin";
        public string USER_AUTH_MANAFE = "manage";
        public string USER_AUTH_COMPANY = "company";
        public string USER_AUTH_USER = "user";
        public string USER_UPDATE_SUCCESS_MESSAGE = "修改成功!";

        public string SILVER_INSUFFUCUENT_MESSAGE = "銀幣不足!";
        public string SILVER_ACTIVITY_TOTAL_LIMIT_MESSAGE = "活動銀幣已達上限";
        public string SILVER_ACTIVITY_NOT_OPEN = "活動尚未開放";

        public string AWARD_SUCCESS_MESSAGE = "兌換成功!";

        public string COMPANY_NOT_FACEBOOK = "目前沒有臉書專頁唷!";
        public string COMPANY_NOT_WEB = "目前沒有網站唷!";
        public string EXHIBIT_NOT_VIDEO = "目前沒有影片唷!";
        public string EXHIBIT_NOT_WEB = "目前沒有網站唷!";

        public string LOGIN_ACCOUNT = "帳號";
        public string LOGIN_PASSWORD = "密碼";

        
    }
}