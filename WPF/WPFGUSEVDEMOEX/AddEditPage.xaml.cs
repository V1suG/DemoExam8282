using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using WPFGUSEVDEMOEX.ClassesForAll;

namespace WPFGUSEVDEMOEX
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Partners_product_import_ _currentPurchase = new Partners_product_import_();
        public AddEditPage(Partners_product_import_ selectedPurchase)
        {
            InitializeComponent();
           if (selectedPurchase != null)
                _currentPurchase = selectedPurchase;
            DataContext = selectedPurchase;
            ComboProducts.ItemsSource = DemoExamEntities.GetContext().Product_import_.ToList();
            ComboPartners.ItemsSource = DemoExamEntities.GetContext().Partners_import_.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder erros = new StringBuilder();
            if (_currentPurchase.Product_import_ == null)
                erros.AppendLine("Выберите продукт");
            if (_currentPurchase.Partners_import_ == null)
                erros.AppendLine("Выберите парнёра");
            if (_currentPurchase.quantity_import <= 0)
                erros.AppendLine("Цена должна быть < 0");
            if (_currentPurchase.Date_import == null)
                erros.AppendLine("Выберите дату");
            if (erros.Length > 0)
            {
                MessageBox.Show(erros.ToString());
                return;
            }
            if (DemoExamEntities.GetContext().Entry(_currentPurchase).State == EntityState.Detached)
            {
                DemoExamEntities.GetContext().Partners_product_import_.Add(_currentPurchase);
            }
            try
            {
                DemoExamEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                string errorMessage = "Ошибка: " + ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    errorMessage += "\nInnerException: " + ex.Message;
                }
                MessageBox.Show(errorMessage);
            }
        }
    }
}
