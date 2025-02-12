using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

/// <summary>
/// Summary description for clsExcel
/// </summary>
public class clsExcel
{
    #region Propriedades
    Excel.Application xlApp;
    Excel.Workbook xlWorkBook;
    Excel.Worksheet xlWorkSheet;
    object misValue = System.Reflection.Missing.Value;

    public string MensagemErro { get; set; }
    public int Linha { get; set; }
    public int Coluna { get; set; }
    public int PrimeiraLinha { get; set; }
    public int PrimeiraColuna { get; set; }
    public int UltimaLinha { get; set; }
    public int UltimaColuna { get; set; }
    public String PrimeiraCelula { get; set; }
    public String UltimaCelula { get; set; }
    public int contador { get; set; }
    public int rowCount { get; set; }
    public int colCount { get; set; }

    public string[] alfabeto = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                                 "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                                 "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ",
                                 "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT",
                                 "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ",
                                 "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT",
                                 "BU", "BV", "BW", "BX", "BY", "BZ"};
    #endregion

    public clsExcel()
    {
        xlApp = new Excel.Application();
        xlWorkBook = xlApp.Workbooks.Add(misValue);
        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
    }

    public clsExcel(string caminho)
    {

        Excel.Application xlApp = new Excel.Application();
        xlWorkBook = xlApp.Workbooks.Open(@caminho, 0, false, 5,
            "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, null, null);
        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
    }

    public void FecharExcel()
    {
        xlWorkBook.Close(null, null, null);

        //xlWorkBook.Close(null, null, null);
        //xlApp.Quit();

        Marshal.ReleaseComObject(xlWorkSheet);
        Marshal.ReleaseComObject(xlWorkBook);
        //Marshal.ReleaseComObject(xlApp);

        //MataExcelPerdido();
        //fim alteracao marcus
    }
    public void MataExcelPerdido()
    // cridado por marcus 28/09/2010
    {
        Process[] AllProcesses = Process.GetProcessesByName("EXCEL");

        foreach (Process ExcelProcess in AllProcesses)
        {
            //DateTime dt_atu = DateTime.Now;
            //DateTime dt_processo = ExcelProcess.StartTime;
            //TimeSpan TS = dt_atu.Subtract(dt_processo);
            //if (TS.Hours >= 1)
            ExcelProcess.Kill();
        }
    }
    public void EscreverCelula(int l, int c, string s)
    {
        xlWorkSheet.Cells[l, c] = s;

    }

    public string LerCelula(int l, int c)
    {
        return xlWorkSheet.Cells[l, c].Value;

    }

    public void CountLinhaColuna()
    {
        //rowCount = xlWorkSheet.Rows.Count;
        rowCount = xlWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
        //colCount = xlWorkSheet.Columns.Count;
        colCount = 5;

    }

    public void FormatarFonteEstilo(string celula1, string celula2, bool negrito, bool italico, bool sublinhado)
    {
        if (negrito)
            xlWorkSheet.get_Range(celula1, celula2).Font.Bold = true;

        if (italico)
            xlWorkSheet.get_Range(celula1, celula2).Font.Italic = true;

        if (sublinhado)
            xlWorkSheet.get_Range(celula1, celula2).Font.Underline = true;
    }

    public void FormatarTipoCelulaTexto(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).NumberFormat = "@";
    }

    public void FormatarTipoCelulaNumero(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).NumberFormat = "0";

    }

    public void escreveNumero(string celula1, string celula2, string conteudo)
    {
        xlWorkSheet.get_Range(celula1, celula2).FormulaR1C1 = conteudo;
    }
    public void FormatarTipoCelulaDataHora(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).NumberFormat = "dd/mm/yyyy HH:mm";
    }

    public void FormatarTipoCelulaData(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).NumberFormat = "m/d/yyyy";
    }

    public void FormatarTipoCelulaValores(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).NumberFormat = "#,#0.0";
    }

    public void FormatarTipoCelulaValores2(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).NumberFormat = "###.##0,00";
    }

    public void FormatarTipoCelulaValores3(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).NumberFormat = "###.##0,000";
    }

    public void FormatarTipoCelulaValores(string tipo, string celula1, string celula2)
    {
        if (tipo == "R$")
        {
            xlWorkSheet.get_Range(celula1, celula2).NumberFormat = "$ #,##0.00";
        }
        else
        {
            xlWorkSheet.get_Range(celula1, celula2).NumberFormat = "[$$-409]#,##0.00";
        }
    }
    public void AutoAjustarColunas(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).EntireColumn.AutoFit();
    }

    public void FormatarFonteCor(string celula1, string celula2, System.Drawing.Color corfonte, System.Drawing.Color corfundo)
    {
        xlWorkSheet.get_Range(celula1, celula2).Font.Color = System.Drawing.ColorTranslator.ToOle(corfonte);
        xlWorkSheet.get_Range(celula1, celula2).Interior.Color = System.Drawing.ColorTranslator.ToOle(corfundo);
    }

    public void MesclarCelulas(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).Merge(misValue);
    }

    public void Alinhamento(string celula1, string celula2, string alinhamento)
    {
        if (alinhamento == "centro")
        {
            xlWorkSheet.get_Range(celula1, celula2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            xlWorkSheet.get_Range(celula1, celula2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }
        else
        {
            if (alinhamento == "baixo")
            {
                xlWorkSheet.get_Range(celula1, celula2).VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
            }
            else
            {
                if (alinhamento == "topo")
                {
                    xlWorkSheet.get_Range(celula1, celula2).VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                }
                else
                {
                    if (alinhamento == "justificado")
                    {
                        xlWorkSheet.get_Range(celula1, celula2).VerticalAlignment = Excel.XlVAlign.xlVAlignJustify;
                        xlWorkSheet.get_Range(celula1, celula2).HorizontalAlignment = Excel.XlHAlign.xlHAlignJustify;
                    }
                    else
                    {
                        if (alinhamento == "direita")
                        {
                            xlWorkSheet.get_Range(celula1, celula2).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        }
                        else
                        {
                            if (alinhamento == "esquerda")
                                xlWorkSheet.get_Range(celula1, celula2).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        }
                    }

                }
            }
        }
    }

    public void FormatarFonteTamanho(string celula1, string celula2, int tamanho)
    {
        xlWorkSheet.get_Range(celula1, celula2).Font.Size = tamanho;
    }

    public void FormatarBorda(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
    }

    public void FormatarFundo(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
    }

    public void FormatarFundoRed(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
    }

    public void FormatarFundoOrange(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);
    }

    public void FormatarFundoBlue(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
    }

    public void FormatarFundoGreen(string celula1, string celula2)
    {
        xlWorkSheet.get_Range(celula1, celula2).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
    }


    /*
    public void InserirImagem(string caminho, float esquerda, float topo, float largura, float altura) {
        xlWorkSheet.Shapes.AddPicture(caminho, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, esquerda, topo, largura, altura);
    }
    */
    public void AlterarLargura(string celula1, string celula2, float largura)
    {
        xlWorkSheet.get_Range(celula1, celula2).ColumnWidth = largura;
    }

    public void AlterarAltura(string celula1, string celula2, float altura)
    {
        xlWorkSheet.get_Range(celula1, celula2).RowHeight = altura;
    }

    public void LarguraAuto()
    {
        xlWorkSheet.Columns.AutoFit();
    }

    public void salvar()
    {
        xlWorkBook.Save();

    }
    public void salvarComo(string caminho)
    {
        xlWorkBook.SaveAs(caminho, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                 Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value,
                 Missing.Value, Missing.Value, Missing.Value);

    }

    public void SalvarComoDownload(string caminho)
    {
        xlApp.DisplayAlerts = false;
        xlApp.ActiveWorkbook.SaveAs(caminho
                                  , Excel.XlFileFormat.xlWorkbookNormal
                                  , misValue
                                  , misValue
                                  , misValue
                                  , misValue
                                  , Excel.XlSaveAsAccessMode.xlExclusive
                                  , misValue
                                  , misValue
                                  , misValue
                                  , misValue
                                  , misValue);

        // Pega o arquivo temporario de documento para download.
        FileInfo fInfo = new FileInfo(caminho);
        /*
        // Limpa o conteúdo de saída atual do buffer
        HttpContext.Current.Response.Clear();
        //Adicionando charset em português
        HttpContext.Current.Response.Charset = "UTF-8";
        //Adiciona um cabeçalho que especifica o nome default para a caixa de diálogos Salvar Como...
        HttpContext.Current.Response.ContentType = "application/msexcel";
        //Response.ContentType = "application/msword";
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fInfo.Name + "\"");
        //Adiciona ao cabeçalho o tamanho do arquivo para que o browser possa exibir o progresso do download
        HttpContext.Current.Response.AddHeader("Content-Length", fInfo.Length.ToString());

        xlWorkBook.Close(true, misValue, misValue);
        HttpContext.Current.Response.WriteFile(fInfo.FullName);
        HttpContext.Current.Response.Flush();
        */
        if (fInfo.Exists)
            fInfo.Delete();

        fInfo = null;

        xlApp.Quit();

        releaseObject(xlWorkSheet);
        releaseObject(xlWorkBook);
        releaseObject(xlApp);
    }


    public void SalvarComoDownload2(string caminho)
    {
        //xlApp.DisplayAlerts = false;
        xlWorkBook.Save();
        FileInfo fInfo = new FileInfo(caminho);

        /*
        // Limpa o conteúdo de saída atual do buffer
        HttpContext.Current.Response.Clear();
        //Adicionando charset em português
        HttpContext.Current.Response.Charset = "UTF-8";
        //Adiciona um cabeçalho que especifica o nome default para a caixa de diálogos Salvar Como...
        HttpContext.Current.Response.ContentType = "application/msexcel";
        //Response.ContentType = "application/msword";
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fInfo.Name + "\"");
        //Adiciona ao cabeçalho o tamanho do arquivo para que o browser possa exibir o progresso do download
        HttpContext.Current.Response.AddHeader("Content-Length", fInfo.Length.ToString());

        xlWorkBook.Close(true, misValue, misValue);
        HttpContext.Current.Response.WriteFile(fInfo.FullName);
        HttpContext.Current.Response.Flush();
        */




        //xlApp.Quit();

        releaseObject(xlWorkSheet);
        releaseObject(xlWorkBook);
        releaseObject(xlApp);

        if (fInfo.Exists)
            fInfo.Delete();
        fInfo = null;
    }

    private void releaseObject(object obj)
    {
        try
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            obj = null;
        }
        catch 
        {
            obj = null;
            //Response.Write("Exception Occured while releasing object " + ex.ToString());
        }
        finally
        {
            GC.Collect();
        }
    }
}