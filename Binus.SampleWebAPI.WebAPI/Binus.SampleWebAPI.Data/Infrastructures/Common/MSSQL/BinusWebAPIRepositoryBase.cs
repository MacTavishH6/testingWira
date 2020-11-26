using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using Binus.WebAPI.Model.MSSQL;
using Binus.SampleWebAPI.Data.DBContext.Common.MSSQL;

namespace Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL
{
    public abstract class BinusWebAPIRepositoryBase<T> where T : class
    {
        #region Properties
        private BinusWebAPIDBContext DataContext;
        private readonly IDbSet<T> DBSet;
        public string ConStr;

        enum ExecType { List, Single, NoExecRecord };

        protected BinusWebAPIIDBFactory DBFactory
        {
            get;
            private set;
        }


        protected BinusWebAPIDBContext DBContext
        {
            get { return DataContext ?? (DataContext = DBFactory.Init()); }
        }

        public virtual void ConnectionString(string ConnectionString)
        {
            ConStr = ConnectionString;
        }


        protected BinusWebAPIRepositoryBase(BinusWebAPIIDBFactory DBFactory)
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


        public virtual IEnumerable<T> ExecSPToList(string SPName, SqlParameter[] Param = null)
        {
           
            if (Param == null)
            {
                
                return DBContext.Database
                .SqlQuery<T>(SPName).ToList<T>();
                
            }
            else
            {
                return DBContext.Database
                       .SqlQuery<T>(SPName, Param).ToList<T>();
               
            }
           
        }

        public virtual Tuple<T, object> ExecSPToSingleWithOutput(string SPName, SqlParameter[] Param)
        {
            T Result = default(T);
            var Output = (string)null;
            try
            {
             
                        Result = Result = DBContext.Database
          .SqlQuery<T>(SPName, Param).FirstOrDefault<T>();

                        foreach (SqlParameter Item in Param)
                        {
                            if (Item.Direction == System.Data.ParameterDirection.Output)
                            {
                                Output = Item.Value.ToString();
                            }
                        }
                 
            }
            catch (Exception EX)
            {

            }

            return Tuple.Create(Result, (object)Output);


        }

        public virtual Tuple<IEnumerable<T>,object> ExecSPToListWithOutput(string SPName, SqlParameter[] Param)
        {
            IEnumerable<T> Result = default(IEnumerable<T>);
            var Output = (string)null;
            try
            {
                
                        Result = Result = DBContext.Database
          .SqlQuery<T>(SPName, Param).ToList<T>();

                foreach (SqlParameter Item in Param)
                {
                    if (Item.Direction == System.Data.ParameterDirection.Output)
                    {
                        Output = Item.Value.ToString();
                    }
                }
            }
            catch(Exception EX)
            {

            }
           
            return Tuple.Create(Result, (object)Output);


        }

        public virtual T ExecSPToSingle(string SPName, object[] Param = null)
        {
            if (Param != null)
            {
                
                return DBContext.Database
                            .SqlQuery<T>(SPName, Param).FirstOrDefault<T>();
            }
            else
            {
                return DBContext.Database
                            .SqlQuery<T>(SPName).FirstOrDefault<T>();
            }

        }
        public virtual async Task<T> ExecSPToSingleAsync(string SPName, object[] Param = null)
        {
            if (Param != null)
            {

                var Query = DBContext.Database.SqlQuery<T>(SPName, Param);
                return await Query.FirstOrDefaultAsync();
            }
            else
            {
                var Query = DBContext.Database.SqlQuery<T>(SPName);
                return await Query.FirstOrDefaultAsync();
            }

        }
        public virtual ExecuteResult ExecMultipleSPWithTransaction(List<StoredProcedure> SP)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            ReturnValue.Status = null;
            using (var DBContextTransaction = DBContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (StoredProcedure SPItem in SP)
                    {
                        if (SPItem.SQLParam == null)
                        {
                            //((IObjectContextAdapter)DBContext).ObjectContext.ExecuteFunction<T>(SPItem.SPName, SPItem.ObjectParam);

                            DBContext.Database.SqlQuery<T>(SPItem.SPName).SingleOrDefault();

                        }
                        else
                        {
                            //((IObjectContextAdapter)DBContext).ObjectContext.ExecuteFunction<T>(SPItem.SPName, SPItem.ObjectParam);
                            DBContext.Database.SqlQuery<T>(SPItem.SPName, SPItem.SQLParam).SingleOrDefault();
                        }
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
        public virtual async Task<ExecuteResult> ExecMultipleSPWithTransactionAsync(List<StoredProcedure> SP)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            ReturnValue.Status = null;
            using (var DBContextTransaction = DBContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (StoredProcedure SPItem in SP)
                    {
                        if (SPItem.SQLParam == null)
                        {
                           
                            DBContext.Database.SqlQuery<T>(SPItem.SPName).SingleOrDefault();
                        }
                        else
                        {
                            //((IObjectContextAdapter)DBContext).ObjectContext.ExecuteFunction<T>(SPItem.SPName, SPItem.ObjectParam);

                           DBContext.Database.SqlQuery<T>(SPItem.SPName, SPItem.SQLParam).SingleOrDefault();
                        }
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
        public virtual async Task<IEnumerable<T>> ExecSPToListAsync(string SPName, SqlParameter[] Param = null)
        {
            if (Param == null)
            {
                var Query = DBContext.Database.SqlQuery<T>(SPName);
                return await Query.ToListAsync();
            }
            else
            {
                var Query = DBContext.Database
                       .SqlQuery<T>(SPName, Param);
                return await Query.ToListAsync();
            }

        }
        #endregion
    }
}
