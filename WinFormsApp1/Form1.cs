using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Класс модели материала
        public class MaterialModel
        {
            public int Id { get; set; }  // Добавлен ID для идентификации
            public string Type { get; set; }
            public string Name { get; set; }
            public decimal MinQuantity { get; set; }
            public decimal Quantity { get; set; }
            public decimal Price { get; set; }
            public int UnitsInPack { get; set; }
            public decimal RequiredQuantity { get; set; }
            public string Unit { get; set; }  // единица измерения
        }

        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=demo";

        private List<MaterialModel> currentMaterials = new List<MaterialModel>();

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAndDisplayMaterials();
        }

        private void LoadAndDisplayMaterials()
        {
            currentMaterials = LoadMaterialsFromDatabase();
            DisplayMaterials(currentMaterials);
        }

        private List<MaterialModel> LoadMaterialsFromDatabase()
        {
            var materials = new List<MaterialModel>();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                   SELECT 
                        m.id,
                        mt.name AS Type,
                        m.name AS Name,
                        m.min_quantity AS MinQuantity,
                        m.quantity AS Quantity,
                        m.unit_price AS Price,
                        m.package_quantity AS UnitsInPack,
                        m.unit_id AS Unit
                    FROM materials m
                    JOIN material_types mt ON m.material_type_id = mt.id
                    ORDER BY mt.name, m.name;

                ";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        materials.Add(new MaterialModel
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Type = reader["Type"].ToString(),
                            Name = reader["Name"].ToString(),
                            MinQuantity = Convert.ToDecimal(reader["MinQuantity"]),
                            Quantity = Convert.ToDecimal(reader["Quantity"]),
                            Price = Convert.ToDecimal(reader["Price"]),
                            UnitsInPack = Convert.ToInt32(reader["UnitsInPack"]),
                            Unit = reader["Unit"].ToString(),
                            RequiredQuantity = 0
                        });
                    }
                }
            }

            return materials;
        }

        private void DisplayMaterials(List<MaterialModel> materials)
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (var mat in materials)
            {
                GroupBox groupBox = new GroupBox();
                groupBox.Text = $"{mat.Type} - {mat.Name}";
                groupBox.Width = 800;
                groupBox.Height = 220;
                groupBox.Tag = mat.Id; // сохраняем ID материала в Tag для идентификации

                TableLayoutPanel panel = new TableLayoutPanel();
                panel.Dock = DockStyle.Fill;
                panel.RowCount = 7;
                panel.ColumnCount = 2;
                panel.AutoSize = true;
                panel.Padding = new Padding(5);

                void AddRow(string label, string value)
                {
                    panel.Controls.Add(new Label() { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, panel.RowCount - 1);
                    panel.Controls.Add(new Label() { Text = value, AutoSize = true, Anchor = AnchorStyles.Left }, 1, panel.RowCount - 1);
                    panel.RowCount++;
                }

                AddRow("Тип:", mat.Type);
                AddRow("Наименование:", mat.Name);
                AddRow("Мин. кол-во:", mat.MinQuantity.ToString("0.##"));
                AddRow("На складе:", mat.Quantity.ToString("0.##"));
                AddRow("Цена (₽/ед):", $"{mat.Price:0.00} р/{mat.Unit}");
                AddRow("В упаковке:", mat.UnitsInPack.ToString());
                AddRow("Требуемое кол-во:", mat.RequiredQuantity.ToString("0.##"));

                groupBox.Controls.Add(panel);

                // Добавляем обработчик клика по GroupBox для открытия формы редактирования
                groupBox.Click += GroupBox_Click;

                // Чтобы клик по вложенным элементам тоже вызывал GroupBox_Click:
                foreach (Control ctrl in groupBox.Controls)
                    ctrl.Click += GroupBox_Click;

                flowLayoutPanel1.Controls.Add(groupBox);
            }
        }

        private void GroupBox_Click(object? sender, EventArgs e)
        {
            // Определяем ID материала из Tag
            int materialId = -1;

            if (sender is GroupBox gb && gb.Tag is int id)
                materialId = id;
            else if (sender is Control ctrl && ctrl.Parent is GroupBox parentGb && parentGb.Tag is int pid)
                materialId = pid;

            if (materialId == -1)
                return;

            // Открываем форму редактирования с передачей ID материала
            var editForm = new MaterialForm(connectionString, LoadAndDisplayMaterials, materialId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadAndDisplayMaterials();
            }
        }

        private void btnLoadMaterials_Click(object sender, EventArgs e)
        {
            LoadAndDisplayMaterials();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Добавление нового материала - ID = null
            var addForm = new MaterialForm(connectionString, LoadAndDisplayMaterials, null);
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadAndDisplayMaterials();
            }
        }
    }
}
