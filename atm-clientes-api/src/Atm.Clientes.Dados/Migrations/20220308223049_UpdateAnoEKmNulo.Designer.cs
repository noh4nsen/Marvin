﻿// <auto-generated />
using System;
using Atm.Clientes.Dados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Atm.Clientes.Dados.Migrations
{
    [DbContext(typeof(DbContext))]
    [Migration("20220308223049_UpdateAnoEKmNulo")]
    partial class UpdateAnoEKmNulo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Atm.Clientes.Domain.Carro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<short?>("Ano")
                        .HasColumnType("smallint");

                    b.Property<bool>("Ativo")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("DataAtualizacao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Descricao")
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<string>("Marca")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Modelo")
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<long?>("Quilometragem")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Carro");
                });

            modelBuilder.Entity("Atm.Clientes.Domain.Cliente", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Ativo")
                        .HasColumnType("boolean");

                    b.Property<string>("Cpf")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DataAtualizacao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<string>("Endereco")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Telefone")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("CarroCliente", b =>
                {
                    b.Property<Guid>("CarrosId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientesId")
                        .HasColumnType("uuid");

                    b.HasKey("CarrosId", "ClientesId");

                    b.HasIndex("ClientesId");

                    b.ToTable("CarroCliente");
                });

            modelBuilder.Entity("CarroCliente", b =>
                {
                    b.HasOne("Atm.Clientes.Domain.Carro", null)
                        .WithMany()
                        .HasForeignKey("CarrosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Atm.Clientes.Domain.Cliente", null)
                        .WithMany()
                        .HasForeignKey("ClientesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
