using Microsoft.EntityFrameworkCore.Migrations;

namespace OnboardingSIGDB1.Data.Migrations
{
    public partial class RelacionamentoFuncionarioCargoOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioCargo_Cargo_CargoId",
                table: "FuncionarioCargo");

            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioCargo_Funcionario_FuncionarioId",
                table: "FuncionarioCargo");

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioCargo_Cargo_CargoId",
                table: "FuncionarioCargo",
                column: "CargoId",
                principalTable: "Cargo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioCargo_Funcionario_FuncionarioId",
                table: "FuncionarioCargo",
                column: "FuncionarioId",
                principalTable: "Funcionario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioCargo_Cargo_CargoId",
                table: "FuncionarioCargo");

            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioCargo_Funcionario_FuncionarioId",
                table: "FuncionarioCargo");

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioCargo_Cargo_CargoId",
                table: "FuncionarioCargo",
                column: "CargoId",
                principalTable: "Cargo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioCargo_Funcionario_FuncionarioId",
                table: "FuncionarioCargo",
                column: "FuncionarioId",
                principalTable: "Funcionario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
