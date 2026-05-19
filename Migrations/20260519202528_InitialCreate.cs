using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClyvoCareOSChallenge.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_TUTORES",
                columns: table => new
                {
                    ID_TUTOR = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_COMPLETO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    ENDERECO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    CIDADE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ESTADO = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TUTORES", x => x.ID_TUTOR);
                });

            migrationBuilder.CreateTable(
                name: "TB_PETS",
                columns: table => new
                {
                    ID_PET = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ESPECIE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    RACA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DATA_NASCIMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    SEXO = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    PESO_KG = table.Column<decimal>(type: "NUMBER(5,2)", nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ID_TUTOR = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PETS", x => x.ID_PET);
                    table.ForeignKey(
                        name: "FK_TB_PETS_TB_TUTORES_ID_TUTOR",
                        column: x => x.ID_TUTOR,
                        principalTable: "TB_TUTORES",
                        principalColumn: "ID_TUTOR",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_CONSULTAS",
                columns: table => new
                {
                    ID_CONSULTA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DATA_CONSULTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    MOTIVO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DIAGNOSTICO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    PRESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    VETERINARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CLINICA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PESO_NA_CONSULTA = table.Column<decimal>(type: "NUMBER(5,2)", nullable: false),
                    ID_PET = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CONSULTAS", x => x.ID_CONSULTA);
                    table.ForeignKey(
                        name: "FK_TB_CONSULTAS_TB_PETS_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PETS",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_TRATAMENTOS",
                columns: table => new
                {
                    ID_TRATAMENTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_TRATAMENTO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DATA_INICIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_FIM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    VETERINARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_PET = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TRATAMENTOS", x => x.ID_TRATAMENTO);
                    table.ForeignKey(
                        name: "FK_TB_TRATAMENTOS_TB_PETS_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PETS",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_VACINAS",
                columns: table => new
                {
                    ID_VACINA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_VACINA = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DATA_APLICACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_PROXIMA_DOSE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VETERINARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CLINICA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    OBSERVACOES = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    ID_PET = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_VACINAS", x => x.ID_VACINA);
                    table.ForeignKey(
                        name: "FK_TB_VACINAS_TB_PETS_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PETS",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONSULTAS_ID_PET",
                table: "TB_CONSULTAS",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PETS_ID_TUTOR",
                table: "TB_PETS",
                column: "ID_TUTOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRATAMENTOS_ID_PET",
                table: "TB_TRATAMENTOS",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TUTORES_EMAIL",
                table: "TB_TUTORES",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_VACINAS_ID_PET",
                table: "TB_VACINAS",
                column: "ID_PET");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CONSULTAS");

            migrationBuilder.DropTable(
                name: "TB_TRATAMENTOS");

            migrationBuilder.DropTable(
                name: "TB_VACINAS");

            migrationBuilder.DropTable(
                name: "TB_PETS");

            migrationBuilder.DropTable(
                name: "TB_TUTORES");
        }
    }
}
