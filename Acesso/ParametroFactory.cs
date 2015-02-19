using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using Oracle.DataAccess.Client;

namespace ConexaoBD.Acesso
{
    public static class ParametroFactory
    {
        public static OracleParameter Criar(string nomeParametro, OracleDbType tipoParametro)
        {
            var parametro = new OracleParameter 
            {
                ParameterName = nomeParametro,
                OracleDbType = tipoParametro
            };
            return parametro;
        }

        public static OracleParameter Criar(string nomeParametro, OracleDbType tipoParametro, ParameterDirection direcaoParametro)
        {
            var parametro = Criar(nomeParametro, tipoParametro);
            parametro.Direction = direcaoParametro;
            return parametro;
        }

        public static OracleParameter Criar(string nomeParametro, OracleDbType tipoParametro, object valorParametro)
        {
            var parametro = Criar(nomeParametro, tipoParametro);
            parametro.Value = valorParametro;
            return parametro;
        }

        public static OracleDynamicParameters.ParamInfo CriarDinamico(string nomeParametro, OracleDbType tipoParametro)
        {
            var parametro = new OracleDynamicParameters.ParamInfo
            {
                Name = nomeParametro,
                DbType = tipoParametro
            };
            return parametro;
        }

        public static OracleDynamicParameters.ParamInfo CriarDinamico(string nomeParametro, OracleDbType tipoParametro, ParameterDirection direcaoParametro)
        {
            var parametro = CriarDinamico(nomeParametro, tipoParametro);
            parametro.ParameterDirection = direcaoParametro;

            return parametro;
        }

        public static OracleDynamicParameters.ParamInfo CriarDinamico(string nomeParametro, OracleDbType tipoParametro, object valorParametro)
        {
            var parametro = CriarDinamico(nomeParametro, tipoParametro);
            parametro.Value = valorParametro;
            parametro.ParameterDirection = null;

            return parametro;
        }
        
    }
}
