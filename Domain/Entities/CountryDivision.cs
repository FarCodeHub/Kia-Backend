using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CountryDivision : BaseEntity
    {
        public int? ParentId { get; set; }
        public string Title { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual User ModifiedBy { get; set; }
        public virtual Role OwnerRole { get; set; }
        public virtual CountryDivision? Parent { get; set; }
        public virtual ICollection<CountryDivision> InverseParent { get; set; }
        public virtual ICollection<Person> PersonBirthPlaces { get; set; }
        public virtual ICollection<Person> PersonZipCodes { get; set; }
    }
}
