using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace PalisaidMeta.Model
{
	public abstract class Entity : IDisposable
	{
		public Guid DefaultTenantId { get; set; } = new Guid("146b7ab1-d6b0-436e-8b7c-e96f39861a26");

		public Entity()
		{
			if (EntityId == Guid.Empty)
			{
				EntityId = Guid.NewGuid();
			}

			TenantId = DefaultTenantId;
			OwnerId = TenantId;
			
			CreateDate = DateTimeOffset.Now;
			LastUpdate = DateTimeOffset.Now;
			IsActive = true;
		}

		public Entity(Guid tenantId, Guid ownerId)
		{
			if (EntityId == Guid.Empty)
			{
				EntityId = Guid.NewGuid();
			}

			this.OwnerId = ownerId;
			this.TenantId = tenantId;

			if (tenantId == Guid.Empty)
			{
				TenantId = DefaultTenantId;
			}

			if (ownerId == Guid.Empty)
			{
				OwnerId = TenantId;
			}
		}

		#region internalkeys
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
		
		/// <summary>
		/// The id of the original data might not be a UUID. OriginId is a string that will contain whatever the original id was. There 
		/// will be a lookup table assciatedt with the entity at the Tennant level to resolve the Id.
		/// </summary>
		public string OriginId { get; set; } = string.Empty;
		//public Type OriginIdType { get; set; } = typeof(Guid);

        /*
        public Type RegisterTenantIdType { 
            get => OriginIdType; 
            set => OriginIdType = value; 
        }

        public void RegisterTenetIdResolverDictionary(string connectstring)
        {
            RegisterTenantIdType = typeof(Guid);
        }
        */
		
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

		public void MarkDeleted()
		{
			MarkAsUpdated();
			IsActive = false;
			IsDeleted = true;
		}

		public void UnDelete()
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

