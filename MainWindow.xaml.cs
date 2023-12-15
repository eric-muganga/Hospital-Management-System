using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using MaterialDesignThemes.Wpf;

namespace HospitalManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        Hospital hospital;
        public MainWindow()
        {
            InitializeComponent();
            hospital = new Hospital();
            Loaded += MainWindow_Loaded;
            //Closing += MainWindow_Closing;
        }

        
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if(isDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string filePath = @"EmployeeData.bin";
            //hospital.Employees = SerializationHelper.DeserializeFromFile(filePath);
            // Check if the file exists and is not empty
            if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
            {
                // Load the employee data from the file
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    hospital.Employees = (List<Employee>)binaryFormatter.Deserialize(stream);
                    // Use the loaded employee data to update your application's state
                }
            }
            else
            {
                // The file does not exist or is empty, so use default employee data
                hospital.Employees = new List<Employee>
                {
                    new Administrator("Eric", "Muganga", "0234456750", "admin", "admin")
                };
            }
            //view.Refresh();
        }
        

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            //DragMove();
        }

        private void OnLoginBtnClick(object sender, RoutedEventArgs e)
        {
            string enteredUsername = txtUsername.Text;

            string enteredPassword = txtPassword.Password;

            if(enteredUsername == null  || enteredUsername.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter Username", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //after clicking ok on the message box the focus is set on amount textBox
                txtUsername.Focus();
                return;
            }else if (enteredPassword == null || enteredPassword.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter password", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //after clicking ok on the message box the focus is set on PasswordBox
                txtPassword.Focus();
                return;
            }
            else
            {
                Employee employee = AuthenticateUser(enteredUsername, enteredPassword);
                if(employee == null)
                {
                    MessageBox.Show("Incorrent username or password","Login",MessageBoxButton.OK, MessageBoxImage.Information);
                    txtUsername.Focus();
                }
                else if (employee is Doctor || employee is Nurse)
                {
                    //DisplayingAllDocsAndNurses();
                    DoctorNurseHome doctorNurseHome = new DoctorNurseHome(hospital);
                    doctorNurseHome.Show();
                    this.Close();
                }
                else if (employee is Administrator)
                {
                    AdminHome adminHome = new AdminHome(hospital);
                    adminHome.Show();
                    this.Close();
                    //DisplayAllEmployees();
                    //Console.WriteLine("\n");
                    //Console.WriteLine(string.Join("\n", AdministratorMenu()));
                    //Console.WriteLine("\n");
                    //AdminChoice();

                } 
            }

        }

        private Employee AuthenticateUser(string username, string password)
        {
            return hospital.Employees.FirstOrDefault(empolyee => empolyee.Username == username && empolyee.Password == password);
        }
    }
}
