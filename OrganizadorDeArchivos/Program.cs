using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OrganizadorDeArchivos
{
    internal class Program
    {
        static string salir = "no";
        static void Main(string[] args)
        {
            string continuar = "si";
            while (continuar == "si")
            {
                Menu();
                Console.Clear();
                if (salir == "no")
                {
                    Console.WriteLine("¿Quieres realizar otra accion? (si/no)");
                    Console.WriteLine("Nota: Datos diferentes a si o no seran considerados como no");
                    continuar = Console.ReadLine();
                }
                else
                {
                    continuar = "no";
                }
            }
        }

        static void Menu()
        {
            string respuesta = "0";
            while (respuesta == "0")
            {
                Random rand = new Random();
                int randColor = rand.Next(1, 16);
                Console.Clear();
                Console.ForegroundColor = (ConsoleColor)randColor;
                Console.WriteLine("===========================");
                Console.WriteLine("||Organizador de archivos||");
                Console.WriteLine("===========================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("¿Que quieres realizar? [ ]");
                Console.WriteLine("=======================");
                Console.WriteLine("1. Organizar por extension");
                Console.WriteLine("2. Organizar por fecha de modificacion");
                Console.WriteLine("3. Organizar por año de modificacion");
                Console.WriteLine("4. Ver lista de archivos por espacio");
                Console.WriteLine("5. Organizar por tamaño");
                Console.WriteLine("6. Organizar por primera letra (A-Z)");
                Console.WriteLine("7. Como usar");
                Console.WriteLine("8. Salir");
                Console.WriteLine("=======================");
                Console.WriteLine("© 2025 Froy edition. All Rights Reserved.");
                Console.SetCursorPosition(24, 3);
                respuesta = Console.ReadLine();
                Thread.Sleep(1500);
            }

            switch (respuesta)
            {
                case "1":
                    Organizador();
                    break;
                case "2":
                    OrganizadorPorFecha();
                    break;
                case "3":
                    OrganizadorPorAño();
                    break;
                case "4":
                    ListaDeArchivos();
                    break;
                case "5":
                    OrganizadorPorTamaño();
                    break;
                case "6":
                    OrganizadorPorLetra();
                    break;
                case "7":
                    ComoUsar();
                    break;
                case "8":
                    Console.Clear();
                    salir = "si";
                    break;
                default:
                    Console.WriteLine("Opcion no valida");
                    Console.WriteLine("Presiona una tecla para continuar");
                    Console.ReadKey();
                    break;
            }
        }

        static void Organizador()
        {
            Console.Clear();
            Console.WriteLine("Ingresa el directorio a organizar");
            Console.WriteLine("Ejemplo: C:/Users/Usuario/Desktop/Archivos");
            string path = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Esta ruta de archivos no existe");
                Console.WriteLine("Presiona una tecla para continuar");
                Console.ReadKey();
                return;
            }

            foreach (var i in Directory.GetFiles(path).ToList())
            {
                string extension = Path.GetExtension(i);
                string nombre = Path.GetFileName(i);
                string carpeta = Path.Combine(path, extension.Replace(".", ""));
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }
                File.Move(i, Path.Combine(carpeta, nombre));
            }
        }

        static void OrganizadorPorFecha()
        {
            Console.Clear();
            Console.WriteLine("Ingresa el directorio a organizar");
            Console.WriteLine("Ejemplo: C:/Users/Usuario/Desktop/Archivos");
            string path = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Esta ruta de archivos no existe");
                Console.WriteLine("Presiona una tecla para continuar");
                Console.ReadKey();
                return;
            }

            foreach (var i in Directory.GetFiles(path).ToList())
            {
                string fecha = File.GetLastWriteTime(i).ToString("yyyy-MM-dd");
                string nombre = Path.GetFileName(i);
                string carpeta = Path.Combine(path, fecha);
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }
                File.Move(i, Path.Combine(carpeta, nombre));
            }
        }

        static void OrganizadorPorAño()
        {
            Console.Clear();
            Console.WriteLine("Ingresa el directorio a organizar");
            Console.WriteLine("Ejemplo: C:/Users/Usuario/Desktop/Archivos");
            string path = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Esta ruta de archivos no existe");
                Console.WriteLine("Presiona una tecla para continuar");
                Console.ReadKey();
                return;
            }

            foreach (var i in Directory.GetFiles(path).ToList())
            {
                string fecha = File.GetLastWriteTime(i).ToString("yyyy");
                string nombre = Path.GetFileName(i);
                string carpeta = Path.Combine(path, fecha);
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }
                File.Move(i, Path.Combine(carpeta, nombre));
            }
        }

        static void ListaDeArchivos()
        {
            Console.Clear();
            Console.WriteLine("Ingresa el directorio a ver");
            Console.WriteLine("Ejemplo: C:/Users/Usuario/Desktop/Archivos");
            string path = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Esta ruta de archivos no existe");
                Console.WriteLine("Presiona una tecla para continuar");
                Console.ReadKey();
                return;
            }

            var archivos = Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                                    .Select(f => new FileInfo(f))
                                    .OrderBy(f => f.Length)
                                    .ToList();

            var carpetas = Directory.GetDirectories(path)
                                    .Select(d => new DirectoryInfo(d))
                                    .OrderBy(d => GetDirectorySize(d))
                                    .ToList();

            Console.WriteLine("Archivos:");
            foreach (var archivo in archivos)
            {
                string nombre = archivo.Name;
                string carpeta = archivo.DirectoryName.Replace(path, "").Trim(Path.DirectorySeparatorChar);
                double tamaño = archivo.Length / (1024.0 * 1024.0);
                Console.WriteLine($"Nombre: {nombre} Tamaño: {tamaño:F2} MB Carpeta: {carpeta}");
            }

            Console.WriteLine("\nCarpetas:");
            foreach (var carpeta in carpetas)
            {
                string nombre = carpeta.Name;
                double tamaño = GetDirectorySize(carpeta) / (1024.0 * 1024.0);
                Console.WriteLine($"Nombre: {nombre} Tamaño: {tamaño:F2} MB");
            }

            Console.WriteLine("Presiona una tecla para continuar");
            Console.ReadKey();
        }

        static void OrganizadorPorTamaño()
        {
            Console.Clear();
            Console.WriteLine("Ingresa el directorio a organizar");
            Console.WriteLine("Ejemplo: C:/Users/Usuario/Desktop/Archivos");
            string path = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Esta ruta de archivos no existe");
                Console.WriteLine("Presiona una tecla para continuar");
                Console.ReadKey();
                return;
            }

            var archivos = Directory.GetFiles(path).Select(f => new FileInfo(f)).OrderBy(f => f.Length).ToList();

            foreach (var archivo in archivos)
            {
                string tamaño = (archivo.Length / (1024.0 * 1024.0)).ToString("F2") + "MB";
                string nombre = archivo.Name;
                string carpeta = Path.Combine(path, tamaño);
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }
                File.Move(archivo.FullName, Path.Combine(carpeta, nombre));
            }
        }

        static void OrganizadorPorLetra()
        {
            Console.Clear();
            Console.WriteLine("Ingresa el directorio a organizar");
            Console.WriteLine("Ejemplo: C:/Users/Usuario/Desktop/Archivos");
            string path = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Esta ruta de archivos no existe");
                Console.WriteLine("Presiona una tecla para continuar");
                Console.ReadKey();
                return;
            }

            foreach (var i in Directory.GetFiles(path).ToList())
            {
                string letra = Path.GetFileName(i).Substring(0, 1).ToUpper();
                string nombre = Path.GetFileName(i);
                string carpeta = Path.Combine(path, letra);
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }
                File.Move(i, Path.Combine(carpeta, nombre));
            }
        }

        static long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            return directoryInfo.GetFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
        }

        static void ComoUsar()
        {
            Console.Clear();
            Console.WriteLine("Para usar el programa sigue los siguientes pasos:");
            Console.WriteLine("1. Selecciona la opcion que deseas realizar");
            Console.WriteLine("2. Ingresa la ruta donde se encuentran los archivos");
            Console.WriteLine("Ejemplo de como copiar la ruta: C:/Users/Usuario/Desktop/Archivos");
            Console.WriteLine("3. Espera a que el programa termine de organizar los archivos o detectarlos");
            Console.WriteLine("Nota: Esto puede variar dependiendo de la cantidad de archivos que tengas");
            Console.WriteLine("4. Listo, tus archivos estaran organizados o la lista se proporcionara");
            Console.WriteLine("5. Si deseas realizar otra accion, selecciona la opcion de nuevo");
            Console.WriteLine("6. Si deseas salir, selecciona la opcion salir");
            Console.WriteLine("7. Listo, disfruta del programa");
            Console.WriteLine("=====================================");
            Console.WriteLine("© 2025 Froy edition. All Rights Reserved.");
            Console.WriteLine("");
            Console.WriteLine("Presiona una tecla para continuar...");
            Console.ReadKey();
        }
    }
}

