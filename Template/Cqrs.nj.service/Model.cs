using Application.Common.Mappings;
using AutoMapper;

namespace $rootnamespace$.$fileinputname$.Model
{
    public class $safeitemname$ : IMapFrom<Domain.Entities.$fileinputname$>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.$fileinputname$, $safeitemname$>();
        }
    }
}
