using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private const string FilePath = "students.json";

        // 获取所有学生
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            var students = ReadStudentsFromFile();
            return Ok(students);
        }

        // 获取单个学生
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var students = ReadStudentsFromFile();
            var student = students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // 添加学生
        [HttpPost]
        public ActionResult<Student> AddStudent(Student student)
        {
            var students = ReadStudentsFromFile();

            // 自动生成ID
            student.ID = students.Count > 0 ? students.Max(s => s.ID) + 1 : 1;
            students.Add(student);

            WriteStudentsToFile(students);
            return CreatedAtAction(nameof(GetStudent), new { id = student.ID }, student);
        }

        // 更新学生信息
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student updatedStudent)
        {
            var students = ReadStudentsFromFile();
            var student = students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            // 更新学生信息
            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Class = updatedStudent.Class;

            WriteStudentsToFile(students);
            return NoContent();
        }

        // 删除学生
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var students = ReadStudentsFromFile();
            var student = students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            students.Remove(student);
            WriteStudentsToFile(students);
            return NoContent();
        }

        // 从文件读取学生数据
        private List<Student> ReadStudentsFromFile()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                return new List<Student>();
            }

            var json = System.IO.File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
        }

        // 将学生数据写入文件
        private void WriteStudentsToFile(List<Student> students)
        {
            var json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(FilePath, json);
        }
    }
}
