using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskForForward
{
    class Engine //Вводные данные - сделать из файла
    {
        public const double Hm = 0.01;   // Коэффициент зависимости скорости нагрева от крутящего момента
        public const double Hv = 0.0001; // Коэффициент зависимости скорости нагрева от скорости вращения коленвала
        public const double C = 0.1;     // Коэффициент зависимости скорости охлаждения от температуры двигателя
        public const int I = 10;         // Момент инерции двигателя

        public double errorStable = 0.001; // Примерное значение M, при котором цикл не прекращается
        public double overheatTemperature = 110;   // Температура перегрева

        public List<double> listM = new List<double>() { 20, 75, 100, 105, 75, 0 };   // Крутящий момент    
        public List<double> listV = new List<double>() { 0, 75, 150, 200, 250, 300 }; // Скорость вращения коленвала

    }
}
