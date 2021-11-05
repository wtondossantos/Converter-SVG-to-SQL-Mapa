using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace ConverterSVGtoSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            var caminho = @"G:\OneDrive\Projetos\Mapa do Brasil\programa_conversor\";

            string FileToRead = caminho + "mapabrasil30-sem-nomes.xml";
            string FileToRead2 = caminho + "cidades.txt";

            var id_estado = string.Empty;
            var id_macro = string.Empty;
            var id_ilha = string.Empty;
            var id_micro = string.Empty;
            var itens = new List<Desenho>();

            XmlTextReader xmlReader = new XmlTextReader(FileToRead);
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        var item = new Desenho();

                        while (xmlReader.MoveToNextAttribute())
                        {
                            if (xmlReader.Name == "id")
                            {
                                if (xmlReader.Value.Contains("brasil_g"))
                                {
                                }
                                else if (xmlReader.Value.Contains("_estado_g"))
                                {
                                    id_estado = xmlReader.Value.Substring(0, xmlReader.Value.Length - 2);
                                }
                                else if (xmlReader.Value.Contains("_macro_g"))
                                {
                                    id_macro = xmlReader.Value.Substring(0, xmlReader.Value.Length - 2);
                                }
                                else if (xmlReader.Value.Contains("_micro_g"))
                                {
                                    id_micro = xmlReader.Value.Substring(0, xmlReader.Value.Length - 2);
                                }
                                else if (xmlReader.Value.Contains("_ilha_gr") && xmlReader.Value.Contains("_micro"))
                                {
                                    item.Id = xmlReader.Value;
                                    item.IdPai = id_macro;
                                    item.Nome = xmlReader.Value.Substring(3, xmlReader.Value.Length - 9);
                                }
                                else if (xmlReader.Value.Contains("_ilha_gr"))
                                {
                                    item.Id = xmlReader.Value;
                                    item.IdPai = id_micro;
                                    item.Nome = xmlReader.Value.Substring(3, xmlReader.Value.Length - 3);
                                }
                                else if (xmlReader.Value.Contains("_estado"))
                                {
                                    item.Id = xmlReader.Value;
                                    item.IdPai = "brasil";
                                    item.Nome = xmlReader.Value.Substring(3, xmlReader.Value.Length - 10);

                                    id_estado = xmlReader.Value;
                                }
                                else if (xmlReader.Value.Contains("_macro"))
                                {
                                    item.Id = xmlReader.Value;
                                    item.IdPai = id_estado;
                                    item.Nome = xmlReader.Value.Substring(3, xmlReader.Value.Length - 9);

                                    id_macro = xmlReader.Value;
                                }
                                else if (xmlReader.Value.Contains("_micro"))
                                {
                                    item.Id = xmlReader.Value;
                                    item.IdPai = id_macro;
                                    item.Nome = xmlReader.Value.Substring(3, xmlReader.Value.Length - 9);

                                    id_micro = xmlReader.Value;
                                }
                                else if (xmlReader.Value.Contains("_ilha_g"))
                                {
                                    item.Id = xmlReader.Value.Substring(0, xmlReader.Value.Length - 2);
                                    item.IdPai = id_macro;
                                    item.Nome = xmlReader.Value.Substring(3, xmlReader.Value.Length - 10);

                                    id_ilha = item.Id;
                                }
                                else if (xmlReader.Value.Contains("_composto_g"))
                                {

                                }
                                else
                                {
                                    item.Id = xmlReader.Value;
                                    item.IdPai = string.IsNullOrEmpty(id_ilha) ? id_micro : id_ilha;
                                    if (xmlReader.Value.Contains("_interno") || xmlReader.Value.Contains("_externo"))
                                        item.Nome = xmlReader.Value.Substring(3, xmlReader.Value.Length - 11);
                                    else
                                        item.Nome = xmlReader.Value.Substring(3, xmlReader.Value.Length - 3);
                                }
                            }
                            else if (xmlReader.Name == "d")
                            {
                                item.Shape = xmlReader.Value.Replace("\t","").Replace("\n","").Replace("\r","");
                            }
                        }
                        if (!string.IsNullOrEmpty(item.Id))
                        {
                            itens.Add(item);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        id_ilha = string.Empty;
                        break;
                }
            }

            var municipios = new List<Desenho>();
            var count = 0;

            foreach (var item in itens.OrderBy(x => x.Id))
            {
                count++;
                item.Index = count;
                municipios.Add(item);
            }

            IList<string> lines = File.ReadLines(FileToRead2).ToList();
            var cidades = new List<Desenho>();
            var nome = string.Empty;
            count = 0;
            var contarShapeEstado = 0;
            var contarShapeMesorregiao = 0;
            var contarShapeMicrorregiao = 0;
            var contarShapeMunicipio = 0;
            var contarShapeIlha = 0;
            var contarNome = 0;
            var contarId = 0;


            var sw2 = new StreamWriter(caminho + "RegioesFora.txt");
            var sw3 = new StreamWriter(caminho + "CidadesComparar.txt");


            var ilhas = new StreamWriter(caminho + "ilhas.txt");
            var estados = new StreamWriter(caminho + "estados.txt");
            var mesorregioes = new StreamWriter(caminho + "mesorregioes.txt");
            var ac_micro = new StreamWriter(caminho + "ac_micro.txt");
            var al_micro = new StreamWriter(caminho + "al_micro.txt");
            var am_micro = new StreamWriter(caminho + "am_micro.txt");
            var ap_micro = new StreamWriter(caminho + "ap_micro.txt");
            var ac_cidade = new StreamWriter(caminho + "ac_cidade.txt");
            var al_cidade = new StreamWriter(caminho + "al_cidade.txt");
            var am_cidade = new StreamWriter(caminho + "am_cidade.txt");
            var ap_cidade = new StreamWriter(caminho + "ap_cidade.txt");
            var ba_micro = new StreamWriter(caminho + "ba_micro.txt");
            var ba_cidade = new StreamWriter(caminho + "ba_cidade.txt");
            var ce_micro = new StreamWriter(caminho + "ce_micro.txt");
            var ce_cidade = new StreamWriter(caminho + "ce_cidade.txt");
            var df_micro = new StreamWriter(caminho + "df_micro.txt");
            var df_cidade = new StreamWriter(caminho + "df_cidade.txt");
            var es_micro = new StreamWriter(caminho + "es_micro.txt");
            var es_cidade = new StreamWriter(caminho + "es_cidade.txt");
            var go_micro = new StreamWriter(caminho + "go_micro.txt");
            var go_cidade = new StreamWriter(caminho + "go_cidade.txt");
            var ma_micro = new StreamWriter(caminho + "ma_micro.txt");
            var ma_cidade = new StreamWriter(caminho + "ma_cidade.txt");
            var mg_micro = new StreamWriter(caminho + "mg_micro.txt");
            var mg_cidade = new StreamWriter(caminho + "mg_cidade.txt");
            var ms_micro = new StreamWriter(caminho + "ms_micro.txt");
            var ms_cidade = new StreamWriter(caminho + "ms_cidade.txt");
            var mt_micro = new StreamWriter(caminho + "mt_micro.txt");
            var mt_cidade = new StreamWriter(caminho + "mt_cidade.txt");
            var pa_micro = new StreamWriter(caminho + "pa_micro.txt");
            var pa_cidade = new StreamWriter(caminho + "pa_cidade.txt");
            var pb_micro = new StreamWriter(caminho + "pb_micro.txt");
            var pb_cidade = new StreamWriter(caminho + "pb_cidade.txt");
            var pe_micro = new StreamWriter(caminho + "pe_micro.txt");
            var pe_cidade = new StreamWriter(caminho + "pe_cidade.txt");
            var pi_micro = new StreamWriter(caminho + "pi_micro.txt");
            var pi_cidade = new StreamWriter(caminho + "pi_cidade.txt");
            var pr_micro = new StreamWriter(caminho + "pr_micro.txt");
            var pr_cidade = new StreamWriter(caminho + "pr_cidade.txt");
            var rj_micro = new StreamWriter(caminho + "rj_micro.txt");
            var rj_cidade = new StreamWriter(caminho + "rj_cidade.txt");
            var ro_micro = new StreamWriter(caminho + "ro_micro.txt");
            var ro_cidade = new StreamWriter(caminho + "ro_cidade.txt");
            var rn_micro = new StreamWriter(caminho + "rn_micro.txt");
            var rn_cidade = new StreamWriter(caminho + "rn_cidade.txt");
            var rr_micro = new StreamWriter(caminho + "rr_micro.txt");
            var rr_cidade = new StreamWriter(caminho + "rr_cidade.txt");
            var rs_micro = new StreamWriter(caminho + "rs_micro.txt");
            var rs_cidade = new StreamWriter(caminho + "rs_cidade.txt");
            var sc_micro = new StreamWriter(caminho + "sc_micro.txt");
            var sc_cidade = new StreamWriter(caminho + "sc_cidade.txt");
            var se_micro = new StreamWriter(caminho + "se_micro.txt");
            var se_cidade = new StreamWriter(caminho + "se_cidade.txt");
            var sp_micro = new StreamWriter(caminho + "sp_micro.txt");
            var sp_cidade = new StreamWriter(caminho + "sp_cidade.txt");
            var to_micro = new StreamWriter(caminho + "to_micro.txt");
            var to_cidade = new StreamWriter(caminho + "to_cidade.txt");


            foreach (var item in municipios.OrderBy(x => x.Id)) {

                if (count == item.Index - 1)
                {
                    foreach (var line in lines)
                    {
                        if (item.Id.Substring(0, 2).ToUpper() == line.Substring(line.Length - 2, 2).ToUpper())
                        {
                            nome = line.Substring(0, line.Length - 5);
                            if (item.Id.Substring(3, item.Nome.Replace("'", "").Replace("´", "").Replace("`", "").Length).Replace("_", "").ToUpper() == removerAcentos(nome).ToUpper())
                            {
                                cidades.Add(item);
                                item.Nome = nome.Replace("'","''");
                                Console.WriteLine(item.Id);

                                if (contarNome < item.Nome.Length)
                                    contarNome = item.Nome.Length;

                                if (contarId < item.Id.Length)
                                    contarId = item.Id.Length;

                                if (item.Id.Contains("brasil_g"))
                                {
                                    //sw.WriteLine(string.Format("INSERT INTO TbPais(Id, Nome) VALUES('bra_brasil', 'bra_brasil'); ", item.Id));
                                }
                                else if (item.Id.Contains("_ilha_gr") && xmlReader.Value.Contains("_micro"))
                                {
                                    if (contarShapeMicrorregiao < item.Shape.Length)
                                        contarShapeMicrorregiao = item.Shape.Length;

                                    switch (item.Id.Substring(0, 3))
                                    {
                                        case "ac_":
                                            ac_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "al_":
                                            al_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "am_":
                                            al_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ap_":
                                            ap_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ba_":
                                            ba_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ce_":
                                            ce_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "df_":
                                            df_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "es_":
                                            es_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "go_":
                                            go_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ma_":
                                            ma_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "mg_":
                                            mg_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ms_":
                                            ms_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "mt_":
                                            mt_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pa_":
                                            pa_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pb_":
                                            pb_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pe_":
                                            pe_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pi_":
                                            pi_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pr_":
                                            pr_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rj_":
                                            rj_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ro_":
                                            ro_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rn_":
                                            rn_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rr_":
                                            rr_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rs_":
                                            rs_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "sc_":
                                            sc_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "se_":
                                            se_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "sp_":
                                            sp_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "to_":
                                            to_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                    }
                                }
                                else if (item.Id.Contains("_ilha_gr") && !xmlReader.Value.Contains("_micro"))
                                {
                                    if (contarShapeMunicipio < item.Shape.Length)
                                        contarShapeMunicipio = item.Shape.Length;

                                    switch (item.Id.Substring(0, 3))
                                    {
                                        case "ac_":
                                            ac_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "al_":
                                            al_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "am_":
                                            am_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ap_":
                                            ap_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ba_":
                                            ba_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ce_":
                                            ce_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "df_":
                                            df_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "es_":
                                            es_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "go_":
                                            go_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ma_":
                                            ma_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "mg_":
                                            mg_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ms_":
                                            ms_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "mt_":
                                            mt_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pa_":
                                            pa_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pb_":
                                            pb_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pe_":
                                            pe_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pi_":
                                            pi_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pr_":
                                            pr_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rj_":
                                            rj_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ro_":
                                            ro_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rn_":
                                            rn_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rr_":
                                            rr_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rs_":
                                            rs_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "sc_":
                                            sc_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "se_":
                                            se_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "sp_":
                                            sp_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "to_":
                                            to_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                    }
                                }
                                else if (item.Id.Contains("_estado"))
                                {
                                    if (contarShapeEstado < item.Shape.Length)
                                        contarShapeEstado = item.Shape.Length;

                                    estados.WriteLine(string.Format("INSERT INTO TbEstado(Id, IdPais, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                }
                                else if (item.Id.Contains("_macro"))
                                {
                                    if (contarShapeMesorregiao < item.Shape.Length)
                                        contarShapeMesorregiao = item.Shape.Length;

                                    mesorregioes.WriteLine(string.Format("INSERT INTO TbMesorregiao(Id, IdEstado, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                }
                                else if (item.Id.Contains("_micro"))
                                {
                                    if (contarShapeMicrorregiao < item.Shape.Length)
                                        contarShapeMicrorregiao = item.Shape.Length;

                                    switch (item.Id.Substring(0, 3))
                                    {
                                        case "ac_":
                                            ac_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "al_":
                                            al_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "am_":
                                            al_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ap_":
                                            ap_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ba_":
                                            ba_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ce_":
                                            ce_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "df_":
                                            df_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "es_":
                                            es_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "go_":
                                            go_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ma_":
                                            ma_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "mg_":
                                            mg_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ms_":
                                            ms_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "mt_":
                                            mt_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pa_":
                                            pa_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pb_":
                                            pb_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pe_":
                                            pe_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pi_":
                                            pi_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pr_":
                                            pr_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rj_":
                                            rj_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ro_":
                                            ro_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rn_":
                                            rn_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rr_":
                                            rr_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rs_":
                                            rs_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "sc_":
                                            sc_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "se_":
                                            se_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "sp_":
                                            sp_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "to_":
                                            to_micro.WriteLine(string.Format("INSERT INTO TbMicrorregiao(Id, IdMesorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                    }
                                }
                                else if (item.Id.Contains("_arquipelago_"))
                                {
                                    ilhas.WriteLine(string.Format("INSERT INTO TbArquipelago(Id, IdMesorregiao, Nome) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                }
                                else if (item.IdPai.Contains("_arquipelago_"))
                                {
                                    if (contarShapeIlha < item.Shape.Length)
                                        contarShapeIlha = item.Shape.Length;

                                    ilhas.WriteLine(string.Format("INSERT INTO TbIlha(Id, IdArquipelago, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                }
                                else
                                {
                                    if (contarShapeMunicipio < item.Shape.Length)
                                        contarShapeMunicipio = item.Shape.Length;

                                    switch (item.Id.Substring(0, 3))
                                    {
                                        case "ac_":
                                            ac_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "al_":
                                            al_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "am_":
                                            am_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ap_":
                                            ap_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ba_":
                                            ba_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ce_":
                                            ce_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "df_":
                                            df_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "es_":
                                            es_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "go_":
                                            go_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ma_":
                                            ma_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "mg_":
                                            mg_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ms_":
                                            ms_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "mt_":
                                            mt_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pa_":
                                            pa_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pb_":
                                            pb_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pe_":
                                            pe_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pi_":
                                            pi_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "pr_":
                                            pr_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rj_":
                                            rj_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "ro_":
                                            ro_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rn_":
                                            rn_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rr_":
                                            rr_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "rs_":
                                            rs_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "sc_":
                                            sc_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "se_":
                                            se_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "sp_":
                                            sp_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                        case "to_":
                                            to_cidade.WriteLine(string.Format("INSERT INTO TbMunicipio(Id, IdMicrorregiao, Nome, Desenho) VALUES('{0}', '{1}', '{2}', '{3}'); ", item.Id, item.IdPai, item.Nome, item.Shape));
                                            break;
                                    }
                                }
                                sw3.WriteLine(line);
                                
                                break;
                            }
                        }
                    }
                    count++;
                }
            }

            foreach (var item in municipios.OrderBy(x => x.Id))
            {
                if (!cidades.Contains(item))
                {
                    sw2.WriteLine(string.Format("ID: {0}, ID_PAI: {1}, NOME: {2}", item.Id, item.IdPai, item.Nome));
                }
            }

            Console.WriteLine(string.Format("Estado: {0}, Macro: {1}, Micro: {2}, Municipio: {3}, Ilha: {4}, Nome: {5}, Id: {6}", contarShapeEstado, contarShapeMesorregiao, contarShapeMicrorregiao, contarShapeMunicipio, contarShapeIlha, contarNome, contarId));

            sw2.Close();
            sw3.Close();
            ilhas.Close();
            estados.Close();
            mesorregioes.Close();
            ac_micro.Close();
            al_micro.Close();
            am_micro.Close();  
            ap_micro.Close();
            ap_cidade.Close();
            ba_micro.Close();
            ba_cidade.Close();
            ce_micro.Close();
            ce_cidade.Close();
            df_micro.Close();
            df_cidade.Close();
            es_micro.Close();
            es_cidade.Close();
            go_micro.Close();
            go_cidade.Close();
            ma_micro.Close();
            ma_cidade.Close();
            mg_micro.Close();
            mg_cidade.Close();
            ms_micro.Close();
            ms_cidade.Close();
            mt_micro.Close();
            mt_cidade.Close();
            pa_micro.Close();
            pa_cidade.Close();
            pb_micro.Close();
            pb_cidade.Close();
            pe_micro.Close();
            pe_cidade.Close();
            pi_micro.Close();
            pi_cidade.Close();
            pr_micro.Close();
            pr_cidade.Close();
            rj_micro.Close();
            rj_cidade.Close();
            ro_micro.Close();
            ro_cidade.Close();
            rn_micro.Close();
            rn_cidade.Close();
            rr_micro.Close();
            rr_cidade.Close();
            rs_micro.Close();
            rs_cidade.Close();
            sc_micro.Close();
            sc_cidade.Close();
            se_micro.Close();
            se_cidade.Close();
            sp_micro.Close();
            sp_cidade.Close();
            to_micro.Close();
            to_cidade.Close();

        }
        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto.Replace("/", "").Replace("'","").Replace("´","").Replace("`","").Replace("-","").Replace(" ","");
        }
    }
}
