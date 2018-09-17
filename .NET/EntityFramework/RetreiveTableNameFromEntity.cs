public partial class DatabaseContext : DbContext
{
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted && p.Entity is IEntity && p.Entity is IEntityStatus))
            SoftDelete(entry);

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync()
    {
        return SaveChangesAsync(default(CancellationToken));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted && p.Entity is IEntity && p.Entity is IEntityStatus))
            SoftDelete(entry);

        return await base.SaveChangesAsync(cancellationToken);
    }
    
    private void SoftDelete(DbEntityEntry entry)
    {
        Type entryEntityType = entry.Entity.GetType();

        string tableName = GetTableName(entryEntityType);

        string deletequery = $"UPDATE {tableName} SET StatusId = {(int)Statuses.Deleted} WHERE Id = @id";

        Database.ExecuteSqlCommand(
            deletequery,
            new SqlParameter("@id", entry.OriginalValues["Id"]));

        //Marking it Unchanged prevents the hard delete
        //entry.State = EntityState.Unchanged;
        //So does setting it to Detached:
        //And that is what EF does when it deletes an item
        //http://msdn.microsoft.com/en-us/data/jj592676.aspx
        entry.State = EntityState.Detached;
    }

    private static Dictionary<Type, string> _tableNameCache = new Dictionary<Type, string>();

    private string GetTableName(Type CLRType)
    {
        if (CLRType.BaseType != null && CLRType.Namespace == "System.Data.Entity.DynamicProxies")
            CLRType = CLRType.BaseType;

        if (_tableNameCache.ContainsKey(CLRType))
        {
            return _tableNameCache[CLRType];
        }

        var metadata = ((IObjectContextAdapter)this).ObjectContext.MetadataWorkspace;

        // Get the part of the model that contains info about the actual CLR types
        var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

        // Get the entity type from the model that maps to the CLR type
        var entityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == CLRType);

        // Get the entity set that uses this entity type
        var entitySet = metadata
            .GetItems<EntityContainer>(DataSpace.CSpace)
            .Single()
            .EntitySets
            .Single(s => s.ElementType.Name == entityType.Name);

        // Find the mapping between conceptual and storage model for this entity set
        var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                .Single()
                .EntitySetMappings
                .Single(s => s.EntitySet == entitySet);

        // Find the storage entity sets (tables) that the entity is mapped
        var tables = mapping
            .EntityTypeMappings
            .Single()
            .Fragments;

        // Return the table name from the storage entity set
        var tablesName = tables.Select(f => (string)f.StoreEntitySet.MetadataProperties["Table"].Value ?? f.StoreEntitySet.Name);

        _tableNameCache.Add(CLRType, tablesName.Single());
        return _tableNameCache[CLRType];
    }
}
