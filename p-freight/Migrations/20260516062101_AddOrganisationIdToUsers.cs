using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace p_freight.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganisationIdToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "organisation_id",
                table: "users",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "organisation_id",
                table: "users");
        }
    }
}
