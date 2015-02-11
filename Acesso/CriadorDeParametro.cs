using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace ConexaoBD.Acesso
{
    public class CriadorDeParametro
    {
        private DbProviderFactory _fabricaAcesso;

        public CriadorDeParametro(DbProviderFactory fabricaAcesso)
        {
            _fabricaAcesso = fabricaAcesso;
        }

        public DbParameter Criar(string nomeParametro, OracleDbType tipoParametro)
        {
            var parametro = (OracleParameter)_fabricaAcesso.CreateParameter();
            parametro.ParameterName = nomeParametro;
            parametro.OracleDbType = tipoParametro;
            return parametro;
        }

        public DbParameter Criar(string nomeParametro, OracleDbType tipoParametro, ParameterDirection direcaoParametro)
        {
            DbParameter parametro = Criar(nomeParametro, tipoParametro);
            parametro.Direction = direcaoParametro;
            return parametro;
        }

        public OracleDynamicParameters.ParamInfo CriaDinamico(string nomeParametro, OracleDbType tipoParametro, ParameterDirection direcaoParametro)
        {
            var parametro = new OracleDynamicParameters.ParamInfo
            {
                Name = nomeParametro,
                DbType = tipoParametro,
                ParameterDirection = direcaoParametro
            };

            return parametro;
        }

        public OracleDynamicParameters.ParamInfo CriaDinamico(string nomeParametro, OracleDbType tipoParametro, object valorParametro)
        {
            var parametro = new OracleDynamicParameters.ParamInfo
            {
                Name = nomeParametro,
                DbType = tipoParametro,
                Value = valorParametro,
                ParameterDirection = null
            };

            return parametro;
        }

        public DbParameter Criar(string nomeParametro, DbType tipoParametro)
        {
            DbParameter parametro = _fabricaAcesso.CreateParameter();
            parametro.ParameterName = nomeParametro;
            parametro.DbType = tipoParametro;
            return parametro;
        }

        public DbParameter Criar(string nomeParametro, DbType tipoParametro, ParameterDirection direcaoParametro)
        {
            DbParameter p = Criar(nomeParametro, tipoParametro);
            p.Direction = direcaoParametro;
            return p;
        }

        public DbParameter Criar(string nomeParametro, DbType tipoParametro, object valorParametro)
        {
            DbParameter parametro = Criar(nomeParametro, tipoParametro);
            parametro.Value = valorParametro;
            return parametro;
        }

        public DbParameter Criar(string nomeParametro, DbType tipoParametro, object valorParametro, ParameterDirection direcaoParametro)
        {
            DbParameter parametro = Criar(nomeParametro, tipoParametro);
            parametro.Value = valorParametro;
            parametro.Direction = direcaoParametro;
            return parametro;
        }
    }
}
