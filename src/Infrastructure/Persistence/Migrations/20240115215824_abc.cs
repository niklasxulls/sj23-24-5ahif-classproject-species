using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace DiveSpecies.Infrastructure.persistence.migrations
{
    public partial class abc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DangerousFeeds",
                columns: table => new
                {
                    DangerousFeedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DangerousFeedExposedId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Headline = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    OccuresAtFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OccuresAtTill = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepthStartInMeter = table.Column<double>(type: "float", nullable: true),
                    DepthEndInMeter = table.Column<double>(type: "float", nullable: true),
                    Area = table.Column<MultiPolygon>(type: "geography", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangerousFeeds", x => x.DangerousFeedId);
                });

            migrationBuilder.CreateTable(
                name: "Dives",
                columns: table => new
                {
                    DiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiveExposedId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserExposedId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dives", x => x.DiveId);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesSearchHistory",
                columns: table => new
                {
                    SpeciesSearchHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserExposedId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchTerm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpeciesTypeId = table.Column<int>(type: "int", nullable: true),
                    OccuresIn = table.Column<int>(type: "int", nullable: true),
                    Area = table.Column<MultiPolygon>(type: "geography", nullable: true),
                    Population = table.Column<int>(type: "int", nullable: true),
                    PopulationOperator = table.Column<int>(type: "int", nullable: true),
                    DepthStartInMeter = table.Column<double>(type: "float", nullable: true),
                    DepthEndInMeter = table.Column<double>(type: "float", nullable: true),
                    SortBy = table.Column<int>(type: "int", nullable: false),
                    SortDesc = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesSearchHistory", x => x.SpeciesSearchHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesTypes",
                columns: table => new
                {
                    SpeciesTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesTypes", x => x.SpeciesTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpeciesExposedId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Population = table.Column<int>(type: "int", nullable: false),
                    SpeciesTypeId = table.Column<int>(type: "int", nullable: false),
                    OccuresIn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.SpeciesId);
                    table.ForeignKey(
                        name: "FK_Species_SpeciesTypes_SpeciesTypeId",
                        column: x => x.SpeciesTypeId,
                        principalTable: "SpeciesTypes",
                        principalColumn: "SpeciesTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DangerousFeedSpecies",
                columns: table => new
                {
                    DangerousFeedsDangerousFeedId = table.Column<int>(type: "int", nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangerousFeedSpecies", x => new { x.DangerousFeedsDangerousFeedId, x.SpeciesId });
                    table.ForeignKey(
                        name: "FK_DangerousFeedSpecies_DangerousFeeds_DangerousFeedsDangerousFeedId",
                        column: x => x.DangerousFeedsDangerousFeedId,
                        principalTable: "DangerousFeeds",
                        principalColumn: "DangerousFeedId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DangerousFeedSpecies_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sightings",
                columns: table => new
                {
                    SightingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SightingExposedId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiveId = table.Column<int>(type: "int", nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: false),
                    DepthStartInMeter = table.Column<double>(type: "float", nullable: true),
                    DepthEndInMeter = table.Column<double>(type: "float", nullable: true),
                    DiveIntoInMinutes = table.Column<double>(type: "float", nullable: false),
                    Area = table.Column<MultiPolygon>(type: "geography", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sightings", x => x.SightingId);
                    table.ForeignKey(
                        name: "FK_Sightings_Dives_DiveId",
                        column: x => x.DiveId,
                        principalTable: "Dives",
                        principalColumn: "DiveId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sightings_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesImage",
                columns: table => new
                {
                    SpeciesImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesImage", x => x.SpeciesImageId);
                    table.ForeignKey(
                        name: "FK_SpeciesImage_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesOccuresAt",
                columns: table => new
                {
                    SpeciesOccuresAtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpeciesId = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<Polygon>(type: "geography", nullable: true),
                    DepthStartInMeter = table.Column<double>(type: "float", nullable: true),
                    DepthEndInMeter = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesOccuresAt", x => x.SpeciesOccuresAtId);
                    table.ForeignKey(
                        name: "FK_SpeciesOccuresAt_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SightingImage",
                columns: table => new
                {
                    SightingImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SightingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SightingImage", x => x.SightingImageId);
                    table.ForeignKey(
                        name: "FK_SightingImage_Sightings_SightingId",
                        column: x => x.SightingId,
                        principalTable: "Sightings",
                        principalColumn: "SightingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DangerousFeeds_DangerousFeedId",
                table: "DangerousFeeds",
                column: "DangerousFeedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DangerousFeedSpecies_SpeciesId",
                table: "DangerousFeedSpecies",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_SightingImage_SightingId",
                table: "SightingImage",
                column: "SightingId");

            migrationBuilder.CreateIndex(
                name: "IX_Sightings_DiveId",
                table: "Sightings",
                column: "DiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Sightings_SightingExposedId",
                table: "Sightings",
                column: "SightingExposedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sightings_SpeciesId",
                table: "Sightings",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_SpeciesId",
                table: "Species",
                column: "SpeciesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_SpeciesTypeId",
                table: "Species",
                column: "SpeciesTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesImage_SpeciesId",
                table: "SpeciesImage",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesOccuresAt_SpeciesId",
                table: "SpeciesOccuresAt",
                column: "SpeciesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangerousFeedSpecies");

            migrationBuilder.DropTable(
                name: "SightingImage");

            migrationBuilder.DropTable(
                name: "SpeciesImage");

            migrationBuilder.DropTable(
                name: "SpeciesOccuresAt");

            migrationBuilder.DropTable(
                name: "SpeciesSearchHistory");

            migrationBuilder.DropTable(
                name: "DangerousFeeds");

            migrationBuilder.DropTable(
                name: "Sightings");

            migrationBuilder.DropTable(
                name: "Dives");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "SpeciesTypes");
        }
    }
}
