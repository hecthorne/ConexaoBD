using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace ConexaoBD.Acesso
{
    public class Conexao
    {
        public DbProviderFactory FabricaAcesso { get; private set; }
        public DbConnection dbConexao { get; private set; }
        public DbTransaction dbTransacao { get; private set; }
        public bool comTransacao { get; private set; }

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
            dbConexao = FabricaAcesso.CreateConnection();
            dbConexao.ConnectionString = ConfigurationManager.AppSettings["ConnectionString" + sufixoConexao];
            comTransacao = iniciaTransacao;

            if (comTransacao)
            {
                dbConexao.Open();
                dbTransacao = dbConexao.BeginTransaction();
            }
        }
        #endregion

        #region Abrir e Fechar Conexão
        public void AbreConexao()
        {
            dbConexao.Open();
        }

        public void FechaConexao()
        {
            dbConexao.Close();
        }

        public bool ConexaoFechada()
        {
            return dbConexao.State == ConnectionState.Closed;
        }
        #endregion

        #region Comando de Transacao

        public void IniciarTransacao()
        {
            if (comTransacao)
            {
                throw new Exception("Já existe uma transação em aberto.");
            }
            if (ConexaoFechada())
                AbreConexao();

            comTransacao = true;
            dbTransacao = dbConexao.BeginTransaction();
        }

        public void ConcluirTransacao()
        {
            if (!comTransacao)
            {
                throw new Exception("Não existe nenhuma transação em aberto.");
            }
            dbTransacao.Commit();
            dbConexao.Close();
            comTransacao = false;
        }

        public void CancelaTransacao()
        {
            if (!comTransacao)
            {
                throw new Exception("Não existe nenhuma transação em aberto.");
            }
            dbTransacao.Rollback();
            dbConexao.Close();
            comTransacao = false;
        }
        #endregion
    }
}
