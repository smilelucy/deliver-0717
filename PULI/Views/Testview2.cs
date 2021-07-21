using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Views
{
    public class Testview2 : ContentPage
    {
        public Testview2()
        {
            
        }

        private async void setView()
        {
            // 透過api從後台抓取問卷內容資訊

            // 判斷今天是星期幾

            // 判斷哪些問卷視符合同上功能資格的
        }

        private void reset()
        {
            // 重新run頁面
        }

        //public StackLayout questionView(questionnaire questionList)
        //{
            // stack加入案主姓名、問卷編號、工作問卷

            // if題目的題號小於4(還沒觸發第4題之前，都只產生前三題)
                // if題目類型(qb02 == 1)是選擇題
                    // stack加入題號 + 題目
                    // if題號是第三題且是時間是星期一~星期三之間(第三題只有"已發"選項)
                        // 判斷Dictionary中是否有該題答案(有代表已填過)(for 重啟app的sqlite資料填回來的部分)
                        // if有的話
                            // 把答案抓出來並加回上傳的list
                            // 判斷checkbox的勾選
                            // 判斷是否有checkbox事件
                                // 如果有
                                    // 判斷checklist中是否已經有那個問卷的那筆題目的選擇紀錄
                                        // 如果有的話
                                            // 把原本那筆紀錄找出來並刪掉
                                    // 把目前checkbox事件的答案填到dictionary和存到SQLite
                                    // 把目前的答案存到checklist(for 檢查)和checklist2(for 上傳到後台)
                                    // 處理畫面上的重置
                                        // 找尋被點擊的那個stack
                                        // 把原本的label(選項)刪掉，再把新的有顏色的label(選項)加回來
                            // 判斷字的顏色
                            // stack加入checkbox和label
                    // else
                        // if題號是第三題且時間是星期五
                            // 判斷Dictionary中是否有該題答案(有代表已填過)(for 重啟app的sqlite資料填回來的部分)
                                // if有的話
                                    // 把答案抓出來並加回上傳的list
                                    // 判斷checkbox的勾選
                                    // 判斷是否有checkbox事件
                                        // 如果有
                                            // 判斷checklist中是否已經有那個問卷的那筆題目的選擇紀錄
                                                // 如果有的話
                                                    // 把原本那筆紀錄找出來並刪掉
                                            // 把目前checkbox事件的答案填到dictionary和存到SQLite
                                            // 把目前的答案存到checklist(for 檢查)和checklist2(for 上傳到後台)
                                            // if選擇"未發"
                                                // reset
                                            // else 
                                                // 處理畫面上的重置
                                                    // 找尋被點擊的那個stack
                                                    // 判斷他是第一次勾選還是要換選項
                                                        // if是第一次勾選
                                                            // 把原本的label(選項)刪掉，再把新的有顏色的label(選項)加回來
                                                            // 把dictionary填入那次勾選的選項(是 / 否...)，好讓以後勾選判斷
                                                        // else(第一次以後的勾選，需要把原本溝的選項也重置)
                                                            // 把上次已經勾選的checkbox刪掉
                                                            // 換成原本沒有勾選過的checkbox
                                                            // 把上次的label(有顏色的選項)刪掉
                                                            // 換成沒有顏色的選項
                                                            // 把原本沒選過，這次選的checkbox換成有勾選的checkbox
                                                            // 把原本沒選過的label換成有顏色的label
                                    // 判斷字的顏色
                                    // stack加入checkbox和label
                        // if是第三題且時間是星期四
                            // 判斷Dictionary中是否有該題答案(有代表已填過)(for 重啟app的sqlite資料填回來的部分)
                                // if有的話
                                    // 把答案抓出來並加回上傳的list
                                    // 判斷checkbox的勾選
                                    // 判斷是否有checkbox事件
                                        // 如果有
                                            // 判斷checklist中是否已經有那個問卷的那筆題目的選擇紀錄
                                                // 如果有的話
                                                    // 把原本那筆紀錄找出來並刪掉
                                            // 把目前checkbox事件的答案填到dictionary和存到SQLite
                                            // 把目前的答案存到checklist(for 檢查)和checklist2(for 上傳到後台)
                                            // if選擇"未發"
                                                // reset
                                            // else 
                                                // 處理畫面上的重置
                                                    // 找尋被點擊的那個stack
                                                    // 判斷他是第一次勾選還是要換選項
                                                        // if是第一次勾選
                                                            // 把原本的label(選項)刪掉，再把新的有顏色的label(選項)加回來
                                                            // 把dictionary填入那次勾選的選項(是 / 否...)，好讓以後勾選判斷
                                                        // else(第一次以後的勾選，需要把原本溝的選項也重置)
                                                            // 把上次已經勾選的checkbox刪掉
                                                            // 換成原本沒有勾選過的checkbox
                                                            // 把上次的label(有顏色的選項)刪掉
                                                            // 換成沒有顏色的選項
                                                            // 把原本沒選過，這次選的checkbox換成有勾選的checkbox
                                                            // 把原本沒選過的label換成有顏色的label
                                    // 判斷字的顏色
                                    // stack加入checkbox和label
                        // else
                            // if題目是第一題
                                //  // 判斷Dictionary中是否有該題答案(有代表已填過)(for 重啟app的sqlite資料填回來的部分)
                                // if有的話
                                    // 把答案抓出來並加回上傳的list
                                    // 判斷checkbox的勾選
                                    // 判斷是否有checkbox事件
                                        // 如果有
                                            // 判斷checklist中是否已經有那個問卷的那筆題目的選擇紀錄
                                                // 如果有的話
                                                    // 把原本那筆紀錄找出來並刪掉
                                            // 把目前checkbox事件的答案填到dictionary和存到SQLite
                                            // 把目前的答案存到checklist(for 檢查)和checklist2(for 上傳到後台)
                                            // if選擇"未發"
                                                // reset
                                            // else 
                                                // 處理畫面上的重置
                                                    // 找尋被點擊的那個stack
                                                    // 判斷他是第一次勾選還是要換選項
                                                        // if是第一次勾選
                                                            // 把原本的label(選項)刪掉，再把新的有顏色的label(選項)加回來
                                                            // 把dictionary填入那次勾選的選項(是 / 否...)，好讓以後勾選判斷
                                                        // else(第一次以後的勾選，需要把原本溝的選項也重置)
                                                            // 把上次已經勾選的checkbox刪掉
                                                            // 換成原本沒有勾選過的checkbox
                                                            // 把上次的label(有顏色的選項)刪掉
                                                            // 換成沒有顏色的選項
                                                            // 把原本沒選過，這次選的checkbox換成有勾選的checkbox
                                                            // 把原本沒選過的label換成有顏色的label
                                    // 判斷字的顏色
                                    // stack加入checkbox和label
                        // if題目是第二題
                            // 判斷Dictionary中是否有該題答案(有代表已填過)(for 重啟app的sqlite資料填回來的部分)
                                // if有的話
                                    // 把答案抓出來並加回上傳的list
                                    // 判斷checkbox的勾選
                                    // 判斷是否有checkbox事件
                                        // 如果有
                                            // 判斷checklist中是否已經有那個問卷的那筆題目的選擇紀錄
                                                // 如果有的話
                                                    // 把原本那筆紀錄找出來並刪掉
                                            // 把目前checkbox事件的答案填到dictionary和存到SQLite
                                            // 把目前的答案存到checklist(for 檢查)和checklist2(for 上傳到後台)
                                            // if選擇"未發"
                                                // reset
                                            // else 
                                                // 處理畫面上的重置
                                                    // 找尋被點擊的那個stack
                                                    // 判斷他是第一次勾選還是要換選項
                                                        // if是第一次勾選
                                                            // 把原本的label(選項)刪掉，再把新的有顏色的label(選項)加回來
                                                            // 把dictionary填入那次勾選的選項(是 / 否...)，好讓以後勾選判斷
                                                        // else(第一次以後的勾選，需要把原本溝的選項也重置)
                                                            // 把上次已經勾選的checkbox刪掉
                                                            // 換成原本沒有勾選過的checkbox
                                                            // 把上次的label(有顏色的選項)刪掉
                                                            // 換成沒有顏色的選項
                                                            // 把原本沒選過，這次選的checkbox換成有勾選的checkbox
                                                            // 把原本沒選過的label換成有顏色的label
                                    // 判斷字的顏色
                                    // stack加入checkbox和label
                        // else
                            // 判斷Dictionary中是否有該題答案(有代表已填過)(for 重啟app的sqlite資料填回來的部分)
                                // if有的話
                                    // 把答案抓出來並加回上傳的list
                                    // 判斷checkbox的勾選
                                    // 判斷是否有checkbox事件
                                        // 如果有
                                            // 判斷checklist中是否已經有那個問卷的那筆題目的選擇紀錄
                                                // 如果有的話
                                                    // 把原本那筆紀錄找出來並刪掉
                                            // 把目前checkbox事件的答案填到dictionary和存到SQLite
                                            // 把目前的答案存到checklist(for 檢查)和checklist2(for 上傳到後台)
                                            // if選擇"未發"
                                                // reset
                                            // else 
                                                // 處理畫面上的重置
                                                    // 找尋被點擊的那個stack
                                                    // 判斷他是第一次勾選還是要換選項
                                                        // if是第一次勾選
                                                            // 把原本的label(選項)刪掉，再把新的有顏色的label(選項)加回來
                                                            // 把dictionary填入那次勾選的選項(是 / 否...)，好讓以後勾選判斷
                                                        // else(第一次以後的勾選，需要把原本溝的選項也重置)
                                                            // 把上次已經勾選的checkbox刪掉
                                                            // 換成原本沒有勾選過的checkbox
                                                            // 把上次的label(有顏色的選項)刪掉
                                                            // 換成沒有顏色的選項
                                                            // 把原本沒選過，這次選的checkbox換成有勾選的checkbox
                                                            // 把原本沒選過的label換成有顏色的label
                                    // 判斷字的顏色
                                    // stack加入checkbox和label


                // 把姓名、問卷編號、工作編號stack加到最大stack
                // 題號、題目stack和checkbox、選項stack加到另一個stack並垂直
                // 用frame把題目資訊stack包起來
                // 把frame加到最大的stack
            // else(觸發第四題之後)
                // 判斷Dictionary中是否有該題答案(有代表已填過)(for 重啟app的sqlite資料填回來的部分)
                                // if有的話
                                    // 把答案抓出來並加回上傳的list
                                    // 判斷checkbox的勾選
                                    // 判斷是否有checkbox事件
                                        // 如果有
                                            // 判斷checklist中是否已經有那個問卷的那筆題目的選擇紀錄
                                                // 如果有的話
                                                    // 把原本那筆紀錄找出來並刪掉
                                            // 把目前checkbox事件的答案填到dictionary和存到SQLite
                                            // 把目前的答案存到checklist(for 檢查)和checklist2(for 上傳到後台)
                                            // if選擇"未發"
                                                // reset
                                            // else 
                                                // 處理畫面上的重置
                                                    // 找尋被點擊的那個stack
                                                    // 判斷他是第一次勾選還是要換選項
                                                        // if是第一次勾選
                                                            // 把原本的label(選項)刪掉，再把新的有顏色的label(選項)加回來
                                                            // 把dictionary填入那次勾選的選項(是 / 否...)，好讓以後勾選判斷
                                                        // else(第一次以後的勾選，需要把原本溝的選項也重置)
                                                            // 把上次已經勾選的checkbox刪掉
                                                            // 換成原本沒有勾選過的checkbox
                                                            // 把上次的label(有顏色的選項)刪掉
                                                            // 換成沒有顏色的選項
                                                            // 把原本沒選過，這次選的checkbox換成有勾選的checkbox
                                                            // 把原本沒選過的label換成有顏色的label
                                    // 判斷字的顏色
                                    // stack加入checkbox和label
                // 把姓名、問卷編號、工作編號stack加到最大stack
                // 題號、題目stack和checkbox、選項stack加到另一個stack並垂直
                // 用frame把全部題目資訊stack包起來
                // 把frame加到最大的stack
            // if問題類型是問答題(qb02 == 3)(第五題)
                // 判斷SQLite裡面有沒有這題的答案
                    // if有的話
                        // 把答案抓回來填到dixtionary(把她是否觸發第五題的dixtionary填成true)
                // 判斷SQLite裡面是否有這題的答案
                    // if有的話
                        // 把答案抓出來加到要上船的list
                // if要觸發第五題的dictionary是true(代表要run第五題)
                    // stack加入第五題的題號跟題目
                    // stack加入entry
                    // 判斷是否有觸發entry事件
                        // if有
                            // 把答案填到dictionary
                            // 把答案加到要上船的list
                            // 把答案存到SQLite
                // 把姓名、問卷編號、工作編號stack加到最大stack
                // 題號、題目stack和entry 加到另一個stack並垂直
                // 用frame把全部題目資訊stack包起來
                // 把frame加到最大的stack
            // 如果是同上的那筆問卷
                
                // 判斷有沒有button_click事件
                    // if有的話
                        // 找到這個案主上一筆題目
                        // 把上一筆的答案抓出來
                        // 加回這筆
                        // 把答案加到checklist(for check)
                        // 把答案加到checklist2(for upload)
                        // 把答案存到SQLite
                        // 把答案存到dictionary
                        // 處理畫面重置
                            // 找到那題在stack中的位置並把他remove
                            // 判斷他是第幾題
                            // 把新的checkbox跟labe加回去stack中的那個位置
                // 把同上的button加入最大stack
        //}
        //private async void post_questionClicked(object sender, EventArgs e)
        //{
            // 把資料透過api上傳到後台
            // if 上傳成功
                // 把SQLite裡面的資料都清空
                // 把Dictionary都變回預設
                // reset
        //}

    }
}