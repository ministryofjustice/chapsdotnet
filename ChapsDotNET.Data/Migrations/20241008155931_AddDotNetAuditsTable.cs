using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChapsDotNET.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDotNetAuditsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    AlertID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Live = table.Column<bool>(type: "bit", nullable: false),
                    EventStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RaisedHours = table.Column<int>(type: "int", nullable: false),
                    WarnStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.AlertID);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    CampaignID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    deactivated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deactivatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.CampaignID);
                });

            migrationBuilder.CreateTable(
                name: "CorrespondenceTypes",
                columns: table => new
                {
                    CorrespondenceTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Acronym = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LimitedOnly = table.Column<bool>(type: "bit", nullable: false),
                    TargetDays = table.Column<int>(type: "int", nullable: true),
                    Signatory = table.Column<bool>(type: "bit", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    deactivated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deactivatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrespondenceTypes", x => x.CorrespondenceTypeID);
                });

            migrationBuilder.CreateTable(
                name: "DotNetAudits",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Object = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectPrimaryKey = table.Column<int>(type: "int", nullable: true),
                    RootPrimaryKey = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DotNetAudits", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "LeadSubjects",
                columns: table => new
                {
                    LeadSubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    deactivated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deactivatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadSubjects", x => x.LeadSubjectId);
                });

            migrationBuilder.CreateTable(
                name: "MoJMinisters",
                columns: table => new
                {
                    MoJMinisterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    prefix = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    suffix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Rank = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    deactivated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deactivatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoJMinisters", x => x.MoJMinisterID);
                });

            migrationBuilder.CreateTable(
                name: "PublicHolidays",
                columns: table => new
                {
                    PublicHolidayID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicHolidays", x => x.PublicHolidayID);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    strength = table.Column<int>(type: "int", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.strength);
                });

            migrationBuilder.CreateTable(
                name: "Salutations",
                columns: table => new
                {
                    salutationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    deactivated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deactivatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salutations", x => x.salutationID);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Acronym = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    isOGD = table.Column<bool>(type: "bit", nullable: false),
                    isPOD = table.Column<bool>(type: "bit", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    deactivated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deactivatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamID);
                });

            migrationBuilder.CreateTable(
                name: "MPs",
                columns: table => new
                {
                    MPID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RtHon = table.Column<bool>(type: "bit", nullable: false),
                    SalutationID = table.Column<int>(type: "int", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AddressLine3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    County = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    FirstNames = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Suffix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Town = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    deactivatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deactivatedByID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MPs", x => x.MPID);
                    table.ForeignKey(
                        name: "FK_MPs_Salutations_SalutationID",
                        column: x => x.SalutationID,
                        principalTable: "Salutations",
                        principalColumn: "salutationID");
                });

            migrationBuilder.CreateTable(
                name: "CorrespondenceTypesByTeams",
                columns: table => new
                {
                    TeamID = table.Column<int>(type: "int", nullable: false),
                    CorrespondenceTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrespondenceTypesByTeams", x => new { x.CorrespondenceTypeID, x.TeamID });
                    table.ForeignKey(
                        name: "FK_CorrespondenceTypesByTeams_CorrespondenceTypes_CorrespondenceTypeID",
                        column: x => x.CorrespondenceTypeID,
                        principalTable: "CorrespondenceTypes",
                        principalColumn: "CorrespondenceTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorrespondenceTypesByTeams_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleStrength = table.Column<int>(type: "int", nullable: false),
                    TeamID = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Changeable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleStrength",
                        column: x => x.RoleStrength,
                        principalTable: "Roles",
                        principalColumn: "strength",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorrespondenceTypesByTeams_TeamID",
                table: "CorrespondenceTypesByTeams",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_MPs_SalutationID",
                table: "MPs",
                column: "SalutationID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleStrength",
                table: "Users",
                column: "RoleStrength");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamID",
                table: "Users",
                column: "TeamID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "CorrespondenceTypesByTeams");

            migrationBuilder.DropTable(
                name: "DotNetAudits");

            migrationBuilder.DropTable(
                name: "LeadSubjects");

            migrationBuilder.DropTable(
                name: "MoJMinisters");

            migrationBuilder.DropTable(
                name: "MPs");

            migrationBuilder.DropTable(
                name: "PublicHolidays");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CorrespondenceTypes");

            migrationBuilder.DropTable(
                name: "Salutations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
