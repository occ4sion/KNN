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
    public struct point
    {
        public double[] coords;
        public int cls;
        public point(Point p, int cls)
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
        List<point> data;
        double h;
        public bool IsFitted()
        {
            return data is not null;
        }
        private double kernel(double r)
        {
            return Math.Exp(-1 * h * r);
        }
        public static double distance(point point1, point point2)
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
            int[] y = new int[points.Count];
            Dictionary<int, double> clss = new Dictionary<int, double>();
            int i = 0;
            foreach (point p in points)
            {
                foreach (int cls in classes)
                {
                    clss[cls] = 0;
                    foreach (point P in data)
                    {
                        clss[cls] += Convert.ToInt32(P.cls == cls) * kernel(distance(P, p)/h);
                    }
                }
                int res = 1;
                for (int j = 0; j < classes.Length-1; j++)
                {
                    if (clss[classes[j]] > clss[classes[j + 1]])
                        res = classes[j];
                }
                y[i] = res;
                i++;
            }
            return y;
        }
    }
    public partial class MainWindow : Window
    {
        bool enabled;
        List<point> data;
        List<point> test;
        List<Rectangle> rect;
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
                test.Add(new point(p, 0));
                rect.Add(rectangle);
            }
                ((Canvas)sender).Children.Add(rectangle);
            Canvas.SetLeft(rectangle, p.X);
            Canvas.SetTop(rectangle, p.Y);
        }

        private void Fit(object sender, RoutedEventArgs e)
        {
            if (data.Count == 0)
                return;
            knn.fit(data, .1);
            
        }

        private void Predict(object sender, RoutedEventArgs e)
        {
            if (!knn.IsFitted())
                return;
            int[] cls = knn.predict(test, new int[] { -1, 1 });
            for (int i = 0; i < cls.Length; i++)
            {
                rect[i].Fill = cls[i] == 1 ? Brushes.DarkBlue : Brushes.DarkRed;
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
