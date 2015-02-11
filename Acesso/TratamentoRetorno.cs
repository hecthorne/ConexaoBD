using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ConexaoBD.Acesso
{
    public class TratamentoRetorno
    {
        #region Propriedades

        private DbProviderFactory _fabricaAcesso;
        private DbConnection _con;
        private DbTransaction _trans;
        private bool _comTrasacao;

        #endregion

        #region Contrutor

        public TratamentoRetorno(DbProviderFactory fabricaAcesso, DbConnection con, DbTransaction trans, bool comTrasacao)
        {
            _fabricaAcesso = fabricaAcesso;
            _con = con;
            _trans = trans;
            _comTrasacao = comTrasacao;
        }

        #endregion

        #region Com Dapper
        public List<T> ObterRetorno<T>(string instSQL, params DbParameter[] parametros)
        {
            DbCommand command = _fabricaAcesso.CreateCommand();
            command.Connection = _con;
            command.CommandTimeout = int.MaxValue;

            return command.Connection.Query<T>(instSQL, parametros, _trans).ToList();
        }

        public List<T> ObterRetorno<T>(string instSQL)
        {
            DbCommand command = _fabricaAcesso.CreateCommand();
            command.Connection = _con;
            command.CommandTimeout = int.MaxValue;

            return command.Connection.Query<T>(instSQL, transaction: _trans).ToList();
        }

        public List<T> ObterRetornoSp<T>(string instSQL)
        {
            DbCommand command = _fabricaAcesso.CreateCommand();
            command.Connection = _con;
            command.CommandTimeout = int.MaxValue;

            return command.Connection.Query<T>(instSQL, commandType: CommandType.StoredProcedure, transaction: _trans).ToList();
        }

        public List<T> ObterRetornoSp<T>(string instSQL, params OracleDynamicParameters.ParamInfo[] parametros)
        {
            DbCommand command = _fabricaAcesso.CreateCommand();
            command.Connection = _con;
            command.CommandTimeout = int.MaxValue;

            var param = OracleParametrosDinamicos(parametros);

            return command.Connection.Query<T>(instSQL, param, commandType: CommandType.StoredProcedure, transaction: _trans).ToList();
        }

        private static OracleDynamicParameters OracleParametrosDinamicos(OracleDynamicParameters.ParamInfo[] parametros)
        {
            OracleDynamicParameters param = new OracleDynamicParameters();

            foreach (var item in parametros)
            {
                param.Add(item.Name, dbType: item.DbType, value: item.Value, direction: item.ParameterDirection);
            }

            return param;
        }

        public List<T> ObterRetornoSp<T>(string instSQL, DynamicParameters parametros)
        {
            DbCommand command = _fabricaAcesso.CreateCommand();
            command.Connection = _con;
            command.CommandTimeout = int.MaxValue;

            return command.Connection.Query<T>(instSQL, parametros, commandType: CommandType.StoredProcedure, transaction: _trans).ToList();
        }
        #endregion

        #region Sem Dapper

        public DbDataReader retornaSPDR(string instSQL, params DbParameter[] parametros)
        {
            DbCommand cmd;
            cmd = _fabricaAcesso.CreateCommand();
            cmd.Connection = _con;
            cmd.CommandText = instSQL;
            cmd.CommandTimeout = int.MaxValue;
            cmd.CommandType = CommandType.StoredProcedure;

            if ((parametros != null) && (parametros.Length > 0))
                cmd.Parameters.AddRange(parametros);
            if (_comTrasacao)
                cmd.Transaction = _trans;
            return cmd.ExecuteReader();
        }

        #endregion
    }
}
