using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WinFormsApp1
{
    public partial class MaterialForm : Form
    {
        private string connectionString;
        private Action refreshMaterialsCallback;

        public MaterialForm(string connStr, Action refreshCallback)
        {
            InitializeComponent();
            connectionString = connStr;
            refreshMaterialsCallback = refreshCallback;
            LoadMaterialTypes();
        }

        private void LoadMaterialTypes()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand("SELECT id, name FROM material_types ORDER BY name", conn);
                    var adapter = new NpgsqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    comboBox1.DisplayMember = "name";
                    comboBox1.ValueMember = "id";
                    comboBox1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки типов материалов: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void MaterialForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Валидация
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип материала.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(tbName.Text) ||
                string.IsNullOrWhiteSpace(tbUnit.Text) ||
                !decimal.TryParse(tbUnitPrice.Text, out decimal unitPrice) ||
                !decimal.TryParse(tbQuantity.Text, out decimal quantity) ||
                !decimal.TryParse(tbMinQuantity.Text, out decimal minQuantity) ||
                !decimal.TryParse(tbPackageQuantity.Text, out decimal packageQuantity))
            {
                MessageBox.Show("Проверьте корректность введённых данных. Все числовые поля должны содержать положительные значения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (unitPrice < 0 || quantity < 0 || minQuantity < 0 || packageQuantity < 0)
            {
                MessageBox.Show("Числовые значения не могут быть отрицательными.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string confirmationMessage =
                                $"Добавить новый материал со следующими данными?\n\n" +
                                $"Тип: {comboBox1.Text}\n" +
                                $"Наименование: {tbName.Text.Trim()}\n" +
                                $"Цена за единицу: {unitPrice}\n" +
                                $"Единица измерения: {tbUnit.Text.Trim()}\n" +
                                $"Количество на складе: {quantity}\n" +
                                $"Минимальное количество: {minQuantity}\n" +
                                $"Количество в упаковке: {packageQuantity}";

                    var result = MessageBox.Show(confirmationMessage, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                    {
                        return; // пользователь отказался
                    }

                    var cmd = new NpgsqlCommand(@"
                        INSERT INTO materials 
                        (name, material_type_id, unit_price, quantity, min_quantity, package_quantity, unit_id) 
                        VALUES (@name, @type_id, @price, @quantity, @min_quantity, @package_quantity, @unit_id)", conn);

                    cmd.Parameters.AddWithValue("name", tbName.Text.Trim());
                    cmd.Parameters.AddWithValue("type_id", (int)comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("price", unitPrice);
                    cmd.Parameters.AddWithValue("quantity", quantity);
                    cmd.Parameters.AddWithValue("min_quantity", minQuantity);
                    cmd.Parameters.AddWithValue("package_quantity", packageQuantity);
                    cmd.Parameters.AddWithValue("unit_id", tbUnit.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Материал успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refreshMaterialsCallback(); // Обновление главной формы
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении материала: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
