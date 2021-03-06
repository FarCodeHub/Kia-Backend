// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.reverse;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Domain.Entities.reverse
{
    public interface IApplicationUnitOfWorkProcedures
    {
        Task<CensusesSpResult> CensusesSpAsync(DateTime? From, DateTime? To, int? UserId, int? UnitId, int? BranchId,
            OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);

    }

    public partial class KiaautomationContext
    {
       
    }

    public partial class ApplicationUnitOfWorkProcedures : IApplicationUnitOfWorkProcedures
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationUnitOfWorkProcedures(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public virtual async Task<CensusesSpResult> CensusesSpAsync(DateTime? From, DateTime? To, int? UserId, int? UnitId, int? BranchId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "From",
                    Value = From ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "To",
                    Value = To ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "UserId",
                    Value = UserId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "UnitId",
                    Value = UnitId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "BranchId",
                    Value = BranchId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
            };
            var _ = await _unitOfWork.Context()
                .SqlQueryAsync<CensusesSpResult>("EXEC [dbo].[CensusesSp] @From, @To, @UserId, @UnitId, @BranchId", 
                sqlParameters,
                cancellationToken);


            return _.FirstOrDefault();
        }
    }
}
