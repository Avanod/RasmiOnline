namespace RasmiOnline.Data.Log
{
    using System;
    using Domain.Enum;
    using System.Linq;
    using Domain.Entity;
    using System.Data.Entity;
    using System.Collections.Generic;
    using Gnu.Framework.EntityFramework;
    using Gnu.Framework.AspNet.HttpRequest;

    public class AuditLoger
    {
        public static void Log(DbContext context, Guid userId)
        {
            var listAudit = new List<ActivityLog>();
            var tableName = string.Empty;
            foreach (var entry in context.ChangeTracker.Entries())
            {
                tableName = entry.Entity.GetType().Name;
                tableName = tableName.Contains("_") ? tableName.Substring(0, tableName.IndexOf('_')) : tableName;
                switch (entry.State)
                {
                    case EntityState.Added:
                        #region Added
                        listAudit.Add(new ActivityLog
                        {
                            Type = ActivityLogType.Insert,
                            UserId = userId,
                            Ip = HttpWebRequest.GetIP(),
                            LogData = EntitySerializer.SerializeRecordToJson(tableName, null, entry.CurrentValues.ToObject(), ActivityLogType.Insert),
                        });
                        break;
                    #endregion

                    case EntityState.Modified:
                        #region Modified
                        listAudit.Add(new ActivityLog
                        {
                            Type = ActivityLogType.Update,
                            UserId = userId,
                            Ip = HttpWebRequest.GetIP(),
                            LogData = EntitySerializer.SerializeRecordToJson(tableName, entry.GetDatabaseValues().ToObject(), entry.CurrentValues.ToObject(), ActivityLogType.Update),
                        });
                        break;
                    #endregion

                    case EntityState.Deleted:
                        #region Deleted
                        listAudit.Add(new ActivityLog
                        {
                            Type = ActivityLogType.Delete,
                            UserId = userId,
                            Ip = HttpWebRequest.GetIP(),
                            LogData = EntitySerializer.SerializeRecordToJson(tableName, entry.OriginalValues.ToObject(), null, ActivityLogType.Delete),
                        });
                        break;
                        #endregion
                }
            }
            context.Set<ActivityLog>().AddRange(listAudit);
        }

        public static bool DeleteLog(DbContext context, long[] activityLogId)
        {
            var result = context.Set<ActivityLog>().RemoveRange(context.Set<ActivityLog>().Where(x => activityLogId.Contains(x.ActivityLogId)));
            return context.SaveChanges().ToSaveChangeResult();
        }
    }
}
