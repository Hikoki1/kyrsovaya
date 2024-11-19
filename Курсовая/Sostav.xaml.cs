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

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для Sostav.xaml
    /// </summary>
    public partial class Sostav : Page
    {
        public Sostav()
        {
            InitializeComponent();
            var currentProducts = NavalBaseEntities2.GetContext().Личный_состав.ToList();
            ProductListView.ItemsSource = currentProducts;
            RangComboBox.SelectedIndex = 0;
            UpdateProductPage();
            
            int ProductMaxCount = 0;
            foreach (Личный_состав product in currentProducts)
            {
                ProductMaxCount++;

            }
            ProductMaxCountTextBlock.Text = ProductMaxCount.ToString();
            ProductListView.Items.Refresh();
        }

        private void UpdateProductPage()
        {
            var currentProducts = NavalBaseEntities2.GetContext().Личный_состав.ToList();

            //поиск 
            currentProducts = currentProducts.Where(p => p.Фамилия.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();

            //сортировка 
            if (RadioButtonUp.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderBy(p => p.Выслуга_лет).ToList();
            }
            if (RadioButtonDown.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.Выслуга_лет).ToList();
            }
            switch (RangComboBox.SelectedIndex)
            {
                case 0: 
                    break;
                case 1: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Матрос").ToList();
                    break;
                case 2: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Сержант").ToList();
                    break;
                case 3: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Старшина 2 статьи").ToList();
                    break;
                case 4: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Старший матрос").ToList();
                    break;
                case 5: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Мичман").ToList();
                    break;
                case 6: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Капитан-лейтенант").ToList();
                    break;
                case 7: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Капитан 3 ранга").ToList();
                    break;
                case 8: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Капитан 2 ранга").ToList();
                    break;
                case 9: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Капитан 1 ранга").ToList();
                    break;
                case 10: 
                    currentProducts = currentProducts.Where(p => p.Должность == "Контр-адмирал").ToList();
                    break;
                default:  
                    break;
            }

            ProductListView.ItemsSource = currentProducts;
            
            int ProductCount = 0;
            foreach (Личный_состав product in currentProducts)
            {
                ProductCount++;
                ProductListView.Items.Refresh();
            }
            ProductMaxCountTextBlock.Text = NavalBaseEntities2.GetContext().Личный_состав.Count().ToString();
            ProductCountTextBlock.Text = ProductCount.ToString();
            ProductListView.Items.Refresh();

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
            UpdateProductPage();
            ProductListView.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = ProductListView.SelectedItem as Личный_состав;

           
            if (currentClient == null)
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.");
                return; // Если ничего не выбрано, функция заканчивается
            }

          
            if (!string.IsNullOrWhiteSpace(currentClient.Награды))
            {
                MessageBox.Show("Невозможно выполнить удаление, так как существуют награды у данного солдата.");
                return;
            }

     
       
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        NavalBaseEntities2.GetContext().Личный_состав.Remove(currentClient);
                        NavalBaseEntities2.GetContext().SaveChanges();
                        MessageBox.Show("Запись успешно удалена.");
                        UpdateProductPage(); // Обновляем данные после успешного удаления
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении записи: {ex.Message}");
                    }

                }
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Личный_состав));
            UpdateProductPage();
            ProductListView.Items.Refresh();
        }

        private void RadioButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProductPage();
        }

        private void RadioButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProductPage();
        }

        private void RangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProductPage();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProductPage();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProductPage();
        }
    }
}
