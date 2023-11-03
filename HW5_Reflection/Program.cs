using System.Diagnostics;
using System.Text.Json;

namespace HW5_Reflection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество итераций:");
            var str = Console.ReadLine();
            var n = int.Parse(str);
            var arr = F.GenarateRandom(n);
            var timesRS = new long[n];
            var timesRD = new long[n];
            var timesJS = new long[n];
            var timesJD = new long[n];
            for (var i = 0; i < n; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                var strRS = ReflectionSerialize<F>(arr[i]);
                sw.Stop();
                timesRS[i] = sw.ElapsedMilliseconds;
                sw.Restart();
                var objRD = ReflectionDeserialize<F>(strRS);
                sw.Stop();
                timesRD[i] = sw.ElapsedMilliseconds;
                sw.Restart();
                var jsonJS = JsonSerializer.Serialize(arr[i]);
                sw.Stop();
                timesJS[i] = sw.ElapsedMilliseconds;
                sw.Restart();
                var objJD = JsonSerializer.Deserialize<F>(jsonJS);
                sw.Stop();
                timesJD[i] = sw.ElapsedMilliseconds;
            }
            Console.WriteLine($"Сериализация с использованием Reflection: {timesRS.Min()}-{timesRS.Max()}мс");
            Console.WriteLine($"Десериализация с использованием Reflection: {timesRD.Min()}-{timesRD.Max()}мс");
            Console.WriteLine($"Сериализация классическая в json: {timesJS.Min()}-{timesJS.Max()}мс");
            Console.WriteLine($"Десериализация классическая из json: {timesJD.Min()}-{timesJD.Max()}мс");
        }

        public static String ReflectionSerialize<T>(T obj) where T : new()
        {
            var type = typeof(T);
            var fields = type.GetFields();
            return String.Join(";", fields.Select(property => $"{property.Name}={property.GetValue(obj)}"));
        }

        public static T ReflectionDeserialize<T>(String str) where T : new()
        {
            var type = typeof(T);
            var obj = new T();
            foreach (var fieldValue in str.Split(';'))
            {
                var splits = fieldValue.Split('=');
                var name = splits[0];
                var value = splits[1];
                var field = type.GetField(name);
                field.SetValue(obj, Convert.ChangeType(value, field.FieldType));
            }
            return obj;
        }
    }

    public class F
    {
        public int i1, i2, i3, i4, i5;
        public F() { }

        public static F[] GenarateRandom(int n)
        {
            var random = new Random();
            var arr = new F[n];
            for (var i = 0; i < n; i++)
            {
                arr[i] = new F();
                arr[i].i1 = random.Next(0, n);
                arr[i].i2 = random.Next(0, n);
                arr[i].i3 = random.Next(0, n);
                arr[i].i4 = random.Next(0, n);
                arr[i].i5 = random.Next(0, n);
            }
            return arr;
        }
    }
}