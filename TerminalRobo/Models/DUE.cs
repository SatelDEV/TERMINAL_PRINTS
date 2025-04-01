using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Objects.DataClasses;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using TerminalRobo.DataBase;

namespace TerminalRobo.Models
{
    public class DUE
    {
        WEBSATELEntities db = new WEBSATELEntities();

        public class Lista
        {
            public int id { get; set; }
            public string text { get; set; }
        }
        public class Lista2
        {
            public string id { get; set; }
            public string text { get; set; }
        }

        public class ncm
        {
            public string CD_NCM { get; set; }
            public string DS_NCM { get; set; }
            public string CD_UNIDADE_MEDIDA { get; set; }
            public string NM_UNIDADE_MEDIDA { get; set; }
            public bool? IC_DESTAQUE { get; set; }
        }
        [EdmFunction("ModelWebSatel.Store", "FN_RETORNA_STATUS_DUE_LPCO")]
        public static string FN_RETORNA_STATUS_DUE_LPCO(int CD_DUE)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }
        [EdmFunction("ModelWebSatel.Store", "FN_RETORNA_PROCESSO_LOTE_DUE")]
        public static string FN_RETORNA_PROCESSO_LOTE_DUE(int CD_DUE)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }
        [EdmFunction("ModelWebSatel.Store", "FN_RETORNA_CONTAINER_PROCESSO")]
        public static string FN_RETORNA_CONTAINER_PROCESSO(int CD_PROCESSO_RESERVA)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }
        [EdmFunction("ModelWebSatel.Store", "FN_RETORNA_CONTAINER_PROCESSO_DUE")]
        public static string FN_RETORNA_CONTAINER_PROCESSO_DUE(int CD_PROCESSO_RESERVA)
        {
            throw new NotSupportedException("Direct calls are not supported;");
        }
        #region Busca
        [Display(Name = "Cliente", Description = "Cliente")]
        public string nmBuscarCliente { get; set; }

        [Display(Name = "Período", Description = "Período")]
        public string nmBuscarPeriodo { get; set; }

        [Display(Name = "Grupo de cliente/analista", Description = "Grupo de cliente/analista")]
        public string nmBuscarGrupoCliente { get; set; }
        public int idBuscarCliente { get; set; }
        public int idBuscarGrupoCliente { get; set; }
        public int idBuscarPeriodo { get; set; }
        [Display(Name = "Status", Description = "Status")]
        public int ModoBusca { get; set; }
        [Display(Name = "Filtrar por:", Description = "Filtrar por:")]
        public int ModoFiltro { get; set; }
        #endregion

        #region NovaDUE

        #endregion

        public class camposDUE
        {

            public int? CD_DUE { get; set; }
            public int? CD_PROCESSO { get; set; }
            public int? CD_PROCESSORESERVA { get; set; }
            public int? CD_RESERVA { get; set; }
            [Display(Name = "Tipo DUE", Description = "Tipo DUE")]
            public int? SG_TIPO_DUE { get; set; }
            [Display(Name = "Nr. Processo", Description = "Nr. Processo")]
            public string NR_PROCESSO { get; set; }
            [Display(Name = "CNPJ/CPF do declarante", Description = "CNPJ/CPF do declarante")]
            public string NR_CNPJ { get; set; }
            [Display(Name = "Chave Nota Fiscal Eletrônica", Description = "Chave Nota Fiscal Eletrônica")]
            public string NR_NFE { get; set; }

            public string DS_REFERENCIA_PROCESSO { get; set; }
        
            [Display(Name = "Refêrencia do cliente", Description = "Refêrencia do cliente")]
            public string DS_REFERENCIA { get; set; }
            [Display(Name = "Data de Criação", Description = "Data de Criação")]
            public DateTime? DT_DUE { get; set; }
            [Display(Name = "Data de Registro", Description = "Data de Registro")]
            public DateTime? DT_TRANSMISSAO { get; set; }
            [Display(Name = "Data de Desembaraço", Description = "Data de Desembaraço")]
            public DateTime? DT_DESEMBARACO { get; set; }
            [Display(Name = "Número da DUE", Description = "Número da DUE")]
            public string CD_NUMERO_DUE { get; set; }
            [Display(Name = "Número da RUC", Description = "Número da RUC")]
            public string CD_NUMERO_RUC { get; set; }
            [Display(Name = "Moeda de Negociação", Description = "Moeda de Negociação")]
            public string STR_CODIGOMOEDA { get; set; }
            [Display(Name = "Situação", Description = "Situação")]
            public string DS_SITUACAO { get; set; }
            public int? CD_EXPORTADOR { get; set; }
            [Display(Name = "Exportador", Description = "Exportador")]
            public string DS_EXPORTADOR { get; set; }
            [Display(Name = "Forma de exportação", Description = "Forma de exportação")]
            public string CD_FORMA_EXPORTACAO { get; set; }
            [Display(Name = "Forma de exportação", Description = "Forma de exportação")]
            public string NM_FORMA_EXPORTACAO { get; set; }
            [Display(Name = "Local de Despacho", Description = "Local de Despacho")]
            public string STR_CODIGOUNIDADERF_DESPACHO { get; set; }
            [Display(Name = "Local de Despacho", Description = "Local de Despacho")]
            public string NM_DESPACHO { get; set; }
            [Display(Name = "Recinto aduaneiro", Description = "Recinto aduaneiro")]
            public string CD_RECINTO_DESPACHO { get; set; }
            [Display(Name = "Em recinto aduaneiro", Description = "Em recinto aduaneiro")]
            public int? IC_RECINTO_ADUANEIRO_DESPACHO { get; set; }
            [Display(Name = "Recinto aduaneiro", Description = "Recinto aduaneiro")]
            public string NM_RECINTO_DESPACHO { get; set; }
            [Display(Name = "Local de embarque", Description = "Local de embarque")]
            public string STR_CODIGOUNIDADERF_EMBARQUE { get; set; }
            [Display(Name = "Local de Embarque", Description = "Local de Embarque")]
            public string NM_EMBARQUE { get; set; }
            [Display(Name = "Recinto aduaneiro", Description = "Recinto aduaneiro")]
            public string NM_RECINTO { get; set; }
            [Display(Name = "Recinto aduaneiro", Description = "Recinto aduaneiro")]
            public string CD_RECINTO_EMBARQUE { get; set; }
            public int? CD_RECINTO_EMBARQUE_PROCESSO { get; set; }
            public int? CD_RECINTO_EMBARQUE_ENTIDADE { get; set; }
            [Display(Name = "Em recinto aduaneiro", Description = "Em recinto aduaneiro")]
            public int? IC_RECINTO_ADUANEIRO_EMBARQUE { get; set; }
            [Display(Name = "Recinto aduaneiro", Description = "Recinto aduaneiro")]
            public string NM_RECINTO_EMBARQUE { get; set; }
            [Display(Name = "Referência de endereço", Description = "Referência de endereço")]
            public string DS_REFERENCIA_ENDERECO { get; set; }
            [Display(Name = "Informações complementares", Description = "Informações complementares")]
            public string DS_INFORMACOES_COMPLEMENTARES { get; set; }
            [Display(Name = "Via especial de transporte", Description = "Via especial de transporte")]
            public int? CD_VIA_TRANSPORTE_ESPECIAL { get; set; }
            [Display(Name = "Valor Frete e Seguro Internacional", Description = "Valor Frete e Seguro Internacional")]
            public decimal? VL_FRETE_INTERNACIONAL { get; set; }
            [Display(Name = "All in", Description = "All in")]
            public bool? IC_EMBUTIDO { get; set; }

            public int? CD_USUARIO_DUE { get; set; }
            [Display(Name = "Usuário Elaboração", Description = "Usuário Elaboração")]
            public string NM_USUARIO { get; set; }
            public int? CD_USUARIO_TRANSMISSAO { get; set; }
            [Display(Name = "Usuário Transmissão", Description = "Usuário Transmissão")]
            public string NM_USUARIO_TRANSMISSAO { get; set; }
            [Display(Name = "Usuário Retificou", Description = "Usuário Retificou")]
            public string NM_USUARIO_RETIFICACAO { get; set; }
            [Display(Name = "Valor Desconto", Description = "Valor Desconto")]
            public decimal? VL_DESCONTO { get; set; }

            public int? IC_STATUS_PARA_ENVIO_TRANSMISSAO { get; set; }
            public bool? SG_FRETE_INTERNACIONAL_EMBUTIDO { get; set; }
            public bool? SG_SEGURO_INTERNACIONAL_EMBUTIDO { get; set; }
            [Display(Name = "Valor Seguro", Description = "Valor Seguro")]
            public decimal? VL_SEGURO { get; set; }


            [Display(Name = "Texto DUE", Description = "Texto DUE")]
            public int CD_TEXTO_DUE { get; set; }

            public int? CD_NAVIO { get; set; }
            public int? CD_DESTINO { get; set; }
            public string DS_NAVIO { get; set; }
            public DateTime? DT_DEADLINE_CARGA { get; set; }
            public DateTime? DT_ETA { get; set; }
            public DateTime? DT_DEADLINE_DRAFT { get; set; }
            public string DS_TERMINAL_ATRACACAO { get; set; }
            public string DS_BOOKING { get; set; }
            public string DS_ORIGEM { get; set; }
            public string DS_DESTINO { get; set; }
            public string DS_ARMADOR { get; set; }
            public string NR_CONTAINER { get; set; }
            public string DS_ARQUIVO_XML { get; set; }
            public DateTime? DT_CANCELADO { get; set; }
            public int? IC_BLOQUEIO { get; set; }
            public int? IC_ADM { get; set; }
            public int? CD_ORIGEM { get; set; }
            public int? IC_SITUACAO_CARGA { get; set; }
            public string DS_STATUS_EMBARQUE2 { get; set; }
            public int? CD_STATUS_EMBARQUE2 { get; set; }
            public DateTime? DT_ENTREGA_DESPACHO { get; set; }
            public DateTime? DT_EMBARQUE { get; set; }
            public DateTime? DT_FATURADO { get; set; }
            public DateTime? DT_CRIACAO_BL { get; set; }
            public DateTime? DT_AVERBADO { get; set; }
            [Display(Name = "Chave de Acesso", Description = "Chave de Acesso")]
            public string CD_CHAVE_ACESSO { get; set; }
            public DateTime? DT_DEPOSITADO { get; set; }
            public string NM_TERMINAL_REDEX { get; set; }
            [Display(Name = "Processo Lote", Description = "Processo Lote")]
            public string NR_PROCESSO_LOTE { get; set; }
            public DateTime? DT_CANAL_VERMELHO { get; set; }
            public int? SG_TIPO_DUE_ANT { get; set; }

            [Display(Name = "Motivo da dispensa de nota fiscal", Description = "Motivo da dispensa de nota fiscal")]
            public string CD_MOTIVO_DISPENSA_NF { get; set; }
            public string CD_DUE_RECUPERAR { get; set; }
            public string STR_CODIGOVIATRANSPORTE { get; set; }

            public int? CD_GRUPO_CLI { get; set; }

            public string NM_STATUS_DUE_LPCO { get; set; }
            public Boolean? IC_EMBARQUE_CANCELADO { get; set; }
            public DateTime? DT_SAIDA_NAVIO { get; set; }
            public DateTime? DT_TERMINAL_REDEX { get; set; }
             public DateTime? DT_TERMINAL_ATRACACAO { get; set; }
             public DateTime? DT_CHEGADA_CSI { get; set; }
            [Display(Name = "Chave do XML da Nota", Description = "Chave do XML da Nota")]
             public string NR_CHAVE_NF { get; set; }

            public DateTime? DT_ENVIO_DOCS { get; set; }
            public string DS_ENVIO_DOCS { get; set; }

            public DateTime? DT_ENVIO_COURRIER { get; set; }
            public string DS_COURRIER { get; set; }
            public string NR_COURRIER { get; set; }
            public string NR_BL { get; set; }
            public string NR_CSI { get; set; }

            public DateTime? DT_EMBARQUE_CONTAINER { get; set; }
            public DateTime? DT_BL_REAL { get; set; }
        }

        public class camposDUENF
        {
            public int CD_NF { get; set; }
            public int CD_DUE { get; set; }
            [Display(Name = "Data Emissão", Description = "Data Emissão")]
            public DateTime? DT_EMISSAO { get; set; }
            [Display(Name = "Data Saída", Description = "Data Saída")]
            public DateTime? DT_SAIDA { get; set; }
            public int? CD_CLIENTE { get; set; }
            [Display(Name = "Cliente", Description = "Cliente")]
            public string NM_CLIENTE { get; set; }
            [Display(Name = "Chave de Acesso", Description = "Chave de Acesso")]
            public string CD_NUMERO_CHAVE_NF { get; set; }
            [Display(Name = "Nr. Nota Fiscal", Description = "Nr. Nota Fiscal")]
            public string CD_NUMERO_NF { get; set; }
            [Display(Name = "Nr. Série", Description = "Nr. Série")]
            public string CD_SERIE_NF { get; set; }
            [Display(Name = "Natureza Operação", Description = "Natureza Operação")]
            public string CD_NATUREZA_OPERACAO { get; set; }
            [Display(Name = "CNPJ", Description = "CNPJ")]
            public string CD_CNPJ { get; set; }
            [Display(Name = "Endereço", Description = "Endereço")]
            public string DS_ENDERECO_DESTINATARIO { get; set; }
            [Display(Name = "Bairro", Description = "Bairro")]
            public string DS_BAIRRO_DESTINATARIO { get; set; }
            [Display(Name = "CEP", Description = "CEP")]
            public string DS_CEP_DESTINATARIO { get; set; }
            [Display(Name = "Cidade", Description = "Cidade")]
            public string DS_MUNICIPIO_DESTINATARIO { get; set; }
            [Display(Name = "Estado", Description = "Estado")]
            public string DS_UF_DESTINATARIO { get; set; }
            [Display(Name = "Telefone", Description = "Telefone")]
            public string DS_FONE_DESTINATARIO { get; set; }
            [Display(Name = "Fatura", Description = "Fatura")]
            public string DS_FATURA { get; set; }
            [Display(Name = "Vl. Produto", Description = "Vl. Produto")]
            public decimal? VL_TOTAL_PRODUTO { get; set; }
            [Display(Name = "Vl. Nota", Description = "Vl. Nota")]
            public decimal? VL_TOTAL_NOTA { get; set; }
            [Display(Name = "Transportadora", Description = "Transportadora")]
            public string DS_TRANSPORTADOR { get; set; }
            [Display(Name = "Placa Veículo", Description = "Placa Veículo")]
            public string DS_PLACA_VEICULO { get; set; }

            [Display(Name = "Placa Veículo", Description = "Placa Veículo")]
            public string DS_PLACA_VEIUCLO_LPCO { get; set; }
            
            [Display(Name = "Estado Veículo", Description = "Estado Veículo")]
            public string DS_ESTADO_VEICULO { get; set; }
            [Display(Name = "CNPJ", Description = "CNPJ")]
            public string CD_CNPJ_TRANSPORTADOR { get; set; }
            [Display(Name = "Endereço", Description = "Endereço")]
            public string DS_ENDERECO_TRANSPORTADOR { get; set; }
            [Display(Name = "Cidade", Description = "Cidade")]
            public string DS_MUNICIPIO_TRANSPORTADOR { get; set; }
            [Display(Name = "Estado", Description = "Estado")]
            public string DS_ESTADO_TRANSPORTADOR { get; set; }
            [Display(Name = "Insc. Estadual", Description = "Insc. Estadual")]
            public string CD_INSC_ESTADUAL_TRANSPORTADOR { get; set; }
            [Display(Name = "Qtd. Total", Description = "Qtd. Total")]
            public decimal? QTD_TOTAL { get; set; }
            [Display(Name = "Espécie", Description = "Espécie")]
            public string DS_ESPECIE { get; set; }
            [Display(Name = "Vl. Peso bruto", Description = "Vl. Peso bruto")]
            public decimal? VL_PESO_BRUTO { get; set; }
            [Display(Name = "Vl. Peso Líquido", Description = "Vl. Peso Líquido")]
            public decimal? VL_PESO_LIQUIDO { get; set; }

            public decimal? QT_COMERCIALIZACAO { get; set; }
            public decimal? VL_CONDICAO_VENDA_REAL { get; set; }
            public decimal? VL_LOCAL_EMBARQUE { get; set; }
            public decimal? VL_TOTAL_CONDICAO_VENDA_DOLAR { get; set; }
            [Display(Name = "Número LPCO", Description = "Número LPCO")]
            public string NR_LPCO { get; set; }
            public int IC_STATUS { get; set; }
            [Display(Name = "Observação", Description = "Observação")]
            public string DS_OBSERVACAO { get; set; }
            [Display(Name = "CFOP", Description = "CFOP")]
            public string NR_CFOP { get; set; }
            [Display(Name = "Destinatário", Description = "Destinatário")]
            public string DS_NOME_DESTINATARIO { get; set; }

            [Display(Name = "Valor Financiado", Description = "Valor Financiado")]
            public decimal? VL_FINANCIADO { get; set; }

            public int? IC_FINANCIADO { get; set; }
            public int? CD_PROCESSORESERVACONTAINER { get; set; }
            public string NR_CONTAINER { get; set; }
            public string NR_CSI { get; set; }
            public string NR_LACRE_SIF { get; set; }
            public int? CD_USO_PROPOSTO { get; set; }
            public string CD_UNIDADE_VIGIAGRO { get; set; }
            public string STR_CODIGOVIATRANSPORTE { get; set; }
            public decimal VL_DOLAR { get; set; }
            public decimal? VL_COTACAO { get; set; }
            

        }

        public class camposDUENFItem
        {
            public int CD_NF_ITEM { get; set; }
            public int CD_DUE { get; set; }
            public int CD_NF { get; set; }
            public int CD_ITEM { get; set; }
            [Display(Name = "Nr. Nota Fiscal", Description = "Nr. Nota Fiscal")]
            public string NR_NFE { get; set; }
            [Display(Name = "Código NCM", Description = "Código NCM")]
            public string STR_CODIGONCM { get; set; }
            [Display(Name = "Descrição NCM", Description = "Descrição NCM")]
            public string DS_NCM { get; set; }
            public string STR_CODIGONALADI { get; set; }
            [Display(Name = "Descrição Mercadoria", Description = "Descrição Mercadoria")]
            public string DS_MERCADORIA { get; set; }
            [Display(Name = "Código Destaque NCM", Description = "Código Destaque NCM")]
            public string STR_CODIGONCM_DESTAQUE { get; set; }
            [Display(Name = "Descrição Destaque NCM", Description = "Descrição Destaque NCM")]
            public string DS_DESTAQUE_NCM { get; set; }
            [Display(Name = "Exportador", Description = "Exportador")]
            public string NM_EXPORTADOR { get; set; }
            [Display(Name = "Importador", Description = "Importador")]
            public string NM_IMPORTADOR { get; set; }
            [Display(Name = "Endereço Importador", Description = "Endereço Importador")]
            public string DS_ENDERECO_IMPORTADOR { get; set; }
            [Display(Name = "País Importador", Description = "País Importador")]
            public string DS_PAIS_IMPORTADOR { get; set; }

            [Display(Name = "Tratamento Prioritário", Description = "Tratamento Prioritário")]
            public string CD_TRATAMENTO_PRIORITARIO { get; set; }
            [Display(Name = "Unidade Estatística", Description = "Unidade Estatística")]
            public string STR_CODIGOUNIDADEESTATISTICA { get; set; }
            [Display(Name = "Quantidade Estatística", Description = "Quantidade Estatística")]
            public decimal? VL_QUANTIDADE_ESTATISTICA { get; set; }
            [Display(Name = "Preço Unitário", Description = "Preço Unitário")]
            public decimal? VL_PRECO_UNITARIO_ESTATISTICA_CV { get; set; }
            [Display(Name = "Preço Unitário", Description = "Preço Unitário")]
            public decimal? VL_PRECO_UNITARIO_ESTATISTICA_LE { get; set; }


            [Display(Name = "Unidade Comercializada", Description = "Unidade Comercializada")]
            public string STR_CODIGOCOMERCIALIZADA { get; set; }
            [Display(Name = "Quantidade Comercializada", Description = "Quantidade Comercializada")]
            public decimal? QT_COMERCIALIZACAO { get; set; }
            [Display(Name = "Preço Unitário", Description = "Preço Unitário")]
            public decimal? VL_PRECO_UNITARIO_COMERCIALIZACAO_CV { get; set; }
            [Display(Name = "Preço Unitário", Description = "Preço Unitário")]
            public decimal? VL_PRECO_UNITARIO_COMERCIALIZACAO_LE { get; set; }

            [Display(Name = "Peso Líquido", Description = "Peso Líquido")]
            public decimal? VL_PESO_LIQUIDO { get; set; }

            [Display(Name = "VMCV", Description = "VMCV")]
            public decimal? VL_CONDICAO_VENDA_REAL { get; set; }

            [Display(Name = "Valor Comissão Agente", Description = "Valor Comissão Agente")]
            public decimal? VL_COMISSAO_AGENTE { get; set; }

            [Display(Name = "VMLE", Description = "VMLE")]
            public decimal? VL_LOCAL_EMBARQUE { get; set; }

            [Display(Name = "Comissão Agente (%)", Description = "Comissão Agente (%)")]
            public decimal? PC_COMISSAO_AGENTE { get; set; }

            [Display(Name = "Condição de Venda", Description = "Condição de Venda")]
            public string STR_CODIGOCONDICAOVENDA { get; set; }

            public decimal? VL_CONDICAO_VENDA_ESTATISTICA { get; set; }

            public decimal? VL_LOCAL_EMBARQUE_ESTATISTICA { get; set; }

            [Display(Name = "Primeiro Enquadramento", Description = "Primeiro Enquadramento")]
            public string STR_CODIGOENQUADRAMENTO1 { get; set; }
            [Display(Name = "Segundo Enquadramento", Description = "Segundo Enquadramento")]
            public string STR_CODIGOENQUADRAMENTO2 { get; set; }
            [Display(Name = "Terceiro Enquadramento", Description = "Terceiro Enquadramento")]
            public string STR_CODIGOENQUADRAMENTO3 { get; set; }
            [Display(Name = "Quarto Enquadramento", Description = "Quarto Enquadramento")]
            public string STR_CODIGOENQUADRAMENTO4 { get; set; }
            public string STR_CODIGOENQUADRAMENTO5 { get; set; }
            public string STR_CODIGOENQUADRAMENTO6 { get; set; }
            [Display(Name = "Descrição complementar da mercadoria", Description = "Descrição complementar da mercadoria")]
            public string DS_DESCRICAO_COMPLEMENTAR { get; set; }

            [Display(Name = "Número LPCO", Description = "Número LPCO")]
            public string NR_LPCO { get; set; }
            [Display(Name = "Produto", Description = "Produto")]
            public int? CD_PRODUTO { get; set; }

            [Display(Name = "País de Destino", Description = "País de Destino")]
            public string CD_PAIS_DESTINO { get; set; }

            public string DS_UNIDADEESTATISTICA { get; set; }
            public string DS_COMERCIALIZADA { get; set; }
            public string DS_PRODUTO { get; set; }
            public string DS_ENQUADRAMENTO1 { get; set; }
            public bool? IC_EMBARQUE_ANTECIPADO { get; set; }
            public string DS_CODIGO_ATRIBUTO { get; set; }

            [Display(Name = "Chave Referenciada", Description = "Chave referenciada")]
            public string NR_CHAVE_REFERENCE { get; set; }
            [Display(Name = "Item", Description = "Item")]
            public int CD_ITEM_NOTA_REF { get; set; }
            [Display(Name = "Quantidade", Description = "Quantidade")]
            public decimal QT_QUANTIDADE_NOTA_REF { get; set; }
            [Display(Name = "Nr. DAT", Description = "Nr. DAT")]
            public string NR_DAT { get; set; }
            public string NR_CFOP { get; set; }
            public string NR_CNPJ_ITEM { get; set; }
            public string NR_DRAWBACK { get; set; }




            [Display(Name = "Tipo Ato Concessório", Description = "Tipo Ato Concessório")]
            public string NM_ATO_CONCESSORIO { get; set; }
            [Display(Name = "Número AC", Description = "Número AC")]
            public string NR_NUMERO_AC { get; set; }
            [Display(Name = "Nr. Item", Description = "Nr. Item")]
            public int CD_ITEM_DRAW { get; set; }
            [Display(Name = "Nr. NCM", Description = "Nr. NCM")]
            public int NR_NCM { get; set; }
            [Display(Name = "Quantidade", Description = "Quantidade")]
            public decimal QT_ITEM_DRAW { get; set; }
            [Display(Name = "Valor VMLE", Description = "Valor VMLE")]
            public decimal VL_VMLE { get; set; }

            public string SG_PAIS_EXPORTADOR { get; set; }
            public string SG_ESTADO_EXPORTADOR { get; set; }

            public string SG_PAIS_IMPORTADOR { get; set; }

            public string DS_ENDERECO_EXPORTADOR { get; set; }
            public int? SG_TIPO_DUE { get; set; }
            public bool? SG_NF { get; set; }

            [Display(Name = "Valor do Financiamento", Description = "Valor do Financiamento")]
            public decimal? VL_FINANCIAMENTO { get; set; }

            [Display(Name = "Contato", Description = "Contato")]
            public string NM_CONTATO { get; set; }
            [Display(Name = "E-mail", Description = "E-mail")]
            public string DS_EMAIL { get; set; }
            [Display(Name = "Telefone", Description = "Telefone")]
            public string DS_FONE { get; set; }
            [Display(Name = "Justificativa", Description = "Justificativa")]
            public string DS_JUSTIFICATIVA { get; set; }
        }

        public class dadosProcesso
        {
            public string NM_EXPORTADOR { get; set; }
            public int? CD_EXPORTADOR { get; set; }
            public string NR_CNPJ { get; set; }
            public string NR_FATURA { get; set; }

            public decimal? VL_FRETE_INTERNACIONAL { get; set; }
            public decimal? VL_SEGURO { get; set; }

            public string DS_FRETE_INTERNACIONAL { get; set; }
            public string DS_SEGURO { get; set; }
            public bool? IC_FRETE_EMBUTIDO { get; set; }
            public bool? IC_SEGURO_EMBUTIDO { get; set; }

            public string STR_INCOTERM { get; set; }
            public string STR_URFEMBARQUE { get; set; }
            public string STR_URFDESPACHO { get; set; }

            public int? CD_PAIS_DESTINO { get; set; }
            public string SG_PAIS_DESTINO { get; set; }
            public string NR_CHAVE_NF { get; set; }
            public int? SG_TIPO_DUE { get; set; }

        }

        public class listaDueStatus
        {
            public int CD_DUE { get; set; }
            public int? IC_STATUS_PARA_ENVIO_TRANSMISSAO { get; set; }
            public int? CD_PROCESSO { get; set; }
            public DateTime? DT_DESEMBARACO { get; set; }
            public int? CD_PROCESSO_FILHO { get; set; }

        }

        public class listaProcessoLote
        {
            public int? CD_PROCESSO_FILHO { get; set; }
            public int? CD_PROCESSO_LOTE { get; set; }
        }

        public class listaProcesso
        {
            public int? CD_PROCESSO { get; set; }
        }

        public class dadosLog
        {
            public int CD_LOG { get; set; }
            public DateTime? DT_LOG { get; set; }
            public string IC_TIPO { get; set; }
            public int? CD_DUE { get; set; }
            public int? IC_STATUS { get; set; }
            public string DS_MENSAGEM { get; set; }
            public string NM_USUARIO { get; set; }
            public string DS_ARQUIVO_XML { get; set; }
        }

        public class dadosHistorico
        {
            public DateTime? DT_SITUACAO { get; set; }
            public DateTime? DT_CONSULTA { get; set; }
            public int? CD_SITUACAO { get; set; }
            public int? IC_BLOQUEIO { get; set; }
            public int? IC_ADM { get; set; }
            public string DS_SITUACAO { get; set; }
            public string DS_TIPO { get; set; }
            public string NR_DECLARANTE { get; set; }
        }

        public class dadosLPCO
        {
            public int CD_LPCO { get; set; }
            public int? CD_DUE { get; set; }
            public int? CD_NF { get; set; }
            public int CD_NF_ITEM { get; set; }
            public string NR_LPCO { get; set; }
        }

        public class dadosNCM
        {
            public string NR_NCM { get; set; }
            public string CD_DESTAQUE { get; set; }
            public bool? IC_EMBARQUE_ANTECIPADO { get; set; }
            public decimal? QTD_ESTATISTICA { get; set; }
            public string STR_UNIDADECOMERCIALIZADA { get; set; }
        }

        public class dadosNotaReference
        {
            public int CD_NF_FERENCE { get; set; }
            public int? CD_DUE { get; set; }
            public int? CD_NF { get; set; }
            public int CD_NF_ITEM { get; set; }
            public string NR_CHAVE_NOTA_REF { get; set; }
            public int CD_ITEM { get; set; }
            public decimal QT_ITEM { get; set; }
            public bool? IC_MARCADO { get; set; }
        }

        public class dadosNotaDrawBack
        {
            public int CD_DRAWBACK { get; set; }
            public int? CD_DUE { get; set; }
            public int? CD_NF { get; set; }
            public int CD_NF_ITEM { get; set; }
            public int? CD_ATO_CONCESSORIO { get; set; }
            public string NM_ATO_CONCESSORIO { get; set; }
            public string NR_NUMERO_AC { get; set; }
            public string NR_CNPJ { get; set; }
            public int CD_ITEM { get; set; }
            public string NR_NCM { get; set; }
            public decimal? QT_ITEM { get; set; }
            public decimal? VL_VMLE { get; set; }
            public string SG_ATO_CONCESSORIO { get; set; }

        }
        public class dadosLote
        {
            public int CD_PROCESSO { get; set; }
            public string CD_NUMERO_PROCESSO { get; set; }
        }

        public class dadosNFContainer
        {
            public int? CD_DUE { get; set; }
            public int? CD_NF { get; set; }
            public int CD_NF_ITEM { get; set; }
        }


        public List<camposDUE> retornaDUE(int? modo, int? filtro, int? idLimite, int? idcliente, int? idgrupocliente, string idDue, string sDue, string sRUC, string sProcesso, string sRef, DateTime? dtDeadLineSDInicio, DateTime? dtDeadLineSDFim, bool? bOutrosPortos)
        {
            IQueryable<camposDUE> lstDUE = null;

            if (sRUC != null && sRUC.Length > 11)
            {
                #region PESQUISANDO COM NUMERO PROCESSOS

                lstDUE = (from due in db.DUE
                              join nf in db.DUE_NF on due.CD_DUE equals nf.CD_DUE
                              join p in db.PROCESSOS on due.CD_PROCESSO equals p.CD_PROCESSO into leftp
                              from Orip in leftp.DefaultIfEmpty()

                              join cli in db.ENTIDADE on due.CD_EXPORTADOR equals cli.CD_ENTIDADE
                              join usu in db.USUARIO_CA on due.CD_USUARIO_DUE equals usu.CD_USUARIO into usuLeft
                              from usuOri in usuLeft.DefaultIfEmpty()
                              join usut in db.USUARIO_CA on due.CD_USUARIO_TRANSMISSAO equals usut.CD_USUARIO into usutLeft
                              from usutOri in usutLeft.DefaultIfEmpty()
                              join usuR in db.USUARIO_CA on due.CD_USUARIO_RETIRICAR equals usuR.CD_USUARIO into usuRLeft
                              from usuROri in usuRLeft.DefaultIfEmpty()
                              join pr in db.PROCESSORESERVA on Orip.CD_PROCESSO equals pr.CD_PROCESSO into prLeft
                              from prOri in prLeft.DefaultIfEmpty()
                              join res in db.RESERVAS on prOri.CD_RESERVA equals res.CD_RESERVA into resLeft
                              from resOri in resLeft.DefaultIfEmpty()
                              join via in db.VIAGEMARMADOR on resOri.CD_VIAGEM_ARMADOR equals via.CD_VIAGEM_ARMADOR into viaLeft
                              from viaOri in viaLeft.DefaultIfEmpty()
                              join arm in db.ENTIDADE on viaOri.CD_ARMADOR equals arm.CD_ENTIDADE into armLeft
                              from armOri in armLeft.DefaultIfEmpty()
                              join term in db.ENTIDADE on viaOri.CD_TERMINAL_ATRACACAO equals term.CD_ENTIDADE into termLeft
                              from termOri in termLeft.DefaultIfEmpty()


                              join viagem in db.VIAGEM on viaOri.CD_VIAGEM equals viagem.CD_VIAGEM into viagemLeft
                              from viagemOri in viagemLeft.DefaultIfEmpty()
                              join nav in db.NAVIOS on viagemOri.CD_NAVIO equals nav.CD_NAVIO into navLeft
                              from navOri in navLeft.DefaultIfEmpty()
                              join portolocal in db.LOCAIS on resOri.CD_PORTO_ORIGEM equals portolocal.CD_LOCAL into portolocalLeft
                              from portolocalOri in portolocalLeft.DefaultIfEmpty()
                              join paisDestino in db.PAIS on resOri.CD_PAIS_RESERVA equals paisDestino.CD_PAIS into paisDestinoLeft
                              from paisDestinoOri in paisDestinoLeft.DefaultIfEmpty()
                              join localDespacho in db.UNIDRF on due.STR_CODIGOUNIDADERF_DESPACHO equals localDespacho.STR_CODIGOUNIDADERF into localDespachoLeft
                              from localDespachoOri in localDespachoLeft.DefaultIfEmpty()
                              join localEmbarque in db.UNIDRF on due.STR_CODIGOUNIDADERF_EMBARQUE equals localEmbarque.STR_CODIGOUNIDADERF into localEmbarqueLeft
                              from localEmbarqueOri in localEmbarqueLeft.DefaultIfEmpty()

                              join recintoDespacho in db.RECIALFA on due.CD_RECINTO_DESPACHO equals recintoDespacho.STR_CODIGORECIALFA into recintoDespachoLeft
                              from recintoDespachoOri in recintoDespachoLeft.DefaultIfEmpty()
                              join recintoEmbarque in db.RECIALFA on due.CD_RECINTO_EMBARQUE equals recintoEmbarque.STR_CODIGORECIALFA into recintoEmbarqueLeft
                              from recintoEmbarqueOri in recintoEmbarqueLeft.DefaultIfEmpty()

                              //join prc in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA into prcLeft
                              //from prcOri in prcLeft.Where(x => x.CD_STATUS_CONTAINER2 == null).OrderBy(n => n.DT_REDEX).Take(1).DefaultIfEmpty()

                              join prc in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA into prcLeft
                              from prcOri in prcLeft.OrderBy(n => n.DT_REDEX).Take(1).DefaultIfEmpty()

                              join prc2 in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc2.CD_PROCESSORESERVA into prc2Left
                              from prc2Ori in prc2Left.Take(1).DefaultIfEmpty()

                              //join prc2 in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc2.CD_PROCESSORESERVA into prc2Left
                              //from prc2Ori in prc2Left.Where(x => x.CD_STATUS_CONTAINER2 != null).Take(1).DefaultIfEmpty()


                              join status2 in db.STATUS_PROCESSO on prc2Ori.CD_STATUS_CONTAINER2 equals status2.CD_STATUS_PROCESSO into status2Left
                              from status2Ori in status2Left.Where(x=>x.IC_EMBARQUE_CANCELADO == true).DefaultIfEmpty()

                              join formaExport in db.FORMA_EXPORTACAO_DUE on due.CD_FORMA_EXPORTACAO equals formaExport.CD_FORMA_EXPORTACAO
                              where nf.CD_NUMERO_CHAVE_NF == sRUC
                              select new camposDUE
                              {
                                  CD_DUE = due.CD_DUE,
                                  CD_PROCESSO = due.CD_PROCESSO,
                                  CD_PROCESSORESERVA = prOri.CD_PROCESSORESERVA,
                                  NR_PROCESSO = Orip.CD_NUMERO_PROCESSO != null ? "E-" + Orip.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + Orip.CD_NUMERO_PROCESSO.Substring(0, 2) : "",
                                  NR_PROCESSO_LOTE = FN_RETORNA_PROCESSO_LOTE_DUE(due.CD_DUE),
                                  SG_TIPO_DUE = due.SG_TIPO_DUE,
                                  CD_NAVIO = navOri.CD_NAVIO,
                                  CD_DESTINO = paisDestinoOri.CD_PAIS,
                                  DS_NAVIO = navOri.NM_NAVIO,
                                  DS_REFERENCIA_PROCESSO = Orip.DS_REFERENCIA_CLIENTE,
                                  DS_REFERENCIA = due.DS_REFERENCIA,
                                  DT_DEADLINE_CARGA = prOri.DT_DEAD_LINE_CONTAINER,
                                  DT_ETA = viagemOri.DT_ETA,
                                  DT_DEADLINE_DRAFT = prOri.DT_DEAD_LINE_DRAFT,

                                  DS_TERMINAL_ATRACACAO = termOri.NM_FANTASIA_ENTIDADE,
                                  DS_BOOKING = resOri.CD_NUMERO_RESERVA,
                                  DS_ORIGEM = portolocalOri.NM_CIDADE,
                                  CD_ORIGEM = portolocalOri.CD_LOCAL,
                                  DS_DESTINO = paisDestinoOri.STR_NOMEPAIS,
                                  DS_ARMADOR = armOri.NM_FANTASIA_ENTIDADE,
                                  CD_NUMERO_DUE = due.CD_NUMERO_DUE,
                                  CD_NUMERO_RUC = due.CD_NUMERO_RUC,
                                  DT_DUE = due.DT_DUE,
                                  DT_TRANSMISSAO = due.DT_TRANSMISSAO,
                                  CD_USUARIO_DUE = due.CD_USUARIO_DUE,
                                  CD_USUARIO_TRANSMISSAO = due.CD_USUARIO_TRANSMISSAO,
                                  DT_DESEMBARACO = due.DT_DESEMBARACO == null ? due.DT_DESEMBARACO : due.DT_DESEMBARACO,
                                  //NR_CONTAINER = Orip.CD_PROCESSO != null ? FN_RETORNA_CONTAINER_PROCESSO_DUE(Orip.CD_PROCESSO) : "",
                                  NR_CONTAINER = FN_RETORNA_CONTAINER_PROCESSO_DUE(Orip.CD_PROCESSO),
                                  STR_CODIGOMOEDA = due.STR_CODIGOMOEDA,
                                  DS_SITUACAO = due.DS_SITUACAO,
                                  CD_EXPORTADOR = due.CD_EXPORTADOR,
                                  DS_EXPORTADOR = cli.NM_FANTASIA_ENTIDADE,
                                  //NR_CNPJ = cli.CD_CNPJ_ENTIDADE,
                                  NR_CNPJ = due.NR_CNPJ == null ? cli.CD_CNPJ_ENTIDADE : due.NR_CNPJ,
                                  CD_FORMA_EXPORTACAO = due.CD_FORMA_EXPORTACAO,
                                  NM_FORMA_EXPORTACAO = formaExport.DS_FORMA_EXPORTACAO,
                                  STR_CODIGOUNIDADERF_DESPACHO = due.STR_CODIGOUNIDADERF_DESPACHO,
                                  CD_RECINTO_DESPACHO = due.CD_RECINTO_DESPACHO,
                                  IC_RECINTO_ADUANEIRO_DESPACHO = due.IC_RECINTO_ADUANEIRO_DESPACHO,
                                  STR_CODIGOUNIDADERF_EMBARQUE = due.STR_CODIGOUNIDADERF_EMBARQUE,
                                  CD_RECINTO_EMBARQUE = due.CD_RECINTO_EMBARQUE,
                                  IC_RECINTO_ADUANEIRO_EMBARQUE = due.IC_RECINTO_ADUANEIRO_EMBARQUE,
                                  IC_EMBUTIDO = due.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                                  DS_REFERENCIA_ENDERECO = due.DS_REFERENCIA_ENDERECO,
                                  DS_INFORMACOES_COMPLEMENTARES = due.DS_INFORMACOES_COMPLEMENTARES,
                                  CD_VIA_TRANSPORTE_ESPECIAL = due.CD_VIA_TRANSPORTE_ESPECIAL,
                                  VL_FRETE_INTERNACIONAL = due.VL_FRETE_INTERNACIONAL,
                                  VL_DESCONTO = due.VL_DESCONTO,
                                  IC_STATUS_PARA_ENVIO_TRANSMISSAO = due.IC_STATUS_PARA_ENVIO_TRANSMISSAO,
                                  SG_FRETE_INTERNACIONAL_EMBUTIDO = due.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                                  SG_SEGURO_INTERNACIONAL_EMBUTIDO = due.SG_SEGURO_INTERNACIONAL_EMBUTIDO,
                                  VL_SEGURO = due.VL_SEGURO,
                                  NM_DESPACHO = localDespachoOri.STR_CODIGOUNIDADERF + " - " + localDespachoOri.STR_NOMEUNIDADERF,
                                  NM_EMBARQUE = localEmbarqueOri.STR_CODIGOUNIDADERF + " - " + localEmbarqueOri.STR_NOMEUNIDADERF,
                                  NM_RECINTO_DESPACHO = recintoDespachoOri.STR_CODIGORECIALFA + " - " + recintoDespachoOri.STR_DESCRICAO,
                                  NM_RECINTO_EMBARQUE = recintoEmbarqueOri.STR_CODIGORECIALFA + " - " + recintoEmbarqueOri.STR_DESCRICAO,
                                  NM_USUARIO = usuOri.NM_USUARIO,
                                  NM_USUARIO_TRANSMISSAO = usutOri.NM_USUARIO,
                                  NM_USUARIO_RETIFICACAO = usuROri.NM_USUARIO,
                                  DS_ARQUIVO_XML = due.DS_ARQUIVO_XML,
                                  DT_CANCELADO = due.DT_CANCELADO,
                                  IC_BLOQUEIO = due.IC_BLOQUEIO,
                                  IC_ADM = due.IC_CONTROLE_ADM,
                                  IC_SITUACAO_CARGA = due.IC_SITUACAO_CARGA,
                                  DS_STATUS_EMBARQUE2 = status2Ori.NM_STATUS_PROCESSO,
                                  CD_STATUS_EMBARQUE2 = prc2Ori.CD_STATUS_CONTAINER2,
                                  DT_ENTREGA_DESPACHO = due.DT_ENTREGA_DESPACHO != null ? due.DT_ENTREGA_DESPACHO : Orip.DT_ENTREGA_DESPACHO != null ? Orip.DT_ENTREGA_DESPACHO : (from xdde in db.DDE where Orip.CD_PROCESSO == xdde.CD_PROCESSO select new { xdde.DT_ENTREGA_DESPACHO }).FirstOrDefault().DT_ENTREGA_DESPACHO,
                                  DT_EMBARQUE = prcOri.DT_EMBARQUE,
                                  DT_FATURADO = Orip.DT_PASTA_FATURAMENTO,
                                  DT_CRIACAO_BL = (from ev in db.PROCESSOEVENTO where Orip.CD_PROCESSO == ev.CD_PROCESSO && (ev.CD_EVENTO == 24 || ev.CD_EVENTO == 25 || ev.CD_EVENTO == 26 || ev.CD_EVENTO == 46) select ev.DT_PROCESSOEVENTO).FirstOrDefault(),
                                  CD_CHAVE_ACESSO = due.CD_CHAVE_ACESSO,
                                  //DT_AVERBADO = due.DT_ABERBADO == null ? (from his in db.DUE_SITUACAO_HISTORICO where due.CD_DUE == his.CD_DUE && his.CD_SITUACAO == 70 select new { his.DT_SITUACAO }).FirstOrDefault().DT_SITUACAO : due.DT_ABERBADO
                                  DT_AVERBADO = due.DT_ABERBADO,
                                  //DT_DEPOSITADO = prcOri.DT_REDEX != null ? prcOri.DT_REDEX : prcOri.DT_DEPOSITO_TERM_EMBARQUE,
                                  DT_DEPOSITADO = (from prcx in db.PROCESSORESERVACONTAINER where prOri.CD_PROCESSORESERVA == prcx.CD_PROCESSORESERVA select new { DT_DEP = prcx.DT_REDEX != null ? prcx.DT_REDEX : prcx.DT_DEPOSITO_TERM_EMBARQUE }).OrderBy(p => p.DT_DEP).FirstOrDefault().DT_DEP,
                                  NM_TERMINAL_REDEX = prcOri.DT_REDEX != null && prcOri.DT_DEPOSITO_TERM_EMBARQUE == null ? (from termR in db.ENTIDADE where prcOri.CD_TERMINAL_REDEX == termR.CD_ENTIDADE select new { termR.NM_FANTASIA_ENTIDADE }).FirstOrDefault().NM_FANTASIA_ENTIDADE : null,
                                  DT_CANAL_VERMELHO = due.DT_CANAL_VERMELHO,
                                  SG_TIPO_DUE_ANT = due.SG_TIPO_DUE_ANT,
                                  CD_MOTIVO_DISPENSA_NF = due.CD_MOTIVO_DISPENSA_NF,
                                  STR_CODIGOVIATRANSPORTE = Orip.STR_CODIGOVIATRANSPORTE,
                                  //NM_STATUS_DUE_LPCO = due.CD_DUE != null ? FN_RETORNA_STATUS_DUE_LPCO(due.CD_DUE) : "",
                                  NM_STATUS_DUE_LPCO = FN_RETORNA_STATUS_DUE_LPCO(due.CD_DUE),
                                  IC_EMBARQUE_CANCELADO = status2Ori.IC_EMBARQUE_CANCELADO,
                                  CD_RECINTO_EMBARQUE_ENTIDADE = recintoEmbarqueOri.CD_ENTIDADE,
                                  CD_RECINTO_EMBARQUE_PROCESSO = viaOri.CD_TERMINAL_ATRACACAO,
                                  NR_CHAVE_NF = due.NR_CHAVE_NF,
                                  DT_EMBARQUE_CONTAINER = viaOri.DT_DESATRACACAO /*(from _pr in db.PROCESSORESERVA
                                                           join _prc in db.PROCESSORESERVACONTAINER on _pr.CD_PROCESSORESERVA equals _prc.CD_PROCESSORESERVA
                                                           where _pr.CD_PROCESSO == Orip.CD_PROCESSO
                                                           select _prc.DT_EMBARQUE).Max()*/

                              });

                #endregion PESQUISANDO COM NUMERO PROCESSOS

                return lstDUE.ToList();
            }
            else
            {
                lstDUE = (from due in db.DUE

                              join p in db.PROCESSOS on due.CD_PROCESSO equals p.CD_PROCESSO into leftp
                              from Orip in leftp.DefaultIfEmpty()

                              join cli in db.ENTIDADE on due.CD_EXPORTADOR equals cli.CD_ENTIDADE
                              join usu in db.USUARIO_CA on due.CD_USUARIO_DUE equals usu.CD_USUARIO into usuLeft
                              from usuOri in usuLeft.DefaultIfEmpty()
                              join usut in db.USUARIO_CA on due.CD_USUARIO_TRANSMISSAO equals usut.CD_USUARIO into usutLeft
                              from usutOri in usutLeft.DefaultIfEmpty()
                              join usuR in db.USUARIO_CA on due.CD_USUARIO_RETIRICAR equals usuR.CD_USUARIO into usuRLeft
                              from usuROri in usuRLeft.DefaultIfEmpty()
                              join pr in db.PROCESSORESERVA on Orip.CD_PROCESSO equals pr.CD_PROCESSO into prLeft
                              from prOri in prLeft.DefaultIfEmpty()
                              join res in db.RESERVAS on prOri.CD_RESERVA equals res.CD_RESERVA into resLeft
                              from resOri in resLeft.DefaultIfEmpty()
                              join via in db.VIAGEMARMADOR on resOri.CD_VIAGEM_ARMADOR equals via.CD_VIAGEM_ARMADOR into viaLeft
                              from viaOri in viaLeft.DefaultIfEmpty()
                              join arm in db.ENTIDADE on viaOri.CD_ARMADOR equals arm.CD_ENTIDADE into armLeft
                              from armOri in armLeft.DefaultIfEmpty()
                              join term in db.ENTIDADE on viaOri.CD_TERMINAL_ATRACACAO equals term.CD_ENTIDADE into termLeft
                              from termOri in termLeft.DefaultIfEmpty()


                              join viagem in db.VIAGEM on viaOri.CD_VIAGEM equals viagem.CD_VIAGEM into viagemLeft
                              from viagemOri in viagemLeft.DefaultIfEmpty()
                              join nav in db.NAVIOS on viagemOri.CD_NAVIO equals nav.CD_NAVIO into navLeft
                              from navOri in navLeft.DefaultIfEmpty()
                              join portolocal in db.LOCAIS on resOri.CD_PORTO_ORIGEM equals portolocal.CD_LOCAL into portolocalLeft
                              from portolocalOri in portolocalLeft.DefaultIfEmpty()
                              join paisDestino in db.PAIS on resOri.CD_PAIS_RESERVA equals paisDestino.CD_PAIS into paisDestinoLeft
                              from paisDestinoOri in paisDestinoLeft.DefaultIfEmpty()
                              join localDespacho in db.UNIDRF on due.STR_CODIGOUNIDADERF_DESPACHO equals localDespacho.STR_CODIGOUNIDADERF into localDespachoLeft
                              from localDespachoOri in localDespachoLeft.DefaultIfEmpty()
                              join localEmbarque in db.UNIDRF on due.STR_CODIGOUNIDADERF_EMBARQUE equals localEmbarque.STR_CODIGOUNIDADERF into localEmbarqueLeft
                              from localEmbarqueOri in localEmbarqueLeft.DefaultIfEmpty()

                              join recintoDespacho in db.RECIALFA on due.CD_RECINTO_DESPACHO equals recintoDespacho.STR_CODIGORECIALFA into recintoDespachoLeft
                              from recintoDespachoOri in recintoDespachoLeft.DefaultIfEmpty()
                              join recintoEmbarque in db.RECIALFA on due.CD_RECINTO_EMBARQUE equals recintoEmbarque.STR_CODIGORECIALFA into recintoEmbarqueLeft
                              from recintoEmbarqueOri in recintoEmbarqueLeft.DefaultIfEmpty()

                          //join prc in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA into prcLeft
                          //from prcOri in prcLeft.Where(x => x.CD_STATUS_CONTAINER2 == null).OrderBy(n => n.DT_REDEX).Take(1).DefaultIfEmpty()

                          join prc in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc.CD_PROCESSORESERVA into prcLeft
                          from prcOri in prcLeft.OrderBy(n => n.DT_REDEX).Take(1).DefaultIfEmpty()

                          join prc2 in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc2.CD_PROCESSORESERVA into prc2Left
                          from prc2Ori in prc2Left.Take(1).DefaultIfEmpty()

                          //join prc2 in db.PROCESSORESERVACONTAINER on prOri.CD_PROCESSORESERVA equals prc2.CD_PROCESSORESERVA into prc2Left
                          //from prc2Ori in prc2Left.Where(x => x.CD_STATUS_CONTAINER2 != null).Take(1).DefaultIfEmpty()

                              join status2 in db.STATUS_PROCESSO on prc2Ori.CD_STATUS_CONTAINER2 equals status2.CD_STATUS_PROCESSO into status2Left
                              from status2Ori in status2Left.DefaultIfEmpty()

                              join formaExport in db.FORMA_EXPORTACAO_DUE on due.CD_FORMA_EXPORTACAO equals formaExport.CD_FORMA_EXPORTACAO

                              select new camposDUE
                                   {
                                       CD_DUE = due.CD_DUE,
                                       CD_PROCESSO = due.CD_PROCESSO,
                                       CD_PROCESSORESERVA = prOri.CD_PROCESSORESERVA,
                                       NR_PROCESSO = Orip.CD_NUMERO_PROCESSO != null ? "E-" + Orip.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + Orip.CD_NUMERO_PROCESSO.Substring(0, 2) : "",
                                       NR_PROCESSO_LOTE =  FN_RETORNA_PROCESSO_LOTE_DUE(due.CD_DUE),
                                       SG_TIPO_DUE = due.SG_TIPO_DUE,
                                       CD_NAVIO = navOri.CD_NAVIO,
                                       CD_DESTINO = paisDestinoOri.CD_PAIS,
                                       DS_NAVIO = navOri.NM_NAVIO,
                                       DS_REFERENCIA_PROCESSO = Orip.DS_REFERENCIA_CLIENTE,
                                       DS_REFERENCIA = due.DS_REFERENCIA,
                                       DT_DEADLINE_CARGA = viaOri.DT_DEAD_LINE_CONTAINER,
                                       DT_ETA = viagemOri.DT_ETA,
                                       DT_DEADLINE_DRAFT = viaOri.DT_DEAD_LINE_DRAFT,

                                       DS_TERMINAL_ATRACACAO = termOri.NM_FANTASIA_ENTIDADE,
                                       DS_BOOKING = resOri.CD_NUMERO_RESERVA,
                                       DS_ORIGEM = portolocalOri.NM_CIDADE,
                                       CD_ORIGEM = portolocalOri.CD_LOCAL,
                                       DS_DESTINO = paisDestinoOri.STR_NOMEPAIS,
                                       DS_ARMADOR = armOri.NM_FANTASIA_ENTIDADE,
                                       CD_NUMERO_DUE = due.CD_NUMERO_DUE,
                                       CD_NUMERO_RUC = due.CD_NUMERO_RUC,
                                       DT_DUE = due.DT_DUE,
                                       DT_TRANSMISSAO = due.DT_TRANSMISSAO,
                                       CD_USUARIO_DUE = due.CD_USUARIO_DUE,
                                       CD_USUARIO_TRANSMISSAO = due.CD_USUARIO_TRANSMISSAO,
                                       DT_DESEMBARACO = due.DT_DESEMBARACO == null ? due.DT_DESEMBARACO : due.DT_DESEMBARACO,
                                        //NR_CONTAINER = Orip.CD_PROCESSO != null ? FN_RETORNA_CONTAINER_PROCESSO_DUE(Orip.CD_PROCESSO) : "",
                                        NR_CONTAINER = FN_RETORNA_CONTAINER_PROCESSO_DUE(Orip.CD_PROCESSO),
                                        STR_CODIGOMOEDA = due.STR_CODIGOMOEDA,
                                       DS_SITUACAO = due.DS_SITUACAO,
                                       CD_EXPORTADOR = due.CD_EXPORTADOR,
                                       DS_EXPORTADOR = cli.NM_FANTASIA_ENTIDADE,
                                       NR_CNPJ = due.NR_CNPJ == null ? cli.CD_CNPJ_ENTIDADE : due.NR_CNPJ,
                                       CD_FORMA_EXPORTACAO = due.CD_FORMA_EXPORTACAO,
                                       NM_FORMA_EXPORTACAO = formaExport.DS_FORMA_EXPORTACAO,
                                       STR_CODIGOUNIDADERF_DESPACHO = due.STR_CODIGOUNIDADERF_DESPACHO,
                                       CD_RECINTO_DESPACHO = due.CD_RECINTO_DESPACHO,
                                       IC_RECINTO_ADUANEIRO_DESPACHO = due.IC_RECINTO_ADUANEIRO_DESPACHO,

                                       STR_CODIGOUNIDADERF_EMBARQUE = due.STR_CODIGOUNIDADERF_EMBARQUE,
                                       CD_RECINTO_EMBARQUE = due.CD_RECINTO_EMBARQUE,

                                       IC_RECINTO_ADUANEIRO_EMBARQUE = due.IC_RECINTO_ADUANEIRO_EMBARQUE,
                                       IC_EMBUTIDO = due.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                                       DS_REFERENCIA_ENDERECO = due.DS_REFERENCIA_ENDERECO,
                                       DS_INFORMACOES_COMPLEMENTARES = due.DS_INFORMACOES_COMPLEMENTARES,
                                       CD_VIA_TRANSPORTE_ESPECIAL = due.CD_VIA_TRANSPORTE_ESPECIAL,
                                       VL_FRETE_INTERNACIONAL = due.VL_FRETE_INTERNACIONAL,

                                       VL_DESCONTO = due.VL_DESCONTO,
                                       IC_STATUS_PARA_ENVIO_TRANSMISSAO = due.IC_STATUS_PARA_ENVIO_TRANSMISSAO,
                                       SG_FRETE_INTERNACIONAL_EMBUTIDO = due.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                                       SG_SEGURO_INTERNACIONAL_EMBUTIDO = due.SG_SEGURO_INTERNACIONAL_EMBUTIDO,
                                       VL_SEGURO = due.VL_SEGURO,
                                       NM_DESPACHO = localDespachoOri.STR_CODIGOUNIDADERF + " - " + localDespachoOri.STR_NOMEUNIDADERF,
                                       NM_EMBARQUE = localEmbarqueOri.STR_CODIGOUNIDADERF + " - " + localEmbarqueOri.STR_NOMEUNIDADERF,
                                       NM_RECINTO_DESPACHO = recintoDespachoOri.STR_CODIGORECIALFA + " - " + recintoDespachoOri.STR_DESCRICAO,
                                       NM_RECINTO_EMBARQUE = recintoEmbarqueOri.STR_CODIGORECIALFA + " - " + recintoEmbarqueOri.STR_DESCRICAO,
                                       NM_USUARIO = usuOri.NM_USUARIO,
                                       NM_USUARIO_TRANSMISSAO = usutOri.NM_USUARIO,
                                       NM_USUARIO_RETIFICACAO = usuROri.NM_USUARIO,
                                       DS_ARQUIVO_XML = due.DS_ARQUIVO_XML,
                                       DT_CANCELADO = due.DT_CANCELADO,
                                       IC_BLOQUEIO = due.IC_BLOQUEIO,
                                       IC_ADM = due.IC_CONTROLE_ADM,
                                       IC_SITUACAO_CARGA = due.IC_SITUACAO_CARGA,
                                       DS_STATUS_EMBARQUE2 = status2Ori.NM_STATUS_PROCESSO,
                                       CD_STATUS_EMBARQUE2 = prc2Ori.CD_STATUS_CONTAINER2,
                                       DT_ENTREGA_DESPACHO = due.DT_ENTREGA_DESPACHO != null ? due.DT_ENTREGA_DESPACHO : Orip.DT_ENTREGA_DESPACHO != null ? Orip.DT_ENTREGA_DESPACHO : (from xdde in db.DDE where Orip.CD_PROCESSO == xdde.CD_PROCESSO select new { xdde.DT_ENTREGA_DESPACHO }).FirstOrDefault().DT_ENTREGA_DESPACHO,
                                       DT_EMBARQUE = prcOri.DT_EMBARQUE,
                                       DT_FATURADO = Orip.DT_PASTA_FATURAMENTO,
                                       DT_CRIACAO_BL = (from ev in db.PROCESSOEVENTO where Orip.CD_PROCESSO == ev.CD_PROCESSO && (ev.CD_EVENTO == 24 || ev.CD_EVENTO == 25 || ev.CD_EVENTO == 26 || ev.CD_EVENTO == 46) select ev.DT_PROCESSOEVENTO).FirstOrDefault(),
                                       CD_CHAVE_ACESSO = due.CD_CHAVE_ACESSO,
                                       //DT_AVERBADO = due.DT_ABERBADO == null ? (from his in db.DUE_SITUACAO_HISTORICO where due.CD_DUE == his.CD_DUE && his.CD_SITUACAO == 70 select new { his.DT_SITUACAO }).FirstOrDefault().DT_SITUACAO : due.DT_ABERBADO
                                       DT_AVERBADO = due.DT_ABERBADO,
                                       DT_DEPOSITADO = (from prcx in db.PROCESSORESERVACONTAINER where prOri.CD_PROCESSORESERVA == prcx.CD_PROCESSORESERVA select new { DT_DEP = prcx.DT_REDEX != null ? prcx.DT_REDEX : prcx.DT_DEPOSITO_TERM_EMBARQUE }).OrderBy(p => p.DT_DEP).FirstOrDefault().DT_DEP,
                                       NR_CSI = (from prcx in db.PROCESSORESERVACONTAINER where prOri.CD_PROCESSORESERVA == prcx.CD_PROCESSORESERVA select new { nr_csi = prcx.NR_CSI }).OrderBy(p => p.nr_csi ).FirstOrDefault().nr_csi,
                                       NM_TERMINAL_REDEX = prcOri.DT_REDEX != null && prcOri.DT_DEPOSITO_TERM_EMBARQUE == null ? (from termR in db.ENTIDADE where prcOri.CD_TERMINAL_REDEX == termR.CD_ENTIDADE select new { termR.NM_FANTASIA_ENTIDADE }).FirstOrDefault().NM_FANTASIA_ENTIDADE : null,
                                       DT_CANAL_VERMELHO = due.DT_CANAL_VERMELHO,
                                       SG_TIPO_DUE_ANT = due.SG_TIPO_DUE_ANT,
                                       CD_MOTIVO_DISPENSA_NF = due.CD_MOTIVO_DISPENSA_NF,
                                       STR_CODIGOVIATRANSPORTE = Orip.STR_CODIGOVIATRANSPORTE,
                                      //NM_STATUS_DUE_LPCO =  due.CD_DUE != null ? FN_RETORNA_STATUS_DUE_LPCO(due.CD_DUE) : "",
                                       NM_STATUS_DUE_LPCO =  FN_RETORNA_STATUS_DUE_LPCO(due.CD_DUE),
                                       IC_EMBARQUE_CANCELADO = status2Ori.IC_EMBARQUE_CANCELADO, 
                                       CD_RECINTO_EMBARQUE_ENTIDADE = recintoEmbarqueOri.CD_ENTIDADE,
                                       CD_RECINTO_EMBARQUE_PROCESSO = viaOri.CD_TERMINAL_ATRACACAO,
                                       NR_CHAVE_NF = due.NR_CHAVE_NF,
                                       DT_EMBARQUE_CONTAINER = viaOri.DT_DESATRACACAO/*(from _pr in db.PROCESSORESERVA
                                                                    join _prc in db.PROCESSORESERVACONTAINER on _pr.CD_PROCESSORESERVA equals _prc.CD_PROCESSORESERVA
                                                                    where _pr.CD_PROCESSO == Orip.CD_PROCESSO
                                                                    select _prc.DT_EMBARQUE).Max()*/

                                   }).OrderByDescending(p => p.DT_DUE).ThenBy(x => x.CD_DUE);

                if ((idDue != null && idDue != "") || (sDue != null && sDue != "") || (sProcesso != null && sProcesso != "") || (sRef != "" && sRef != null) || (sRUC != null && sRUC != ""))
                {
                    if (sDue != null && sDue != "")
                    {

                        lstDUE = lstDUE.Where(x => x.CD_NUMERO_DUE == sDue);

                    }
                    if (idDue != null && idDue != "")
                    {
                        int xDUE = int.Parse(idDue);
                        lstDUE = lstDUE.Where(x => x.CD_DUE == xDUE);

                    }
                    if (sRUC != null && sRUC != "")
                    {

                        lstDUE = lstDUE.Where(x => x.NR_CONTAINER.Contains(sRUC));

                    }
                    if (sProcesso != null && sProcesso != "")
                    {
                        //string xProcesso = sProcesso.Replace("-", "").Replace("/", "").ToUpper().Replace("E", "");
                        //xProcesso = xProcesso.Substring(6, 2) + xProcesso.Substring(0, 6);
                        lstDUE = lstDUE.Where(x => x.NR_PROCESSO == sProcesso || x.NR_PROCESSO_LOTE.Contains(sProcesso));

                    }
                    if (sRef != null && sRef != "")
                    {


                        lstDUE = lstDUE.Where(x => x.DS_REFERENCIA.Contains(sRef));

                    }
                }
                else
                {
                    if (bOutrosPortos != true)
                    {
                        lstDUE = lstDUE.Where(x => x.CD_ORIGEM == 128);
                    }



                    //Retorna somente os processos não embarcados
                    if (modo == 1)
                    {
                        lstDUE = lstDUE.Where(x => x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 0);
                    }
                    if (modo == 2)
                    {
                        lstDUE = lstDUE.Where(x => ((x.IC_STATUS_PARA_ENVIO_TRANSMISSAO != 40 && x.IC_STATUS_PARA_ENVIO_TRANSMISSAO != 0) || (x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 40 && x.IC_BLOQUEIO == 1)) && (!(x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 70 || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 80
                              || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 81 || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 82 || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 83 || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 83
                              || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 86)));
                    }
                    if (modo == 3)
                    {
                        lstDUE = lstDUE.Where(x => x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 40 && x.IC_BLOQUEIO == 2);
                    }
                    if (modo == 5)
                    {
                        lstDUE = lstDUE.Where(x => x.CD_PROCESSO == null);
                    }
                    if (modo == 7)
                    {
                        lstDUE = lstDUE.Where(x => x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 40 && x.IC_BLOQUEIO == 1);
                    }
                    if (modo == 8)
                    {
                        lstDUE = lstDUE.Where(x => x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 30);
                    }
                    if (modo == 9)
                    {
                        lstDUE = lstDUE.Where(x => x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 21);
                    }
                    if (modo == 10)
                    {
                        lstDUE = lstDUE.Where(x => x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 70);
                    }
                    if (modo == 11)
                    {
                        lstDUE = lstDUE.Where(x => ((x.IC_SITUACAO_CARGA == 1 && x.IC_STATUS_PARA_ENVIO_TRANSMISSAO != 40 && x.IC_STATUS_PARA_ENVIO_TRANSMISSAO != 0) || (x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 40 && x.IC_BLOQUEIO == 1)) && (!(x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 70 || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 80
                               || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 82 || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 83 
                               || x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 86)));
                    }
                    if (modo == 12)
                    {
                        lstDUE = lstDUE.Where(x => (x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 40 && x.IC_BLOQUEIO == 2 && x.DT_ENTREGA_DESPACHO == null) && (x.CD_RECINTO_EMBARQUE != "8931356" && x.CD_RECINTO_EMBARQUE != "8931359") && (x.DT_EMBARQUE == null && x.DT_FATURADO == null));
                    }
                    if (modo == 13)
                    {
                        lstDUE = lstDUE.Where(x => x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 40 && x.IC_BLOQUEIO == 2 && x.DT_ENTREGA_DESPACHO != null);
                    }
                    if (modo == 14)
                    {
                        lstDUE = lstDUE.Where(x => x.IC_STATUS_PARA_ENVIO_TRANSMISSAO == 81);
                    }

                    if (modo == 6 || modo == 14)
                    {
                        lstDUE = lstDUE.Where(x => x.DT_CANCELADO != null);
                    }
                    else
                    {
                        lstDUE = lstDUE.Where(x => x.DT_CANCELADO == null);
                    }

                    if (modo == 15)
                    {
                        lstDUE = lstDUE.Where(x => x.DT_CANCELADO == null && x.DT_DESEMBARACO != null && x.IC_BLOQUEIO == 2 && x.DT_AVERBADO == null);
                    }


                    if (modo != 14)
                    {
                        if (idLimite == 0)
                        {
                            DateTime dtPartida = DateTime.Now.Date.AddDays(-15);
                            lstDUE = lstDUE.Where(x => x.DT_DUE > dtPartida);
                        }
                        if (idLimite == 1)
                        {
                            DateTime dtPartida = DateTime.Now.Date.AddDays(-30);
                            lstDUE = lstDUE.Where(x => x.DT_DUE > dtPartida);
                        }
                        if (idLimite == 2)
                        {
                            DateTime dtPartida = DateTime.Now.Date.AddDays(-60);
                            lstDUE = lstDUE.Where(x => x.DT_DUE > dtPartida);
                        }
                        if (idLimite == 3)
                        {
                            DateTime dtPartida = DateTime.Now.Date.AddDays(-90);
                            lstDUE = lstDUE.Where(x => x.DT_DUE > dtPartida);
                        }
                        if (idLimite == 4)
                        {
                            DateTime dtPartida = DateTime.Now.Date.AddDays(-120);
                            lstDUE = lstDUE.Where(x => x.DT_DUE > dtPartida);
                        }
                        if (idLimite == 5)
                        {
                            DateTime dtPartida = DateTime.Now.Date.AddDays(-180);
                            lstDUE = lstDUE.Where(x => x.DT_DUE > dtPartida);
                        }
                    }
                    if (idcliente != null && idcliente != 0)
                    {
                        lstDUE = lstDUE.Where(x => x.CD_EXPORTADOR == idcliente);
                    }
                    if (idgrupocliente != null && idgrupocliente != 0)
                    {
                        List<int> lstCliente = (from gru in db.GRUPOCLI_ENTIDADE
                                                where gru.CD_GRUPOCLI == idgrupocliente
                                                select gru.CD_ENTIDADE).ToList();

                        lstDUE = lstDUE.Where(x => lstCliente.Contains((int)x.CD_EXPORTADOR));
                        //lstProcesso = lstProcesso.Where(x => x.CD_GRUPO_CLIENTE == idgrupocliente);
                    }

                    if (filtro != 0)
                    {
                        if (filtro == 1)
                        {
                            if (dtDeadLineSDInicio != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_DEADLINE_DRAFT >= dtDeadLineSDInicio);

                            }

                            if (dtDeadLineSDFim != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_DEADLINE_DRAFT <= dtDeadLineSDFim);

                            }
                        }
                        else if (filtro == 2)
                        {

                            if (dtDeadLineSDInicio != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_DEADLINE_CARGA >= dtDeadLineSDInicio);

                            }
                            if (dtDeadLineSDFim != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_DEADLINE_CARGA <= dtDeadLineSDFim);

                            }
                        }
                        else if (filtro == 3)
                        {

                            if (dtDeadLineSDInicio != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_DESEMBARACO >= dtDeadLineSDInicio);

                            }
                            if (dtDeadLineSDFim != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_DESEMBARACO <= dtDeadLineSDFim);

                            }
                        }
                        else if (filtro == 4)
                        {
                            if (dtDeadLineSDInicio != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_AVERBADO >= dtDeadLineSDInicio);

                            }
                            if (dtDeadLineSDFim != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_AVERBADO <= dtDeadLineSDFim);

                            }
                        }
                        else if (filtro == 5)
                        {
                            if (dtDeadLineSDInicio != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_CANAL_VERMELHO >= dtDeadLineSDInicio);

                            }
                            if (dtDeadLineSDFim != null)
                            {
                                lstDUE = lstDUE.Where(x => x.DT_CANAL_VERMELHO <= dtDeadLineSDFim);

                            }
                        }
                        if (modo != 14 && modo != 6)
                            lstDUE = lstDUE.Where(x => x.DT_CANCELADO == null);
                    }
                }

                //OrderByDescending(p => p.DT_DUE).

                return lstDUE.Take(3000).ToList();
            }
            //var ListaProcessos = (from p in db.  
        }

        public List<camposDUE> retornaDUE(List<int> idDUE)
        {

            var lstDUE = (from due in db.DUE

                          join p in db.PROCESSOS on due.CD_PROCESSO equals p.CD_PROCESSO into leftp
                          from Orip in leftp.DefaultIfEmpty()
                          join cli in db.ENTIDADE on due.CD_EXPORTADOR equals cli.CD_ENTIDADE
                          join pr in db.PROCESSORESERVA on Orip.CD_PROCESSO equals pr.CD_PROCESSO into prLeft
                          from prOri in prLeft.DefaultIfEmpty()
                          join res in db.RESERVAS on prOri.CD_RESERVA equals res.CD_RESERVA into resLeft
                          from resOri in resLeft.DefaultIfEmpty()
                          join via in db.VIAGEMARMADOR on resOri.CD_VIAGEM_ARMADOR equals via.CD_VIAGEM_ARMADOR into viaLeft
                          from viaOri in viaLeft.DefaultIfEmpty()
                          join viagem in db.VIAGEM on viaOri.CD_VIAGEM equals viagem.CD_VIAGEM into viagemLeft
                          from viagemOri in viagemLeft.DefaultIfEmpty()
                          join nav in db.NAVIOS on viagemOri.CD_NAVIO equals nav.CD_NAVIO into navLeft
                          from navOri in navLeft.DefaultIfEmpty()
                          join recintoDespacho in db.RECIALFA on due.CD_RECINTO_DESPACHO equals recintoDespacho.STR_CODIGORECIALFA into recintoDespachoLeft
                          from recintoDespachoOri in recintoDespachoLeft.DefaultIfEmpty()
                          join recintoEmbarque in db.RECIALFA on due.CD_RECINTO_EMBARQUE equals recintoEmbarque.STR_CODIGORECIALFA into recintoEmbarqueLeft
                          from recintoEmbarqueOri in recintoEmbarqueLeft.DefaultIfEmpty()

                          join formaExport in db.FORMA_EXPORTACAO_DUE on due.CD_FORMA_EXPORTACAO equals formaExport.CD_FORMA_EXPORTACAO
                          where idDUE.Contains(due.CD_DUE)
                          select new camposDUE
                               {
                                   CD_DUE = due.CD_DUE,
                                   CD_PROCESSO = due.CD_PROCESSO,
                                   CD_PROCESSORESERVA = prOri.CD_PROCESSORESERVA,
                                   NR_PROCESSO = Orip.CD_NUMERO_PROCESSO != null ? "E-" + Orip.CD_NUMERO_PROCESSO.Substring(2, 6) + "/" + Orip.CD_NUMERO_PROCESSO.Substring(0, 2) : "",

                                   SG_TIPO_DUE = due.SG_TIPO_DUE,
                                   CD_NAVIO = navOri.CD_NAVIO,
                                   //CD_DESTINO = paisDestinoOri.CD_PAIS,
                                   DS_NAVIO = navOri.NM_NAVIO,
                                   DS_REFERENCIA = due.DS_REFERENCIA,
                                   DT_DEADLINE_CARGA = viaOri.DT_DEAD_LINE_CONTAINER,
                                   DT_ETA = viagemOri.DT_ETA,
                                   DT_DEADLINE_DRAFT = viaOri.DT_DEAD_LINE_DRAFT,

                                   //DS_TERMINAL_ATRACACAO = termOri.NM_FANTASIA_ENTIDADE,
                                   DS_BOOKING = resOri.CD_NUMERO_RESERVA,
                                   //DS_ORIGEM = portolocalOri.NM_CIDADE,
                                   //CD_ORIGEM = portolocalOri.CD_LOCAL,
                                   //DS_DESTINO = paisDestinoOri.STR_NOMEPAIS,
                                   //DS_ARMADOR = armOri.NM_FANTASIA_ENTIDADE,
                                   CD_NUMERO_DUE = due.CD_NUMERO_DUE,
                                   CD_NUMERO_RUC = due.CD_NUMERO_RUC,
                                   DT_DUE = due.DT_DUE,
                                   DT_TRANSMISSAO = due.DT_TRANSMISSAO,
                                   CD_USUARIO_DUE = due.CD_USUARIO_DUE,
                                   CD_USUARIO_TRANSMISSAO = due.CD_USUARIO_TRANSMISSAO,
                                   DT_DESEMBARACO = due.DT_DESEMBARACO == null ? due.DT_DESEMBARACO : due.DT_DESEMBARACO,
                                    //NR_CONTAINER = Orip.CD_PROCESSO != null ? FN_RETORNA_CONTAINER_PROCESSO_DUE(Orip.CD_PROCESSO) : "",
                                    NR_CONTAINER = FN_RETORNA_CONTAINER_PROCESSO_DUE(Orip.CD_PROCESSO),  
                                    STR_CODIGOMOEDA = due.STR_CODIGOMOEDA,
                                   DS_SITUACAO = due.DS_SITUACAO,
                                   CD_EXPORTADOR = due.CD_EXPORTADOR,
                                   DS_EXPORTADOR = cli.NM_FANTASIA_ENTIDADE,
                                   NR_CNPJ = due.NR_CNPJ == null ? cli.CD_CNPJ_ENTIDADE : due.NR_CNPJ,
                                   CD_FORMA_EXPORTACAO = due.CD_FORMA_EXPORTACAO,
                                   NM_FORMA_EXPORTACAO = formaExport.DS_FORMA_EXPORTACAO,
                                   STR_CODIGOUNIDADERF_DESPACHO = due.STR_CODIGOUNIDADERF_DESPACHO,
                                   CD_RECINTO_DESPACHO = due.CD_RECINTO_DESPACHO,
                                   IC_RECINTO_ADUANEIRO_DESPACHO = due.IC_RECINTO_ADUANEIRO_DESPACHO,

                                   STR_CODIGOUNIDADERF_EMBARQUE = due.STR_CODIGOUNIDADERF_EMBARQUE,
                                   CD_RECINTO_EMBARQUE = due.CD_RECINTO_EMBARQUE,

                                   IC_RECINTO_ADUANEIRO_EMBARQUE = due.IC_RECINTO_ADUANEIRO_EMBARQUE,
                                   IC_EMBUTIDO = due.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                                   DS_REFERENCIA_ENDERECO = due.DS_REFERENCIA_ENDERECO,
                                   DS_INFORMACOES_COMPLEMENTARES = due.DS_INFORMACOES_COMPLEMENTARES,
                                   CD_VIA_TRANSPORTE_ESPECIAL = due.CD_VIA_TRANSPORTE_ESPECIAL,
                                   VL_FRETE_INTERNACIONAL = due.VL_FRETE_INTERNACIONAL,

                                   VL_DESCONTO = due.VL_DESCONTO,
                                   IC_STATUS_PARA_ENVIO_TRANSMISSAO = due.IC_STATUS_PARA_ENVIO_TRANSMISSAO,
                                   SG_FRETE_INTERNACIONAL_EMBUTIDO = due.SG_FRETE_INTERNACIONAL_EMBUTIDO,
                                   SG_SEGURO_INTERNACIONAL_EMBUTIDO = due.SG_SEGURO_INTERNACIONAL_EMBUTIDO,
                                   VL_SEGURO = due.VL_SEGURO,
                                   //NM_DESPACHO = localDespachoOri.STR_CODIGOUNIDADERF + " - " + localDespachoOri.STR_NOMEUNIDADERF,
                                   //NM_EMBARQUE = localEmbarqueOri.STR_CODIGOUNIDADERF + " - " + localEmbarqueOri.STR_NOMEUNIDADERF,
                                   NM_RECINTO_DESPACHO = recintoDespachoOri.STR_CODIGORECIALFA + " - " + recintoDespachoOri.STR_DESCRICAO,
                                   NM_RECINTO_EMBARQUE = recintoEmbarqueOri.STR_CODIGORECIALFA + " - " + recintoEmbarqueOri.STR_DESCRICAO,
                                   //NM_USUARIO = usuOri.NM_USUARIO,
                                   //NM_USUARIO_TRANSMISSAO = usutOri.NM_USUARIO,
                                   //NM_USUARIO_RETIFICACAO = usuROri.NM_USUARIO,
                                   DS_ARQUIVO_XML = due.DS_ARQUIVO_XML,
                                   DT_CANCELADO = due.DT_CANCELADO,
                                   IC_BLOQUEIO = due.IC_BLOQUEIO,
                                   IC_ADM = due.IC_CONTROLE_ADM,
                                   IC_SITUACAO_CARGA = due.IC_SITUACAO_CARGA,
                                   //DS_STATUS_EMBARQUE2 = status2Ori.NM_STATUS_PROCESSO,
                                   //CD_STATUS_EMBARQUE2 = prcOri.CD_STATUS_CONTAINER2,
                                   DT_ENTREGA_DESPACHO = due.DT_ENTREGA_DESPACHO,
                                   STR_CODIGOVIATRANSPORTE = Orip.STR_CODIGOVIATRANSPORTE,
                                   NR_CHAVE_NF = due.NR_CHAVE_NF

                               });
            return lstDUE.ToList();

        }


        public string GetTokenBTP()
        {
            return  (from pa in db.PARAMETRO_WEB
                         where pa.CD_PARAMETRO_WEB == 1
                         select pa.VR_PARAMETRO_WEB
                         ).FirstOrDefault();





        }

    }
}