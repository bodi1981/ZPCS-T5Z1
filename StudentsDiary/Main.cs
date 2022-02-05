using StudentsDiary.Properties;
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
    public partial class Main : Form
    {
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        public bool IsMaximize 
        {
            get
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;
            }
        }
        public Main()
        {
            InitializeComponent();
            SetMainClassComboBox();
            RefreshDiary();
            SetColumnHeaders();
            

            if (IsMaximize)
                WindowState = FormWindowState.Maximized;
        }

        private void SetMainClassComboBox()
        {
            var classList = Enum.GetNames(typeof(StudentClasses)).ToList();
            classList.Insert(0, "Wszyscy");

            cmbMainClass.DataSource = classList;
        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromXml();
            var classFilter = cmbMainClass.Text;

            if (classFilter == "Wszyscy")
            {
                dgvDiary.DataSource = students.OrderBy(s => s.Id).ToList();
            }
            else
            {
                dgvDiary.DataSource = (from s in students
                                      where s.StudentClass == (StudentClasses)Enum.Parse(typeof(StudentClasses), classFilter)
                                      orderby s.Id
                                      select s).ToList();
            }
        }

        private void SetColumnHeaders()
        {
            dgvDiary.Columns[0].HeaderText = "ID";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy";
            dgvDiary.Columns[9].HeaderText = "Zajęcia dodatkowe";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            //addEditStudent.StudentAdded += AddEditStudent_StudentAdded;
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void AddEditStudent_StudentAdded()
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count==0)
            {
                MessageBox.Show("Najpierw zaznacz wiersz", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var studentId = Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value);
            var addEditStudent = new AddEditStudent(studentId);
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Najpierw zaznacz wiersz", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            var result = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {selectedStudent.Cells[1].Value} {selectedStudent.Cells[2].Value}", "Usunięcie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromXml();
            students.RemoveAll(s => s.Id == id);
            _fileHelper.SerializeToXml(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                IsMaximize = true;
            }
            else
            {
                IsMaximize = false;
            }
            Settings.Default.Save();
        }
    }
}
