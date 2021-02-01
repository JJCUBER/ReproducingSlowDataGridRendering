using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

            MakeCustomDataGrid<ExampleTableRow>(dataGrid);
        }

        private void filterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            view.Refresh();
        }

        private static readonly Style CENTER_ELEMENT_STYLE = new() { Setters = { new Setter(HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Center) } };

        private static readonly Dictionary<Type, Func<string, DataGridColumn>> DATA_GRID_COLUMN_MAPPER = new()
        {
            [typeof(bool)] = (s) => new DataGridCheckBoxColumn() { Binding = new Binding(s) },
            [typeof(bool?)] = (s) => new DataGridCheckBoxColumn() { Binding = new Binding(s), IsThreeState = true },
            [typeof(int)] = (s) => new DataGridTextColumn() { Binding = new Binding(s), ElementStyle = CENTER_ELEMENT_STYLE },
            [typeof(int?)] = (s) => new DataGridTextColumn() { Binding = new Binding(s), ElementStyle = CENTER_ELEMENT_STYLE },
            [typeof(float)] = (s) => new DataGridTextColumn() { Binding = new Binding(s), ElementStyle = CENTER_ELEMENT_STYLE },
            [typeof(float?)] = (s) => new DataGridTextColumn() { Binding = new Binding(s), ElementStyle = CENTER_ELEMENT_STYLE },
            [typeof(string)] = (s) => new DataGridTextColumn() { Binding = new Binding(s) },
            [typeof(DateTime)] = (s) => new DataGridTextColumn() { Binding = new Binding(s) { StringFormat = "M/dd/yyyy h:mm:ss.FFFF tt" } },
            [typeof(DateTime?)] = (s) => new DataGridTextColumn() { Binding = new Binding(s) { StringFormat = "M/dd/yyyy h:mm:ss.FFFF tt" } }
        };

        public void MakeCustomDataGrid<T>(DataGrid dataGrid, int? primaryKeyId = null)
        {
            Type type = typeof(T);

            dataGrid.IsReadOnly = true;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.Columns.Clear();

            bool isFirstNameCol = true;

            foreach (PropertyInfo pi in type.GetProperties())
            {
                DataGridColumn col = DATA_GRID_COLUMN_MAPPER[pi.PropertyType](pi.Name);
                col.Header = Regex.Replace(pi.Name, "[A-Z]", " $0")[1..];

                if (isFirstNameCol && pi.Name.Contains("Name"))
                {
                    col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                    isFirstNameCol = false;
                }

                dataGrid.Columns.Add(col);


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

        }
    }
}
