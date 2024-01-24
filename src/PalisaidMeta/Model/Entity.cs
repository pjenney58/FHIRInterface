using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace PalisaidMeta.Model
{
	public abstract class Entity : IDisposable
	{
		public Entity()
		{
            CreateDate = DateTimeOffset.Now;
            IsActive = false;
        }

		public Entity(Guid OwnerId, Guid TenantId)
		{
			if(EntityId == Guid.Empty)
			{
				EntityId = Guid.NewGuid();
			}

			if(TenantId == Guid.Empty)
			{
				throw new ArgumentException("TenantId cannot be empty");
			}

			this.OwnerId = OwnerId;
			this.TenantId = TenantId;
		}

		// Fhir doesn't necessarily use a uuid for the key, so we need to be able to set capture it as a long
		public long EntityKey { get; set; }
		
		#region key
		[Key]
		public Guid EntityId { get; set; } = Guid.Empty;
		public Guid OwnerId { get; set; } = Guid.Empty;
        public Guid TenantId { get; set; } = Guid.Empty;
        #endregion key

		/// <summary>
		/// The hash of the original data that was used to create this entity. This is used to determine if the data has changed, 
		/// and if so, the entity needs to be updated.
		/// </summary>
		public string OriginHash { get; set; } = string.Empty;

        public long Version { get; set; }

        public DateTimeOffset CreateDate { get; set; }
		public DateTimeOffset LastUpdate { get; set; }

		public bool IsActive { get; set; } = true;
		public bool IsDeleted { get; set; } = false;

		public void MarkAsUpdated()
		{
			Version++;
			LastUpdate = DateTimeOffset.Now;
		}

		protected void Delete()
		{
			MarkAsUpdated();
			IsActive = false;
			IsDeleted = true;
		}

		protected void UnDelete()
		{
			MarkAsUpdated();
			IsDeleted = false;
			IsActive = true;
		}

        #region dispose
        protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{ }
		}

        public void Dispose()
        {
			Dispose(true);
			GC.SuppressFinalize(this);
        }
        #endregion dispose
    }
}

