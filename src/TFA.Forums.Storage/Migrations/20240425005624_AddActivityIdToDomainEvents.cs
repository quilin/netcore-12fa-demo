using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFA.Forums.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityIdToDomainEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityId",
                table: "DomainEvents",
                type: "character varying(55)",
                maxLength: 55,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "DomainEvents");
        }
    }
}
