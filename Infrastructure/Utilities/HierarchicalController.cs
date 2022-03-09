using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Attributes;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Utilities
{
    public class HierarchicalController : IHierarchicalController
    {
        private readonly IUnitOfWork _unitOfWork;
        private string LastSideLevelCode { get; set; }
        public IModel Model { get; set; }

        public HierarchicalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this.Model = _unitOfWork.Model();
        }

        public async Task<TEntity> SetLevelCode<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (typeof(IHierarchical).IsAssignableFrom(typeof(TEntity)))
            {
                if (((IHierarchical)entity).ParentId != default)
                {
                    var parentLevelCode = (await _unitOfWork.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == ((IHierarchical)entity).ParentId) as IHierarchical)?.LevelCode;
                    var previouseLevelCode = (await _unitOfWork.Set<TEntity>().AsNoTracking().OrderBy(x => x.Id)
                        .LastOrDefaultAsync(x => (x as IHierarchical).ParentId == ((IHierarchical)entity).ParentId) as IHierarchical)?.LevelCode;

                    if (previouseLevelCode != default)
                    {

                        if (LastSideLevelCode != default && !LastSideLevelCode.Substring(0, LastSideLevelCode.Length - 4)
                            .Equals(parentLevelCode))
                        {
                            LastSideLevelCode = default;
                        }


                        if (LastSideLevelCode == default)
                        {
                            LastSideLevelCode = previouseLevelCode?.Substring(parentLevelCode.Length,
                                previouseLevelCode.Length - parentLevelCode.Length);
                        }

                        var i = LastSideLevelCode.Length > 4 ? int.Parse(LastSideLevelCode.Substring(LastSideLevelCode.Length - 4, 4)) : int.Parse(LastSideLevelCode);
                        LastSideLevelCode = $"{parentLevelCode}{++i:0000}";
                        ((IHierarchical)entity).LevelCode = LastSideLevelCode;
                    }
                    else
                    {
                        ((IHierarchical)entity).LevelCode = $"{parentLevelCode}0001";
                    }
                }
                else if ((entity as IHierarchical).LevelCode.Length > 4)
                {
                    var previouseLevelCode = (await _unitOfWork.Set<TEntity>().AsNoTracking().OrderBy(x => x.Id)
                        .LastOrDefaultAsync(x => (x as IHierarchical).ParentId == null || (x as IHierarchical).ParentId == 0) as IHierarchical)?.LevelCode;
                    var i = int.Parse(previouseLevelCode);
                    var newLevelCode = $"{++i:0000}";

                    ((IHierarchical)entity).LevelCode = newLevelCode;
                }
            }

            return entity;
        }

        public async Task<TEntity> UpdateLevelCode<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (typeof(IBaseValueType).IsAssignableFrom(typeof(TEntity)))
            {

            }
            if (typeof(IHierarchical).IsAssignableFrom(typeof(TEntity)))
            {
                var aparentIdHasChanged = ((IHierarchical)entity).ParentId != (await _unitOfWork.Set<TEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == entity.Id) as IHierarchical)?.ParentId;

                var lastLevelCode = ((IHierarchical)entity).LevelCode;
                if (aparentIdHasChanged)
                {
                    var res = await SetLevelCode(entity);
                    await _unitOfWork.SaveAsync(new CancellationToken());
                    foreach (var child in _unitOfWork.Set<TEntity>()
                        .Where(x => ((IHierarchical)x).LevelCode.StartsWith(lastLevelCode)))
                    {
                        var childLevelCode = ((IHierarchical)child).LevelCode;
                        var newLevelCode = ((IHierarchical)res).LevelCode +
                                           childLevelCode.Remove(0, ((IHierarchical)res).LevelCode.Length);
                        ((IHierarchical)child).LevelCode = newLevelCode;
                    }

                    return res;
                }
                else
                {
                    return entity;
                }
            }
            else
            {
                return entity;
            }
        }

        public TEntity DeleteChildren<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            foreach (var child in _unitOfWork.Set<TEntity>().Where(x => ((IHierarchical)x).LevelCode.StartsWith(((IHierarchical)entity).LevelCode)))
            {
                DeleteUniqueProperty(child);
            }

            return entity;
        }

        public TEntity DeleteUniqueProperty<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (typeof(TEntity).GetCustomAttributes(false).Any(x => x.GetType() == typeof(HasUniqueIndex)))
            {
                foreach (var propertyInfo in typeof(TEntity).GetProperties().Where(x => x.GetCustomAttributes(false).Any(x => x.GetType() == typeof(UniqueIndex))))
                {
                    var findEntityType = Model.FindEntityType(typeof(TEntity));
                    var maxChar = 0;
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        maxChar = findEntityType.GetProperty(propertyInfo.Name).GetMaxLength() - (propertyInfo.GetValue(entity) as string)?.Length ?? 0;
                    }

                    propertyInfo.SetValue(entity, propertyInfo.GetValue(entity) + RandomMaker.GenerateRandomString(maxChar));
                }
            }

            return entity;
        }
    }


    public interface IHierarchicalController
    {
        TEntity DeleteUniqueProperty<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        Task<TEntity> SetLevelCode<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        Task<TEntity> UpdateLevelCode<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
    }
}