using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web2Prosjektoppgave.api.Migrations
{
    /// <inheritdoc />
    public partial class addBlogPostTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "Users",
                newName: "ModifiedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Users",
                newName: "CreatedById");

            migrationBuilder.CreateTable(
                name: "BlogPostTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostBlogPostTag",
                columns: table => new
                {
                    BlogPostTagsId = table.Column<int>(type: "int", nullable: false),
                    BlogPostsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostBlogPostTag", x => new { x.BlogPostTagsId, x.BlogPostsId });
                    table.ForeignKey(
                        name: "FK_BlogPostBlogPostTag_BlogPostTags_BlogPostTagsId",
                        column: x => x.BlogPostTagsId,
                        principalTable: "BlogPostTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostBlogPostTag_BlogPosts_BlogPostsId",
                        column: x => x.BlogPostsId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "ctUJ0iRYCEI0/vQyzG1Rd+miScPoGw4nGuHLd9iXOfA=", "Vhx5U9Ix0S2Qzimzn/8mVWZPl+VdFhCmYFZXpOSRBZ0=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "r2oduGsxrQ79ieIkKUvoMrpD33AbAPfJXWEfom8Tm4A=", "3GS6GFv+zrAfM2VhIZtNWBQ0AasSm29mZ/lYHpXgMqg=" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostBlogPostTag_BlogPostsId",
                table: "BlogPostBlogPostTag",
                column: "BlogPostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostBlogPostTag");

            migrationBuilder.DropTable(
                name: "BlogPostTags");

            migrationBuilder.RenameColumn(
                name: "ModifiedById",
                table: "Users",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Users",
                newName: "CreatedBy");

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
        }
    }
}
