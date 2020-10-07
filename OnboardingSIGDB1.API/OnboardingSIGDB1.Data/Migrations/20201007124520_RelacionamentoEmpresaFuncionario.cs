﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace OnboardingSIGDB1.Data.Migrations
{
    public partial class RelacionamentoEmpresaFuncionario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmpresaId",
                table: "Funcionario",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_EmpresaId",
                table: "Funcionario",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionario_Empresa_EmpresaId",
                table: "Funcionario",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionario_Empresa_EmpresaId",
                table: "Funcionario");

            migrationBuilder.DropIndex(
                name: "IX_Funcionario_EmpresaId",
                table: "Funcionario");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Funcionario");
        }
    }
}
