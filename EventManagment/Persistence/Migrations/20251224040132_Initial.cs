using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastOtpSentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EventTyp__3214EC0726DF4889", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Registra__3214EC077CE0B27D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tags__3214EC07845EC1D8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DomainUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Department = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3214EC07F44ED148", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainUsers_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationStart = table.Column<DateOnly>(type: "date", nullable: false),
                    RegistrationEnd = table.Column<DateOnly>(type: "date", nullable: false),
                    Location_LocationType = table.Column<int>(type: "int", nullable: false),
                    Location_Address_VenueName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Location_Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location_Address_City = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Location_RoomNumber = table.Column<int>(type: "int", nullable: false),
                    Location_FloorNumber = table.Column<int>(type: "int", nullable: false),
                    Location_AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    RegisteredUsers = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    EventTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Events__3214EC07C821D3B6", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Events__CreatedB__4D94879B",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Events__EventTyp__4CA06362",
                        column: x => x.EventTypeId,
                        principalTable: "EventTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AgendaItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgendaItems_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EventTag__3214EC07F32EF299", x => x.Id);
                    table.ForeignKey(
                        name: "FK__EventTags__Event__571DF1D5",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__EventTags__TagId__5812160E",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getutcdate())"),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Registra__3214EC07B745E531", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Registrat__Event__5070F446",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Registrat__Statu__52593CB8",
                        column: x => x.StatusId,
                        principalTable: "RegistrationStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Registrat__UserI__5165187F",
                        column: x => x.UserId,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AgendaTracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgendaItemId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speaker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Room = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgendaTracks_AgendaItems_AgendaItemId",
                        column: x => x.AgendaItemId,
                        principalTable: "AgendaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "Employee", "EMPLOYEE" },
                    { 2, null, "Organizer", "ORGANIZER" },
                    { 3, null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastOtpSentTime", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "c6104f8c-3fe0-46f5-87d3-32c6b5752ce3", "admin@demo.com", true, null, false, null, "ADMIN@DEMO.COM", "ADMIN@DEMO.COM", "AQAAAAIAAYagAAAAEEXwNjh/cHM8TuIe4nQwj6aqktRgMFamv0A52R9W52LODdV0TMqC0mDPyvzepoQn/Q==", null, false, "b8d0818d-f3bf-428f-b2ad-121c7fd982fb", false, "admin@demo.com" },
                    { 2, 0, "97c37233-bb96-41ac-9dc1-c0c76339c83f", "organizer@demo.com", true, null, false, null, "ORGANIZER@DEMO.COM", "ORGANIZER@DEMO.COM", "AQAAAAIAAYagAAAAEOgg/owrGvo5hFd7T8J3XskHHez2flBYU5M+e2p5tzHx0AbRyuxkwSdx9VjgDnjmIA==", null, false, "02ee173f-cc43-4c2a-8000-bd3f9732728d", false, "organizer@demo.com" },
                    { 3, 0, "d443077e-bc3d-4946-8b99-b79fa201ff99", "employee1@demo.com", true, null, false, null, "EMPLOYEE1@DEMO.COM", "EMPLOYEE1@DEMO.COM", "AQAAAAIAAYagAAAAEEjHqwLXo0dEcJv26u5i5mGzlo4mQIou7aguIqlDupxvoRDEObE/xZapFxexoJlTRQ==", null, false, "639747a7-de46-47f4-b4cf-50684d086ca3", false, "employee1@demo.com" },
                    { 4, 0, "c95670b8-7af5-48d2-8867-826b920657a8", "employee2@demo.com", true, null, false, null, "EMPLOYEE2@DEMO.COM", "EMPLOYEE2@DEMO.COM", "AQAAAAIAAYagAAAAEPbXObJ+p+f4NnGi4dYtYs0Dm8+rWQqTuuXguH6aPb+xHrglIKZFlPX7nFEjMAJQ6g==", null, false, "4ce2bc8d-dc6b-42b5-84ef-35ec829f2ace", false, "employee2@demo.com" }
                });

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Hands-on session", true, "Workshop" },
                    { 2, "Community meetup", true, "Meetup" },
                    { 3, "Large multi-session event", true, "Conference" },
                    { 4, "Online session", true, "Webinar" },
                    { 5, "Internal team activity", true, "Team Building" }
                });

            migrationBuilder.InsertData(
                table: "RegistrationStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Awaiting approval", "Pending" },
                    { 2, "No spots currently available", "Waitlisted" },
                    { 3, "Registration cancelled", "Cancelled" },
                    { 4, "Spot confirmed", "Confirmed" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Category", "Name" },
                values: new object[,]
                {
                    { 1, "theme", "outdoor" },
                    { 2, "theme", "team-building" },
                    { 3, "perk", "free-food" },
                    { 4, "theme", "networking" },
                    { 5, "topic", "tech" },
                    { 6, "topic", "wellness" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 2, 2 },
                    { 1, 3 },
                    { 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "DomainUsers",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "FullName", "IsActive", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 2, "admin@demo.com", "Demo Admin", true, 3 },
                    { 2, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 4, "organizer@demo.com", "Demo Organizer", true, 2 },
                    { 3, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 6, "employee1@demo.com", "Demo Employee 1", true, 1 },
                    { 4, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, "employee2@demo.com", "Demo Employee 2", true, 1 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Location_Address_City", "Location_Address_Street", "Location_Address_VenueName", "Capacity", "CreatedAt", "CreatedById", "Description", "EndDateTime", "EventTypeId", "ImageUrl", "IsActive", "RegisteredUsers", "RegistrationEnd", "RegistrationStart", "StartDateTime", "Title", "UpdatedAt", "Location_AdditionalInformation", "Location_FloorNumber", "Location_LocationType", "Location_RoomNumber" },
                values: new object[,]
                {
                    { 1, "Tbilisi", "Gerdasd", "Building", 10, new DateTime(2025, 8, 1, 9, 0, 0, 0, DateTimeKind.Utc), 2, "Introductory workshop (past event)", new DateTime(2025, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), 1, null, true, 1, new DateOnly(2025, 9, 9), new DateOnly(2025, 8, 20), new DateTime(2025, 9, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Past Workshop: EF Core Basics", new DateTime(2025, 9, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Sigma", 5, 0, 4 },
                    { 2, "Tbilisi", "Rustaveli Ave", "Tech Hub", 60, new DateTime(2025, 12, 10, 10, 0, 0, 0, DateTimeKind.Utc), 2, "Community meetup and short talks", new DateTime(2026, 1, 15, 19, 0, 0, 0, DateTimeKind.Utc), 1, "https://pics.example.com/events/graphql.png", true, 1, new DateOnly(2026, 1, 14), new DateOnly(2025, 12, 20), new DateTime(2026, 1, 15, 17, 0, 0, 0, DateTimeKind.Utc), "GraphQL Meetup: APIs in Practice", new DateTime(2025, 12, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Snacks provided", 2, 0, 12 },
                    { 3, "Remote", "N/A", "Online", 200, new DateTime(2026, 1, 5, 11, 0, 0, 0, DateTimeKind.Utc), 2, "Live Q&A + examples", new DateTime(2026, 2, 5, 17, 30, 0, 0, DateTimeKind.Utc), 1, null, true, 0, new DateOnly(2026, 2, 4), new DateOnly(2026, 1, 10), new DateTime(2026, 2, 5, 16, 0, 0, 0, DateTimeKind.Utc), "Virtual: Clean Architecture Q&A", new DateTime(2026, 1, 5, 11, 0, 0, 0, DateTimeKind.Utc), "Join link in email", 0, 1, 0 },
                    { 4, "Tbilisi", "Chavchavadze", "Conference Center", 40, new DateTime(2026, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), 2, "Hands-on + streaming", new DateTime(2026, 3, 12, 12, 0, 0, 0, DateTimeKind.Utc), 1, null, true, 1, new DateOnly(2026, 3, 11), new DateOnly(2026, 2, 10), new DateTime(2026, 3, 12, 9, 0, 0, 0, DateTimeKind.Utc), "Hybrid: Docker for .NET Developers", new DateTime(2026, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Streaming available", 1, 2, 7 },
                    { 5, "Tbilisi", "Kazbegi Ave", "DB Lab", 25, new DateTime(2025, 9, 15, 12, 0, 0, 0, DateTimeKind.Utc), 2, "Indexing and query tuning", new DateTime(2025, 10, 22, 15, 0, 0, 0, DateTimeKind.Utc), 1, null, true, 0, new DateOnly(2025, 10, 21), new DateOnly(2025, 10, 1), new DateTime(2025, 10, 22, 13, 0, 0, 0, DateTimeKind.Utc), "Past: SQL Performance Clinic", new DateTime(2025, 10, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Bring laptop", 4, 0, 3 },
                    { 6, "Remote", "N/A", "Online", 300, new DateTime(2026, 3, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, "xUnit + Moq + patterns", new DateTime(2026, 4, 8, 18, 0, 0, 0, DateTimeKind.Utc), 1, "https://pics.example.com/events/testing.png", true, 1, new DateOnly(2026, 4, 7), new DateOnly(2026, 3, 10), new DateTime(2026, 4, 8, 15, 0, 0, 0, DateTimeKind.Utc), "Virtual: Unit Testing Masterclass", new DateTime(2026, 3, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Recording will be shared", 0, 1, 0 },
                    { 7, "Tbilisi", "Tamarashvili", "Community Space", 80, new DateTime(2026, 4, 1, 8, 0, 0, 0, DateTimeKind.Utc), 2, "Bring a project, ship something", new DateTime(2026, 5, 20, 21, 0, 0, 0, DateTimeKind.Utc), 1, null, true, 0, new DateOnly(2026, 5, 19), new DateOnly(2026, 4, 20), new DateTime(2026, 5, 20, 18, 0, 0, 0, DateTimeKind.Utc), "In-Person: Hack Night", new DateTime(2026, 4, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Open seating", 6, 0, 21 },
                    { 8, "Tbilisi", "Pekini Ave", "Cloud Lab", 50, new DateTime(2026, 5, 1, 9, 0, 0, 0, DateTimeKind.Utc), 2, "CI/CD + deploy walkthrough", new DateTime(2026, 6, 11, 14, 0, 0, 0, DateTimeKind.Utc), 1, "https://pics.example.com/events/cloud.png", true, 0, new DateOnly(2026, 6, 10), new DateOnly(2026, 5, 10), new DateTime(2026, 6, 11, 10, 0, 0, 0, DateTimeKind.Utc), "Hybrid: Cloud Deployment Workshop", new DateTime(2026, 5, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Workshop materials included", 3, 2, 9 }
                });

            migrationBuilder.InsertData(
                table: "EventTags",
                columns: new[] { "Id", "EventId", "TagId" },
                values: new object[,]
                {
                    { 1, 1, 5 },
                    { 2, 2, 3 },
                    { 3, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "Registrations",
                columns: new[] { "Id", "CancelledAt", "EventId", "RegisteredAt", "StatusId", "UserId" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2025, 9, 1, 10, 0, 0, 0, DateTimeKind.Utc), 4, 3 },
                    { 2, null, 2, new DateTime(2025, 11, 20, 10, 0, 0, 0, DateTimeKind.Utc), 4, 4 },
                    { 3, null, 4, new DateTime(2025, 11, 10, 10, 0, 0, 0, DateTimeKind.Utc), 4, 3 },
                    { 4, null, 6, new DateTime(2025, 12, 10, 10, 0, 0, 0, DateTimeKind.Utc), 4, 4 },
                    { 5, null, 2, new DateTime(2025, 11, 21, 10, 0, 0, 0, DateTimeKind.Utc), 2, 3 },
                    { 6, new DateTime(2025, 8, 28, 10, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 8, 25, 10, 0, 0, 0, DateTimeKind.Utc), 3, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgendaItems_EventId_StartTime",
                table: "AgendaItems",
                columns: new[] { "EventId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AgendaTracks_AgendaItemId",
                table: "AgendaTracks",
                column: "AgendaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D10534D7C521D4",
                table: "DomainUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Users_Email",
                table: "DomainUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedById",
                table: "Events",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventTypeId_StartDateTime_IsActive",
                table: "Events",
                columns: new[] { "EventTypeId", "StartDateTime", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_Location_Address_VenueName_Location_Address_City",
                table: "Events",
                columns: new[] { "Location_Address_VenueName", "Location_Address_City" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDateTime_Active",
                table: "Events",
                column: "StartDateTime",
                filter: "([IsActive]=(1))");

            migrationBuilder.CreateIndex(
                name: "IX_EventTags_TagId",
                table: "EventTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "UQ_EventTags",
                table: "EventTags",
                columns: new[] { "EventId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__EventTyp__737584F6755CCC5F",
                table: "EventTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_EventId_StatusId",
                table: "Registrations",
                columns: new[] { "EventId", "StatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_StatusId",
                table: "Registrations",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_UserId",
                table: "Registrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ_Registrations_EventUser_Active",
                table: "Registrations",
                columns: new[] { "EventId", "UserId" },
                unique: true,
                filter: "([StatusId]>(3))");

            migrationBuilder.CreateIndex(
                name: "UQ__Registra__737584F6E6264F98",
                table: "RegistrationStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tags__737584F69205C21B",
                table: "Tags",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgendaTracks");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EventTags");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "AgendaItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "RegistrationStatuses");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "DomainUsers");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
