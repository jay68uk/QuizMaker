using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizUser.Migrations
{
    /// <inheritdoc />
    public partial class UserPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "carts:add");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "carts:read");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "carts:remove");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "categories:read");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "categories:update");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "event-statistics:read");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "events:read");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "events:search");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "events:update");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "orders:create");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "orders:read");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "ticket-types:read");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "ticket-types:update");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "tickets:check-in");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "tickets:read");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "users:read");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "users:update");


            var newPermissions = new[]
            {
                "users:read",
                "users:update",
                "quizzes:read",
                "quizzes:run",
                "quizzes:update"
            };

            migrationBuilder.InsertData(
                schema: "users",
                table: "permissions",
                column: "code",
                values: newPermissions);

            var roles = new[]
            {
                "Member",
                "Administrator"
            };

// Define role-permission associations
            var rolePermissions = new (string Role, string Permission)[]
            {
                // Member permissions
                (roles[0], "users:read"),
                (roles[0], "users:update"),
                (roles[0], "quizzes:read"),
                (roles[0], "quizzes:run"),
                (roles[0], "quizzes:update"),
                // Administrator permissions
                (roles[1], "users:read"),
                (roles[1], "users:update"),
                (roles[1], "quizzes:read"),
                (roles[1], "quizzes:run"),
                (roles[1], "quizzes:update")
            };


            foreach (var rp in rolePermissions)
            {
                migrationBuilder.InsertData(
                    schema: "users",
                    table: "role_permissions",
                    columns: new[] { "permission_code", "role_name" },
                    values: new object[] { rp.Permission, rp.Role });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var newRolePermissions = new (string Role, string Permission)[]
            {
                // Member permissions
                ("Member", "users:read"),
                ("Member", "users:update"),
                ("Member", "quizzes:read"),
                ("Member", "quizzes:run"),
                // Administrator permissions
                ("Administrator", "users:read"),
                ("Administrator", "users:update"),
                ("Administrator", "quizzes:read"),
                ("Administrator", "quizzes:run"),
                ("Administrator", "quizzes:update")
            };

            foreach (var rp in newRolePermissions)
            {
                migrationBuilder.DeleteData(
                    schema: "users",
                    table: "role_permissions",
                    keyColumns: new[] { "permission_code", "role_name" },
                    keyValues: new object[] { rp.Permission, rp.Role });
            }

            // Delete new permissions
            var newPermissions = new[]
            {
                "users:read",
                "users:update",
                "quizzes:read",
                "quizzes:run",
                "quizzes:update"
            };

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValues: newPermissions);

            migrationBuilder.InsertData(
                schema: "users",
                table: "permissions",
                column: "code",
                values: new object[]
                {
                    "carts:add",
                    "carts:read",
                    "carts:remove",
                    "categories:read",
                    "categories:update",
                    "event-statistics:read",
                    "events:read",
                    "events:search",
                    "events:update",
                    "orders:create",
                    "orders:read",
                    "ticket-types:read",
                    "ticket-types:update",
                    "tickets:check-in",
                    "tickets:read",
                    "users:read",
                    "users:update"
                });
        }
    }
}
