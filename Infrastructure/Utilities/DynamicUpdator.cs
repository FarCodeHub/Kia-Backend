using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Utilities
{
    public class DynamicUpdator
    {
        private readonly IModel _model;

        public DynamicUpdator(IModel model)
        {
            _model = model;
        }

        public TEntity Update<TEntity>(TEntity entity, TEntity updatEntity) where TEntity : class
        {
            var findEntityType = _model.FindEntityType(typeof(TEntity));

            foreach (var updatEntityProperty in updatEntity.GetType().GetProperties())
            {
                foreach (var entityProperty in entity.GetType().GetProperties())
                {
                    if (entityProperty.Name == updatEntityProperty.Name)
                    {
                        if (entityProperty.GetValue(entity) != updatEntityProperty.GetValue(updatEntity))
                        {
                            if (updatEntityProperty.GetValue(updatEntity) == null)
                            {
                                if (findEntityType.GetProperty(entityProperty.Name).IsNullable)
                                {
                                    entityProperty.SetValue(entity, updatEntityProperty.GetValue(updatEntity));
                                }
                            }
                            else
                            {
                                entityProperty.SetValue(entity, updatEntityProperty.GetValue(updatEntity));
                            }
                        }
                    }
                }
            }
            return entity;
        }
    }
}