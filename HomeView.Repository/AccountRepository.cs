using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using HomeView.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace HomeView.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _config;

        public AccountRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<IdentityResult> CreateAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dataTable = new DataTable();
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("NormalizedUsername", typeof(string));
            dataTable.Columns.Add("Firstname", typeof(string));
            dataTable.Columns.Add("Lastname", typeof(string));
            dataTable.Columns.Add("Dateofbirth", typeof(string));
            dataTable.Columns.Add("PasswordHash", typeof(string));
            dataTable.Columns.Add("Addressline1", typeof(string));
            dataTable.Columns.Add("Addressline2", typeof(string));
            dataTable.Columns.Add("Addressline3", typeof(string));
            dataTable.Columns.Add("Town", typeof(string));
            dataTable.Columns.Add("City", typeof(string));
            dataTable.Columns.Add("Postcode", typeof(string));
            dataTable.Columns.Add("Accounttype", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("NormalizedEmail", typeof(string));
            dataTable.Columns.Add("Phone", typeof(string));
            dataTable.Columns.Add("MarketingEmail", typeof(bool));
            dataTable.Columns.Add("MarketingPhone", typeof(bool));
            dataTable.Columns.Add("ProfilePictureUrl", typeof(string));

            dataTable.Rows.Add(
                user.Username,
                user.NormalizedUsername,
                user.Firstname,
                user.Lastname,
                user.Dateofbirth,
                user.PasswordHash,
                user.Addressline1,
                user.Addressline2,
                user.Addressline3,
                user.Town,
                user.City,
                user.Postcode,
                user.Accounttype,
                user.Email,
                user.NormalizedEmail,
                user.Phone,
                user.MarketingEmail,
                user.MarketingPhone,
                user.ProfilepictureUrl
            );

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync(cancellationToken);

                await connection.ExecuteAsync("Account_Insert",
                    new {Account = dataTable.AsTableValuedParameter("dbo.AccountType")},
                    commandType: CommandType.StoredProcedure);
            }

            return IdentityResult.Success;
        }

        public async Task<UserIdentity> GetByUsernameAsync(string normalizedUsername, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            UserIdentity user;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync(cancellationToken);

                user = await connection.QuerySingleOrDefaultAsync<UserIdentity>(
                    "Account_GetByUsername", new {NormalizedUsername = normalizedUsername},
                    commandType: CommandType.StoredProcedure
                );
            }

            return user;
        }
    }
}
