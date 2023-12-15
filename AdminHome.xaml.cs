using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace HospitalManagementSystem
{
    /// <summary>
    /// Interaction logic for AdminHome.xaml
    /// </summary>
    public partial class AdminHome : Window
    {
        Hospital hospital;
        
        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        ICollectionView view; 
        public AdminHome(Hospital h)
        {
            InitializeComponent();
            hospital = h;
            ShowEmployees();
            view = CollectionViewSource.GetDefaultView(employeesList.ItemsSource);
            Closing += AdminHome_Closing;
        }


        //Serialization at this window's closure
        private void AdminHome_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string filePath = @"EmployeeData.bin";
            //SerializationHelper.SerializeToFile(hospital.Employees, filePath);
            using (Stream stream = File.Open(filePath, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, hospital.Employees);
            }
        }

        //Exit button 
        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //
        private void ShowEmployees()
        {          
            employeesList.ItemsSource = hospital.Employees; 
        }

        //Button click to delete an employee
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure You want to delete this Employee", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Employee employee = (Employee)employeesList.SelectedItem;
                hospital.Employees.RemoveAt(employeesList.Items.IndexOf(employee));
                if(employee is MedicalStaff)
                {
                    MedicalStaff medicalStaff = (MedicalStaff)employee;
                    medicalStaff.OnCallDuty.Clear();                   
                }
                view.Refresh();
                OnDuttyCallList.Items.Refresh();
            }            
        }

        //Button click to Add an employee
        private void AddEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            Employee newEmployee;
            string name = txtname.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string pESELNumber = txtPeselNumber.Text.Trim();
            //if(pESELNumber.Length != 11)
            //{
            //    MessageBox.Show("PESEL number should ebe ")
            //}
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string role = ((ComboBoxItem)cmbRole.SelectedItem).Content.ToString();
            

            if(role == "Doctor")
            {
                string pWZNumber = txtPWZNumber.Text.Trim();
                string docSpeciality = ((ComboBoxItem)cmbSpeciality.SelectedItem).Content.ToString();
                newEmployee = new Doctor(name, surname, pESELNumber, username, password, pWZNumber, docSpeciality);
                hospital.Addemployee(newEmployee);
                txtPWZNumber.Text = string.Empty;
                cmbSpeciality.SelectedIndex = -1;
            }
            else if(role == "Nurse")
            {
                newEmployee = new Nurse(name, surname, pESELNumber, username, password);
                hospital.Addemployee(newEmployee);
            }
            else if(role == "Administrator")
            {
                newEmployee = new Administrator(name, surname, pESELNumber, username, password);
                hospital.Addemployee(newEmployee);
            }
            ClearText();
            view.Refresh();
        }

        //Button click to update details of an employee
        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (employeesList.SelectedItem != null)
            {
                Employee selectedEmployee = (Employee)employeesList.SelectedItem;
                try
                {
                    if (selectedEmployee is Doctor)
                    {
                        
                       //selectedEmployee = (Doctor)selectedEmployee;
                        ((Doctor)selectedEmployee).Updated(txtname.Text, txtSurname.Text,
                            txtPeselNumber.Text, txtUsername.Text, txtPassword.Password,
                            txtPWZNumber.Text, ((ComboBoxItem)cmbSpeciality.SelectedItem).Content.ToString(), hospital);

                    }
                    else
                    {
                        selectedEmployee.Updated(txtname.Text, txtSurname.Text,
                            txtPeselNumber.Text, txtUsername.Text, txtPassword.Password, hospital);
                    }
                    
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                
                
                ClearText();
                employeesList.Items.Refresh();
            }
        }

        private void CmbSelection(object sender, SelectionChangedEventArgs e)
        {
            
            cmbSpeciality.IsEnabled = txtPWZNumber.IsEnabled = cmbRole.SelectedItem != null && ((ComboBoxItem)cmbRole.SelectedItem).Content.ToString() == "Doctor";
            //MessageBox.Show();
        }


        //
        private void employeesList_Change(object sender, SelectionChangedEventArgs e)
        {
            if(employeesList.SelectedItem != null)
            {
                Employee selectedEmployee = (Employee)employeesList.SelectedItem;
                if (selectedEmployee is Doctor)
                {  
                    cmbSpeciality.IsEnabled = txtPWZNumber.IsEnabled = true;
                    //txtPWZNumber.Text = 
                    Doctor doctor = (Doctor)selectedEmployee;
                    txtname.Text = doctor.Name;
                    txtSurname.Text = doctor.Surname;
                    txtPeselNumber.Text = doctor.PESELNumber;
                    txtUsername.Text = doctor.Username;
                    txtPassword.Password = doctor.Password;
                    txtPWZNumber.Text = doctor.PWZNumber;
                    stackPanel.IsEnabled = stackPanel2.IsEnabled = stackPanel3.IsEnabled = true;
                    OnDuttyCallList.ItemsSource = doctor.OnCallDuty;
                    cmbRole.SelectedIndex = 0;
                    //error i can't display the doctors specialist in the comboBox
                    cmbSpeciality.Text = doctor.Specialist;
                    

                    ///
                    //datePicked.SelectedDates.ToList() = doctor.OnCallDuty;
                }
                else
                {
                    txtname.Text = selectedEmployee.Name;
                    txtSurname.Text = selectedEmployee.Surname;
                    txtPeselNumber.Text = selectedEmployee.PESELNumber;
                    txtUsername.Text = selectedEmployee.Username;
                    txtPassword.Password = selectedEmployee.Password;

                    if (selectedEmployee is Nurse)
                    {
                        Nurse nurse = (Nurse)selectedEmployee;
                        cmbRole.SelectedIndex = 1;
                        stackPanel.IsEnabled = stackPanel2.IsEnabled = stackPanel3.IsEnabled = true;
                        OnDuttyCallList.ItemsSource = nurse.OnCallDuty;
                    }
                    else if(selectedEmployee is Administrator)
                    {
                        cmbRole.SelectedIndex = 2;
                        stackPanel.IsEnabled = stackPanel2.IsEnabled = stackPanel3.IsEnabled = false;
                        //datePicked.IsEnabled = false;
                    }
                }
            }
        }

        // Clears the input fields
        public void ClearText()
        {
            txtname.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtPeselNumber.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtPassword.Password = string.Empty;
            cmbRole.SelectedItem = null;
            txtPWZNumber.Text = string.Empty;
            
        }

        // Button click to save the selected MedicalStaff's On Duty Call dates
        private void saveDatesBtn_Click(object sender, RoutedEventArgs e)
        {
            MedicalStaff selectedMedicalStaff = (MedicalStaff)employeesList.SelectedItem;
            AddDuty((DateTime)datePicked.SelectedDate);
            OnDuttyCallList.Items.Refresh();
            //OnDuttyCallList.Items.Add(datePicked.SelectedDate);

            //Adds Duty to the  24-hour on-call duty schedule
            void AddDuty(DateTime date)
            {
                if (CheckDutyInMonth(date))
                {
                    if (DutyDayCheck(date))
                    {
                        if (!DoctorWithSameSpecialtyOnDuty(date))
                        {
                            selectedMedicalStaff.OnCallDuty.Add(date.Date);
                        }
                        else
                        {
                            MessageBox.Show($"Duty is already assigned to another {((Doctor)(selectedMedicalStaff)).Specialist.ToString()}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }                        
                }
            }

            //Checks if there's a doctor of a given specialization is already assigned to a given day
            bool DoctorWithSameSpecialtyOnDuty(DateTime date)
            {
                date = date.Date;       
                Doctor doctor = (Doctor)selectedMedicalStaff;
                return (hospital.Employees.OfType<Doctor>().Any(doc => doc.Specialist == doctor.Specialist && doc.OnCallDuty.Contains(date)));
            }

            

            bool DutyDayCheck(DateTime date)
            {
                for (int i = 0; i < selectedMedicalStaff.OnCallDuty.Count; i++)
                {
                    //Checks if that given duty after another duty for the same selected MedicalStaff
                    if (date.Date.AddDays(-1) == selectedMedicalStaff.OnCallDuty[i].Date)
                    {
                        MessageBox.Show("Can't add duty after another duty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                    //Checks if that given date is already assigned to that selected MedicalStaff
                    else if (date.Date == selectedMedicalStaff.OnCallDuty[i].Date)
                    {
                        MessageBox.Show("Can't add duty that already exists", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                    //Checks if that given duty before another duty for the same selected MedicalStaff
                    else if (date.Date.AddDays(1) == selectedMedicalStaff.OnCallDuty[i].Date)
                    {
                        MessageBox.Show("Can't add duty that's before another duty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
                return true;
            }

            bool CheckDutyInMonth(DateTime date)
            {
                int month = date.Month;
                int year = date.Year;
                int count = 1;
                for (int i = 0; i < selectedMedicalStaff.OnCallDuty.Count; i++)
                {
                    if (selectedMedicalStaff.OnCallDuty[i].Month == month && selectedMedicalStaff.OnCallDuty[i].Year == year)
                    {
                        count++;
                    }
                }
                //Check if the limit is exceeded
                if (count > 10)
                {
                    MessageBox.Show("The maximum number  of On call_duties for this month has been reached",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;

                }
                return true;
    
            }
        }



        //On Mouse Double Click to delete the selected MedicalStaff's On duty Call
        private void OnMouseDoubleClickDelete(object sender, MouseButtonEventArgs e)
        {
            MedicalStaff selectedMedicalStaff = (MedicalStaff)employeesList.SelectedItem;
            
            if(MessageBox.Show("Are you sure You want to delete this Duty", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                selectedMedicalStaff.RemoveDuty((DateTime)OnDuttyCallList.SelectedItem);
            }
            
            OnDuttyCallList.Items.Refresh();
        }

        // LogOut Button click
        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            view.Refresh();
            this.Close();
            
        }
    }
}
