﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Techbuzzers_bank.Data;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Techbuzzers_bank.Models.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("UserDetailsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("accountName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isPrimary")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserDetailsId");

                    b.ToTable("account");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.LoanPayables", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<string>("LoanId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoansId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("transactionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("LoansId");

                    b.HasIndex("transactionId");

                    b.ToTable("payables");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.Loans", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Due")
                        .HasColumnType("real");

                    b.Property<float>("LoanAmount")
                        .HasColumnType("real");

                    b.Property<string>("LoanType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Tenure")
                        .HasColumnType("int");

                    b.Property<float>("TenureAmount")
                        .HasColumnType("real");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserDetailsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("loanDetailsId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("paidTenures")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserDetailsId");

                    b.ToTable("loans");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.Transactions", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<string>("CreditId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreditUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreditUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DebitUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoansId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<double>("closingBalance")
                        .HasColumnType("float");

                    b.Property<double>("openingBalance")
                        .HasColumnType("float");

                    b.Property<double>("receiverClosingBalance")
                        .HasColumnType("float");

                    b.Property<double>("receiverOpeningBalance")
                        .HasColumnType("float");

                    b.Property<string>("transactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LoansId");

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.UserDetails", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("AdhaarNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PANNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.Property<int>("Pin")
                        .HasColumnType("int");

                    b.Property<string>("PrimaryAccountId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("userDetails");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.Bill", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserDetailsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("amount")
                        .HasColumnType("real");

                    b.Property<string>("billDetailsId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("billNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("billType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("transactionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserDetailsId");

                    b.HasIndex("transactionId");

                    b.ToTable("bill");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.BillDetails", b =>
                {
                    b.Property<string>("BillId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BillProviderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BillType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BillingAccount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("BillingAccountPhoneNumber")
                        .HasColumnType("bigint");

                    b.HasKey("BillId");

                    b.ToTable("billDetails");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.Insurance", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UniqueIdentificationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserDetailsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float?>("amountCovered")
                        .HasColumnType("real");

                    b.Property<bool>("claimed")
                        .HasColumnType("bit");

                    b.Property<float?>("installmentAmount")
                        .HasColumnType("real");

                    b.Property<string>("insurancePolicyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("purchaseAmount")
                        .HasColumnType("float");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("valididTill")
                        .HasColumnType("datetime2");

                    b.Property<int>("yearOfPurchase")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("UserDetailsId");

                    b.ToTable("insurance");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.InsurancePayables", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("InstallmentAmount")
                        .HasColumnType("real");

                    b.Property<int>("InstallmentYear")
                        .HasColumnType("int");

                    b.Property<string>("InsuranceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("transactionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("InsuranceId");

                    b.HasIndex("transactionId");

                    b.ToTable("insurancePayables");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.InsurancePolicies", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InsuranceAccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InsuranceType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Insurancevalidity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("insurancePolicies");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.InsuranceRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("accountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("amountCovered")
                        .HasColumnType("float");

                    b.Property<double>("balance")
                        .HasColumnType("float");

                    b.Property<double>("installmentAmount")
                        .HasColumnType("float");

                    b.Property<string>("insuranceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("insuranceType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("purchaseAmount")
                        .HasColumnType("float");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("uniqueIdentificationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("yearOfPurchase")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("insuranceRequests");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.LoanDetails", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AmouuntGranted")
                        .HasColumnType("float");

                    b.Property<string>("LoanType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxLoanTenure")
                        .HasColumnType("int");

                    b.Property<float>("ROI")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("loanDetails");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.LoanRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("accountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("balance")
                        .HasColumnType("float");

                    b.Property<string>("loanId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("loanType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("requestedAmount")
                        .HasColumnType("float");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("tenure")
                        .HasColumnType("int");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("loanRequests");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.Account", b =>
                {
                    b.HasOne("Techbuzzers_bank.Models.UserDetails", null)
                        .WithMany("accounts")
                        .HasForeignKey("UserDetailsId");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.LoanPayables", b =>
                {
                    b.HasOne("Techbuzzers_bank.Models.Loans", null)
                        .WithMany("Payables")
                        .HasForeignKey("LoansId");

                    b.HasOne("Techbuzzers_bank.Models.Transactions", "transaction")
                        .WithMany()
                        .HasForeignKey("transactionId");

                    b.Navigation("transaction");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.Loans", b =>
                {
                    b.HasOne("Techbuzzers_bank.Models.UserDetails", null)
                        .WithMany("loans")
                        .HasForeignKey("UserDetailsId");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.Transactions", b =>
                {
                    b.HasOne("Techbuzzers_bank.Models.Loans", null)
                        .WithMany("paidTenuresList")
                        .HasForeignKey("LoansId");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.Bill", b =>
                {
                    b.HasOne("Techbuzzers_bank.Models.UserDetails", null)
                        .WithMany("bills")
                        .HasForeignKey("UserDetailsId");

                    b.HasOne("Techbuzzers_bank.Models.Transactions", "transaction")
                        .WithMany()
                        .HasForeignKey("transactionId");

                    b.Navigation("transaction");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.Insurance", b =>
                {
                    b.HasOne("Techbuzzers_bank.Models.UserDetails", null)
                        .WithMany("insurances")
                        .HasForeignKey("UserDetailsId");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.InsurancePayables", b =>
                {
                    b.HasOne("TeechBuzzersBank.Models.Insurance", null)
                        .WithMany("payables")
                        .HasForeignKey("InsuranceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Techbuzzers_bank.Models.Transactions", "transaction")
                        .WithMany()
                        .HasForeignKey("transactionId");

                    b.Navigation("transaction");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.Loans", b =>
                {
                    b.Navigation("Payables");

                    b.Navigation("paidTenuresList");
                });

            modelBuilder.Entity("Techbuzzers_bank.Models.UserDetails", b =>
                {
                    b.Navigation("accounts");

                    b.Navigation("bills");

                    b.Navigation("insurances");

                    b.Navigation("loans");
                });

            modelBuilder.Entity("TeechBuzzersBank.Models.Insurance", b =>
                {
                    b.Navigation("payables");
                });
#pragma warning restore 612, 618
        }
    }
}
