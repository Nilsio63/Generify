using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Generify.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SpotifyId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshToken = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlaylistDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TargetPlaylistId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistDefinitions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderInstructions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderNr = table.Column<int>(type: "int", nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    OrderDirection = table.Column<int>(type: "int", nullable: false),
                    PlaylistDefinitionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInstructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderInstructions_PlaylistDefinitions_PlaylistDefinitionId",
                        column: x => x.PlaylistDefinitionId,
                        principalTable: "PlaylistDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlaylistSources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderNr = table.Column<int>(type: "int", nullable: false),
                    SourceId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    InclusionType = table.Column<int>(type: "int", nullable: false),
                    PlaylistDefinitionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistSources_PlaylistDefinitions_PlaylistDefinitionId",
                        column: x => x.PlaylistDefinitionId,
                        principalTable: "PlaylistDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OrderInstructions_PlaylistDefinitionId",
                table: "OrderInstructions",
                column: "PlaylistDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistDefinitions_UserId",
                table: "PlaylistDefinitions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSources_PlaylistDefinitionId",
                table: "PlaylistSources",
                column: "PlaylistDefinitionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderInstructions");

            migrationBuilder.DropTable(
                name: "PlaylistSources");

            migrationBuilder.DropTable(
                name: "PlaylistDefinitions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
