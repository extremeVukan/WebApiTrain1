using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Class { get; set; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri("https://localhost:7101/api/Student/");

            Console.WriteLine("1. 获取所有学生信息");
            await GetAllStudents();

            Console.WriteLine("\n2. 添加新学生");
            await AddStudent(new Student { Name = "张三", Age = 20, Class = "计算机科学" });

            Console.WriteLine("\n3. 获取单个学生信息");
            await GetStudentById(1);

            Console.WriteLine("\n4. 更新学生信息");
            await UpdateStudent(1, new Student { ID = 1, Name = "李四", Age = 21, Class = "软件工程" });

            Console.WriteLine("\n5. 删除学生");
            await DeleteStudent(4);

            Console.WriteLine("\n操作完成！");
        }

        // 获取所有学生
        private static async Task GetAllStudents()
        {
            var response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var students = JsonSerializer.Deserialize<List<Student>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                foreach (var student in students)
                {
                    Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}, Class: {student.Class}");
                }
            }
            else
            {
                Console.WriteLine($"获取学生失败: {response.StatusCode}");
            }
        }

        // 获取单个学生
        private static async Task GetStudentById(int id)
        {
            var response = await client.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var student = JsonSerializer.Deserialize<Student>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}, Class: {student.Class}");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"获取学生失败: {response.StatusCode}, 错误信息: {error}");
            }
        }


        // 添加学生
        private static async Task AddStudent(Student student)
        {
            var json = JsonSerializer.Serialize(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("学生添加成功！");
            }
            else
            {
                Console.WriteLine($"添加学生失败: {response.StatusCode}");
            }
        }

        // 更新学生
        private static async Task UpdateStudent(int id, Student student)
        {
            if (!await StudentExists(id))
            {
                Console.WriteLine($"更新失败，学生 ID {id} 不存在。");
                return;
            }

            var json = JsonSerializer.Serialize(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{id}", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("学生更新成功！");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"更新学生失败: {response.StatusCode}, 错误信息: {error}");
            }
        }


        // 删除学生
        private static async Task DeleteStudent(int id)
        {
            if (!await StudentExists(id))
            {
                Console.WriteLine($"删除失败，学生 ID {id} 不存在。");
                return;
            }

            var response = await client.DeleteAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("学生删除成功！");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"删除学生失败: {response.StatusCode}, 错误信息: {error}");
            }
        }


        // 检查学生是否存在
        private static async Task<bool> StudentExists(int id)
        {
            var response = await client.GetAsync($"{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"检查学生失败: {response.StatusCode}, 错误信息: {error}");
            }
            return response.IsSuccessStatusCode;
        }

    }
}
