using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;
using model;

namespace bll
{
    /// <summary>
    /// 创建job
    /// </summary>
    public class CreateJob
    {


        //private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CreateJob));//log4net 日志
        public IScheduler CreateSched(string cronExpression, string trigger_str, string group_str, Type ijob)
        {

            try
            {
                CronScheduleBuilder csb = CronScheduleBuilder.CronSchedule(cronExpression);
                ITrigger trigger = TriggerBuilder.Create().WithIdentity(trigger_str, group_str).WithSchedule(csb).Build();
                IJobDetail job = JobBuilder.Create(ijob).Build();
                ISchedulerFactory sf = new StdSchedulerFactory();
                IScheduler sched = sf.GetScheduler();
                sched.ScheduleJob(job, trigger);
                string info = string.Format("[{0}],创建成功", cronExpression);
                logger.Info(info);
                MyDelegateFunc.WriteFmLog(info);
                return sched;
            }
            catch (Exception ex)
            {

                logger.Error(ex.Message);
                MyDelegateFunc.WriteFmLog(ex.Message);
            }
            return null;
        }




        /// <summary>
        /// 验证表达试
        /// </summary>
        /// <param name="str">表达试</param>
        /// <returns></returns>
        public bool CheckCronExpression(string expression)
        {
            string msg = "";
            if (!CronExpression.IsValidExpression(expression))
            {
                msg = string.Format("表达式：[{0}],无效", expression);

                return false;
            }

            return true;
        }

    }//end
}//end
