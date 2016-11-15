using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace KIID_Frontend.classi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    class ScegliDocumento
    {
        public string path;
        public ScegliDocumento()
        {
            path = scegliDocumento("");
        }
        public ScegliDocumento(string defaultPath)
        {
            path = scegliDocumento(defaultPath);
        }
        public string scegliDocumento(string defaultPath)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (Directory.Exists(defaultPath))
            {
                dlg.InitialDirectory = defaultPath;
            }


            Nullable<bool> result = specifico(defaultPath, dlg);

            if (result == true)
            {
                return dlg.FileName;
            }
            else
            {
                return string.Empty;
            }

        }

        internal virtual bool? specifico(string defaultPath, Microsoft.Win32.OpenFileDialog dlg)
        {
            return true;
        }

    }

    class ScegliDocumentoPowerPoint : ScegliDocumento
    {
        public ScegliDocumentoPowerPoint()
        {

        }
        public ScegliDocumentoPowerPoint(string defaultPath)
            : base(defaultPath)
        {

        }
        internal override bool? specifico(string defaultPath, Microsoft.Win32.OpenFileDialog dlg)
        {
            dlg.DefaultExt = ".pptx";
            dlg.FileName = defaultPath;
            dlg.Filter = "documento PowerPoint (.pptx)|*.pptx";

            Nullable<bool> result = dlg.ShowDialog();
            return result;
        }
    }
    class ScegliDocumentoExcel : ScegliDocumento
    {
        public ScegliDocumentoExcel()
        {

        }
        public ScegliDocumentoExcel(string defaultPath)
            : base(defaultPath)
        {

        }
        internal override bool? specifico(string defaultPath, Microsoft.Win32.OpenFileDialog dlg)
        {
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "documento Excel (.xlsx)|*.xlsx";

            Nullable<bool> result = dlg.ShowDialog();
            return result;
        }

    }
    class ScegliDocumentoWord : ScegliDocumento
    {
        public ScegliDocumentoWord()
        {

        }
        public ScegliDocumentoWord(string defaultPath)
            : base(defaultPath)
        {

        }

        internal override bool? specifico(string defaultPath, Microsoft.Win32.OpenFileDialog dlg)
        {
            dlg.DefaultExt = ".docx";
            dlg.Filter = "documento Word (.docx)|*.docx";
            Nullable<bool> result = dlg.ShowDialog();
            return result;

        }


    }
    class ScegliDocumentoXML : ScegliDocumento
    {
        public ScegliDocumentoXML()
        {

        }
        public ScegliDocumentoXML(string defaultPath)
            : base(defaultPath)
        {

        }
        internal override bool? specifico(string defaultPath, Microsoft.Win32.OpenFileDialog dlg)
        {
            dlg.DefaultExt = ".xml";
            dlg.Filter = "documento xml (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
            return result;
        }
    }
    class ScegliDirectory
    {
        public string path;

        public ScegliDirectory()
        {
            path = sceltaDirectory("");

        }
        public ScegliDirectory(string _defaultPath)
        {
            path = sceltaDirectory(_defaultPath);

        }
        private string sceltaDirectory(string defaultPath)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = defaultPath;
            DialogResult result = fbd.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                return fbd.SelectedPath;
            }
            else
            {
                return string.Empty;
            }


        }


    }
}

