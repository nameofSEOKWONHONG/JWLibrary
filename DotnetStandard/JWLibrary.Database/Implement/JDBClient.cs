using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using JWLibrary.Core;

namespace JWLibrary.Database
{
    /// <summary>
    /// Database Client
    /// </summary>
    public class JDBClient
    {
        /// <summary>
        /// DbConnection Interface
        /// </summary>
        /// <value></value>
        public IDbConnection Connection {get; private set;}

        /// <summary>
        /// Creator
        /// </summary>
        /// <param name="db"></param>
        public JDBClient(JDataBase db) {
            if(db.jIsNull()) throw new NoNullAllowedException("JDataBase is null");

            this.Connection = db.DBType switch {
                JDataBase.JDBType.MSSQL => (new MSSqlDataBase(db)).Connection,
                JDataBase.JDBType.MYSQL => (new MYSqlDataBase(db)).Connection,
                _ => null
            };

            try
            {
                this.Connection.Open();
            }
            catch {
                throw;
            }
            finally {
                this.Connection.Close();
            }
        }

        public bool jToMigrateSqlToMySql(DataTable dt, string con)
        {
            return false;
        }
    }
}


/*
CREATE TABLE [dbo].[WorkOut](
    [WorkOutID] [bigint] IDENTITY(1,1) NOT NULL,
    [TimeSheetDate] [datetime] NOT NULL,
    [DateOut] [datetime] NOT NULL,
    [EmployeeID] [int] NOT NULL,
    [IsMainWorkPlace] [bit] NOT NULL,
    [DepartmentUID] [uniqueidentifier] NOT NULL,
    [WorkPlaceUID] [uniqueidentifier] NULL,
    [TeamUID] [uniqueidentifier] NULL,
    [WorkShiftCD] [nvarchar](10) NULL,
    [WorkHours] [real] NULL,
    [AbsenceCode] [varchar](25) NULL,
    [PaymentType] [char](2) NULL,
    [CategoryID] [int] NULL,
    [Year]  AS (datepart(year,[TimeSheetDate])),
 CONSTRAINT [PK_WorkOut] PRIMARY KEY CLUSTERED
(
    [WorkOutID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[WorkOut] ADD
CONSTRAINT [DF__WorkOut__IsMainW__2C1E8537]  DEFAULT ((1)) FOR [IsMainWorkPlace]

ALTER TABLE [dbo].[WorkOut]  WITH CHECK ADD  CONSTRAINT [FK_WorkOut_Employee_EmployeeID] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])

ALTER TABLE [dbo].[WorkOut] CHECK CONSTRAINT [FK_WorkOut_Employee_EmployeeID]
*/