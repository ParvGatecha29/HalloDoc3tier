﻿using HalloDocDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HalloDocDAL.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Adminregion> Adminregions { get; set; }

    public virtual DbSet<Aspnetrole> Aspnetroles { get; set; }

    public virtual DbSet<Aspnetuser> Aspnetusers { get; set; }

    public virtual DbSet<Aspnetuserrole> Aspnetuserroles { get; set; }

    public virtual DbSet<Blockrequest> Blockrequests { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Businesstype> Businesstypes { get; set; }

    public virtual DbSet<Casetag> Casetags { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Chatdatum> Chatdata { get; set; }

    public virtual DbSet<Concierge> Concierges { get; set; }

    public virtual DbSet<Emaillog> Emaillogs { get; set; }

    public virtual DbSet<EncounterForm> EncounterForms { get; set; }

    public virtual DbSet<Healthprofessional> Healthprofessionals { get; set; }

    public virtual DbSet<Healthprofessionaltype> Healthprofessionaltypes { get; set; }

    public virtual DbSet<Invoicing> Invoicings { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Physician> Physicians { get; set; }

    public virtual DbSet<Physicianlocation> Physicianlocations { get; set; }

    public virtual DbSet<Physiciannotification> Physiciannotifications { get; set; }

    public virtual DbSet<Physicianpayrate> Physicianpayrates { get; set; }

    public virtual DbSet<Physicianregion> Physicianregions { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Requestbusiness> Requestbusinesses { get; set; }

    public virtual DbSet<Requestclient> Requestclients { get; set; }

    public virtual DbSet<Requestclosed> Requestcloseds { get; set; }

    public virtual DbSet<Requestconcierge> Requestconcierges { get; set; }

    public virtual DbSet<Requestnote> Requestnotes { get; set; }

    public virtual DbSet<Requeststatuslog> Requeststatuslogs { get; set; }

    public virtual DbSet<Requesttype> Requesttypes { get; set; }

    public virtual DbSet<Requestwisefile> Requestwisefiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolemenu> Rolemenus { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Shiftdetail> Shiftdetails { get; set; }

    public virtual DbSet<Shiftdetailregion> Shiftdetailregions { get; set; }

    public virtual DbSet<Smslog> Smslogs { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Database=HalloDoc; Username=postgres; Password=2002; Integrated Security=true; Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Adminid).HasName("admin_pkey");

            entity.ToTable("admin");

            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.Altphone)
                .HasMaxLength(20)
                .HasColumnName("altphone");
            entity.Property(e => e.Aspnetuserid)
                .HasMaxLength(128)
                .HasColumnName("aspnetuserid");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.State)
                .HasColumnType("character varying")
                .HasColumnName("state");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Zip)
                .HasMaxLength(10)
                .HasColumnName("zip");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.Admins)
                .HasForeignKey(d => d.Aspnetuserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("admin_aspnetuserid_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Admins)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("admin_regionid_fkey");
        });

        modelBuilder.Entity<Adminregion>(entity =>
        {
            entity.HasKey(e => e.Adminregionid).HasName("adminregion_pkey");

            entity.ToTable("adminregion");

            entity.Property(e => e.Adminregionid).HasColumnName("adminregionid");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Regionid).HasColumnName("regionid");

            entity.HasOne(d => d.Admin).WithMany(p => p.Adminregions)
                .HasForeignKey(d => d.Adminid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("adminregion_adminid_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Adminregions)
                .HasForeignKey(d => d.Regionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("adminregion_regionid_fkey");
        });

        modelBuilder.Entity<Aspnetrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aspnetroles_pkey");

            entity.ToTable("aspnetroles");

            entity.Property(e => e.Id)
                .HasMaxLength(128)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Aspnetuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aspnetusers_pkey");

            entity.ToTable("aspnetusers");

            entity.Property(e => e.Id)
                .HasMaxLength(128)
                .HasColumnName("id");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Username)
                .HasMaxLength(256)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Aspnetuserrole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("aspnetuserroles_pkey");

            entity.ToTable("aspnetuserroles");

            entity.Property(e => e.UserId).HasMaxLength(128);
            entity.Property(e => e.RoleId).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserroles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("aspnetuserroles_UserId_fkey");
        });

        modelBuilder.Entity<Blockrequest>(entity =>
        {
            entity.HasKey(e => e.Blockrequestid).HasName("blockrequests_pkey");

            entity.ToTable("blockrequests");

            entity.Property(e => e.Blockrequestid).HasColumnName("blockrequestid");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(50)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.Businessid).HasName("business_pkey");

            entity.ToTable("business");

            entity.Property(e => e.Businessid).HasColumnName("businessid");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("email");
            entity.Property(e => e.Faxnumber)
                .HasMaxLength(20)
                .HasColumnName("faxnumber");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isregistered).HasColumnName("isregistered");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(10)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.BusinessCreatedbyNavigations)
                .HasForeignKey(d => d.Createdby)
                .HasConstraintName("business_createdby_fkey");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.BusinessModifiedbyNavigations)
                .HasForeignKey(d => d.Modifiedby)
                .HasConstraintName("business_modifiedby_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("business_regionid_fkey");
        });

        modelBuilder.Entity<Businesstype>(entity =>
        {
            entity.HasKey(e => e.Businesstypeid).HasName("businesstype_pkey");

            entity.ToTable("businesstype");

            entity.Property(e => e.Businesstypeid).HasColumnName("businesstypeid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Casetag>(entity =>
        {
            entity.HasKey(e => e.Casetagid).HasName("casetag_pkey");

            entity.ToTable("casetag");

            entity.Property(e => e.Casetagid).HasColumnName("casetagid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chats_pkey");

            entity.ToTable("chats");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Patientid).HasColumnName("patientid");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
        });

        modelBuilder.Entity<Chatdatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chatdata_pkey");

            entity.ToTable("chatdata");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Chatid).HasColumnName("chatid");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Messageby)
                .HasMaxLength(250)
                .HasColumnName("messageby");
            entity.Property(e => e.Sendername)
                .HasMaxLength(250)
                .HasColumnName("sendername");

            entity.HasOne(d => d.Chat).WithMany(p => p.Chatdata)
                .HasForeignKey(d => d.Chatid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chatdata_chatid_fkey");
        });

        modelBuilder.Entity<Concierge>(entity =>
        {
            entity.HasKey(e => e.Conciergeid).HasName("concierge_pkey");

            entity.ToTable("concierge");

            entity.Property(e => e.Conciergeid).HasColumnName("conciergeid");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Conciergename)
                .HasMaxLength(100)
                .HasColumnName("conciergename");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Roleid)
                .HasMaxLength(20)
                .HasColumnName("roleid");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(50)
                .HasColumnName("street");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(50)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.Region).WithMany(p => p.Concierges)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("concierge_regionid_fkey");
        });

        modelBuilder.Entity<Emaillog>(entity =>
        {
            entity.HasKey(e => e.Emaillogid).HasName("emaillog_pkey");

            entity.ToTable("emaillog");

            entity.Property(e => e.Emaillogid)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(4L, null, null, 9999999999L, null, null)
                .HasColumnName("emaillogid");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Confirmationnumber)
                .HasMaxLength(200)
                .HasColumnName("confirmationnumber");
            entity.Property(e => e.Createdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdate");
            entity.Property(e => e.Emailid)
                .HasMaxLength(200)
                .HasColumnName("emailid");
            entity.Property(e => e.Emailtemplate).HasColumnName("emailtemplate");
            entity.Property(e => e.Filepath).HasColumnName("filepath");
            entity.Property(e => e.Isemailsent).HasColumnName("isemailsent");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Sentdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sentdate");
            entity.Property(e => e.Senttries).HasColumnName("senttries");
            entity.Property(e => e.Subjectname)
                .HasMaxLength(200)
                .HasColumnName("subjectname");
        });

        modelBuilder.Entity<EncounterForm>(entity =>
        {
            entity.HasKey(e => e.EncounterFormId).HasName("EncounterForm_pkey");

            entity.ToTable("EncounterForm");

            entity.Property(e => e.Abd).HasColumnName("ABD");
            entity.Property(e => e.Cv).HasColumnName("CV");
            entity.Property(e => e.Hr).HasColumnName("HR");
            entity.Property(e => e.Rr).HasColumnName("RR");

            entity.HasOne(d => d.Admin).WithMany(p => p.EncounterForms)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("EncounterForm_AdminId_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.EncounterForms)
                .HasForeignKey(d => d.PhysicianId)
                .HasConstraintName("EncounterForm_PhysicianId_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.EncounterForms)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("EncounterForm_RequestId_fkey");
        });

        modelBuilder.Entity<Healthprofessional>(entity =>
        {
            entity.HasKey(e => e.Vendorid).HasName("healthprofessionals_pkey");

            entity.ToTable("healthprofessionals");

            entity.Property(e => e.Vendorid).HasColumnName("vendorid");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("address");
            entity.Property(e => e.Businesscontact)
                .HasMaxLength(100)
                .HasColumnName("businesscontact");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Faxnumber)
                .HasMaxLength(50)
                .HasColumnName("faxnumber");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(100)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Profession).HasColumnName("profession");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.Vendorname)
                .HasMaxLength(100)
                .HasColumnName("vendorname");
            entity.Property(e => e.Zip)
                .HasMaxLength(50)
                .HasColumnName("zip");

            entity.HasOne(d => d.ProfessionNavigation).WithMany(p => p.Healthprofessionals)
                .HasForeignKey(d => d.Profession)
                .HasConstraintName("healthprofessionals_profession_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Healthprofessionals)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("healthprofessionals_regionid_fkey");
        });

        modelBuilder.Entity<Healthprofessionaltype>(entity =>
        {
            entity.HasKey(e => e.Healthprofessionalid).HasName("healthprofessionaltype_pkey");

            entity.ToTable("healthprofessionaltype");

            entity.Property(e => e.Healthprofessionalid).HasColumnName("healthprofessionalid");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Professionname)
                .HasMaxLength(50)
                .HasColumnName("professionname");
        });

        modelBuilder.Entity<Invoicing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("invoicing_pkey");

            entity.ToTable("invoicing");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Admindescription)
                .HasColumnType("character varying")
                .HasColumnName("admindescription");
            entity.Property(e => e.Bonus).HasColumnName("bonus");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Enddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enddate");
            entity.Property(e => e.Invoicetotal).HasColumnName("invoicetotal");
            entity.Property(e => e.Isapproved).HasColumnName("isapproved");
            entity.Property(e => e.Isfinalize).HasColumnName("isfinalize");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Startdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("startdate");

            entity.HasOne(d => d.Physician).WithMany(p => p.Invoicings)
                .HasForeignKey(d => d.Physicianid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoicing_physicianid_fkey");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Menuid).HasName("menu_pkey");

            entity.ToTable("menu");

            entity.Property(e => e.Menuid).HasColumnName("menuid");
            entity.Property(e => e.Accounttype).HasColumnName("accounttype");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Sortorder).HasColumnName("sortorder");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderdetails_pkey");

            entity.ToTable("orderdetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Businesscontact)
                .HasMaxLength(100)
                .HasColumnName("businesscontact");
            entity.Property(e => e.Createdby)
                .HasMaxLength(100)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Faxnumber)
                .HasMaxLength(50)
                .HasColumnName("faxnumber");
            entity.Property(e => e.Noofrefill).HasColumnName("noofrefill");
            entity.Property(e => e.Prescription).HasColumnName("prescription");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Vendorid).HasColumnName("vendorid");
        });

        modelBuilder.Entity<Physician>(entity =>
        {
            entity.HasKey(e => e.Physicianid).HasName("physician_pkey");

            entity.ToTable("physician");

            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.Adminnotes).HasColumnName("adminnotes");
            entity.Property(e => e.Altphone)
                .HasMaxLength(20)
                .HasColumnName("altphone");
            entity.Property(e => e.Aspnetuserid)
                .HasMaxLength(128)
                .HasColumnName("aspnetuserid");
            entity.Property(e => e.Businessname)
                .HasMaxLength(100)
                .HasColumnName("businessname");
            entity.Property(e => e.Businesswebsite)
                .HasMaxLength(200)
                .HasColumnName("businesswebsite");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Isagreementdoc).HasColumnName("isagreementdoc");
            entity.Property(e => e.Isbackgrounddoc).HasColumnName("isbackgrounddoc");
            entity.Property(e => e.Iscredentialdoc).HasColumnName("iscredentialdoc");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Islicensedoc).HasColumnName("islicensedoc");
            entity.Property(e => e.Isnondisclosuredoc).HasColumnName("isnondisclosuredoc");
            entity.Property(e => e.Istokengenerate)
                .HasDefaultValueSql("false")
                .HasColumnName("istokengenerate");
            entity.Property(e => e.Istrainingdoc).HasColumnName("istrainingdoc");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Medicallicense).HasColumnName("medicallicense");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Npinumber)
                .HasMaxLength(500)
                .HasColumnName("npinumber");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Signature).HasColumnName("signature");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Syncemailaddress)
                .HasMaxLength(50)
                .HasColumnName("syncemailaddress");
            entity.Property(e => e.Zip)
                .HasMaxLength(10)
                .HasColumnName("zip");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.PhysicianAspnetusers)
                .HasForeignKey(d => d.Aspnetuserid)
                .HasConstraintName("physician_aspnetuserid_fkey");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.PhysicianCreatedbyNavigations)
                .HasForeignKey(d => d.Createdby)
                .HasConstraintName("physician_createdby_fkey");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.PhysicianModifiedbyNavigations)
                .HasForeignKey(d => d.Modifiedby)
                .HasConstraintName("physician_modifiedby_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Physicians)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("physician_regionid_fkey");
        });

        modelBuilder.Entity<Physicianlocation>(entity =>
        {
            entity.HasKey(e => e.Locationid).HasName("physicianlocation_pkey");

            entity.ToTable("physicianlocation");

            entity.Property(e => e.Locationid).HasColumnName("locationid");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Latitude)
                .HasPrecision(9, 6)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasPrecision(9, 6)
                .HasColumnName("longitude");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Physicianname)
                .HasMaxLength(50)
                .HasColumnName("physicianname");

            entity.HasOne(d => d.Physician).WithMany(p => p.Physicianlocations)
                .HasForeignKey(d => d.Physicianid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("physicianlocation_physicianid_fkey");
        });

        modelBuilder.Entity<Physiciannotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("physiciannotification_pkey");

            entity.ToTable("physiciannotification");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isnotificationstopped).HasColumnName("isnotificationstopped");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");

            entity.HasOne(d => d.Physician).WithMany(p => p.Physiciannotifications)
                .HasForeignKey(d => d.Physicianid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("physiciannotification_physicianid_fkey");
        });

        modelBuilder.Entity<Physicianpayrate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("physicianpayrate_pkey");

            entity.ToTable("physicianpayrate");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Physician).WithMany(p => p.Physicianpayrates)
                .HasForeignKey(d => d.PhysicianId)
                .HasConstraintName("physicianpayrate_PhysicianId_fkey");
        });

        modelBuilder.Entity<Physicianregion>(entity =>
        {
            entity.HasKey(e => e.Physicianregionid).HasName("physicianregion_pkey");

            entity.ToTable("physicianregion");

            entity.Property(e => e.Physicianregionid).HasColumnName("physicianregionid");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Regionid).HasColumnName("regionid");

            entity.HasOne(d => d.Physician).WithMany(p => p.Physicianregions)
                .HasForeignKey(d => d.Physicianid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("physicianregion_physicianid_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Physicianregions)
                .HasForeignKey(d => d.Regionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("physicianregion_regionid_fkey");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Regionid).HasName("region_pkey");

            entity.ToTable("region");

            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(50)
                .HasColumnName("abbreviation");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Requestid).HasName("request_pkey");

            entity.ToTable("request");

            entity.Property(e => e.Requestid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requestid");
            entity.Property(e => e.Accepteddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("accepteddate");
            entity.Property(e => e.Calltype).HasColumnName("calltype");
            entity.Property(e => e.Casenumber)
                .HasMaxLength(50)
                .HasColumnName("casenumber");
            entity.Property(e => e.Casetag)
                .HasMaxLength(50)
                .HasColumnName("casetag");
            entity.Property(e => e.Casetagphysician)
                .HasMaxLength(50)
                .HasColumnName("casetagphysician");
            entity.Property(e => e.Completedbyphysician).HasColumnName("completedbyphysician");
            entity.Property(e => e.Confirmationnumber)
                .HasMaxLength(20)
                .HasColumnName("confirmationnumber");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Createduserid).HasColumnName("createduserid");
            entity.Property(e => e.Declinedby)
                .HasMaxLength(250)
                .HasColumnName("declinedby");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Ismobile).HasColumnName("ismobile");
            entity.Property(e => e.Isurgentemailsent).HasColumnName("isurgentemailsent");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Lastreservationdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastreservationdate");
            entity.Property(e => e.Lastwellnessdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastwellnessdate");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Patientaccountid)
                .HasMaxLength(128)
                .HasColumnName("patientaccountid");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(23)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Relationname)
                .HasMaxLength(100)
                .HasColumnName("relationname");
            entity.Property(e => e.Requesttypeid)
                .HasDefaultValueSql("1")
                .HasColumnName("requesttypeid");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("1")
                .HasColumnName("status");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Physician).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Physicianid)
                .HasConstraintName("request_physicianid_fkey");

            entity.HasOne(d => d.Requesttype).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Requesttypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_requesttypeid_fkey");
        });

        modelBuilder.Entity<Requestbusiness>(entity =>
        {
            entity.HasKey(e => e.Requestbusinessid).HasName("requestbusiness_pkey");

            entity.ToTable("requestbusiness");

            entity.Property(e => e.Requestbusinessid).HasColumnName("requestbusinessid");
            entity.Property(e => e.Businessid).HasColumnName("businessid");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Requestid).HasColumnName("requestid");

            entity.HasOne(d => d.Business).WithMany(p => p.Requestbusinesses)
                .HasForeignKey(d => d.Businessid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestbusiness_businessid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestbusinesses)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestbusiness_requestid_fkey");
        });

        modelBuilder.Entity<Requestclient>(entity =>
        {
            entity.HasKey(e => e.Requestclientid).HasName("requestclient_pkey");

            entity.ToTable("requestclient");

            entity.Property(e => e.Requestclientid).HasColumnName("requestclientid");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Communicationtype).HasColumnName("communicationtype");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Intdate).HasColumnName("intdate");
            entity.Property(e => e.Intyear).HasColumnName("intyear");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Ismobile).HasColumnName("ismobile");
            entity.Property(e => e.Isreservationremindersent).HasColumnName("isreservationremindersent");
            entity.Property(e => e.Issetfollowupsent).HasColumnName("issetfollowupsent");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Latitude)
                .HasPrecision(9, 6)
                .HasColumnName("latitude");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Longitude)
                .HasPrecision(9, 6)
                .HasColumnName("longitude");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Notiemail)
                .HasMaxLength(50)
                .HasColumnName("notiemail");
            entity.Property(e => e.Notimobile)
                .HasMaxLength(20)
                .HasColumnName("notimobile");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(23)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Remindhousecallcount).HasColumnName("remindhousecallcount");
            entity.Property(e => e.Remindreservationcount).HasColumnName("remindreservationcount");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
            entity.Property(e => e.Strmonth)
                .HasMaxLength(20)
                .HasColumnName("strmonth");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(10)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.Region).WithMany(p => p.Requestclients)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("requestclient_regionid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestclients)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestclient_requestid_fkey");
        });

        modelBuilder.Entity<Requestclosed>(entity =>
        {
            entity.HasKey(e => e.Requestclosedid).HasName("requestclosed_pkey");

            entity.ToTable("requestclosed");

            entity.Property(e => e.Requestclosedid).HasColumnName("requestclosedid");
            entity.Property(e => e.Clientnotes).HasColumnName("clientnotes");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Phynotes).HasColumnName("phynotes");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Requeststatuslogid).HasColumnName("requeststatuslogid");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestcloseds)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestclosed_requestid_fkey");
        });

        modelBuilder.Entity<Requestconcierge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requestconcierge_pkey");

            entity.ToTable("requestconcierge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Conciergeid).HasColumnName("conciergeid");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Requestid).HasColumnName("requestid");

            entity.HasOne(d => d.Concierge).WithMany(p => p.Requestconcierges)
                .HasForeignKey(d => d.Conciergeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestconcierge_conciergeid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestconcierges)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestconcierge_requestid_fkey");
        });

        modelBuilder.Entity<Requestnote>(entity =>
        {
            entity.HasKey(e => e.Requestnotesid).HasName("requestnotes_pkey");

            entity.ToTable("requestnotes");

            entity.Property(e => e.Requestnotesid).HasColumnName("requestnotesid");
            entity.Property(e => e.Administrativenotes).HasColumnName("administrativenotes");
            entity.Property(e => e.Adminnotes).HasColumnName("adminnotes");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Intdate).HasColumnName("intdate");
            entity.Property(e => e.Intyear).HasColumnName("intyear");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Physiciannotes).HasColumnName("physiciannotes");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Strmonth)
                .HasMaxLength(20)
                .HasColumnName("strmonth");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.RequestnoteCreatedbyNavigations)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestnotes_createdby_fkey");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.RequestnoteModifiedbyNavigations)
                .HasForeignKey(d => d.Modifiedby)
                .HasConstraintName("requestnotes_modifiedby_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestnotes)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestnotes_requestid_fkey");
        });

        modelBuilder.Entity<Requeststatuslog>(entity =>
        {
            entity.HasKey(e => e.Requeststatuslogid).HasName("requeststatuslog_pkey");

            entity.ToTable("requeststatuslog");

            entity.Property(e => e.Requeststatuslogid).HasColumnName("requeststatuslogid");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Transtoadmin).HasColumnName("transtoadmin");
            entity.Property(e => e.Transtophysicianid).HasColumnName("transtophysicianid");

            entity.HasOne(d => d.Admin).WithMany(p => p.Requeststatuslogs)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("requeststatuslog_adminid_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.RequeststatuslogPhysicians)
                .HasForeignKey(d => d.Physicianid)
                .HasConstraintName("requeststatuslog_physicianid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requeststatuslogs)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requeststatuslog_requestid_fkey");

            entity.HasOne(d => d.Transtophysician).WithMany(p => p.RequeststatuslogTranstophysicians)
                .HasForeignKey(d => d.Transtophysicianid)
                .HasConstraintName("requeststatuslog_transtophysicianid_fkey");
        });

        modelBuilder.Entity<Requesttype>(entity =>
        {
            entity.HasKey(e => e.Requesttypeid).HasName("requesttype_pkey");

            entity.ToTable("requesttype");

            entity.Property(e => e.Requesttypeid).HasColumnName("requesttypeid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Requestwisefile>(entity =>
        {
            entity.HasKey(e => e.Requestwisefileid).HasName("requestwisefile_pkey");

            entity.ToTable("requestwisefile");

            entity.Property(e => e.Requestwisefileid).HasColumnName("requestwisefileid");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Doctype).HasColumnName("doctype");
            entity.Property(e => e.Filename)
                .HasMaxLength(500)
                .HasColumnName("filename");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Iscompensation).HasColumnName("iscompensation");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isfinalize).HasColumnName("isfinalize");
            entity.Property(e => e.Isfrontside).HasColumnName("isfrontside");
            entity.Property(e => e.Ispatientrecords).HasColumnName("ispatientrecords");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Requestid).HasColumnName("requestid");

            entity.HasOne(d => d.Admin).WithMany(p => p.Requestwisefiles)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("requestwisefile_adminid_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.Requestwisefiles)
                .HasForeignKey(d => d.Physicianid)
                .HasConstraintName("requestwisefile_physicianid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestwisefiles)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestwisefile_requestid_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Accounttype).HasColumnName("accounttype");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Rolemenu>(entity =>
        {
            entity.HasKey(e => e.Rolemenuid).HasName("rolemenu_pkey");

            entity.ToTable("rolemenu");

            entity.Property(e => e.Rolemenuid).HasColumnName("rolemenuid");
            entity.Property(e => e.Menuid).HasColumnName("menuid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Menu).WithMany(p => p.Rolemenus)
                .HasForeignKey(d => d.Menuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rolemenu_menuid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Rolemenus)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rolemenu_roleid_fkey");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Shiftid).HasName("shift_pkey");

            entity.ToTable("shift");

            entity.Property(e => e.Shiftid).HasColumnName("shiftid");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isrepeat).HasColumnName("isrepeat");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Repeatupto).HasColumnName("repeatupto");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
            entity.Property(e => e.Weekdays)
                .HasMaxLength(7)
                .IsFixedLength()
                .HasColumnName("weekdays");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shift_createdby_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.Physicianid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shift_physicianid_fkey");
        });

        modelBuilder.Entity<Shiftdetail>(entity =>
        {
            entity.HasKey(e => e.Shiftdetailid).HasName("shiftdetail_pkey");

            entity.ToTable("shiftdetail");

            entity.Property(e => e.Shiftdetailid).HasColumnName("shiftdetailid");
            entity.Property(e => e.Endtime).HasColumnName("endtime");
            entity.Property(e => e.Eventid)
                .HasMaxLength(100)
                .HasColumnName("eventid");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Issync).HasColumnName("issync");
            entity.Property(e => e.Lastrunningdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastrunningdate");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Shiftdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("shiftdate");
            entity.Property(e => e.Shiftid).HasColumnName("shiftid");
            entity.Property(e => e.Starttime).HasColumnName("starttime");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.Shiftdetails)
                .HasForeignKey(d => d.Modifiedby)
                .HasConstraintName("shiftdetail_modifiedby_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Shiftdetails)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("shiftdetail_regionid_fkey");

            entity.HasOne(d => d.Shift).WithMany(p => p.Shiftdetails)
                .HasForeignKey(d => d.Shiftid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftdetail_shiftid_fkey");
        });

        modelBuilder.Entity<Shiftdetailregion>(entity =>
        {
            entity.HasKey(e => e.Shiftdetailregionid).HasName("shiftdetailregion_pkey");

            entity.ToTable("shiftdetailregion");

            entity.Property(e => e.Shiftdetailregionid).HasColumnName("shiftdetailregionid");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Shiftdetailid).HasColumnName("shiftdetailid");

            entity.HasOne(d => d.Region).WithMany(p => p.Shiftdetailregions)
                .HasForeignKey(d => d.Regionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftdetailregion_regionid_fkey");

            entity.HasOne(d => d.Shiftdetail).WithMany(p => p.Shiftdetailregions)
                .HasForeignKey(d => d.Shiftdetailid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftdetailregion_shiftdetailid_fkey");
        });

        modelBuilder.Entity<Smslog>(entity =>
        {
            entity.HasKey(e => e.Smslogid).HasName("smslog_pkey");

            entity.ToTable("smslog");

            entity.Property(e => e.Smslogid)
                .ValueGeneratedNever()
                .HasColumnName("smslogid");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Confirmationnumber)
                .HasMaxLength(200)
                .HasColumnName("confirmationnumber");
            entity.Property(e => e.Createdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdate");
            entity.Property(e => e.Issmssent).HasColumnName("issmssent");
            entity.Property(e => e.Mobilenumber)
                .HasMaxLength(50)
                .HasColumnName("mobilenumber");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Sentdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sentdate");
            entity.Property(e => e.Senttries).HasColumnName("senttries");
            entity.Property(e => e.Smstemplate).HasColumnName("smstemplate");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("timesheet_pkey");

            entity.ToTable("timesheet");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Billname)
                .HasMaxLength(250)
                .HasColumnName("billname");
            entity.Property(e => e.Consult).HasColumnName("consult");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Housecall).HasColumnName("housecall");
            entity.Property(e => e.Invoiceid).HasColumnName("invoiceid");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Item)
                .HasMaxLength(250)
                .HasColumnName("item");
            entity.Property(e => e.Totalhours).HasColumnName("totalhours");
            entity.Property(e => e.Weekend).HasColumnName("weekend");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.Invoiceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheet_invoiceid_fkey");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("tokens_pkey");

            entity.ToTable("tokens");

            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Token1)
                .HasColumnType("character varying")
                .HasColumnName("token");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Aspnetuserid)
                .HasMaxLength(128)
                .HasColumnName("aspnetuserid");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Intdate).HasColumnName("intdate");
            entity.Property(e => e.Intyear).HasColumnName("intyear");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Ismobile).HasColumnName("ismobile");
            entity.Property(e => e.Isrequestwithemail).HasColumnName("isrequestwithemail");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
            entity.Property(e => e.Strmonth)
                .HasMaxLength(20)
                .HasColumnName("strmonth");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(10)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.UserAspnetusers)
                .HasForeignKey(d => d.Aspnetuserid)
                .HasConstraintName("User_aspnetuserid_fkey");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.UserCreatedbyNavigations)
                .HasForeignKey(d => d.Createdby)
                .HasConstraintName("User_createdby_fkey");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.UserModifiedbyNavigations)
                .HasForeignKey(d => d.Modifiedby)
                .HasConstraintName("User_modifiedby_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Users)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("User_regionid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
