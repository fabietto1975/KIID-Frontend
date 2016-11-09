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
        private string _template;
        private string _datafile;
        private string outputfolder;
        private string language;
        private DateTime datagenerazione;


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
        }

        private void BtnScegliWord(object sender, RoutedEventArgs e)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "TEMPLATE";
            ScegliDocumento doc = new ScegliDocumentoWord(appPath);
            template = doc.path;
            lblTemplate.Content = template.Substring(template.LastIndexOf(@"\") + 1);
        }
        private void BtnScegliExcel(object sender, RoutedEventArgs e)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "INPUT";
            ScegliDocumento doc = new ScegliDocumentoExcel(appPath);
            datafile = doc.path;
            lblDataFile.Content = datafile.Substring(datafile.LastIndexOf(@"\") + 1);
        }
        private void BtnEseguiClick(object sender, RoutedEventArgs e)
        {
            ScegliDirectory doc = new ScegliDirectory();
            outputfolder = doc.path;
            language = cboLingua.SelectedItem.ToString();
            datagenerazione = (DateTime)datePick.SelectedDate;

            
            KIIDService service = new KIIDService(template, datafile, outputfolder, language, datagenerazione);
            List<KIIDData> kiidDataList = service.readFundsData();
            foreach (KIIDData kiiddata in kiidDataList)
            {
                service.generateOutput(kiiddata);                
            }
        }
    }
}
