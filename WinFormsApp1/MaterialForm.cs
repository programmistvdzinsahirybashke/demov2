using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class MaterialForm : Form
    {
        private string connectionString;
        private Action refreshMaterialsCallback;
        private int? materialId = null; // null — добавление, не null — редактирование

        // Конструктор для добавления
        public MaterialForm(string connStr, Action refreshCallback, int? materialId = null)
        {
            InitializeComponent();
            connectionString = connStr;
            refreshMaterialsCallback = refreshCallback;
            LoadMaterialTypes();
        }

        // Конструктор для редактирования
        public MaterialForm(string connStr, Action refreshCallback, int materialIdToEdit)
            : this(connStr, refreshCallback)
        {
            materialId = materialIdToEdit;
            LoadMaterialData(materialIdToEdit);
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

        private void LoadMaterialData(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(@"
                        SELECT name, material_type_id, unit_price, quantity, min_quantity, package_quantity, unit_id
                        FROM materials WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tbName.Text = reader.GetString(0);
                            comboBox1.SelectedValue = reader.GetInt32(1);
                            tbUnitPrice.Text = reader.GetDecimal(2).ToString();
                            tbQuantity.Text = reader.GetDecimal(3).ToString();
                            tbMinQuantity.Text = reader.GetDecimal(4).ToString();
                            tbPackageQuantity.Text = reader.GetDecimal(5).ToString();
                            tbUnit.Text = reader.GetString(6);

                            this.Text = "Редактирование материала";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных материала: " + ex.Message);
            }
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

            string confirmationMessage =
                (materialId.HasValue ? "Обновить материал с данными:" : "Добавить новый материал со следующими данными?") + "\n\n" +
                $"Тип: {comboBox1.Text}\n" +
                $"Наименование: {tbName.Text.Trim()}\n" +
                $"Цена за единицу: {unitPrice}\n" +
                $"Единица измерения: {tbUnit.Text.Trim()}\n" +
                $"Количество на складе: {quantity}\n" +
                $"Минимальное количество: {minQuantity}\n" +
                $"Количество в упаковке: {packageQuantity}";

            var result = MessageBox.Show(confirmationMessage, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    NpgsqlCommand cmd;
                    if (materialId.HasValue)
                    {
                        // Редактирование
                        cmd = new NpgsqlCommand(@"
                            UPDATE materials SET
                                name = @name,
                                material_type_id = @type_id,
                                unit_price = @price,
                                quantity = @quantity,
                                min_quantity = @min_quantity,
                                package_quantity = @package_quantity,
                                unit_id = @unit_id
                            WHERE id = @id", conn);

                        cmd.Parameters.AddWithValue("id", materialId.Value);
                    }
                    else
                    {
                        // Добавление
                        cmd = new NpgsqlCommand(@"
                            INSERT INTO materials 
                            (name, material_type_id, unit_price, quantity, min_quantity, package_quantity, unit_id) 
                            VALUES (@name, @type_id, @price, @quantity, @min_quantity, @package_quantity, @unit_id)", conn);
                    }

                    cmd.Parameters.AddWithValue("name", tbName.Text.Trim());
                    cmd.Parameters.AddWithValue("type_id", (int)comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("price", unitPrice);
                    cmd.Parameters.AddWithValue("quantity", quantity);
                    cmd.Parameters.AddWithValue("min_quantity", minQuantity);
                    cmd.Parameters.AddWithValue("package_quantity", packageQuantity);
                    cmd.Parameters.AddWithValue("unit_id", tbUnit.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(materialId.HasValue ? "Материал успешно обновлён." : "Материал успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refreshMaterialsCallback();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении материала: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
