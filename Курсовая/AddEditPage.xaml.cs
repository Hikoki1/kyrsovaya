using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Личный_состав _currentChel = new Личный_состав();


        public AddEditPage(Личный_состав SelectedChel)
        {
            InitializeComponent();
           
            if (SelectedChel != null)
            {
                _currentChel = SelectedChel;

              
                ComboType.SelectedItem = ComboType.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => (item.Content as string) == _currentChel.Должность);





            }

            DataContext = _currentChel;
        }

      

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            myOpenFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"; // Добавление фильтра для выбора изображений
            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentChel.Фото = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
           

            if (string.IsNullOrWhiteSpace(_currentChel.Фамилия))
            {
                errors.AppendLine("Укажите фамилию солдата");
            }

            if (_currentChel.Часть <= 0)
            {
                errors.AppendLine("Укажите часть солдата");
            }



            if (ComboType.SelectedItem == null)
                errors.AppendLine("Укажите должность");
            else
            {
                _currentChel.Должность = (ComboType.SelectedItem as ComboBoxItem)?.Content.ToString();

            }


            if (_currentChel.Год_рождения <= 0)
            {
                errors.AppendLine("Укажите год рождения");
            }

            if (_currentChel.Год_поступления_на_службу <= 0)
            {
                errors.AppendLine("Укажите год поступления на службу");
            }


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentChel.Номер_солдата == 0)
            {

                NavalBaseEntities2.GetContext().Личный_состав.Add(_currentChel);
            }
                    
            try
            {
                NavalBaseEntities2.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }







        }
    }
   
}
