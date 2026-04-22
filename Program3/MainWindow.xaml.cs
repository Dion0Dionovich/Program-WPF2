using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Program3
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isSidebarCollapsed = false;
        private ObservableCollection<ISeries> _efficiencySeries;
        private ObservableCollection<ISeries> _statusSeries;
        private ObservableCollection<ISeries> _salesSeries;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            InitializeCharts();
            SetupMenuHandlers();
        }

        private void InitializeCharts()
        {
            // Эффективность сети (полукруг)
            EfficiencySeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double>
                {
                    Values = new double[] { 85 },
                    Name = "Работает",
                    Fill = new SolidColorPaint(SKColors.SteelBlue),
                    InnerRadius = 60
                },
                new PieSeries<double>
                {
                    Values = new double[] { 15 },
                    Name = "Не работает",
                    Fill = new SolidColorPaint(SKColors.LightGray)
                }
            };

            // Состояние сети
            StatusSeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double> { Values = new double[] { 70 }, Name = "Работает", Fill = new SolidColorPaint(SKColor.Parse("#00a65a")) },
                new PieSeries<double> { Values = new double[] { 20 }, Name = "Не работает", Fill = new SolidColorPaint(SKColor.Parse("#f39c12")) },
                new PieSeries<double> { Values = new double[] { 10 }, Name = "Обслуживание", Fill = new SolidColorPaint(SKColor.Parse("#dd4b39")) }
            };

            // Динамика продаж
            SalesSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new double[] { 42000, 45000, 38000, 47000, 52000, 49000, 55000, 58000, 54000, 60000 },
                    Fill = null,
                    GeometrySize = 8,
                    LineSmoothness = 0.5,
                    Stroke = new SolidColorPaint(SKColors.SteelBlue, 3)
                },
                new ColumnSeries<double>
                {
                    Values = new double[] { 150, 160, 140, 170, 180, 175, 190, 200, 185, 210 },
                    Stroke = new SolidColorPaint(SKColors.OrangeRed, 1)
                }
            };
        }



        private void SetupMenuHandlers()
        {
            // Находим все пункты меню с вложенными списками и добавляем обработчики
            foreach (var item in FindVisualChildren<ListBoxItem>(SidebarBorder))
            {
                var expander = FindVisualChild<Expander>(item);
                if (expander != null) continue;

                // Проверяем, есть ли вложенный ListBox
                var innerListBox = FindVisualChild<ListBox>(item);
                if (innerListBox != null)
                {
                    innerListBox.Visibility = Visibility.Collapsed;
                    item.PreviewMouseLeftButtonDown += (s, e) =>
                    {
                        innerListBox.Visibility = innerListBox.Visibility == Visibility.Visible
                            ? Visibility.Collapsed
                            : Visibility.Visible;
                        e.Handled = true;
                    };
                }
            }

            // Кнопка сворачивания боковой панели
            ToggleSidebarButton.Click += (s, e) =>
            {
                _isSidebarCollapsed = !_isSidebarCollapsed;
                SidebarColumn.Width = _isSidebarCollapsed
                    ? new GridLength(50)
                    : new GridLength(230);

                // Скрываем текст при свёрнутом меню
                foreach (var textBlock in FindVisualChildren<TextBlock>(SidebarBorder))
                {
                    if (textBlock.Text != "НАВИГАЦИЯ")
                        textBlock.Visibility = _isSidebarCollapsed ? Visibility.Collapsed : Visibility.Visible;
                }
            };
        }

        // Вспомогательные методы для поиска элементов
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t)
                    yield return t;

                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }

        private static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t)
                    return t;

                var childItem = FindVisualChild<T>(child);
                if (childItem != null)
                    return childItem;
            }
            return null;
        }

        public ObservableCollection<ISeries> EfficiencySeries
        {
            get => _efficiencySeries;
            set { _efficiencySeries = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ISeries> StatusSeries
        {
            get => _statusSeries;
            set { _statusSeries = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ISeries> SalesSeries
        {
            get => _salesSeries;
            set { _salesSeries = value; OnPropertyChanged(); }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}