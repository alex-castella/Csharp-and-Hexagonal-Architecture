using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Persistence
{
    public class ApplicationDbContext : DbContext // This class represents the database context for the application,
                                                  // allowing us to interact with the database using Entity Framework Core.
                                                  // It inherits from DbContext, which is a part of the Entity Framework Core library.
                                                  // The constructor takes DbContextOptions as a parameter, which allows us to configure
                                                  // the database connection and other options when we create an instance of ApplicationDbContext.
                                                  // The context is typically used to define DbSet properties for each entity in the application, which represent tables in the database.
                                                  // The context is the core of the data access layer in an application that uses Entity Framework Core, and it provides methods for querying and saving data to the database.
                                                  // It is used in the repositories to perform CRUD operations on the entities, and it is typically registered in the dependency injection container of the application to be injected into the repositories and other services that need to access the database.
                                                  // It is also used in the interfaces implementations to perform the actual data access operations, such as retrieving, adding, updating, and deleting records in the database.
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) // This constructor initializes the ApplicationDbContext with the specified options.
                                                                                                    // The options parameter is passed to the base DbContext class, which uses it to configure the database connection and other settings.
                                                                                                    // DbContextOptions is a class that contains configuration information for the database context, such as the connection string, database provider, and other options.
        {

        }

        public DbSet<PersonEntity> Persons { get; set; } // This property represents a DbSet of PersonEntity objects, which corresponds to a table in the database.
                                                         // The DbSet allows us to perform CRUD operations on the PersonEntity table in the database.
                                                         // Each PersonEntity object represents a row in the Persons table, and the properties of the PersonEntity class correspond to the columns in the table.
                                                         // The structure is defined in the PersonEntity class, which is part of the Domain namespace. 


        protected override void OnModelCreating(ModelBuilder modelBuilder) // This method is overridden to configure the model using the ModelBuilder API.
                                                                           // It allows us to specify how the entities should be mapped to the database schema, including relationships, constraints, and other configurations.
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonEntity>(entity =>//This is like a lambda expression that configures the PersonEntity entity. The entity parameter represents the configuration for the PersonEntity class, and we can use it to specify how the properties of the PersonEntity class should be mapped to the database columns,
                                                       //as well as any relationships or constraints that should be applied to the entity.
            {
                entity.ToTable("Persons"); // This line specifies that the PersonEntity class should be mapped to a table named "Persons" in the database.
                                           // If this line were omitted, Entity Framework would use a default naming convention to determine the table name, which might not match the desired name.

                entity.HasKey(e => e.Id); // This line specifies that the Id property of the PersonEntity class is the primary key for the Persons table in the database.
                                          
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

                entity.Property(e => e.Code).IsRequired().HasMaxLength(20); // This line specifies that the FirstName property is required (cannot be null) and has a maximum length of 100 characters in the database.

                entity.HasIndex(e => e.Code).IsUnique(); // This line creates a unique index on the Code property, ensuring that each value in the Code column is unique across all records in the Persons table.

                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50); // This line specifies that the FirstName property is required (cannot be null) and has a maximum length of 500 characters in the database.

                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);// This line specifies that the LastName property is required (cannot be null) and has a maximum length of 500 characters in the database.

                entity.Property(e => e.Email).IsRequired().HasMaxLength(100); // This line specifies that the Email property is required (cannot be null) and has a maximum length of 100 characters in the database.

                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15); // This line specifies that the PhoneNumber property is required (cannot be null) and has a maximum length of 15 characters in the database.

                entity.Ignore(e => e.FullName); // This line tells Entity Framework to ignore the FullName property when mapping the PersonEntity class to the database. 
                                                // The FullName property is a computed property that concatenates the FirstName and LastName properties, and it does not need to be stored in the database as a separate column.

                entity.Property<DateTime>("CreateAt").IsRequired().HasDefaultValueSql("GETUTCDATE()"); // This line adds a shadow property named CreateAt of type DateTime to the PersonEntity entity. 
                                                                                                       // The IsRequired() method specifies that this property is required (cannot be null), and the HasDefaultValueSql("GETUTCDATE()") method sets the default value of this property to the current UTC date and time when a new record is inserted into the database. 
                                                                                                       // This means that whenever a new PersonEntity is created and saved to the database, the CreateAt column will automatically be populated with the current UTC date and time, allowing us to track when each record was created without having to manually set this value in our code. 
                                                                                                       // Shadow properties are properties that are not defined in the entity class but are still part of the model and can be used for various purposes, such as tracking creation or modification timestamps, without cluttering the entity class with additional properties.

                entity.Property<DateTime>("UpdatedAt").IsRequired().HasDefaultValueSql("GETUTCDATE()"); // This line adds a shadow property named UpdatedAt of type DateTime to the PersonEntity entity. 
                                                                                                        // The IsRequired() method specifies that this property is required (cannot be null), and the HasDefaultValueSql("GETUTCDATE()") method sets the default value of this property to the current UTC date and time when a new record is inserted into the database. 
                                                                                                        // The ValueGeneratedOnAddOrUpdate() method indicates that the value of this property should be generated by the database both when a new record is added and when an existing record is updated. 
                                                                                                         // This means that whenever a new PersonEntity is created or an existing one is updated and saved to the database, the UpdatedAt column will automatically be populated with the current UTC date and time, allowing us to track when each record was last modified without having to manually set this value in our code.
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) // This method is overridden to provide custom behavior when saving changes to the database. 
        {
            var entries = ChangeTracker.Entries(); // This line retrieves all the tracked entities that are being added, modified, or deleted in the current context.
            foreach (var entry in entries) // This loop iterates through each tracked entity entry.
            {
                if (entry.Entity is PersonEntity) // This condition checks if the entity being tracked is of type PersonEntity.
                {
                    if (entry.State == EntityState.Added) // If the entity is being added to the database,
                    {
                        entry.Property("CreateAt").CurrentValue = DateTime.UtcNow; // This line sets the value of the CreateAt shadow property to the current UTC date and time when a new PersonEntity is added.
                    }
                    if (entry.State == EntityState.Modified) // If the entity is being modified,
                    {
                        entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow; // This line sets the value of the UpdatedAt shadow property to the current UTC date and time when an existing PersonEntity is modified.
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken); // Finally, this line calls the base implementation of SaveChangesAsync to save all changes to the database, including any updates made to the shadow properties.
        }
    }
}
