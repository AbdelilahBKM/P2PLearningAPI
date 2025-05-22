using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PLearningAPI.Migrations
{
    /// <inheritdoc />
    public partial class addSimularityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Simularities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simularities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Simularities_Posts_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SimularityQuestions",
                columns: table => new
                {
                    SimularityId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimularityQuestions", x => new { x.SimularityId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_SimularityQuestions_Posts_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SimularityQuestions_Simularities_SimularityId",
                        column: x => x.SimularityId,
                        principalTable: "Simularities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Simularities_QuestionId",
                table: "Simularities",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SimularityQuestions_QuestionId",
                table: "SimularityQuestions",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimularityQuestions");

            migrationBuilder.DropTable(
                name: "Simularities");
        }
    }
}
