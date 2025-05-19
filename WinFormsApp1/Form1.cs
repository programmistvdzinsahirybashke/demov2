using Npgsql;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Класс модели материала
        public class MaterialModel
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public decimal MinQuantity { get; set; }
            public decimal Quantity { get; set; }
            public decimal Price { get; set; }
            public int UnitsInPack { get; set; }
            public decimal RequiredQuantity { get; set; }
            public string Unit { get; set; }  // добавлено
        }


        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=demo";


        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAndDisplayMaterials();
        }

        private void LoadAndDisplayMaterials()
        {
            var materials = LoadMaterialsFromDatabase();
            DisplayMaterials(materials);
        }

        private List<MaterialModel> LoadMaterialsFromDatabase()
        {
            var materials = new List<MaterialModel>();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                                SELECT 
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
                            Type = reader["Type"].ToString(),
                            Name = reader["Name"].ToString(),
                            MinQuantity = Convert.ToDecimal(reader["MinQuantity"]),
                            Quantity = Convert.ToDecimal(reader["Quantity"]),
                            Price = Convert.ToDecimal(reader["Price"]),
                            UnitsInPack = Convert.ToInt32(reader["UnitsInPack"]),
                            Unit = reader["Unit"].ToString(),  // тут берем unit_id
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
                flowLayoutPanel1.Controls.Add(groupBox);
            }
        }

        private void btnLoadMaterials_Click(object sender, EventArgs e)
        {
            LoadAndDisplayMaterials();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var addForm = new MaterialForm(connectionString, LoadAndDisplayMaterials); // без ID - добавление
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadAndDisplayMaterials(); // обновить список
            }
        }

    }
}
