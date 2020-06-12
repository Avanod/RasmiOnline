namespace RasmiOnline.Domain.Entity
{
    using System;
    using Properties;
    using Domain.Enum;
    using Gnu.Framework.Core;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(ActivityLog), Schema = "Log")]
    public class ActivityLog : IInsertDateProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ActivityLogId { get; set; }

        [Display(Name = nameof(DisplayName.InsertDateSh), ResourceType = typeof(DisplayName))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public ActivityLogType Type { get; set; }

        [Display(Name = nameof(DisplayName.InsertDateMi), ResourceType = typeof(DisplayName))]
        public DateTime InsertDateMi { get; set; }

        [Column(TypeName = "char")]
        [Display(Name = nameof(DisplayName.InsertDateSh), ResourceType = typeof(DisplayName))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public Guid UserId { get; set; }

        [Column(TypeName = "varchar")]
        [Display(Name = nameof(DisplayName.Ip), ResourceType = typeof(DisplayName))]
        [MaxLength(15, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Ip { get; set; }

        [Column(TypeName = "nvarchar")]
        [Display(Name = nameof(DisplayName.LogData), ResourceType = typeof(DisplayName))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string LogData { get; set; }
    }
}
