namespace RasmiOnline.Domain.Entity
{
    using System;
    using Properties;
    using Gnu.Framework.Core;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(SurveyOption), Schema = "Base")]
    public class SurveyOption : IInsertDateProperties, ISoftDeleteProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SurveyOptionId { get; set; }

        [ForeignKey(nameof(SurveyId))]
        [Display(Name = nameof(DisplayName.Survey), ResourceType = typeof(DisplayName))]
        public Survey Survey { get; set; }

        [Display(Name = nameof(DisplayName.Survey), ResourceType = typeof(DisplayName))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int SurveyId { get; set; }

        [Display(Name = nameof(DisplayName.SelectedOption), ResourceType = typeof(DisplayName))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int SelectedOption { get; set; }

        [Display(Name = nameof(DisplayName.IsDeleted), ResourceType = typeof(DisplayName))]
        public bool IsDeleted { get; set; }

        [Display(ResourceType = typeof(DisplayName), Name = nameof(DisplayName.InsertDateSh))]
        public DateTime InsertDateMi { get; set; }

        [Column(TypeName = "char")]
        [Display(ResourceType = typeof(DisplayName), Name = nameof(DisplayName.InsertDateSh))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = nameof(DisplayName.Text), ResourceType = typeof(DisplayName))]
        [MaxLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Text { get; set; }
    }
}