using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;


namespace PalisaidMeta.Model
{
    public abstract class Entity : IDisposable
	{
		public Entity()
		{
			if (string.IsNullOrEmpty(EntityId))
			{
				EntityId = Guid.NewGuid().ToString();
			}

			TenantId = BaseConstants.DefaultTenantId;
			OwnerId = TenantId;
			
			CreateDate = DateTimeOffset.Now;
			LastUpdate = DateTimeOffset.Now;
			IsActive = true;
		}

		public Entity(Guid tenantId, Guid ownerId)
		{
			if (string.IsNullOrEmpty(EntityId))
			{
				EntityId = Guid.NewGuid().ToString();
			}

			OwnerId = ownerId;
			TenantId = tenantId;

			if (tenantId == Guid.Empty)
			{
				TenantId = BaseConstants.DefaultTenantId;
			}

			if (ownerId == Guid.Empty)
			{
				OwnerId = TenantId;
			}
		}

		#region internalkeys
		string _entityId = string.Empty;
		
		[Key]
		public string EntityId 
		{ 
			get => _entityId; 
			set => _entityId = value ?? Guid.NewGuid().ToString(); 
		}

		public Guid OwnerId { get; set; } = Guid.Empty;
		public Guid TenantId { get; set; } = Guid.Empty;
		#endregion internalkeys

		/// <summary>
		/// The hash of the original data that was used to create this entity. This is used to determine if the data has changed, 
		/// and if so, the entity needs to be updated.
		/// </summary>
		public string OriginHash { get; set; } = string.Empty;

		public string GenerateOriginHash(string data)
		{
			HashAlgorithm hashAlgorithm = SHA256.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] bytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < bytes.Length; i++)
            {
                sBuilder.Append(bytes[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
		}
		
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

		public void MarkAsDeleted()
		{
			MarkAsUpdated();
			IsActive = false;
			IsDeleted = true;
		}

		public void MarkAsUnDeleted()
		{
			MarkAsUpdated();
			IsDeleted = false;
			IsActive = true;
		}

		#region dispose
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
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

