using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        public event Action StudentAdded;

        private int _studentId;
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();

            this._studentId = id;
            cmbClass.DataSource = Enum.GetValues(typeof(StudentClasses));
            GetStudentData();

            tbFirstName.Select();
        }

        private void OnStudentAdded()
        {
            StudentAdded?.Invoke();
        }

        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edycja ucznia";

                var students = _fileHelper.DeserializeFromXml();
                var student = (from s in students
                               where s.Id == _studentId
                               select s).FirstOrDefault();

                if (student == null)
                {
                    throw new Exception("Brak ucznia o podanym ID");
                }

                FillTextBoxes(student);
            }
        }

        private void FillTextBoxes(Student student)
        {
            tbId.Text = student.Id.ToString();
            tbFirstName.Text = student.FirstName;
            tbLastName.Text = student.LastName;
            tbMaths.Text = student.Maths;
            tbTechology.Text = student.Technology;
            tbPhysics.Text = student.Physics;
            tbPolish.Text = student.Polish;
            tbForeign.Text = student.Foreign;
            rtbComments.Text = student.Comments;
            chkAddedClass.Checked = student.IsAddedClass;
            cmbClass.Text = student.StudentClass.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromXml();

            if (_studentId != 0)
                students.RemoveAll(s => s.Id == _studentId);
            else
                AssignIdToNewStudent(students);
            
            AddNewStudentToList(students);
            _fileHelper.SerializeToXml(students);
            OnStudentAdded();
            Close();
        }

        private void AddNewStudentToList(List<Student> students)
        {
            var newStudnet = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                Maths = tbMaths.Text,
                Technology = tbTechology.Text,
                Physics = tbPhysics.Text,
                Polish = tbPolish.Text,
                Foreign = tbForeign.Text,
                IsAddedClass = chkAddedClass.Checked,
                StudentClass = (StudentClasses)Enum.Parse(typeof(StudentClasses), cmbClass.Text)
        };

            students.Add(newStudnet);
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            _studentId = (from student in students
                          orderby student.Id descending
                          select student.Id).FirstOrDefault();
            _studentId++;
        }
    }
}
