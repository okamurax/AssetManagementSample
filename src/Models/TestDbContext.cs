using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Models
{
    public partial class TestDbContext : DbContext
    {
        public TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TestTable33> TestTable33s { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Program.csで設定
                // optionsBuilder.UseSqlServer("Server=localhost,49153\\SQLEXPRESS;Database=TestDb;persist security info=True;user id=sa;password=passw0rd!;MultipleActiveResultSets=True");
            }
        }

        // Update-databaseではDbContextは更新されない

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestTable33>(entity =>
            {
                entity.ToTable("TestTable33");

                //entity.HasKey(t => t.Id);
                // 主キーを指定

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Idユーザー名) // これはmodel側のプロパティ名
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID/ユーザー名"); // これはDB側のカラム名

                entity.Property(e => e.パスワード)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.パスワード１)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.パスワード２)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.パスワード３)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.メーカー)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.メール)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.モデル名)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.一般名称)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.備考)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.導入年月)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.所有形態)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.担当者１)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.担当者２)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.管理番号)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.設置保存場所)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("設置/保存場所");

                entity.Property(e => e.関連番号)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
