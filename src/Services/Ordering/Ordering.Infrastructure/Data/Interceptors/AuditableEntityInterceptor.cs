using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		UpdateEntities(eventData.Context);
		return base.SavingChanges(eventData, result);
	}

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
		CancellationToken cancellationToken = new CancellationToken())
	{
		UpdateEntities(eventData.Context);
		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private static void UpdateEntities(DbContext? context)
	{
		if (context == null) return;

		foreach (var entry in context.ChangeTracker.Entries<IEntity>())
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedBy = "mehmet";
				entry.Entity.CreatedAt = DateTime.UtcNow;
			}

			if (entry.State != EntityState.Added && entry.State != EntityState.Modified &&
			    !entry.HasChangedOwnedEntities()) continue;
			entry.Entity.LastModifiedBy = "mehmet";
			entry.Entity.LastModified = DateTime.UtcNow;
		}
	}
}

public static class Extensions
{
	/// <summary>
	/// Determines whether an entity has any owned entities (a special type of related entity) that have been added or modified
	/// </summary>
	/// <param name="entry"></param>
	/// <returns></returns>
	public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
		// References is a collection of all navigation properties (relationships) of the entity that point to other entities, including owned types.
		entry.References.Any(r =>
			// Ensures the referenced entity (the "target" of the navigation) is actually loaded and tracked by the change tracker.
			// If TargetEntry is null, the related entity isn't tracked, so it’s skipped.
			r.TargetEntry != null &&
			// Owned entities are a concept in EF Core for modeling 'value objects' or tightly coupled data that shouldn't exist independently of their parent entity.
			// They are typically used for things like addresses, payment details, or other complex types that are "owned" by another entity (e.g., an Order might own an Address).
			r.TargetEntry.Metadata.IsOwned() &&
			// When an owned entity is added or modified, it’s often desirable to treat this as a modification to the parent entity, even if the parent’s scalar properties haven’t changed.
			r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}