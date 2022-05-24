using Microsoft.EntityFrameworkCore;

namespace Practice_bases.Models;

public class ApplicationContext : DbContext
{
    
    public DbSet<Base> Bases { get; set; }
    
    public DbSet<BaseRow> BaseRows { get; set; }
    
    public DbSet<OrganizationPlus> OrganizationsPlus { get; set; }
    
    public DbSet<Supervisor> Supervisors { get; set; }
    
    public DbSet<Address> Address { get; set; }
    
    public DbSet<Group> Groups { get; set; }
    
    public DbSet<Human> Humans { get; set; }
   
    public DbSet<Mail> Mails { get; set; }
    
    public DbSet<Organization> Organizations { get; set; }
    
    public DbSet<Phone> Phones { get; set; }
    
    public DbSet<Website> Websites { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        string adminRoleName = "Admin";
        string userRoleName = "User";
        string unconfirmedRoleName = "Unconfirmed";
        
        // добавляем роли
        Role adminRole = new Role { Id = 1, Name = adminRoleName };
        Role userRole = new Role { Id = 2, Name = userRoleName };
        Role unconfirmedRole = new Role { Id = 3, Name = unconfirmedRoleName };
        
        modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole, unconfirmedRole });
        
        base.OnModelCreating(modelBuilder);
    }
    
    
    /*DELETE FROM public."Address";
    DELETE FROM public."BaseRows";
    DELETE FROM public."Groups";
    DELETE FROM public."Humans";
    DELETE FROM public."Mails";
    DELETE FROM public."Organizations";
    DELETE FROM public."Phones";
    DELETE FROM public."Websites";
    DELETE FROM public."Bases";

    SELECT * FROM public."Address";
    SELECT * FROM public."BaseRows";
    SELECT * FROM public."Bases";
    SELECT * FROM public."Groups";
    SELECT * FROM public."Humans";
    SELECT * FROM public."Mails";
    SELECT * FROM public."Organizations";
    SELECT * FROM public."Phones";
    SELECT * FROM public."Websites";
    
    SELECT pg_terminate_backend(pg_stat_activity.pid)
FROM pg_stat_activity
WHERE pg_stat_activity.datname = 'PracticeBases'
  AND pid <> pg_backend_pid();
  
DROP DATABASE "PracticeBases";
    
    */

}