using System;
using System.IO;

namespace DirectoryManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                DirectoryInfo dirClean = new DirectoryInfo(@"C:\Users\balak\OneDrive\Рабочий стол\папка");

                long begin = DirectoryExtension.CountDirSize(dirClean);
                Console.WriteLine($"Исходный размер папки - {begin} байт");
                Console.WriteLine($"Удалено {DirectoryExtension.CountDeletedFile(dirClean)} файлов");
                long finish = DirectoryExtension.CountDirSize(dirClean);
                Console.WriteLine($"Текущий размер папки - {finish} байт");
                Console.WriteLine($"Освобождено {begin - finish} байт");
               

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not find {ex.Message}");
            }

        }
    }
    public static class DirectoryExtension
    {
        public static long CountDirSize(DirectoryInfo dirClean)
        {
            if (dirClean.Exists)
            {
                long size = 0;

                FileInfo[] files = dirClean.GetFiles();
                foreach (FileInfo file in files)
                {
                    size += file.Length;
                }

                DirectoryInfo[] dirs = dirClean.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    size += CountDirSize(dir);
                }

                return size;

            }
            else
            {
                Console.WriteLine("Такой папки не существует");
                return 0;
            }
        }

        public static int CountDeletedFile(DirectoryInfo dirClean)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(30);

            if (dirClean.Exists)
            {
                int count = 0;

                FileInfo[] files = dirClean.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.LastAccessTime + timeSpan < DateTime.Now)
                    {
                        count += 1;
                        file.Delete();
                    }

                }

                DirectoryInfo[] dirs = dirClean.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    if (dir.LastAccessTime + timeSpan < DateTime.Now)
                    {
                        count += CountDeletedFile(dir);
                    }
                }
                return count;

            }
            else
            {
                Console.WriteLine("Такой папки не существует");
                return 0;
            }

        }
    }
}