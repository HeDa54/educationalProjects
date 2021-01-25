using System;
using System.Collections.Generic;

namespace engineSimulation
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            double ambientTemperature;

            Console.Write("Введите температуру окружающей среды: ");
            while (!double.TryParse(Console.ReadLine(), out ambientTemperature))    // проверка на корректный ввод данных
            {
                Console.WriteLine("Ввод данных должен быть в формате: X,XX...(or X.XX...)");
            }
            Console.WriteLine();

            double time = TestStand(ambientTemperature);
            if (time != 0)
            {
                Console.WriteLine("Время работы двигателя до перегрева: " + time + " секунд");
            }
            else
            {
                Console.WriteLine("Ошибка! Задана слишком низкая/высокая температура окружающей среды");
            }
            Console.ReadLine(); // чтобы не закрывалось окно консоли после выполнения программы
        }
        
        private static double TestStand(double ambientTemperature)
        {
            Engine engine = new Engine();
            double engineTemperature = ambientTemperature;

            double x = 0;
            double t = 0;
            const double step = 0.001;

                while (engineTemperature < engine.overheatTemperature)
                {
                    engine.M = CalkM(x);
                    engine.V += engine.a() * step;
                    x = engine.V;
                    double Vh = engine.Vh();
                    Vh += Vh;
                    double Vc = engine.Vc(ambientTemperature, engineTemperature);
                    double Vsum = Vh + Vc;
                    engineTemperature += Vsum * step;
                    t += step;

                    if (engine.M < engine.errorStable) // проверка на слишком низкую температуру, при которой перегрев труднодостижим
                    {
                    return 0;
                    }
            }

            return Math.Round(t, 3);
        }

        private static double CalkM(double x)
        {
            Engine engine = new Engine();

            double y = 0;
            int start = 0;
            int end = 0;

            for (int i = 0; i < engine.listV.Count - 1; i++)
            {
                if (engine.listV[i] <= x && engine.listV[i + 1] >= x)
                {
                    start = i;
                    end = i + 1;
                }
                double y1 = engine.listM[start];
                double y2 = engine.listM[end];
                double x1 = engine.listV[start];
                double x2 = engine.listV[end];

                double k = (y2 - y1) / (x2 - x1);            //------------------------
                double b = y1 - k * x1;                      // линейная функция, реализация с дополнительными переменными
                y = k * x + b;                               //------------------------

                //y = y1 + (y2 - y1) / (x2 - x1) * (x - x1); // формула линейной интерполяции, второй вариант реализации без ввода дополнительных переменных
            }
            return y;
        }
    }

    class Engine //Вводные данные - сделать из файла
    {
        private const double Hm = 0.01;   // Коэффициент зависимости скорости нагрева от крутящего момента
        private const double Hv = 0.0001; // Коэффициент зависимости скорости нагрева от скорости вращения коленвала
        private const double C = 0.1;     // Коэффициент зависимости скорости охлаждения от температуры двигателя
        private const int I = 10;         // Момент инерции двигателя

        public double M;
        public double V;

        public double errorStable = 0.001; // Примерное значение M, при котором цикл не прекращается
        public double overheatTemperature = 110;   // Температура перегрева

        public List<double> listM = new List<double>() { 20, 75, 100, 105, 75, 0 };   // Крутящий момент    
        public List<double> listV = new List<double>() { 0, 75, 150, 200, 250, 300 }; // Скорость вращения коленвала

        public double a() //вычисление ускорения скорости вращения
        {
            return M / I;
        }

        public double Vc(double ambientTemperature, double enginetemperature)
        {
            return C * (ambientTemperature - enginetemperature);
        }

        public double Vh()
        {
            return M * Hm + Math.Pow(V, 2) * Hv;
        }
    }
}
