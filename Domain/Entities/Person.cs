using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Person : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        public string? NationalCode { get; set; }
        public string? IdentityCode { get; set; }
        public int? BirthPlaceId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? GenderBaseId { get; set; }
        public string? Email { get; set; }
        public string? PostalCode { get; set; }
        public int? ZipCodeId { get; set; }
        public string? Address { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone3 { get; set; }
        public string? ProfilePhotoUrl { get; set; }


        public virtual Customer Customer { get; set; }
        public virtual CountryDivision? BirthPlace { get; set; } = default!;
        public virtual BaseValue? GenderBase { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual CountryDivision? ZipCode { get; set; } = default!;
        public virtual Employee Employee { get; set; } = default!;
        public virtual ICollection<User> Users { get; set; } = default!;
    }
}
