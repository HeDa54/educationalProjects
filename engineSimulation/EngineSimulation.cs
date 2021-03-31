using System;
using System.Collections.Generic;

namespace TestTaskForForward
{
    internal class EngineSimulation
    {
        public static void Main(string[] args)
        {
            double ambientTemperature;
            double timeToOverheat;
            Engine engine = new Engine();

            Console.Write("Введите температуру окружающей среды: ");
            while (!double.TryParse(Console.ReadLine(), out ambientTemperature))    // проверка на корректный ввод данных
            {
                Console.WriteLine("Ввод данных должен быть в формате: X,XX...(or X.XX...)");
            }
            Console.WriteLine();

            timeToOverheat = new TestStand(engine, ambientTemperature).TimeToOverheat;

            if (timeToOverheat != -1)
                Console.WriteLine($"Время работы двигателя до перегрева: {timeToOverheat} секунд");
            else
                Console.WriteLine("Ошибка! Задана слишком низкая температура окружающей среды\nПри такой температуре перегрев труднодостижим");
            Console.ReadLine(); // чтобы не закрывалось окно консоли после выполнения программы
        }
        
    }    
}

 