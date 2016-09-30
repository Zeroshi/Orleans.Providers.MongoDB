﻿namespace Orleans.Providers.MongoDB.Membership
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using global::MongoDB.Driver;

    using Orleans.Messaging;
    using Orleans.Providers.MongoDB.Repository;
    using Orleans.Runtime;
    using Orleans.Runtime.Configuration;

    /// <summary>
    /// The mongo membership provider.
    /// </summary>
    public class MongoMembershipProvider : IMembershipTable, IGatewayListProvider
    {
        private string deploymentId;
        private TimeSpan maxStaleness;
        private Logger logger;
        private IDocumentRepository repository;

        #region Implementation of IMembershipTable

        /// <summary>
        /// Initializes the membership table, will be called before all other methods
        /// </summary>
        /// <param name="globalConfiguration">the give global configuration</param>
        /// <param name="tryInitTableVersion">whether an attempt will be made to init the underlying table</param>
        /// <param name="traceLogger">the logger used by the membership table</param>
        public Task InitializeMembershipTable(
            GlobalConfiguration globalConfiguration,
            bool tryInitTableVersion,
            TraceLogger traceLogger)
        {
            this.logger = traceLogger;
            this.deploymentId = globalConfiguration.DeploymentId;

            if (this.logger.IsVerbose3)
            {
                this.logger.Verbose3("MongoMembershipTable.InitializeMembershipTable called.");
            }

            this.repository = new DocumentRepository(globalConfiguration.DataConnectionString, MongoUrl.Create(globalConfiguration.DataConnectionString).DatabaseName, "MembershipEntry");

            // even if I am not the one who created the table, 
            // try to insert an initial table version if it is not already there,
            // so we always have a first table version row, before this silo starts working.
            //if (tryInitTableVersion)
            //{
            //    var wasCreated = await InitTableAsync();
            //    if (wasCreated)
            //    {
            //        logger.Info("Created new table version row.");
            //    }
            //}

            return TaskDone.Done;
        }

        /// <summary>
        /// Deletes all table entries of the given deploymentId
        /// </summary>
        /// <param name="deploymentId">
        /// The deployment Id.
        /// </param>
        public Task DeleteMembershipTableEntries(string deploymentId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Atomically reads the Membership Table information about a given silo.
        /// The returned MembershipTableData includes one MembershipEntry entry for a given silo and the
        /// TableVersion for this table. The MembershipEntry and the TableVersion have to be read atomically.
        /// </summary>
        /// <param name="entry">The address of the silo whose membership information needs to be read.</param>
        /// <returns>The membership information for a given silo: MembershipTableData consisting one MembershipEntry entry and
        /// TableVersion, read atomically.</returns>
        public Task<MembershipTableData> ReadRow(SiloAddress key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Atomically reads the full content of the Membership Table.
        /// The returned MembershipTableData includes all MembershipEntry entry for all silos in the table and the
        /// TableVersion for this table. The MembershipEntries and the TableVersion have to be read atomically.
        /// </summary>
        /// <returns>The membership information for a given table: MembershipTableData consisting multiple MembershipEntry entries and
        /// TableVersion, all read atomically.</returns>
        public Task<MembershipTableData> ReadAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Atomically tries to insert (add) a new MembershipEntry for one silo and also update the TableVersion.
        /// If operation succeeds, the following changes would be made to the table:
        /// 1) New MembershipEntry will be added to the table.
        /// 2) The newly added MembershipEntry will also be added with the new unique automatically generated eTag.
        /// 3) TableVersion.Version in the table will be updated to the new TableVersion.Version.
        /// 4) TableVersion etag in the table will be updated to the new unique automatically generated eTag.
        /// All those changes to the table, insert of a new row and update of the table version and the associated etags, should happen atomically, or fail atomically with no side effects.
        /// The operation should fail in each of the following conditions:
        /// 1) A MembershipEntry for a given silo already exist in the table
        /// 2) Update of the TableVersion failed since the given TableVersion etag (as specified by the TableVersion.VersionEtag property) did not match the TableVersion etag in the table.
        /// </summary>
        /// <param name="entry">MembershipEntry to be inserted.</param>
        /// <param name="tableVersion">The new TableVersion for this table, along with its etag.</param>
        /// <returns>True if the insert operation succeeded and false otherwise.</returns>
        public Task<bool> InsertRow(MembershipEntry entry, TableVersion tableVersion)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Atomically tries to update the MembershipEntry for one silo and also update the TableVersion.
        /// If operation succeeds, the following changes would be made to the table:
        /// 1) The MembershipEntry for this silo will be updated to the new MembershipEntry (the old entry will be fully substitued by the new entry)
        /// 2) The eTag for the updated MembershipEntry will also be eTag with the new unique automatically generated eTag.
        /// 3) TableVersion.Version in the table will be updated to the new TableVersion.Version.
        /// 4) TableVersion etag in the table will be updated to the new unique automatically generated eTag.
        /// All those changes to the table, update of a new row and update of the table version and the associated etags, should happen atomically, or fail atomically with no side effects.
        /// The operation should fail in each of the following conditions:
        /// 1) A MembershipEntry for a given silo does not exist in the table
        /// 2) A MembershipEntry for a given silo exist in the table but its etag in the table does not match the provided etag.
        /// 3) Update of the TableVersion failed since the given TableVersion etag (as specified by the TableVersion.VersionEtag property) did not match the TableVersion etag in the table.
        /// </summary>
        /// <param name="entry">MembershipEntry to be updated.</param>
        /// <param name="etag">The etag  for the given MembershipEntry.</param>
        /// <param name="tableVersion">The new TableVersion for this table, along with its etag.</param>
        /// <returns>True if the update operation succeeded and false otherwise.</returns>
        public Task<bool> UpdateRow(MembershipEntry entry, string etag, TableVersion tableVersion)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the IAmAlive part (column) of the MembershipEntry for this silo.
        /// This operation should only update the IAmAlive collumn and not change other columns.
        /// This operation is a "dirty write" or "in place update" and is performed without etag validation.
        /// With regards to eTags update:
        /// This operation may automatically update the eTag associated with the given silo row, but it does not have to. It can also leave the etag not changed ("dirty write").
        /// With regards to TableVersion:
        /// this operation should not change the TableVersion of the table. It should leave it untouched.
        /// There is no scenario where this operation could fail due to table semantical reasons. It can only fail due to network problems or table unavailability.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>Task representing the successful execution of this operation. </returns>
        public Task UpdateIAmAlive(MembershipEntry entry)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IGatewayListProvider

        /// <summary>
        /// Initializes the provider, will be called before all other methods
        /// </summary>
        /// <param name="clientConfiguration">the given client configuration</param>
        /// <param name="traceLogger">the logger to be used by the provider</param>
        public Task InitializeGatewayListProvider(ClientConfiguration clientConfiguration, TraceLogger traceLogger)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the list of gateways (silos) that can be used by a client to connect to Orleans cluster.
        /// The Uri is in the form of: "gwy.tcp://IP:port/Generation". See Utils.ToGatewayUri and Utils.ToSiloAddress for more details about Uri format.
        /// </summary>
        public Task<IList<Uri>> GetGateways()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specifies how often this IGatewayListProvider is refreshed, to have a bound on max staleness of its returned infomation.
        /// </summary>
        public TimeSpan MaxStaleness { get; }

        /// <summary>
        /// Specifies whether this IGatewayListProvider ever refreshes its returned infomation, or always returns the same gw list.
        /// (currently only the static config based StaticGatewayListProvider is not updatable. All others are.)
        /// </summary>
        public bool IsUpdatable { get; }

        #endregion
    }
}