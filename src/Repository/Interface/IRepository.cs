/*
 MIT License - IRepository.cs

Copyright (c) 2021 - Present by Sand Drift Software, LLC
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace Hl7Harmonizer.Repository.Interface
{
    public enum RepositoryIntent
    {
        DataStorage,
        SeedData,
        RawDataStorage,
        Logging,
        Configuration,
        Credentials,
        SqlAdmin,
        NoSqlAdmin,
        SqlTesting,
        NoSqlTesting,
        Testing
    }

    public enum RepositoryLocus
    {
        NotSpecified,
        Local,
        Cloud,
        Container
    }

    public enum DatabaseType
    {
        Undefined,
        PostgreSQL,
        CosmosDb,
        SQLite,
        SqlServer,
        MySql,
        MongoDb
    }

    /// <summary>
    /// CRUDL Create Entity Read Entity Update Entity Delete Entity List Entities
    ///
    /// Query Methods: Linq GraphQl
    /// </summary>
    /// <typeparam name="TEntity"> </typeparam>
    public interface IRepository<TEntity> where TEntity : Entity, IDisposable
    {
        /// <summary>
        /// GetIntent <returns> DatabaseIntent </returns>
        /// </summary>
        RepositoryIntent GetIntent();

        /// <summary>
        /// GetType
        /// </summary>
        /// <returns> DatabaseType </returns>
        DatabaseType GetType();

        /// <summary>
        /// Commit
        /// </summary>
        /// <returns> </returns>
        Task<bool> Commit();

        /// <summary>
        /// <c> CreateRecord </c> Insert a record if it doesnn't exist
        /// </summary>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        Task<bool> CreateRecord(TEntity entity);

        /// <summary>
        /// <c> UpdateRecord </c> Update an existing record
        /// </summary>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        Task<bool> UpdateRecord(TEntity entity);

        /// <summary>
        /// <c> DeleteRecord </c>
        /// </summary>
        /// Set TEntity.IsDeleted to true
        /// <param name="entity"> </param>
        /// <returns> </returns>
        Task<bool> DeleteRecord(Guid entity);

        /// <summary>
        /// <c> Get set using c# methods </c>
        /// </summary>
        /// <param name="predicate"> </param>
        /// <returns> </returns>
        Task<IEnumerable<TEntity>?> QueryFluent(Expression<Func<TEntity, bool>> predicate, int limit = -1);

        /// <summary>
        /// <c> Get set using c# methods </c>
        /// </summary>
        /// <param name="predicate"> </param>
        /// <returns> </returns>
        Task<IEnumerable<TEntity>?> QueryFluent(string predicate, int limit = -1);

        /// <summary>
        /// <c> QueryGraph </c> Get set using GraphQl methods
        /// </summary>
        /// <param name="query"> </param>
        /// <returns> </returns>
        Task<IEnumerable<TEntity>?> QueryGraph(string query);

        /// <summary>
        /// <c> ListAll </c> Return all items in the collection. If partiton[0] == 0x0, return the
        /// whole collection, else just the current partition
        /// </summary>
        /// <param name="filterBools"> Filter using IsDeleted and IsActive </param>
        /// <returns> </returns>
        Task<IEnumerable<TEntity>?> GetAll(bool filterBools = true);

        /// <summary>
        /// <c> GetByIdAsync </c> Get item async based on MyId
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        Task<TEntity?> GetByIdAsync(Guid id);

        /// <summary>
        /// <c> GetById </c>
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        TEntity? GetById(Guid id);

        /// <summary>
        /// GetById <typeparamref name="TEntity" /> Get Id based on OwnerId
        /// Usage: Get a parent record
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="id"> </param>
        /// <returns> </returns>
        Task<TEntity?> GetItemByParentId(Guid parent, Guid id);

        /// <summary>
        /// GetById <typeparamref name="TEntity" /> Get Id based on OwnerId
        /// Usage: Get all by parent record
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="id"> </param>
        /// <returns> </returns>
        Task<IEnumerable<TEntity>?> GetAllByOwnerId(Guid parent);

        /// <summary>
        /// <c> SetPartition </c> Allows the selection of a specific db partition for a Repository
        /// <typeparamref name="TEntity" /> without having to create a new instance
        /// </summary>
        /// <param name="tenantId"> byte[512] partition id </param>
        void SetTenant(string? tenantId);

        /// <summary>
        /// <c> SetPartition </c> Allows the selection of a specific db partition for a Repository
        /// <typeparamref name="TEntity" /> without having to create a new instance
        /// </summary>
        /// <param name="tenantId"> UUID </param>
        void SetTenant(Guid tenantId);
    }
}