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
        private string template;
        private string datafile;
        private string outputfolder;
        private string language;
        private DateTime datagenerazione;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnScegliWord(object sender, RoutedEventArgs e)
        {
            ScegliDocumento doc = new scegliDocumentoWord();
            template = doc.scegliDocumento();
            //MessageBox.Show(docTemplatePath);
        }
        private void BtnScegliExcel(object sender, RoutedEventArgs e)
        {
            ScegliDocumento doc = new scegliDocumentoExcel();
            datafile = doc.scegliDocumento();
            //MessageBox.Show(datafile);
        }
        private void BtnEseguiClick(object sender, RoutedEventArgs e)
        {
            ScegliDocumento doc = new scegliDirectory();
            outputfolder = doc.scegliDocumento();

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
