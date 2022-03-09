using System;
using System.Collections.Generic;
using System.Linq;
using Application.CommandQueries.Commission.Model;
using Application.CommandQueries.Employee.Model;
using Application.CommandQueries.Project.Model;
using Application.CommandQueries.Task.Model;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Contract.Model
{
    public class ContractModel : IMapFrom<Domain.Entities.Contract>
    {
        public int Id { get; set; }
        public int TaskId { get; set; } = default!;
        public string? Descriptions { get; set; }
        public TaskModel TaskModel { get; set; }
        public ICollection<ProjectModel> ProjectModels { get; set; }
        public ICollection<ContractAttachmentModel> ContractAttachmentModels { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByFullName { get; set; }
        public ICollection<CommissionModel> InvolvedEmployees { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Contract, ContractModel>()
                .ForMember(x=>x.TaskModel,opt=>opt.MapFrom(x=>x.Task))
                .ForMember(x=>x.CreatedAt,opt=>opt.MapFrom(x=>x.CreatedAt))
                .ForMember(x=>x.CreatedById,opt=>opt.MapFrom(x=>x.CreatedBy.Id))
                .ForMember(x=>x.CreatedByFullName,opt=>opt.MapFrom(x=>x.CreatedBy.Person.FirstName + " " + x.CreatedBy.Person.LastName))
                .ForMember(x=>x.ProjectModels,opt=>opt.MapFrom(x=>x.ContractProjects.Select(o=>o.Project)))
                .ForMember(x=>x.ContractAttachmentModels,opt=>opt.MapFrom(x=>x.ContractAttachments))
                .ForMember(x=>x.InvolvedEmployees,opt=>opt.MapFrom(x=>x.Commissions))
                ;
        }
    }
}
