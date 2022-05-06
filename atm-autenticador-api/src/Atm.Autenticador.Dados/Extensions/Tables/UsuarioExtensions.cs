using Atm.Autenticador.Dados.Helpers;
using Atm.Autenticador.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Atm.Autenticador.Dados.Extensions.Tables
{
    internal static class UsuarioExtensions
    {
        internal static void SetupUsuario(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Usuario>()
                .HasIndex(u => u.Id);

            modelBuilder
                .Entity<Usuario>()
                .Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder
                .Entity<Usuario>()
                .Property(u => u.Senha)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder
                .Entity<Usuario>()
                .Property(u => u.Token)
                .HasMaxLength(300);
        }

        internal static void SetupUsuarioData(this MigrationBuilder migrationBuilder)
        {
            Guid id = Guid.NewGuid();
            migrationBuilder.InsertData("Usuario",
                new string[] { "Id", "DataCadastro", "DataAtualizacao", "Login", "Senha", "Token" },
                new object[] { id, DateHelper.GetLocalTime(), null, "automar", CryptographyHelper.ToHash("marvin-atm"), CryptographyHelper.GenerateJwt(id)});
        }
    }
}
