using it.valuelab.hedgeinvest.KIID.model;
using it.valuelab.hedgeinvest.KIID.service;
using KIID_Frontend.classi;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KIID_Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string templatePath;
        private string _template;
        private string _datafile;
        private string outputfolder;
        private string language;
        private DateTime datagenerazione;

        private KIIDService service;


        private string template
        {
            get
            {
                return _template;
            }
            set
            {
                _template = value;
                chkDati();
            }

        }
        private string datafile
        {
            get
            {
                return _datafile;
            }
            set
            {
                _datafile = value;
                chkDati();
            }

        }

        private void chkDati()
        {
            if (!string.IsNullOrEmpty(_template) & !string.IsNullOrEmpty(_datafile))
            {
                btnEsegui.IsEnabled = true;
            }
            else
            {
                btnEsegui.IsEnabled = false;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            FiledatiXML datiFissi = new FiledatiXML("datifissi.xml");
            if (datiFissi.fileTrovato)
            {
                datiFissi.valoreCercato = "OutputFolder";
                outputfolder = datiFissi.result;
                datiFissi.valoreCercato = "PathTemplate";
                templatePath = datiFissi.result;
            }
            else
            {
                errDatiFissi();
            }
            ProgressBar1.Visibility = Visibility.Hidden;
            lblError.Visibility = Visibility.Hidden;
            txtError.DataContext = service;
            lblError.DataContext = service;

            
            XmlDataProvider lingueXml = new XmlDataProvider();
            lingueXml.Source = new Uri(@"lingue.xml",UriKind.Relative);
            lingueXml.XPath = "Lingue";        
            Binding bind = new Binding();
            bind.Source = lingueXml;
            bind.XPath = "./Lingua";
            cboLingua.SetBinding(ComboBox.ItemsSourceProperty, bind);
            lingueXml.Refresh();

        }

        private void BtnScegliWord(object sender, RoutedEventArgs e)
        {
            // string appPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "TEMPLATE";
            lblError.Visibility = Visibility.Hidden;
            ScegliDocumento doc = new ScegliDocumentoWord(templatePath);
            template = doc.path;
            lblTemplate.Content = template.Substring(template.LastIndexOf(@"\") + 1);

        }
        private void BtnScegliExcel(object sender, RoutedEventArgs e)
        {
            // string appPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "INPUT";
            lblError.Visibility = Visibility.Hidden;
            ScegliDocumento doc = new ScegliDocumentoExcel(templatePath);
            datafile = doc.path;
            lblDataFile.Content = datafile.Substring(datafile.LastIndexOf(@"\") + 1);
        }
        private async void BtnEseguiClick(object sender, RoutedEventArgs e)
        {

            ScegliDirectory doc = new ScegliDirectory(outputfolder);

            if (!string.IsNullOrEmpty(doc.path))
            {
                outputfolder = doc.path;
                language = cboLingua.SelectedValue.ToString();
                datagenerazione = (DateTime)datePick.SelectedDate;

                service = new KIIDService(template, datafile, outputfolder, language, datagenerazione);
                txtError.DataContext = service;
                ProgressBar1.DataContext = service;
                ProgressBar1.Visibility = Visibility.Visible;
                ProgressBar1.Value = 0.05;
                service.error = ""; //<-- è necessario settare una variabile per attivare il binding del DataConext
                await EseguiService();
                ProgressBar1.Visibility = Visibility.Hidden;
                if (string.IsNullOrEmpty(service.error))
                {
                    MessageBox.Show(string.Format("I file sono stati generati correttamente e salvati in {0}", outputfolder));
                    SaveDefaultPaths(); //memorizzo il percorso di salvataggio dei file  
                }
                else
                {
                    lblError.Visibility = Visibility.Visible;
                }


            }
            else
            {
                MessageBox.Show("Operazione annullata");
            }
        }
        private async Task EseguiService()
        {
            await Task.Run(() =>
               {
                   service.GenerateKIID();
               });
        }



        private void SaveDefaultPaths()
        {
            FiledatiXML datiFissi = new FiledatiXML("datifissi.xml");
            if (datiFissi.fileTrovato)
            {
                datiFissi.SetValore("OutputFolder", outputfolder);
                templatePath = template.Substring(0, template.LastIndexOf("\\"));
                datiFissi.SetValore("PathTemplate", templatePath);
                datiFissi.SaveFile();
            }

        }

        private void ButtonDatiFissi_Click(object sender, RoutedEventArgs e)
        {
            FiledatiXML datiFissi = new FiledatiXML("datifissi.xml");
            if (datiFissi.fileTrovato)
            {
                Dictionary<string, string> elenco = datiFissi.GetElencoValori();
                StringBuilder sb = new StringBuilder();
                foreach (var item in elenco)
                {
                    sb.Append(item.Key);
                    sb.Append(" = ");
                    sb.Append(item.Value);
                    sb.Append(Environment.NewLine);
                }
                MessageBox.Show(sb.ToString(), "Dati Fissi");
            }
            else
            {
                errDatiFissi();
            }
        }

        private void errDatiFissi()
        {
            MessageBox.Show(string.Format("Non trovo il file 'datifissi.xml' nella directory '{0}'", AppDomain.CurrentDomain.BaseDirectory.ToString()));
        }

        private void ButtonEsci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (System.Xml.XmlElement item in cboLingua.Items)
            {
                sb.Append( item.Attributes["Name"].Value.ToString());
                sb.Append( " ; ");
                sb.Append( item.Attributes["Value"].Value.ToString());
                sb.Append( Environment.NewLine);
            }
            MessageBox.Show(sb.ToString(),"ComboBox Lingue",MessageBoxButton.OK);
        }

        private void cboLingua_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboLingua.SelectedIndex==-1)
            {
                cboLingua.SelectedIndex = 0;
            }
        }

    }
}
