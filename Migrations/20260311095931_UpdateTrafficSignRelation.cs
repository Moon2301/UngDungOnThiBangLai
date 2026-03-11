using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UngDungOnThiBangLai.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrafficSignRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Creadit",
                table: "Users",
                newName: "Credit");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TrafficSignId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LicenseCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LicenseCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrafficSignId",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TrafficSigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficSigns", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$fXbwVeaPJkXI/KHd6Oak3.l9CKDqPAVC3RBX61JhPcqsQ389DYCnK");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TrafficSignId",
                table: "Questions",
                column: "TrafficSignId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_TrafficSignId",
                table: "Answers",
                column: "TrafficSignId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_TrafficSigns_TrafficSignId",
                table: "Answers",
                column: "TrafficSignId",
                principalTable: "TrafficSigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TrafficSigns_TrafficSignId",
                table: "Questions",
                column: "TrafficSignId",
                principalTable: "TrafficSigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_TrafficSigns_TrafficSignId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TrafficSigns_TrafficSignId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "TrafficSigns");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TrafficSignId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Answers_TrafficSignId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "TrafficSignId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "TrafficSignId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "Credit",
                table: "Users",
                newName: "Creadit");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LicenseCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LicenseCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$1PkMTDFPQvmhKVXCHphuT.LUwyQo0JwM8BVPbdJBBVXGtlfKrpNjG");
        }
    }
}
