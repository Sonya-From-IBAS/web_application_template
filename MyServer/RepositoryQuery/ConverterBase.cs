using System.Collections;
using System.Linq.Expressions;

namespace MyServer.RepositoryQuery
{
    public abstract class ConverterBase<TViewObject, TEntity> : IConverter<TViewObject, TEntity>
        where TEntity : class
    {
        #region public methods

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            return Convert(tuple);
        }

        public IList TransformList(IList collection)
        {
            return collection;
        }

        #endregion


        #region Implementation of IConverter

        public abstract IEnumerable<Expression<Func<TEntity, object>>> Projections { get; }

        #endregion
        protected abstract TViewObject Convert(object[] tuple);
    }
}
