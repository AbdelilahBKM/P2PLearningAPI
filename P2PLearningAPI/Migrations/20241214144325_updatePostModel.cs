using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PLearningAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatePostModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Posts",
                newName: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AnswerId",
                table: "Posts",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_AnswerId",
                table: "Posts",
                column: "AnswerId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_AnswerId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AnswerId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "AnswerId",
                table: "Posts",
                newName: "PostId");
        }
    }
}
