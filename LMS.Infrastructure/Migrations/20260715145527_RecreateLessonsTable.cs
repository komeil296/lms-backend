using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecreateLessonsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "Lessons",
        columns: table => new
        {
            Id = table.Column<Guid>(
                type: "uniqueidentifier",
                nullable: false),

            CourseId = table.Column<Guid>(
                type: "uniqueidentifier",
                nullable: false),

            Title = table.Column<string>(
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false),

            Content = table.Column<string>(
                type: "nvarchar(max)",
                nullable: false),

            VideoUrl = table.Column<string>(
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true),

            OrderIndex = table.Column<int>(
                type: "int",
                nullable: false),

            IsPublished = table.Column<bool>(
                type: "bit",
                nullable: false,
                defaultValue: false),

            CreatedAt = table.Column<DateTime>(
                type: "datetime2",
                nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey(
                name: "PK_Lessons",
                columns: x => x.Id);

            table.ForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                column: x => x.CourseId,
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        });

    migrationBuilder.CreateIndex(
        name: "IX_Lessons_CourseId_OrderIndex",
        table: "Lessons",
        columns: new[]
        {
            "CourseId",
            "OrderIndex"
        },
        unique: true);
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropTable(
        name: "Lessons");
}
    }
}
