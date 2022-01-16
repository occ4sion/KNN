using System.Windows;

namespace KNN
{
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
}
