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
using WPFGUSEVDEMOEX.ClassesForAll;
using System.Data.Entity;

namespace WPFGUSEVDEMOEX
{
    /// <summary>
    /// Логика взаимодействия для PurchasesPage.xaml
    /// </summary>
    public partial class PurchasesPage : Page
    {
        public PurchasesPage()
        {
            InitializeComponent();
            DGridPurchases.ItemsSource = DemoExamEntities.GetContext().Partners_product_import_.Include(p => p.Product_import_).Include(p => p.Partners_import_).ToList();
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext as Partners_product_import_;
            var context = DemoExamEntities.GetContext();
            var loadedItem = context.Partners_product_import_.Include(p => p.Product_import_).FirstOrDefault(p => p.partner_products_import_ID == selectedItem.partner_products_import_ID);
            Manager.MainFrame.Navigate(new AddEditPage(loadedItem));
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var purchasesForRemoving = DGridPurchases.SelectedItems.Cast<Partners_product_import_>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следущие {purchasesForRemoving.Count()} элементов?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DemoExamEntities.GetContext().Partners_product_import_.RemoveRange(purchasesForRemoving);
                    DemoExamEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DemoExamEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridPurchases.ItemsSource = DemoExamEntities.GetContext().Partners_product_import_.Include(p => p.Product_import_).Include(p => p.Partners_import_).ToList();
            }
        }
    }
}
