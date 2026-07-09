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
        /// <summary>
        /// Interaction logic for NhapHangReportView.xaml
        /// </summary>
        public partial class NhapHangReportView : Window
        {
        public NhapHangReportView()
        {
            InitializeComponent();
        }
        private string maPhieuNhap;
            public NhapHangReportView(string maPhieu)
            {
                InitializeComponent();
                this.maPhieuNhap = maPhieu;
                LoadReport();
            }
            void LoadReport()
            {
                NhapHangRP rpt = new NhapHangRP();
                rpt.SetParameterValue("LocMaPhieu", maPhieuNhap);
                rpt.SetDatabaseLogon("sa", "123", @"LAPTOP-HEL4EHQV", "QuanLySieuThiMini");
                rptNhapHang.ViewerCore.ReportSource = rpt;
            }
        }
    }
