using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web2Prosjektoppgave.api.Migrations
{
    /// <inheritdoc />
    public partial class updateColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_BlogPosts_PostId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Comments",
                newName: "BlogPostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                newName: "IX_Comments_BlogPostId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "tJyhbLZldhwrJEYinJ+oQGVc21PMOgehy52kp779k1A=", "i0miSAhDk7TYzD69tRsLWdJ177Qtxbw4thW40ixPnzI=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "pEAeNMXpl4Gr2U9R54CPbjG7RfVEjXThd3hLJJxyHe0=", "zFJHQSomoKVINPSBoEY1sW6Gz8sXMQmbEfdP8W8pyUU=" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_BlogPosts_BlogPostId",
                table: "Comments",
                column: "BlogPostId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_BlogPosts_BlogPostId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "BlogPostId",
                table: "Comments",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_BlogPostId",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "gDHPqXGuX20e24xVxmdeBJIrpyt1lEp8yqFRGI65O08=", "xCHFhxlYsbFP8NDyNKANpntLi4KDuKCH5E8TNLv9peU=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "F0jCc/XXUMD7GXSLohaGCZ2Cjc2g3MTxfHfswe+aPj4=", "gapiY1xxghOcQyOU8A05vJ0mGX5iFYArN7Mv+HybJMs=" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_BlogPosts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
