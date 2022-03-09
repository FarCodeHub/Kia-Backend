using System.Collections.Generic;
using Infrastructure.Interfaces;

namespace Domain.Entities
{
    public partial class BaseValue : BaseEntity,IHierarchical
    {
        public int BaseValueTypeId { get; set; } = default!;
        public int? ParentId { get; set; }
        public string LevelCode { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string UniqueName { get; set; } = default!;
        public string? Value { get; set; }
        public int OrderIndex { get; set; } = default!;
        public bool IsReadOnly { get; set; } = default!;
        public bool IsActive { get; set; } = default!;

        public virtual BaseValue? Parent { get; set; }
        public virtual BaseValueType BaseValueType { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<Advertisement> AdvertisementAdvertisementSourceBases { get; set; } = default!;
        public virtual ICollection<Advertisement> AdvertisementAdvertisementTypeBases { get; set; } = default!;
        public virtual ICollection<Advertisement> AdvertisementHeadLineNumberBases { get; set; } = default!;
        public virtual ICollection<Communication> Communications { get; set; } = default!;
        public virtual ICollection<Person> Persons { get; set; } = default!;
        public virtual ICollection<Project> Projects { get; set; } = default!;
        public virtual ICollection<Question> QuestionAnswerTypeBases { get; set; } = default!;
        public virtual ICollection<Question> QuestionQuestionTypBases { get; set; } = default!;
        public virtual ICollection<SessionSurvey> SessionSurveys { get; set; } = default!;
        public virtual ICollection<Task> TaskResultBases { get; set; } = default!;
        public virtual ICollection<Task> TaskTypeBases { get; set; } = default!;
        public virtual ICollection<Case> Cas { get; set; }

    }
}
