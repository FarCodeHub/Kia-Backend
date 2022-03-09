using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Unit.Model
{
    public class UnitModel : IMapFrom<Domain.Entities.Unit>
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Title { get; set; }
        public string LevelCode { get; set; }
        public int? ParentId { get; set; }
        public string BranchTitle { get; set; }
        public string ParentTitle { get; set; }
        public ICollection<int> PositionsIdList { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Unit, UnitModel>()
                .ForMember(x=>x.BranchTitle,opt=>opt.MapFrom(x=>x.Branch.Title))
                .ForMember(x=>x.ParentTitle,opt=>opt.MapFrom(x=>x.Parent.Title))
                .ForMember(x=>x.PositionsIdList,opt=>opt.MapFrom(x=>x.UnitPositions.Select(x=>x.PositionId)));
        }
    }
}
