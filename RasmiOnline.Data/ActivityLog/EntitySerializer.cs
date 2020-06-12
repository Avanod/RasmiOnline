namespace RasmiOnline.Data.Log
{
    using Domain.Enum;
    using System.Linq;
    using System.Xml.Linq;
    using Gnu.Framework.Core;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EntitySerializer
    {
        public static XElement SerializeRecordToXml(string tableName, object tableRecord)
        {
            string xml = "";
            try
            {
                xml = "<?xml version=\"1.0\"?><" + tableName + ">\n";
                var record = tableRecord.GetType().GetProperties();
                foreach (var field in record)
                {
                    if (!field.PropertyType.IsSerializable) continue;
                    if (field.PropertyType.Name == "Binary") continue;
                    xml += "<" + field.Name + ">" +
                           (field.GetValue(tableRecord, null) == null ? "" : field.GetValue(tableRecord, null).ToString()) +
                           "</" + field.Name + ">\n";
                }
                xml += "</" + tableName + ">";
                return XElement.Parse(xml);
            }
            catch
            {
                try
                {
                    xml = xml.Replace("&", "_");
                    return XElement.Parse(xml);
                }
                catch
                {
                    return XElement.Parse("<?xml version=\"1.0\"?><" + tableName + ">\n Parsing Error \n" +
                        "</" + tableName + ">");
                }
            }
        }

        public static string SerializeRecordToJson(string tableName, object tableRecord)
        {
            var Json = "";
            object fieldValue;
            try
            {
                Json = "{\"EntityName\":\"" + tableName + "\", \"Record\":{";
                var record = tableRecord.GetType().GetProperties();
                foreach (var field in record)
                {
                    if (!field.PropertyType.IsSerializable) continue;
                    if (field.PropertyType.Name == "Binary") continue;
                    fieldValue = field.GetValue(tableRecord, null);
                    Json += ("\"" + field.Name + "\":\"" + (fieldValue == null ? "" : fieldValue.ToString()) + "\",");
                }
                return Json.Substring(0, Json.Length - 1) + "}}";
            }
            catch
            {
                return ("{ \"Serializing Error\" : \"true\" }");
            }
        }

        public static string SerializeRecordToJson(string tableName, object oldRecord, object newRecord, ActivityLogType type)
        {
            var logData = new LogData { EntityName = tableName };
            var logDetails = new List<LogDetail>();
            object oldFieldValue;
            object newFieldValue;
            try
            {
                switch (type)
                {
                    case ActivityLogType.Insert:
                        {
                            #region Create Insert Log
                            var newData = newRecord != null ? newRecord.GetType().GetProperties() : null;
                            foreach (var newField in newData)
                            {
                                if (!newField.PropertyType.IsSerializable) continue;
                                if (newField.PropertyType.Name == "Binary") continue;
                                newFieldValue = newField.GetValue(newRecord, null);

                                var log = new LogDetail
                                {
                                    Name = newField.Name,
                                    Title = newField.GetAttribute(typeof(DisplayAttribute)) == null ? newField.Name : ((DisplayAttribute)newField.GetAttribute(typeof(DisplayAttribute))).GetName(),
                                    NewValue = newFieldValue != null ? newFieldValue.ToString() : string.Empty,
                                    OldValue = string.Empty
                                };
                                logDetails.Add(log);
                            }
                            #endregion
                        }
                        break;
                    case ActivityLogType.Update:
                        {
                            #region Create Update Log
                            var oldData = oldRecord != null ? oldRecord.GetType().GetProperties() : null;
                            var newData = newRecord != null ? newRecord.GetType().GetProperties() : null;
                            foreach (var oldField in oldData)
                            {
                                if (!oldField.PropertyType.IsSerializable) continue;
                                if (oldField.PropertyType.Name == "Binary") continue;
                                var newField = newData.FirstOrDefault(x => x.Name == oldField.Name);
                                newFieldValue = newField.GetValue(newRecord, null);
                                oldFieldValue = oldField.GetValue(oldRecord, null);

                                var log = new LogDetail
                                {
                                    Name = oldField.Name,
                                    Title = oldField.GetAttribute(typeof(DisplayAttribute)) == null ? oldField.Name : ((DisplayAttribute)oldField.GetAttribute(typeof(DisplayAttribute))).GetName(),
                                    NewValue = newFieldValue != null ? newFieldValue.ToString() : string.Empty,
                                    OldValue = oldFieldValue != null ? oldFieldValue.ToString() : string.Empty
                                };
                                logDetails.Add(log);
                            }
                            #endregion
                        }
                        break;
                    case ActivityLogType.Delete:
                        {
                            #region Create Delete Log
                            var oldData = oldRecord != null ? oldRecord.GetType().GetProperties() : null;
                            foreach (var oldField in oldData)
                            {
                                if (!oldField.PropertyType.IsSerializable) continue;
                                if (oldField.PropertyType.Name == "Binary") continue;
                                oldFieldValue = oldField.GetValue(oldRecord, null);

                                var log = new LogDetail
                                {
                                    Name = oldField.Name,
                                    Title = oldField.GetAttribute(typeof(DisplayAttribute)) == null ? oldField.Name : ((DisplayAttribute)oldField.GetAttribute(typeof(DisplayAttribute))).GetName(),
                                    NewValue = string.Empty,
                                    OldValue = oldFieldValue != null ? oldFieldValue.ToString() : string.Empty
                                };
                                logDetails.Add(log);
                            }
                            #endregion
                        }
                        break;
                }

                logData.LogDetails = logDetails;
                return logData.SerializeToJson();
            }
            catch
            {
                return ("{ \"Serializing Error\" : \"true\" }");
            }
        }
    }
}
