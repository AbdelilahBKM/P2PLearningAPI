using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PLearningAPI.Migrations
{
    /// <inheritdoc />
    public partial class addColumn_IsAIGenerated_answers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAIGenerated",
                table: "Posts",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAIGenerated",
                table: "Posts");
        }
    }
}
