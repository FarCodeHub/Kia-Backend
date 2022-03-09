using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Service.Services.Unit
{
    public class UnitService : IUnitService
    {
        private readonly IRepository _repository;
        private readonly IUnitPositionService _unitPositionService;

        public UnitService(IRepository repository, IUnitPositionService unitPositionService)
        {
            _repository = repository;
            _unitPositionService = unitPositionService;
        }

        public IQueryable<Domain.Entities.Unit> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Unit>();
        }



        public IQueryable<Domain.Entities.Unit> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Unit>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Unit>> Add(Domain.Entities.Unit inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Unit>> Add(Domain.Entities.Unit inpute, List<int> positionsId)
        {
            var unit = _repository.Insert(inpute);
            foreach (var i in positionsId)
            {
                await _unitPositionService.Add(new Domain.Entities.UnitPosition() { PositionId = i, Unit = unit.Entity });
            }

            return unit;
        }

        public async Task<EntityEntry<Domain.Entities.Unit>> Update(Domain.Entities.Unit inpute, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Unit>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.ParentId = inpute.ParentId;
            entity.BranchId = inpute.BranchId;
            entity.Title = inpute.Title;
            return _repository.Update(entity);

        }

        public async Task<EntityEntry<Domain.Entities.Unit>> Update(Domain.Entities.Unit inpute, List<int> positionsId,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Unit>(c =>
                    c.ObjectId(inpute.Id)).Include(x => x.UnitPositions)
                .FirstOrDefaultAsync(cancellationToken);

            entity.ParentId = inpute.ParentId;
            entity.BranchId = inpute.BranchId;
            entity.Title = inpute.Title;


            foreach (var removedPermission in entity.UnitPositions.Select(x => x.PositionId).Except(positionsId))
            {
                var deletingEntity = await _unitPositionService.GetByUnitIdAndPositionId(entity.Id, removedPermission)
                    .FirstOrDefaultAsync(cancellationToken);

                await _unitPositionService.SoftDelete(deletingEntity.Id, cancellationToken);
            }

            foreach (var addedPermission in positionsId.Except(entity.UnitPositions.Select(x => x.PositionId)))
            {
                await _unitPositionService.Add(new Domain.Entities.UnitPosition()
                {
                    PositionId = entity.Id,
                    UnitId = addedPermission
                });
            }



            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Unit>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Unit>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Unit> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Unit>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Unit>(queries))
                   );
        }

        public IQueryable<Domain.Entities.Unit> GetAllByBranchId( int branchId)
        {
            return _repository.GetAll<Domain.Entities.Unit>(cfg =>
                cfg.ConditionExpression(x => x.BranchId == branchId));
        }


      
    }
}