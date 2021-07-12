using Microsoft.EntityFrameworkCore.Migrations;

namespace CMSApplication.Data.Migrations
{
    public partial class AdminUserAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e8498900-4771-44a9-9deb-279e0606ba90", "e8498900-4771-44a9-9deb-279e0606ba90", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cd958c82-4713-4c3c-9f0e-8f31675041d5", "703002a4-c5be-4e5d-81f6-8c116e54a731", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CityId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bc67b882-3779-4af8-a585-2898a35ecf96", 0, 1, "d97931b6-27c9-4db0-8d2a-56de9f142800", "admin@site.com", true, "Admin", null, false, null, "ADMIN@SITE.COM", "ADMIN", "AQAAAAEAACcQAAAAEP2XRyC/sg45HDCtyqWCO0lp9vfqZq6uLuXTegS8MkyhC8CUhzplEOYae8B7H7Gm9g==", null, false, "fc6b4849-3b78-4e30-b29e-ef554374476e", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e8498900-4771-44a9-9deb-279e0606ba90", "bc67b882-3779-4af8-a585-2898a35ecf96" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd958c82-4713-4c3c-9f0e-8f31675041d5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e8498900-4771-44a9-9deb-279e0606ba90", "bc67b882-3779-4af8-a585-2898a35ecf96" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8498900-4771-44a9-9deb-279e0606ba90");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bc67b882-3779-4af8-a585-2898a35ecf96");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
