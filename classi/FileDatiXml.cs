using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KIID_Frontend.classi
{
    class FiledatiXML
    {
        private string path;
        private string _valoreCercato;
        private XDocument xdoc;
        public string valoreCercato
        {
            get { return _valoreCercato; }
            set
            {
                _valoreCercato = value;
                result = GetValore();
            }
        }
        public string result;
        public bool fileTrovato;

        public FiledatiXML(string _path, string _value)
        {
            path = _path;
            OpenDocument();
            valoreCercato = _value;
        }
        public FiledatiXML(string _path)
        {
            path = _path;
            OpenDocument();
        }

        private void OpenDocument()
        {
            path = System.AppDomain.CurrentDomain.BaseDirectory + path;
            try
            {
                xdoc = XDocument.Load(path);
                fileTrovato = true;
            }
            catch (Exception)
            {

                fileTrovato = false; 
            }


        }
        private string GetValore()
        {
            try
            {
                var results = xdoc.Descendants(valoreCercato)
                                          .Select(p => p.Attribute("Value").Value);
                return results.FirstOrDefault().ToString();
            }
            catch (Exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(valoreCercato);
                sb.AppendFormat(" non trovato nel file '{0}", path);
                return sb.ToString();
            }

        }
        public void SetValore(string valoreDaCambiare, string nuovoValore)
        {
            var items = from item in xdoc.Descendants(valoreDaCambiare)
                        select item;

            foreach (XElement itemElement in items)
            {
                itemElement.SetAttributeValue("Value", nuovoValore);
            }
        }
        public void SaveFile()
        {
            xdoc.Save(path);
        }
        public Dictionary<string, string> GetElencoValori()
        {
            Dictionary<string, string> elencoNomi = new Dictionary<string, string>();
            try
            {
                var results = from p in xdoc.Elements("DatiFissi").Elements()
                              select p;
                foreach (var item in results)
                {
                    elencoNomi.Add(item.Attribute("Name").Value.ToString(), item.Attribute("Value").Value.ToString());
                }
            }
            catch (Exception ex)
            {
                elencoNomi.Add("Errore", ex.Message.ToString());
            }

            return elencoNomi;
        }
    }
}
