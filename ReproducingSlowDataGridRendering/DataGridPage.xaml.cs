using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReproducingSlowDataGridRendering
{
    /// <summary>
    /// Interaction logic for DataGridPage.xaml
    /// </summary>
    public partial class DataGridPage : Page
    {
        private ObservableCollection<ExampleTableRow> data = new();
        private ICollectionView view;

        public DataGridPage()
        {
            InitializeComponent();

            for(int i = 1; i <= 200; i++)
                data.Add(new ExampleTableRow(){Id = i, DateCreated = DateTime.Now, Name = $"Num: {i * 4 % 7}", DateModified = DateTime.Today, Toggle = true});

            view = CollectionViewSource.GetDefaultView(data);
            view.Filter += (o) =>
            {
                ExampleTableRow row = (ExampleTableRow) o;
                return row.Id.ToString().Contains(filterTextBox.Text);
            };

            dataGrid.ItemsSource = view;
        }

        private void filterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            view.Refresh();
        }
    }
}
