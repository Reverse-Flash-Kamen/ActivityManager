using ActivityManager.App_Data;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows;

namespace ActivityManager
{
    public class MyActivity
    {
        private string activityID;
        private string activityName;
        private string activityIntro;
        private string activityPlaceID;
        private string activityOrgID;
        private string availableCredit = "1";
        private string maxSigned;
        private string signed = "0";
        private string activityState = "1";
        private string signStartDate;
        private string signEndDate;
        private string holdDate;
        private string holdStart;
        private string holdEnd;
        private string submitTime = "1900-01-01 00:00:00";
        private string failReason;
        private string activityType;

        public MyActivity()
        { }

        public MyActivity(String id)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from activity in db.Activity
                      where activity.activityID == id
                      select activity;
            var a = res.First();

            activityID = a.activityID.ToString().Trim();
            activityName = a.activityName.ToString().Trim();
            activityIntro = a.activityIntro.ToString().Trim();
            activityPlaceID = a.activityPlaceID.ToString().Trim();
            activityOrgID = a.activityOrgID.ToString().Trim();
            availableCredit = a.availableCredit.ToString().Trim();
            maxSigned = a.maxSigned.ToString().Trim();
            signed = a.signed.ToString().Trim();
            activityState = a.activityState.ToString().Trim();
            signStartDate = Convert.ToDateTime(a.signStartDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            signEndDate = Convert.ToDateTime(a.signEndDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            holdDate = Convert.ToDateTime(a.holdDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            holdStart = a.holdStart.ToString().Trim();
            holdEnd = a.holdEnd.ToString().Trim();
            submitTime = Convert.ToDateTime(a.submitTime).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            failReason = a.failReason.ToString().Trim();
            activityType = a.activityType.ToString().Trim();
        }

        public MyActivity(Activity a)
        {
            activityID = a.activityID.ToString().Trim();
            activityName = a.activityName.ToString().Trim();
            activityIntro = a.activityIntro.ToString().Trim();
            activityPlaceID = a.activityPlaceID.ToString().Trim();
            activityOrgID = a.activityOrgID.ToString().Trim();
            availableCredit = a.availableCredit.ToString().Trim();
            maxSigned = a.maxSigned.ToString().Trim();
            signed = a.signed.ToString().Trim();
            activityState = a.activityState.ToString().Trim();
            signStartDate = Convert.ToDateTime(a.signStartDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            signEndDate = Convert.ToDateTime(a.signEndDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            holdDate = Convert.ToDateTime(a.holdDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            holdStart = a.holdStart.ToString().Trim();
            holdEnd = a.holdEnd.ToString().Trim();
            submitTime = Convert.ToDateTime(a.submitTime).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            failReason = a.failReason.ToString().Trim();
            activityType = a.activityType.ToString().Trim();
        }

        public void Create()
        {
            // 根据保存时间与顺序计算activityID,如果不按序删除,会导致插入主键重复
            //submitTime = DateTime.Now.ToString("yyyy-MM-dd"); // 时间格式
            string submitTimePro = DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);

            /*string[] strTime = submitTimePro.Split('-');
            submitTimePro = "";
            foreach (var str in strTime) submitTimePro += str;*/

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from a in db.Activity
                      where a.activityID.StartsWith(submitTimePro)
                      select a;

            // 这里有问题
            /*int index = res.Count();
            if (index < 9)
                activityID = "0";
            activityID = submitTimePro + activityID + (index + 1).ToString();*/

            // 每天创建(创建后删除也算)超过99,会溢出
            if (res.Count() > 0)
                activityID = (int.Parse(res.ToList().Last().activityID) + 1).ToString(); // 如果不是今日第一次创建,取最后一个创建的活动ID+1
            else
                activityID = submitTimePro + "01"; // 如果是第一次创建,生成01

            // 处理数据格式
            int intActivityPlaceID = int.Parse(activityPlaceID);
            int intAvailableCredit = int.Parse(availableCredit);
            int intMaxSigned = int.Parse(maxSigned);
            int intSigned = int.Parse(signed);
            int intActivityState = int.Parse(activityState);
            int intActivityType = int.Parse(activityType);

            /*DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            DateTime dtSignStartDate = Convert.ToDateTime(signStartDate, dtFormat);
            DateTime dtSignEndDate = Convert.ToDateTime(signEndDate, dtFormat);
            DateTime dtHoldDate = Convert.ToDateTime(holdDate, dtFormat);
            DateTime dtSubmitTime = Convert.ToDateTime(submitTime);*/

            int intHoldStart = int.Parse(holdStart);
            int intHoldEnd = int.Parse(holdEnd);

            Activity A = new Activity()
            {
                activityID = this.activityID,
                activityName = this.activityName,
                activityIntro = this.activityIntro,
                activityPlaceID = intActivityPlaceID,
                activityOrgID = this.activityOrgID,
                availableCredit = intAvailableCredit,
                maxSigned = intMaxSigned,
                signed = intSigned,
                activityState = 1,
                signStartDate = Convert.ToDateTime(signStartDate),
                signEndDate = Convert.ToDateTime(signEndDate),
                holdDate = Convert.ToDateTime(holdDate),
                holdStart = intHoldStart,
                holdEnd = intHoldEnd,
                submitTime = DateTime.Now,
                failReason = "",
                activityType = intActivityType,
            };

            db.Activity.InsertOnSubmit(A);
            db.SubmitChanges();   // ?
        }

        public void Update()
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from A in db.Activity
                      where A.activityID == activityID
                      select A;

            Activity a = res.First();
            a.activityName = activityName;
            a.activityIntro = activityIntro;
            a.activityPlaceID = Convert.ToInt32(activityPlaceID);
            a.activityOrgID = activityOrgID;
            a.availableCredit = Convert.ToInt32(availableCredit);
            a.maxSigned = Convert.ToInt32(maxSigned);
            a.signed = Convert.ToInt32(signed);
            a.signStartDate = Convert.ToDateTime(signStartDate);
            a.signEndDate = Convert.ToDateTime(signEndDate);
            a.holdDate = Convert.ToDateTime(holdDate);
            a.holdStart = Convert.ToInt32(holdStart);
            a.holdEnd = Convert.ToInt32(holdEnd);
            a.submitTime = DateTime.Now;
            a.failReason = failReason;
            a.activityState = Convert.ToInt32(activityState);
            a.activityType = Convert.ToInt32(activityType);

            db.SubmitChanges();
        }

        public void Delete()
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from a in db.Activity
                      where a.activityID == activityID
                      select a;

            db.Activity.DeleteOnSubmit(res.First());
            db.SubmitChanges();
        }

        public void UpdateState()
        {
            // 根据当前时间更改活动关于时间的状态（待报名5，报名中6，待开始7，活动中8，已结束9）

            // 获取当前时间
            string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //string nowTime = "2028-04-04 14";
            int nowHour = int.Parse(nowTime.Substring(11));
            nowTime = nowTime.Substring(0, 10);

            /*            Console.WriteLine(nowTime);
                        Console.WriteLine(nowHour);*/

            // HttpContext.Current.Response.Write(activityName + " now:" + nowTime + " signStart:" + signStartDate + " signEnd:" + signEndDate + "\\");

            int state = int.Parse(activityState);
            if (int.Parse(activityState) == 2)
            {
                // 对于审核中（2）的活动，如果当前时间超过报名截止时间，则更新为审核过期（4）
                if (!Lesser(nowTime, SignEndDate))
                {
                    state = 4;
                }

                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from a in db.Activity
                          where a.activityID == activityID
                          select a;

                foreach (Activity a in res)
                    a.activityState = state;

                db.SubmitChanges();

                activityState = state.ToString();
            }

            if (int.Parse(activityState) >= 5 && int.Parse(activityState) <= 9)
            {
                // 对于审核通过的活动进行状态更新
                if (Lesser(nowTime, SignStartDate))
                {
                    // 待报名5
                    // 当前时间早于报名开始时间
                    state = 5;
                }
                else if (Lesser(nowTime, SignEndDate))
                {
                    // 报名中6
                    // 当前时间早于报名截止时间且不早于报名开始时间
                    state = 6;
                }
                else if (Lesser(nowTime, HoldDate))
                {
                    // 待开始7
                    // 当前时间晚于报名时段且早于举办时间
                    state = 7;
                }
                else if (Equals(nowTime, HoldDate))
                {
                    // 举办活动当天
                    if (nowHour < int.Parse(holdStart))
                    {
                        // 待开始7
                        state = 7;
                    }
                    else if (nowHour <= int.Parse(holdEnd))
                    {
                        // 活动中8
                        state = 8;
                    }
                    else
                    {
                        // 已结束9
                        state = 9;
                    }
                }
                else
                {
                    // 已结束9
                    state = 9;
                }

                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from a in db.Activity
                          where a.activityID == activityID
                          select a;

                foreach (Activity a in res)
                    a.activityState = state;

                db.SubmitChanges();

                activityState = state.ToString();
            }
        }

        public bool Lesser(string nowTime, string comparedTime)
        {
            bool lesser = false;

            string[] strNowTime = nowTime.Split('-');
            string[] strComTime = comparedTime.Split('-');

            if (int.Parse(strNowTime[0]) < int.Parse(strComTime[0])) lesser = true;
            else if (int.Parse(strNowTime[0]) == int.Parse(strComTime[0]))
            {
                if (int.Parse(strNowTime[1]) < int.Parse(strComTime[1])) lesser = true;
                else if (int.Parse(strNowTime[1]) == int.Parse(strComTime[1]))
                {
                    if (int.Parse(strNowTime[2]) < int.Parse(strComTime[2])) lesser = true;
                    else lesser = false;
                }
                else lesser = false;
            }
            else lesser = false;

            return lesser;
        }

        public string ActivityID { get => activityID; set => activityID = value; }
        public string ActivityName { get => activityName; set => activityName = value; }
        public string ActivityIntro { get => activityIntro; set => activityIntro = value; }
        public string ActivityPlaceID { get => activityPlaceID; set => activityPlaceID = value; }
        public string ActivityOrgID { get => activityOrgID; set => activityOrgID = value; }
        public string AvailableCredit { get => availableCredit; set => availableCredit = value; }
        public string MaxSigned { get => maxSigned; set => maxSigned = value; }
        public string Signed { get => signed; set => signed = value; }
        public string ActivityState { get => activityState; set => activityState = value; }
        public string SignStartDate { get => signStartDate; set => signStartDate = value; }
        public string SignEndDate { get => signEndDate; set => signEndDate = value; }
        public string HoldDate { get => holdDate; set => holdDate = value; }
        public string HoldStart { get => holdStart; set => holdStart = value; }
        public string HoldEnd { get => holdEnd; set => holdEnd = value; }
        public string SubmitTime { get => submitTime; set => submitTime = value; }
        public string FailReason { get => failReason; set => failReason = value; }
        public string ActivityType { get => activityType; set => activityType = value; }
    }
}