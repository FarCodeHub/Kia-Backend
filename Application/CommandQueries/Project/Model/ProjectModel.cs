using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Project.Model
{
    public class ProjectModel : IMapFrom<Domain.Entities.Project>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public int PriorityBaseId { get; set; }
        public string PriorityBaseTitle { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Address { get; set; }
        public int ContractsCounter { get; set; }
        public ICollection<ProjectModel?> Children { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Project, ProjectModel>()
                .ForMember(x=>x.ContractsCounter,opt=>opt.MapFrom(x=>x.ContractProjects.Count))
                .ForMember(x => x.PriorityBaseTitle, opt => opt.MapFrom(x => x.PriorityBase.Title))
                .ForMember(x => x.Lat, opt => opt.MapFrom(x => x.Location.PointOnSurface.Y))
                .ForMember(x => x.Lng, opt => opt.MapFrom(x => x.Location.PointOnSurface.X))
                .ForMember(x => x.Children, opt => opt.MapFrom(x => x.InverseParent))
                ;
        }
    }
}
