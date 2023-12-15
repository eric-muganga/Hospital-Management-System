using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace HospitalManagementSystem
{
    /// <summary>
    /// Interaction logic for DoctorNurseHome.xaml
    /// </summary>
    public partial class DoctorNurseHome : Window
    {
        Hospital hospital;
        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        ICollectionView view;
        public DoctorNurseHome(Hospital h)
        {
            InitializeComponent();
            this.hospital = h;
            ShowEmployees();
            view = CollectionViewSource.GetDefaultView(employeesList.ItemsSource);
        }

        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if (isDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                isDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                isDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowEmployees()
        {
            List<Employee> medicalStaffList = new List<Employee>();
            foreach (Employee employee in hospital.Employees)
            {
                if(employee is MedicalStaff)
                {
                    medicalStaffList.Add(employee);
                }
            }
            employeesList.ItemsSource = medicalStaffList;
        }

        private void employeesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MedicalStaff selectedMMedicalStaff = (MedicalStaff)employeesList.SelectedItem;
            onDutycall.ItemsSource = selectedMMedicalStaff.OnCallDuty;
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); 
            this.Close();
        }
    }
}
