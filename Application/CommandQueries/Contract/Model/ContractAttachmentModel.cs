using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Contract.Model
{
    public class ContractAttachmentModel : IMapFrom<Domain.Entities.ContractAttachment>
    {
        public int ContractId { get; set; } = default!;
        public string FilePath { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ContractAttachment, ContractAttachmentModel>().IgnoreAllNonExisting()
                ;
        }
    }
}