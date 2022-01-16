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

namespace KNN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public struct point // Класс для точек данных
    {
        public double[] coords; // координаты
        public int cls; // метка класса
        public point(Point p, int cls) // конструктор для создания объекта из
        // встроенного типа Point
        {
            this.coords = new double[2] { p.X, p.Y };
            this.cls = cls;
        }
        public point(double[] coords, int cls)
        {
            this.coords = coords;
            this.cls = cls;
        }
    }
    public class Model
    {
        List<point> data; // обучающая выборка
        double h; // ширина окна
        public bool IsFitted() // если модель обучена
        // то обучающая выборка не пуста
        {
            return data is not null;
        }
        private double kernel(double r) // функция окна
        {
            return Math.Exp(-1 * h * r);
        }
        public static double distance(point point1, point point2) // расстояние Минковского
        {
            double r = 0;
            for (int i = 0; i < point1.coords.Length; i++)
            {
                r += Math.Sqrt( (point1.coords[i] - point2.coords[i]) * (point1.coords[i] - point2.coords[i]) );
            }
            return r;
        }
        public void fit(List<point> data, double h)
        {
            this.data = data;
            this.h = h;
        }
        public int[] predict(List<point> points, int[] classes)
        {
            int[] y = new int[points.Count]; // метка классов для каждого тестового объекта
            Dictionary<int, double> clss = new Dictionary<int, double>(); // словарь метка класса:сумма
            // костыль для реализации argmax
            int i = 0;
            foreach (point p in points) // для каждоого тестового образца
            {
                foreach (int cls in classes) // для каждой метки класса
                {
                    clss[cls] = 0; // зануляем сумму для метки
                    foreach (point P in data) // суммируем по всем обучающим точкам класса
                    {
                        clss[cls] += Convert.ToInt32(P.cls == cls) * kernel(distance(P, p)/h);
                    }
                }
                int res = 1;
                for (int j = 0; j < classes.Length-1; j++) // argmax
                {
                    if (clss[classes[j]] > clss[classes[j + 1]])
                        res = classes[j];
                }
                y[i] = res; // присваиваем метку для i-го образца
                i++;
            }
            return y;
        }
    }
    public partial class MainWindow : Window
    {
        bool enabled; // нажата ли кнопка для движения мыши
        List<point> data; // обучающая выборка
        List<point> test; // тестовая выборка
        List<Rectangle> rect; // точки тестовых данных 
        Model knn;
        public MainWindow()
        {
            data = new List<point>();
            test = new List<point>();
            rect = new List<Rectangle>();
            knn = new Model();
            InitializeComponent();
        }

        private void Draw(object sender, MouseEventArgs e)
        {
            if (enabled)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Width = 10;
                rectangle.Height = 10;
                Point p = e.GetPosition(this);
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
                    data.Add(new point(p, 0));
                    rect.Add(rectangle);
                }
                ((Canvas)sender).Children.Add(rectangle);
                Canvas.SetLeft(rectangle, p.X);
                Canvas.SetTop(rectangle, p.Y);
            }
        }

        private void Up(object sender, MouseButtonEventArgs e)
        {
            enabled = false;
        }

        private void Down(object sender, MouseButtonEventArgs e)
        {
            //enabled = true;
            Rectangle rectangle = new Rectangle(); // создаем точку
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
            ((Canvas)sender).Children.Add(rectangle); // добавляем точку на холст
            Canvas.SetLeft(rectangle, p.X);
            Canvas.SetTop(rectangle, p.Y);
        }

        private void Fit(object sender, RoutedEventArgs e)
        {
            if (data.Count == 0) // если обучающих данных нет
                return;
            knn.fit(data, .1);
            
        }

        private void Predict(object sender, RoutedEventArgs e)
        {
            if (!knn.IsFitted()) // если модель не обучена
                return;
            int[] cls = knn.predict(test, new int[] { -1, 1 }); // предсказания для тестовых данных
            for (int i = 0; i < cls.Length; i++) // для каждой тестовой точки
            {
                rect[i].Fill = cls[i] == 1 ? Brushes.DarkBlue : Brushes.DarkRed; // покраска соответствующей тояки в цвет её метки класса
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            data.Clear();
            test.Clear();
            rect.Clear();
            canvas.Children.Clear();
        }
    }
}
