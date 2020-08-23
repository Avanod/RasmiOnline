namespace RasmiOnline.Domain.Entity
{
    using System;
    using Properties;
    using Gnu.Framework.Core;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Survey), Schema = "Base")]
    public class Survey : IInsertDateProperties, ISoftDeleteProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SurveyId { get; set; }

        [Display(Name = nameof(DisplayName.IsDeleted), ResourceType = typeof(DisplayName))]
        public bool IsDeleted { get; set; }

        [Display(ResourceType = typeof(DisplayName), Name = nameof(DisplayName.InsertDateSh))]
        public DateTime InsertDateMi { get; set; }

        [Column(TypeName = "char")]
        [Display(ResourceType = typeof(DisplayName), Name = nameof(DisplayName.InsertDateSh))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [Display(Name = nameof(DisplayName.Subject), ResourceType = typeof(DisplayName))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(70, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(70, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Subject { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = nameof(DisplayName.Text), ResourceType = typeof(DisplayName))]
        public string Text { get; set; }

        public ICollection<SurveyOption> SurveyOptions { get; set; }
    }
}