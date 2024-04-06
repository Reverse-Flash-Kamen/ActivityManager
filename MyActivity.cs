using ActivityManager.App_Data;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ActivityManager
{
    public class MyActivity
    {
        private string activityID;
        private string activityName;
        private string activityIntro;
        private int activityPlaceID;
        private string activityOrgID;
        private int availableCredit;
        private int maxSigned;
        private int signed;
        private int activityState;
        private string signStartDate;
        private string signEndDate;
        private string holdDate;
        private int holdStart;
        private int holdEnd;
        private string submitTime = "1900-01-01 00:00:00";
        private string failReason;
        private int activityType;
        private string checkInCode;
        private string checkOutCode;

        public string ActivityID { get => activityID; set => activityID = value; }
        public string ActivityName { get => activityName; set => activityName = value; }
        public string ActivityIntro { get => activityIntro; set => activityIntro = value; }
        public int ActivityPlaceID { get => activityPlaceID; set => activityPlaceID = value; }
        public string ActivityOrgID { get => activityOrgID; set => activityOrgID = value; }
        public int AvailableCredit { get => availableCredit; set => availableCredit = value; }
        public int MaxSigned { get => maxSigned; set => maxSigned = value; }
        public int Signed { get => signed; set => signed = value; }
        public int ActivityState { get => activityState; set => activityState = value; }
        public string SignStartDate { get => signStartDate; set => signStartDate = value; }
        public string SignEndDate { get => signEndDate; set => signEndDate = value; }
        public string HoldDate { get => holdDate; set => holdDate = value; }
        public int HoldStart { get => holdStart; set => holdStart = value; }
        public int HoldEnd { get => holdEnd; set => holdEnd = value; }
        public string SubmitTime { get => submitTime; set => submitTime = value; }
        public string FailReason { get => failReason; set => failReason = value; }
        public int ActivityType { get => activityType; set => activityType = value; }
        public string CheckInCode { get => checkInCode; set => checkInCode = value; }
        public string CheckOutCode { get => checkOutCode; set => checkOutCode = value; }

        public MyActivity()
        { }

        public MyActivity(String id)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from activity in db.Activity
                      where activity.activityID == id
                      select activity;

            if (res.Any())
            {
                var a = res.First();
                activityID = a.activityID.ToString().Trim();
                activityName = a.activityName.ToString().Trim();
                activityIntro = a.activityIntro.ToString().Trim();
                activityPlaceID = a.activityPlaceID;
                activityOrgID = a.activityOrgID.ToString().Trim();
                availableCredit = a.availableCredit;
                maxSigned = a.maxSigned;
                signed = a.signed;
                activityState = a.activityState;
                signStartDate = Convert.ToDateTime(a.signStartDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                signEndDate = Convert.ToDateTime(a.signEndDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                holdDate = Convert.ToDateTime(a.holdDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                holdStart = a.holdStart;
                holdEnd = a.holdEnd;
                submitTime = Convert.ToDateTime(a.submitTime).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                activityType = a.activityType;
                if (a.failReason != null) failReason = a.failReason.ToString().Trim();
                if (a.checkInCode != null) checkInCode = a.checkInCode.ToString().Trim();
                if (a.checkOutCode != null) checkOutCode = a.checkOutCode.ToString().Trim();
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert('数据加载错误,请稍后重试！')</script>");
            }
        }

        public MyActivity(Activity a)
        {
            activityID = a.activityID.ToString().Trim();
            activityName = a.activityName.ToString().Trim();
            activityIntro = a.activityIntro.ToString().Trim();
            activityPlaceID = a.activityPlaceID;
            activityOrgID = a.activityOrgID.ToString().Trim();
            availableCredit = a.availableCredit;
            maxSigned = a.maxSigned;
            signed = a.signed;
            activityState = a.activityState;
            signStartDate = Convert.ToDateTime(a.signStartDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            signEndDate = Convert.ToDateTime(a.signEndDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            holdDate = Convert.ToDateTime(a.holdDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            holdStart = a.holdStart;
            holdEnd = a.holdEnd;
            submitTime = Convert.ToDateTime(a.submitTime).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            activityType = a.activityType;
            if (a.failReason != null) failReason = a.failReason.ToString().Trim();
            if (a.checkInCode != null) checkInCode = a.checkInCode.ToString().Trim();
            if (a.checkOutCode != null) checkOutCode = a.checkOutCode.ToString().Trim();
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
            int intActivityPlaceID = activityPlaceID;
            int intAvailableCredit = availableCredit;
            int intMaxSigned = maxSigned;
            int intSigned = signed;
            int intActivityState = activityState;
            int intActivityType = activityType;

            /*DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            DateTime dtSignStartDate = Convert.ToDateTime(signStartDate, dtFormat);
            DateTime dtSignEndDate = Convert.ToDateTime(signEndDate, dtFormat);
            DateTime dtHoldDate = Convert.ToDateTime(holdDate, dtFormat);
            DateTime dtSubmitTime = Convert.ToDateTime(submitTime);*/

            int intHoldStart = holdStart;
            int intHoldEnd = holdEnd;

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
                // failReason = "",
                activityType = intActivityType,
                // checkInCode = this.checkInCode,
                // checkOutCode = this.checkOutCode,
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
            if (failReason != null) a.failReason = failReason;
            a.activityState = Convert.ToInt32(activityState);
            a.activityType = Convert.ToInt32(activityType);
            if (checkInCode != null) a.checkInCode = checkInCode;
            if (checkOutCode != null) a.checkOutCode = checkOutCode;

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

            int state = activityState;
            ActivityManagerDataContext db = new ActivityManagerDataContext();

            if (activityState == 2)
            {
                // 对于审核中（2）的活动，如果当前时间超过报名截止时间，则更新为审核过期（4）
                if (!Lesser(nowTime, SignEndDate))
                {
                    state = 4;
                }

                var res = from a in db.Activity
                          where a.activityID == activityID
                          select a;

                foreach (Activity a in res)
                    a.activityState = state;

                db.SubmitChanges();

                activityState = state;
            }

            if (activityState >= 5 && activityState <= 9)
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
                    if (nowHour < holdStart)
                    {
                        // 待开始7
                        state = 7;
                    }
                    else if (nowHour <= holdEnd)
                    {
                        // 活动中8
                        state = 8;

                        // 同时更改场地的状态
                        var resPlaceState = from info in db.Place
                                            where info.placeID == activityPlaceID
                                            select info;

                        resPlaceState.First().placeState = 1; // 使用中
                        db.SubmitChanges();
                    }
                    else
                    {
                        // 已结束9
                        state = 9;
                        var resPlaceState = from info in db.Place
                                            where info.placeID == activityPlaceID
                                            select info;

                        resPlaceState.First().placeState = 0; // 结束使用
                        db.SubmitChanges();
                    }
                }
                else
                {
                    // 已结束9
                    state = 9;
                }

                var res = from a in db.Activity
                          where a.activityID == activityID
                          select a;

                foreach (Activity a in res)
                    a.activityState = state;

                db.SubmitChanges();

                activityState = state;
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
    }
}