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

namespace KNN
{
    public partial class MainWindow : Window
    {
        bool IsEnable, IsChecked; // нажата ли кнопка для движения мыши
        List<point> data; // обучающая выборка
        List<point> test, fill_coords; // тестовая выборка
        List<Rectangle> rect, fill; // точки тестовых данных 
        Model model;
        Rectangle rectangle;
        double param;
        public MainWindow()
        {
            data = new List<point>();
            test = new List<point>();
            fill_coords = new List<point>();
            rect = new List<Rectangle>();
            fill = new List<Rectangle>();
            param = .1;
            model = new ParzenWindow(.1);
            InitializeComponent();
            combo.Items.Add("Экспоненциальное ядро");
            combo.Items.Add("K-ближайших соседей");
            combo.Items.Add("Ближайший сосед");
            combo.SelectedItem = "Экспоненциальное ядро";
            // заполняем фон точками на случай, если понадобится рисовать границы
            for (int i = 0; i < 640 - 20; i+=9)
            {
                for (int j = 0; j < 434; j+=9)
                {
                    rectangle = new Rectangle();
                    rectangle.Width = 10;
                    rectangle.Height = 10;
                    rectangle.Fill = Brushes.White;
                    canvas.Children.Add(rectangle);
                    Canvas.SetLeft(rectangle, i);
                    Canvas.SetTop(rectangle, j);
                    fill.Add(rectangle);
                    fill_coords.Add(new point(new double[] { i, j }, 0));
                }
            }
        }
        private void Draw(object sender, MouseEventArgs e)
        {
            if (!IsEnable)
                return;
            rectangle = new Rectangle(); // создаем точку
            rectangle.Width = 10;
            rectangle.Height = 10;
            Point p = e.GetPosition(this);
            // Проверки нажатия кнопки, покраска и добавление в выборку
            if (e.RightButton == MouseButtonState.Pressed)
            {
                rectangle.Fill = Brushes.RoyalBlue;
                data.Add(new point(p, 1));

            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectangle.Fill = Brushes.IndianRed;
                data.Add(new point(p, -1));
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                rectangle.Fill = Brushes.Black;
                test.Add(new point(p, 0));
                rect.Add(rectangle);
            }
            canvas.Children.Add(rectangle); // добавляем точку на холст
            Canvas.SetLeft(rectangle, p.X);
            Canvas.SetTop(rectangle, p.Y);
        }

        private void Up(object sender, MouseButtonEventArgs e)
        {
            IsEnable = false; // кнопка не нажата -> рисовать нельзя
        }

        private void Down(object sender, MouseButtonEventArgs e)
        {
            IsEnable = true; // кнопка нажата -> можно рисовать
            Draw(sender, e);
        }

        private void Fit(object sender, RoutedEventArgs e)
        {
            if (data.Count == 0) // если обучающих данных нет
                return;
            double.TryParse(args.Text, out param);
            switch (combo.SelectedItem)
            {
                case "Экспоненциальное ядро":
                    model = new ParzenWindow(param);
                    label.Content = "Ширина окна";
                    break;
                case "K-ближайших соседей":
                    label.Content = "Количество соседей";
                    break;
                case "Ближайший сосед":
                    label.Content = "";
                    break;
                default:
                    break;
            }
            model.fit(data);
            
        }

        private void Predict(object sender, RoutedEventArgs e)
        {
            if (!model.IsFitted()) // если модель не обучена
                return;
            int[] cls;
            cls = model.predict(test, new int[] { -1, 1 }); // предсказания для тестовых данных
            for (int i = 0; i < cls.Length; i++) // для каждой тестовой точки
            {
                rect[i].Fill = cls[i] == 1 ? Brushes.DarkBlue : Brushes.DarkRed; // покраска соответствующей точки в цвет её метки класса
            }
            if (!IsChecked) // если границы не нужно отображать
                return;
            cls = model.predict(fill_coords, new int[] { -1, 1 }); // предсказания для фоновых точек
            for (int i = 0; i < cls.Length; i++) // для каждой точки на фоне
            {
                fill[i].Fill = cls[i] == 1 ? Brushes.SkyBlue : Brushes.OrangeRed; // покраска соответствующей точки в цвет её метки класса
            }
        }

        private void Select(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            canvas.Children.RemoveRange(fill.Count, data.Count + test.Count + 1); // удаляем всё, кроме фоновых точек
            foreach (Rectangle r in fill)
            {
                r.Fill = Brushes.White; // закрашиваем фон
            }
            data.Clear();
            test.Clear();
            rect.Clear();
        }

        private void Check(object sender, RoutedEventArgs e)
        {
            IsChecked = true; // нужно отображать границы
        }

        private void Uncheck(object sender, RoutedEventArgs e)
        {
            IsChecked = false; // границы не нужно отображать
        }
    }
}
