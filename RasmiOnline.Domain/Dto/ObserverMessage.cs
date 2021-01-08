namespace RasmiOnline.Domain.Dto
{
    using System;
    using System.Collections.Generic;

    public class Concrete
    {
        public string Key { get; set; }
        public List<string> Observers { get; set; }
    }

    public class ObserverMessage
    {
        public string Key { get; set; }
        public Guid UserId { get; set; }
        public Guid OfficeUserId { get; set; }
        public string SmsContent { get; set; }
        public string BotContent { get; set; }
        public string Subject { get; set; }
        public int RecordId { get; set; }
        public int MessageId { get; set; }
    }
}