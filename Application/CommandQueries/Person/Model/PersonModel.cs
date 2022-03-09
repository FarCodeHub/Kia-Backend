using System;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Person.Model
{
    public class PersonModel : IMapFrom<Domain.Entities.Person>
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName { get; set; }


        public string? FatherName { get; set; }
        public string? NationalCode { get; set; }
        public string? IdentityCode { get; set; }
        public int? BirthPlaceId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? GenderBaseId { get; set; } = default!;
        public string? Email { get; set; }
        public string? PostalCode { get; set; }
        public int? ZipCodeId { get; set; }
        public string? Address { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public string GenderBaseTitle { get; set; }
        public string BirthPlaceTitle { get; set; }
        public string ZipCodePlaceTitle { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone3 { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Person, PersonModel>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName))
                .ForMember(x=>x.BirthPlaceTitle,opt=>opt.MapFrom(x=> $"{x.BirthPlace.Parent.Title}-{x.BirthPlace.Title}"))
                .ForMember(x=>x.ZipCodePlaceTitle,opt=>opt.MapFrom(x=> $"{x.ZipCode.Parent.Title}-{x.ZipCode.Title}"))
                .ForMember(x => x.GenderBaseTitle, opt => opt.MapFrom(x => x.GenderBase.Title));
        }
    }
}
