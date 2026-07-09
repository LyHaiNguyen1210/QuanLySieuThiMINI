using CrystalDecisions.CrystalReports.Engine;
using QuanLySieuThiMINI.Report;
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

namespace QuanLySieuThiMINI.View
{
    public partial class HoaDonReportView : Window
    {
        private string maHD;
        public HoaDonReportView(string maHD)
        {
            InitializeComponent();
            this.maHD = maHD; 
            LoadReport();
        }
        void LoadReport()
        {
            HoaDonCK rpt = new HoaDonCK(); 
            rpt.SetParameterValue("maHoaDon", maHD);
            rpt.SetDatabaseLogon("sa", "123", @"LAPTOP-HEL4EHQV", "QuanLySieuThiMINI");
            crvHoaDon.ViewerCore.ReportSource = rpt;
        }
    }
}
