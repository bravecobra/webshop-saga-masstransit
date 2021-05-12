using Microsoft.EntityFrameworkCore;

namespace Webshop.Shared.EfCore
{
    public static class EfCoreExtensions{
        public static void Replace<T>(this DbContext context, T oldEntity, T newEntity) where T : class
        {
            context.ChangeTracker.TrackGraph(oldEntity, e => e.Entry.State = EntityState.Deleted);
            context.ChangeTracker.TrackGraph(newEntity, e => e.Entry.State = e.Entry.IsKeySet ? EntityState.Modified : EntityState.Added);
        }
    }

}
