using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexaoBD.Acesso
{

    public class Conexao
    {
        public DbProviderFactory FabricaAcesso { get; private set; }
        public DbConnection _con { get; private set; }
        public DbTransaction _trans { get; private set; }
        public bool _comTrans { get; private set; }

        private String sufixoConexao = "";

        #region Tratar Conexao

        public Conexao()
        {
            IniciaClasse(false);
        }

        public Conexao(String sufixoConexao)
        {
            this.sufixoConexao = sufixoConexao;
            IniciaClasse(false);
        }

        public Conexao(bool iniciaTransacao)
        {
            IniciaClasse(iniciaTransacao);
        }

        public Conexao(bool iniciaTransacao, String sufixoConexao)
        {
            this.sufixoConexao = sufixoConexao;
            IniciaClasse(iniciaTransacao);
        }

        private void IniciaClasse(bool iniciaTransacao)
        {
            FabricaAcesso = DbProviderFactories.GetFactory(ConfigurationManager.AppSettings["Provedor" + sufixoConexao]);
            _con = FabricaAcesso.CreateConnection();
            _con.ConnectionString = ConfigurationManager.AppSettings["ConnectionString" + sufixoConexao];
            _comTrans = iniciaTransacao;

            if (_comTrans)
            {
                _con.Open();
                _trans = _con.BeginTransaction();
            }
        }
        #endregion

        #region Abrir e Fechar Conexão
        public void AbreConexao()
        {
            _con.Open();
        }

        public void FechaConexao()
        {
            _con.Close();
        }

        public bool ConexaoFechada()
        {
            return _con.State == ConnectionState.Closed;
        }
        #endregion

        #region Comando de Transacao

        public void IniciarTransacao()
        {
            if (_comTrans)
            {
                throw new Exception("Já existe uma transação em aberto.");
            }
            if (ConexaoFechada())
                AbreConexao();

            _comTrans = true;
            _trans = _con.BeginTransaction();
        }

        public void ConcluirTransacao()
        {
            if (!_comTrans)
            {
                throw new Exception("Não existe nenhuma transação em aberto.");
            }
            _trans.Commit();
            _con.Close();
            _comTrans = false;
        }

        public void CancelaTransacao()
        {
            if (!_comTrans)
            {
                throw new Exception("Não existe nenhuma transação em aberto.");
            }
            _trans.Rollback();
            _con.Close();
            _comTrans = false;
        }
        #endregion
    }
}
