using ApplicationInterface.SchoolMaster;
using Dapper;
using DomainModel.SchoolMaster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SchoolMaster
{
    public class PassportTypeService : IPassportTypeRepository
    {
        private readonly string _connectionString;

        public PassportTypeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DatabaseSettings1:ConnectionString")
                ?? throw new ArgumentNullException("DatabaseSettings1:ConnectionString");
        }
        public async Task<IEnumerable<PassportTypeModel>> GetAllAsync()
        {
            try
            {
                using var con = new SqlConnection(_connectionString);
                string sql = "SELECT PassportTypeID,PassportTypeName,Remarks,IsValid,CreatedDate,CreatedBy FROM MstPassportType ORDER BY PassportTypeID ASC";
                return await con.QueryAsync<PassportTypeModel>(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }
        public async Task<int> DeletePassportTypeData(int passportTypeId)
        {
            try
            {
                using var con = new SqlConnection(_connectionString);
                string sql = @"UPDATE MstPassportType SET IsValid = CASE WHEN IsValid = 1 THEN 0 ELSE 1 END
                       WHERE PassportTypeID = @PassportTypeID";
                return await con.ExecuteAsync(sql, new { PassportTypeID = passportTypeId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }
        public async Task<string> AddUpdatePassportType(PassportTypeModel objPassportType)
        {
            try
            {
                string returnValue;
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("V3M_InsertUpdate_PassportType", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PassportTypeID", objPassportType.PassportTypeID);
                        cmd.Parameters.AddWithValue("@PassportTypeName", objPassportType.PassportTypeName);
                        cmd.Parameters.AddWithValue("@Remarks", (object)objPassportType.Remarks ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsValid", objPassportType.IsValid);
                        cmd.Parameters.AddWithValue("@CreatedBy", objPassportType.CreatedBy);
                        SqlParameter returnValueParam = new SqlParameter
                        {
                            ParameterName = "@ReturnValue",
                            SqlDbType = SqlDbType.VarChar,
                            Size = 50,
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(returnValueParam);
                        await cmd.ExecuteNonQueryAsync();
                        returnValue = returnValueParam.Value?.ToString();
                    }
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting/updating Passport Type", ex);
            }
        }
    }
}