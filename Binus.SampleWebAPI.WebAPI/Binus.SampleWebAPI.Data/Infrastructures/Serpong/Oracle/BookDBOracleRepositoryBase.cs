using Binus.SampleWebAPI.Data.DBContext.Serpong.Oracle;
using Binus.WebAPI.Model.MSSQL;
using Binus.WebAPI.Model.Oracle;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Binus.SampleWebAPI.Data.Infrastructures.Serpong.Oracle
{
    public abstract class BookDBOracleRepositoryBase<T> where T : class
    {
        #region Properties
        private BookDBOracleDBContext DataContext;
        private readonly IDbSet<T> DBSet;
        private string ConStr;

        enum ExecType { List, Single, NoExecRecord };

        protected BookDBOracleIDBFactory DBFactory
        {
            get;
            private set;
        }


        protected BookDBOracleDBContext DBContext
        {
            get { return DataContext ?? (DataContext = DBFactory.Init()); }
        }
       

        protected BookDBOracleRepositoryBase(BookDBOracleIDBFactory DBFactory)
        {
           
            this.DBFactory = DBFactory;
            DBSet = DBContext.Set<T>();
        }

        public virtual void Add(T Entity)
        {
            DBSet.Add(Entity);
        }

        public virtual void Update(T Entity)
        {
            DBSet.Attach(Entity);
            DataContext.Entry(Entity).State = EntityState.Modified;
        }

        public virtual void Delete(T Entity)
        {
            DBSet.Remove(Entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> Objects = DBSet.Where<T>(where).AsEnumerable();
            foreach (T OBJ in Objects)
                DBSet.Remove(OBJ);
        }


        public virtual T GetById(int ID)
        {
            return DBSet.Find(ID);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DBSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DBSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return DBSet.Where(where).FirstOrDefault<T>();
        }


        public virtual IEnumerable<T> ExecSQLToList(string SQL, SqlParameter[] Param = null)
        {
            if (Param == null)
            {
                return DBContext.Database
                .SqlQuery<T>(SQL).ToList<T>();
            }
            else
            {
                return DBContext.Database
                       .SqlQuery<T>(SQL, Param).ToList<T>();
            }

        }

        public virtual T ExecSQLToSingle(string SQL, object[] Param = null)
        {
            if (Param != null)
            {

                return DBContext.Database
                            .SqlQuery<T>(SQL, Param).FirstOrDefault<T>();
            }
            else
            {
                return DBContext.Database
                            .SqlQuery<T>(SQL).FirstOrDefault<T>();
            }

        }
        public virtual async Task<T> ExecSQLToSingleAsync(string SQL, object[] Param = null)
        {
            if (Param != null)
            {

                var Query = DBContext.Database.SqlQuery<T>(SQL, Param);
                return await Query.FirstOrDefaultAsync();
            }
            else
            {
                var Query = DBContext.Database.SqlQuery<T>(SQL);
                return await Query.FirstOrDefaultAsync();
            }

        }
        public virtual async Task<IEnumerable<T>> ExecSQLToListAsync(string SQL, SqlParameter[] Param = null)
        {
            if (Param == null)
            {
                try
                {
                    var Query = DBContext.Database.SqlQuery<T>(SQL);
                    return await Query.ToListAsync();
                }
                catch(Exception Ex)
                {
                    return  null;
                }
                
            }
            else
            {
                var Query = DBContext.Database
                       .SqlQuery<T>(SQL, Param);
                return await Query.ToListAsync();
            }

        }
        public virtual ExecuteResult ExecMultipleSQLWithTransaction(List<BulkSQL> BulkSQL)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            ReturnValue.Status = null;
            using (var DBContextTransaction = DBContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (BulkSQL SQLItem in BulkSQL)
                    {
                        DBContext.Database.SqlQuery<T>(SQLItem.SQL);
                    }
                    DBContextTransaction.Commit();
                    ReturnValue.Status = true;
                }
                catch (Exception EX)
                {
                    DBContextTransaction.Rollback();
                    ReturnValue.Exception = EX;
                    ReturnValue.Message = EX.Message;
                    ReturnValue.Status = false;
                }
                return ReturnValue;
            }

        }

        public virtual async Task<ExecuteResult> ExecMultipleSQLWithTransactionAsync(List<BulkSQL> BulkSQL)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            ReturnValue.Status = null;
            using (var DBContextTransaction = DBContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (BulkSQL SQLItem in BulkSQL)
                    {
                        //DBContext.Database.SqlQuery<T>(SQLItem.SQL);
                        DBContext.Database.ExecuteSqlCommand(SQLItem.SQL);
                    }
                    DBContextTransaction.Commit();
                    ReturnValue.Status = true;
                }
                catch (Exception EX)
                {
                    DBContextTransaction.Rollback();
                    ReturnValue.Exception = EX;
                    ReturnValue.Message = EX.Message;
                    ReturnValue.Status = false;
                }
                return await Task.FromResult(ReturnValue);
            }

        }

        #endregion
    }
}
