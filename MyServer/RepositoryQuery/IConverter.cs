using System.Linq.Expressions;

namespace MyServer.RepositoryQuery
{
    public interface IConverter<TViewObject, TEntity>
        where TEntity : class
    {
        IEnumerable<Expression<Func<TEntity, object>>> Projections { get; }
    }
}
