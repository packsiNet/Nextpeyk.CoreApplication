using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreApplication.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMustChangePasswordToUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                table: "UserAccount",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                table: "UserAccount");
        }
    }
}
