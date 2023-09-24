using System;
using System.ComponentModel.DataAnnotations;

namespace DataShapes.Model
{
	public abstract class Entity : IDisposable
	{
		public Entity()
		{
            EntityId = Guid.NewGuid();
            CreateDate = DateTimeOffset.Now;
            Version = 1;
            IsActive = true;
        }

		public Entity(Guid OwnerId, Guid TenantId)
		{
			if(EntityId == Guid.Empty)
			{
				EntityId = Guid.NewGuid();
				CreateDate = DateTimeOffset.Now;
				Version = 1;
				IsActive = true;
			}

			this.OwnerId = OwnerId;
			this.TenantId = TenantId;
		}

		#region key
		[Key]
		public Guid EntityId { get; set; } = Guid.Empty;
		public Guid OwnerId { get; set; } = Guid.Empty;
        public Guid TenantId { get; set; } = Guid.Empty;
        #endregion key

        public long Version { get; set; }

        public DateTimeOffset CreateDate { get; set; }
		public DateTimeOffset LastUpdate { get; set; }

		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }

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

