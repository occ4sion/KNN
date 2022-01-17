using System;
using System.Collections.Generic;

namespace KNN
{
    class Model
    {
        List<point>? data; // обучающая выборка
        public bool IsFitted() // если модель обучена
        // то обучающая выборка не пуста
        {
            return data is not null;
        }
        protected virtual double kernel(double r) // функция окна
        {
            return 0.0;
        }
        private double distance(point point1, point point2) // расстояние Минковского
        {
            double dist = 0, r = 0;
            for (int i = 0; i < point1.coords.Length; i++)
            {
                dist = point1.coords[i] - point2.coords[i];
                r += Math.Sqrt(dist * dist);
            }
            return r;
        }
        public virtual void fit(List<point> data)
        {
            this.data = data;
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
                        clss[cls] += Convert.ToInt32(P.cls == cls) * kernel(distance(P, p));
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
    class ParzenWindow : Model
    {
        double h; // ширина окна

        public ParzenWindow(double h)
        {
            this.h = h;
        }

        protected override double kernel(double r)
        {
            return Math.Exp(-1 * r/h);
        }
    }
}
