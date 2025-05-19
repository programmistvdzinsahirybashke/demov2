namespace WinFormsApp1
{
    partial class MaterialForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comboBox1 = new ComboBox();
            tbName = new TextBox();
            tbUnit = new TextBox();
            tbQuantity = new TextBox();
            tbMinQuantity = new TextBox();
            tbPackageQuantity = new TextBox();
            tbUnitPrice = new TextBox();
            button2 = new Button();
            button1 = new Button();
            l1 = new Label();
            l3 = new Label();
            l4 = new Label();
            lfour = new Label();
            l5 = new Label();
            l2 = new Label();
            label7 = new Label();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(184, 49);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(261, 23);
            comboBox1.TabIndex = 0;
            // 
            // tbName
            // 
            tbName.Location = new Point(184, 78);
            tbName.Name = "tbName";
            tbName.Size = new Size(261, 23);
            tbName.TabIndex = 1;
            // 
            // tbUnit
            // 
            tbUnit.Location = new Point(184, 136);
            tbUnit.Name = "tbUnit";
            tbUnit.Size = new Size(261, 23);
            tbUnit.TabIndex = 2;
            // 
            // tbQuantity
            // 
            tbQuantity.Location = new Point(184, 165);
            tbQuantity.Name = "tbQuantity";
            tbQuantity.Size = new Size(261, 23);
            tbQuantity.TabIndex = 3;
            // 
            // tbMinQuantity
            // 
            tbMinQuantity.Location = new Point(184, 194);
            tbMinQuantity.Name = "tbMinQuantity";
            tbMinQuantity.Size = new Size(261, 23);
            tbMinQuantity.TabIndex = 4;
            // 
            // tbPackageQuantity
            // 
            tbPackageQuantity.Location = new Point(184, 223);
            tbPackageQuantity.Name = "tbPackageQuantity";
            tbPackageQuantity.Size = new Size(261, 23);
            tbPackageQuantity.TabIndex = 5;
            // 
            // tbUnitPrice
            // 
            tbUnitPrice.Location = new Point(184, 107);
            tbUnitPrice.Name = "tbUnitPrice";
            tbUnitPrice.Size = new Size(261, 23);
            tbUnitPrice.TabIndex = 6;
            // 
            // button2
            // 
            button2.Location = new Point(271, 270);
            button2.Name = "button2";
            button2.Size = new Size(174, 23);
            button2.TabIndex = 8;
            button2.Text = "Назад";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(20, 270);
            button1.Name = "button1";
            button1.Size = new Size(233, 23);
            button1.TabIndex = 7;
            button1.Text = "Сохранить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // l1
            // 
            l1.AutoSize = true;
            l1.Location = new Point(20, 86);
            l1.Name = "l1";
            l1.Size = new Size(90, 15);
            l1.TabIndex = 9;
            l1.Text = "Наименование";
            // 
            // l3
            // 
            l3.AutoSize = true;
            l3.Location = new Point(20, 144);
            l3.Name = "l3";
            l3.Size = new Size(116, 15);
            l3.TabIndex = 10;
            l3.Text = "Единица измерения";
            // 
            // l4
            // 
            l4.AutoSize = true;
            l4.Location = new Point(20, 173);
            l4.Name = "l4";
            l4.Size = new Size(128, 15);
            l4.TabIndex = 11;
            l4.Text = "Количество на складе";
            // 
            // lfour
            // 
            lfour.AutoSize = true;
            lfour.Location = new Point(20, 202);
            lfour.Name = "lfour";
            lfour.Size = new Size(154, 15);
            lfour.TabIndex = 12;
            lfour.Text = "Минимальное количество";
            // 
            // l5
            // 
            l5.AutoSize = true;
            l5.Location = new Point(20, 231);
            l5.Name = "l5";
            l5.Size = new Size(134, 15);
            l5.TabIndex = 13;
            l5.Text = "Количество в упаковке";
            // 
            // l2
            // 
            l2.AutoSize = true;
            l2.Location = new Point(21, 115);
            l2.Name = "l2";
            l2.Size = new Size(98, 15);
            l2.TabIndex = 14;
            l2.Text = "Цена за единицу";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(21, 57);
            label7.Name = "label7";
            label7.Size = new Size(89, 15);
            label7.TabIndex = 15;
            label7.Text = "Тип материала";
            // 
            // MaterialForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(518, 323);
            Controls.Add(label7);
            Controls.Add(l2);
            Controls.Add(l5);
            Controls.Add(lfour);
            Controls.Add(l4);
            Controls.Add(l3);
            Controls.Add(l1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(tbUnitPrice);
            Controls.Add(tbPackageQuantity);
            Controls.Add(tbMinQuantity);
            Controls.Add(tbQuantity);
            Controls.Add(tbUnit);
            Controls.Add(tbName);
            Controls.Add(comboBox1);
            Name = "MaterialForm";
            Text = "Добавление материала";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox1;
        private TextBox tbName;
        private TextBox tbUnit;
        private TextBox tbQuantity;
        private TextBox tbMinQuantity;
        private TextBox tbPackageQuantity;
        private TextBox tbUnitPrice;
        private Button button2;
        private Button button1;
        private Label l1;
        private Label l3;
        private Label l4;
        private Label lfour;
        private Label l5;
        private Label l2;
        private Label label7;
    }
}