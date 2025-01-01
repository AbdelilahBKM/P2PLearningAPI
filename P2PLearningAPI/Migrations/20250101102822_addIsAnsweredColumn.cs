using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PLearningAPI.Migrations
{
    /// <inheritdoc />
    public partial class addIsAnsweredColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAnswered",
                table: "Posts",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAnswered",
                table: "Posts");
        }
    }
}
